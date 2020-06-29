using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency.Inspecting
{
    partial class Inspector
    {
        private readonly Type _sourceInterface;
        private readonly TypeInfo _sourceInterfaceTypeInfo;
        private readonly RemoteAgencyInterfaceInfo _result;
        private readonly bool _includesProxyOnlyInfo;
        private readonly bool _includesServiceWrapperOnlyInfo;

        private Type _serializerInterfaceLevelAttributeBaseType;
        private Type _serializerAssetLevelAttributeBaseType;
        private Type _serializerDelegateLevelAttributeBaseType;
        private Type _serializerParameterLevelAttributeBaseType;

        public Inspector(Type sourceInterface, bool includesProxyOnlyInfo, bool includesServiceWrapperOnlyInfo, Type serializerInterfaceLevelAttributeBaseType, Type serializerAssetLevelAttributeBaseType, Type serializerDelegateLevelAttributeBaseType, Type serializerParameterLevelAttributeBaseType)
        {
            _sourceInterface = sourceInterface;
            _sourceInterfaceTypeInfo = sourceInterface.GetTypeInfo();
            _result = new RemoteAgencyInterfaceInfo();
            _includesProxyOnlyInfo = includesProxyOnlyInfo;
            _includesServiceWrapperOnlyInfo = includesServiceWrapperOnlyInfo;
            _serializerInterfaceLevelAttributeBaseType = serializerInterfaceLevelAttributeBaseType;
            _serializerAssetLevelAttributeBaseType = serializerAssetLevelAttributeBaseType;
            _serializerDelegateLevelAttributeBaseType = serializerDelegateLevelAttributeBaseType;
            _serializerParameterLevelAttributeBaseType = serializerParameterLevelAttributeBaseType;
            SetInterfaceTypeBasicInfo(_result, _sourceInterface, _sourceInterfaceTypeInfo);
        }

        public RemoteAgencyInterfaceInfo InterfaceTypeInfo => _result;

        public void Process()
        {
            //interface level
            Stack<MemberInfo> parentPath = new Stack<MemberInfo>();
            _result.InterfaceLevelPassThroughAttributes =
                GetAttributePassThrough(_sourceInterfaceTypeInfo,
                    (m, a) => new InvalidAttributeDataException(m, a, _sourceInterface, parentPath));
            if (_includesProxyOnlyInfo)
                _result.IsProxyStickyTargetSite =
                    GetValueFromAttribute<ProxyStickyTargetSiteAttribute, bool>(_sourceInterface, i => i.IsSticky,
                        out _);
            _result.ThreadLockMode =
                GetValueFromAttribute<ThreadLockAttribute, ThreadLockMode>(_sourceInterface, i => i.ThreadLockMode,
                    out var threadLockAttribute, ThreadLockMode.None);
            if (_result.ThreadLockMode == ThreadLockMode.TaskSchedulerSpecified)
                _result.TaskSchedulerName = threadLockAttribute.TaskSchedulerName;
            var interfaceLevelLocalExceptionHandlingMode =
                GetValueFromAttribute<LocalExceptionHandlingAttribute, LocalExceptionHandlingMode>(_sourceInterface,
                    i => i.LocalExceptionHandlingMode, out _, LocalExceptionHandlingMode.Redirect);
            var interfaceLevelOperatingTimeoutTimeAttribute =
                _sourceInterface.GetCustomAttribute<OperatingTimeoutTimeAttribute>();
            int interfaceLevelMethodCallingTimeout,
                interfaceLevelEventAddingTimeout,
                interfaceLevelEventRemovingTimeout,
                interfaceLevelEventRaisingTimeout,
                interfaceLevelPropertyGettingTimeout,
                interfaceLevelPropertySettingTimeout;
            if (interfaceLevelOperatingTimeoutTimeAttribute == null)
            {
                interfaceLevelMethodCallingTimeout = 0;
                interfaceLevelEventAddingTimeout = 0;
                interfaceLevelEventRemovingTimeout = 0;
                interfaceLevelEventRaisingTimeout = 0;
                interfaceLevelPropertyGettingTimeout = 0;
                interfaceLevelPropertySettingTimeout = 0;
            }
            else
            {
                interfaceLevelMethodCallingTimeout = interfaceLevelOperatingTimeoutTimeAttribute.Timeout;
                interfaceLevelEventAddingTimeout = interfaceLevelOperatingTimeoutTimeAttribute.EventAddingTimeout;
                interfaceLevelEventRemovingTimeout = interfaceLevelOperatingTimeoutTimeAttribute.EventRemovingTimeout;
                interfaceLevelEventRaisingTimeout = interfaceLevelOperatingTimeoutTimeAttribute.EventRaisingTimeout;
                interfaceLevelPropertyGettingTimeout = interfaceLevelOperatingTimeoutTimeAttribute.PropertyGettingTimeout;
                interfaceLevelPropertySettingTimeout = interfaceLevelOperatingTimeoutTimeAttribute.PropertySettingTimeout;
            }

            //NOTE: at this point, interface is pushed into parentPath.
            parentPath.Push(_sourceInterface);

            if (_serializerInterfaceLevelAttributeBaseType != null)
            {
                _result.SerializerInterfaceLevelAttributes =
                    _sourceInterface.GetCustomAttributes(_serializerInterfaceLevelAttributeBaseType, true).Cast<Attribute>().ToList();
            }

            _result.InterfaceLevelGenericArguments =
                ProcessGenericArgument(_sourceInterface.GetGenericArguments(), parentPath);

            //assets start
            var usedClassNames = new HashSet<string>
            {
                _result.ProxyTypeName, _result.ServiceWrapperTypeName
            };
            var usedAssetNames = new HashSet<string>();

            _result.Methods = new List<RemoteAgencyMethodInfo>();
            _result.Events = new List<RemoteAgencyEventInfo>();
            _result.Properties = new List<RemoteAgencyPropertyInfo>();

            //foreach asset: get ignore, one way, preset asset name and entity name
            ReadOriginalAsset(usedAssetNames, usedClassNames, parentPath);

            //foreach asset: auto naming
            AutoNaming(usedAssetNames, usedClassNames, parentPath);

            Task[] tasks = new Task[_result.Methods.Count + _result.Events.Count + _result.Properties.Count];
            int taskIndex = 0;

            //methods
            foreach (var method in _result.Methods)
            {
                var task = Task.Run(() => ProcessMethod(method, _sourceInterfaceTypeInfo,
                    interfaceLevelLocalExceptionHandlingMode, interfaceLevelMethodCallingTimeout));
                tasks[taskIndex++] = task;
            }

            //events
            foreach (var @event in _result.Events)
            {

                var task = Task.Run(() => ProcessEvent(@event, _sourceInterfaceTypeInfo,
                    interfaceLevelLocalExceptionHandlingMode, interfaceLevelEventAddingTimeout,
                    interfaceLevelEventRemovingTimeout, interfaceLevelEventRaisingTimeout));
                tasks[taskIndex++] = task;
            }

            //properties
            foreach (var property in _result.Properties)
            {
                var task = Task.Run(() => ProcessProperty(property, _sourceInterfaceTypeInfo,
                    interfaceLevelLocalExceptionHandlingMode, interfaceLevelPropertyGettingTimeout,
                    interfaceLevelPropertySettingTimeout));
                tasks[taskIndex++] = task;
            }

            Task.WaitAll(tasks);
        }

        private const string AutoNamePlaceHolder = "<<<AutoNameRequired>>>";

        //foreach asset: get ignore, one way, preset asset name and entity name
        void ReadOriginalAsset(HashSet<string> usedAssetNames, HashSet<string> usedClassNames,
            Stack<MemberInfo> parentPath)
        {
            foreach (var method in _sourceInterfaceTypeInfo.GetMethods())
            {
                var item = new RemoteAgencyMethodInfo
                {
                    AssetName = GetAssetNameSpecified(method, parentPath, usedAssetNames, AutoNamePlaceHolder),
                    Asset = method
                };

                if (GetValueFromAttribute<AssetIgnoredAttribute, bool>(method, i => i.IsIgnored, out var assetIgnored))
                {
                    item.IsIgnored = true;
                    item.WillThrowExceptionWhileCalling = assetIgnored.WillThrowException;
                }
                else
                {
                    item.IsOneWay =
                        GetValueFromAttribute<AssetOneWayOperatingAttribute, bool>(method, i => i.IsOneWay, out _);

                    var customizedEntityName = method.GetCustomAttribute<CustomizedMethodEntityNameAttribute>();
                    if (customizedEntityName != null)
                    {
                        if (!string.IsNullOrEmpty(customizedEntityName.ParameterEntityName))
                        {
                            if (!usedClassNames.Add(customizedEntityName.ParameterEntityName))
                            {
                                throw new EntityNameConflictException(
                                    $"The entity name specified for parameters conflicts with others.",
                                    customizedEntityName, method, parentPath);
                            }

                            item.ParameterEntityName = customizedEntityName.ParameterEntityName;
                        }
                        else
                        {
                            item.ParameterEntityName = AutoNamePlaceHolder;
                        }

                        if (!item.IsOneWay)
                        {
                            if (!string.IsNullOrEmpty(customizedEntityName.ReturnValueEntityName))
                            {
                                if (!usedClassNames.Add(customizedEntityName.ReturnValueEntityName))
                                {
                                    throw new EntityNameConflictException(
                                        $"The entity name specified for return values conflicts with others.",
                                        customizedEntityName, method, parentPath);
                                }

                                item.ReturnValueEntityName = customizedEntityName.ReturnValueEntityName;
                            }
                            else
                            {
                                item.ReturnValueEntityName = AutoNamePlaceHolder;
                            }
                        }
                    }
                }

                _result.Methods.Add(item);
            }

            foreach (var @event in _sourceInterfaceTypeInfo.GetEvents())
            {
                var item = new RemoteAgencyEventInfo()
                {
                    AssetName = GetAssetNameSpecified(@event, parentPath, usedAssetNames, AutoNamePlaceHolder),
                    Delegate = @event.EventHandlerType,
                    Asset = @event
                };

                if (GetValueFromAttribute<AssetIgnoredAttribute, bool>(@event, item.Delegate, i => i.IsIgnored,
                    out var assetIgnored))
                {
                    item.IsIgnored = true;
                    item.WillThrowExceptionWhileCalling = assetIgnored.WillThrowException;
                }
                else
                {
                    item.IsOneWay =
                        GetValueFromAttribute<AssetOneWayOperatingAttribute, bool>(@event, item.Delegate,
                            i => i.IsOneWay, out _);

                    var customizedEntityName = @event.GetCustomAttribute<CustomizedEventEntityNameAttribute>();
                    MemberInfo current = @event;
                    if (customizedEntityName == null)
                    {
                        customizedEntityName = item.Delegate.GetCustomAttribute<CustomizedEventEntityNameAttribute>();
                        if (customizedEntityName != null)
                        {
                            current = item.Delegate;
                            parentPath.Push(@event);
                        }
                    }

                    if (customizedEntityName != null)
                    {
                        if (!string.IsNullOrEmpty(customizedEntityName.AddingRequestEntityName))
                        {
                            if (!usedClassNames.Add(customizedEntityName.AddingRequestEntityName))
                            {
                                throw new EntityNameConflictException(
                                    $"The entity name specified for request of event adding conflicts with others.",
                                    customizedEntityName, current, parentPath);
                            }

                            item.AddingRequestEntityName = customizedEntityName.AddingRequestEntityName;
                        }
                        else
                        {
                            item.AddingRequestEntityName = AutoNamePlaceHolder;
                        }

                        if (!string.IsNullOrEmpty(customizedEntityName.AddingResponseEntityName))
                        {
                            if (!usedClassNames.Add(customizedEntityName.AddingResponseEntityName))
                            {
                                throw new EntityNameConflictException(
                                    $"The entity name specified for response of event adding conflicts with others.",
                                    customizedEntityName, current, parentPath);
                            }

                            item.AddingResponseEntityName = customizedEntityName.AddingResponseEntityName;
                        }
                        else
                        {
                            item.AddingResponseEntityName = AutoNamePlaceHolder;
                        }

                        if (!string.IsNullOrEmpty(customizedEntityName.RemovingRequestEntityName))
                        {
                            if (!usedClassNames.Add(customizedEntityName.RemovingRequestEntityName))
                            {
                                throw new EntityNameConflictException(
                                    $"The entity name specified for request of event removing conflicts with others.",
                                    customizedEntityName, current, parentPath);
                            }

                            item.RemovingRequestEntityName = customizedEntityName.RemovingRequestEntityName;
                        }
                        else
                        {
                            item.RemovingRequestEntityName = AutoNamePlaceHolder;
                        }

                        if (!string.IsNullOrEmpty(customizedEntityName.RemovingResponseEntityName))
                        {
                            if (!usedClassNames.Add(customizedEntityName.RemovingResponseEntityName))
                            {
                                throw new EntityNameConflictException(
                                    $"The entity name specified for response of event removing conflicts with others.",
                                    customizedEntityName, current, parentPath);
                            }

                            item.RemovingResponseEntityName = customizedEntityName.RemovingResponseEntityName;
                        }
                        else
                        {
                            item.RemovingResponseEntityName = AutoNamePlaceHolder;
                        }

                        if (!string.IsNullOrEmpty(customizedEntityName.RaisingNotificationEntityName))
                        {
                            if (!usedClassNames.Add(customizedEntityName.RaisingNotificationEntityName))
                            {
                                throw new EntityNameConflictException(
                                    $"The entity name specified for notification of event raising conflicts with others.",
                                    customizedEntityName, current, parentPath);
                            }

                            item.RaisingNotificationEntityName = customizedEntityName.RaisingNotificationEntityName;
                        }
                        else
                        {
                            item.RaisingNotificationEntityName = AutoNamePlaceHolder;
                        }

                        if (!item.IsOneWay)
                        {
                            if (!string.IsNullOrEmpty(customizedEntityName.RaisingFeedbackEntityName))
                            {
                                if (!usedClassNames.Add(customizedEntityName.RaisingFeedbackEntityName))
                                {
                                    throw new EntityNameConflictException(
                                        $"The entity name specified for feedback of event raising conflicts with others.",
                                        customizedEntityName, current, parentPath);
                                }

                                item.RaisingFeedbackEntityName = customizedEntityName.RaisingFeedbackEntityName;
                            }
                            else
                            {
                                item.RaisingFeedbackEntityName = AutoNamePlaceHolder;
                            }
                        }

                        if (current == item.Delegate)
                            parentPath.Pop();
                    }
                }

                _result.Events.Add(item);
            }

            foreach (var property in _sourceInterfaceTypeInfo.GetProperties())
            {
                var item = new RemoteAgencyPropertyInfo()
                {
                    AssetName = GetAssetNameSpecified(property, parentPath, usedAssetNames, AutoNamePlaceHolder),
                    Asset = property
                };

                if (GetValueFromAttribute<AssetIgnoredAttribute, bool>(property, i => i.IsIgnored, out var assetIgnored)
                )
                {
                    item.IsIgnored = true;
                    item.WillThrowExceptionWhileCalling = assetIgnored.WillThrowException;
                }
                else
                {
                    item.IsGettingOneWay =
                        GetValueFromAttribute<PropertyGetOneWayOperatingAttribute, bool>(property, i => i.IsOneWay,
                            out _);
                    item.IsSettingOneWay =
                        GetValueFromAttribute<AssetOneWayOperatingAttribute, bool>(property, i => i.IsOneWay, out _);

                    var customizedGetEntityName =
                        property.GetCustomAttribute<CustomizedPropertyGetEntityNameAttribute>();
                    if (customizedGetEntityName != null)
                    {
                        if (!string.IsNullOrEmpty(customizedGetEntityName.RequestEntityName))
                        {
                            if (!usedClassNames.Add(customizedGetEntityName.RequestEntityName))
                            {
                                throw new EntityNameConflictException(
                                    $"The entity name specified for request of getting conflicts with others.",
                                    customizedGetEntityName, property, parentPath);
                            }

                            item.GettingRequestEntityName = customizedGetEntityName.RequestEntityName;
                        }
                        else
                        {
                            item.GettingRequestEntityName = AutoNamePlaceHolder;
                        }

                        if (!item.IsOneWay)
                        {
                            if (!string.IsNullOrEmpty(customizedGetEntityName.ResponseEntityName))
                            {
                                if (!usedClassNames.Add(customizedGetEntityName.ResponseEntityName))
                                {
                                    throw new EntityNameConflictException(
                                        $"The entity name specified for response of getting conflicts with others.",
                                        customizedGetEntityName, property, parentPath);
                                }

                                item.GettingResponseEntityName = customizedGetEntityName.ResponseEntityName;
                            }
                            else
                            {
                                item.GettingResponseEntityName = AutoNamePlaceHolder;
                            }
                        }
                    }

                    var customizedSetEntityName =
                        property.GetCustomAttribute<CustomizedPropertySetEntityNameAttribute>();
                    if (customizedSetEntityName != null)
                    {
                        if (!string.IsNullOrEmpty(customizedSetEntityName.RequestEntityName))
                        {
                            if (!usedClassNames.Add(customizedSetEntityName.RequestEntityName))
                            {
                                throw new EntityNameConflictException(
                                    $"The entity name specified for request of setting conflicts with others.",
                                    customizedSetEntityName, property, parentPath);
                            }

                            item.SettingRequestEntityName = customizedSetEntityName.RequestEntityName;
                        }
                        else
                        {
                            item.SettingRequestEntityName = AutoNamePlaceHolder;
                        }

                        if (!item.IsOneWay)
                        {
                            if (!string.IsNullOrEmpty(customizedSetEntityName.ResponseEntityName))
                            {
                                if (!usedClassNames.Add(customizedSetEntityName.ResponseEntityName))
                                {
                                    throw new EntityNameConflictException(
                                        $"The entity name specified for response of setting conflicts with others.",
                                        customizedSetEntityName, property, parentPath);
                                }

                                item.SettingResponseEntityName = customizedSetEntityName.ResponseEntityName;
                            }
                            else
                            {
                                item.SettingResponseEntityName = AutoNamePlaceHolder;
                            }
                        }
                    }
                }

                _result.Properties.Add(item);
            }
        }

        //foreach asset: auto naming
        void AutoNaming(HashSet<string> usedAssetNames, HashSet<string> usedClassNames, Stack<MemberInfo> parentPath)
        {
            foreach (var method in _result.Methods)
            {
                if (method.AssetName == AutoNamePlaceHolder)
                {
                    method.AssetName = GetAssetAutoName(method.Asset.Name, usedAssetNames);
                }

                if (method.ParameterEntityName == AutoNamePlaceHolder)
                {
                    method.ParameterEntityName = GetEntityAutoName(_result.ClassNameBase, method.AssetName, "Parameter",
                        usedClassNames);
                }
                if (method.ReturnValueEntityName == AutoNamePlaceHolder)
                {
                    method.ReturnValueEntityName = GetEntityAutoName(_result.ClassNameBase, method.AssetName, "ReturnValue",
                        usedClassNames);
                }
            }

            foreach (var @event in _result.Events)
            {
                if (@event.AssetName == AutoNamePlaceHolder)
                {
                    @event.AssetName = GetAssetAutoName(@event.Asset.Name, usedAssetNames);
                }

                if (@event.AddingRequestEntityName == AutoNamePlaceHolder)
                {
                    @event.AddingRequestEntityName = GetEntityAutoName(_result.ClassNameBase, @event.AssetName, "AddingRequest",
                        usedClassNames);
                }
                if (@event.AddingResponseEntityName == AutoNamePlaceHolder)
                {
                    @event.AddingResponseEntityName = GetEntityAutoName(_result.ClassNameBase, @event.AssetName, "AddingResponse",
                        usedClassNames);
                }
                if (@event.RemovingRequestEntityName == AutoNamePlaceHolder)
                {
                    @event.RemovingRequestEntityName = GetEntityAutoName(_result.ClassNameBase, @event.AssetName, "RemovingRequest",
                        usedClassNames);
                }
                if (@event.RemovingResponseEntityName == AutoNamePlaceHolder)
                {
                    @event.RemovingResponseEntityName = GetEntityAutoName(_result.ClassNameBase, @event.AssetName, "RemovingResponse",
                        usedClassNames);
                }
                if (@event.RaisingNotificationEntityName == AutoNamePlaceHolder)
                {
                    @event.RaisingNotificationEntityName = GetEntityAutoName(_result.ClassNameBase, @event.AssetName, "RaisingNotification",
                        usedClassNames);
                }
                if (@event.RaisingFeedbackEntityName == AutoNamePlaceHolder)
                {
                    @event.RaisingFeedbackEntityName = GetEntityAutoName(_result.ClassNameBase, @event.AssetName, "RaisingFeedback",
                        usedClassNames);
                }
            }
           
            foreach (var property in _result.Properties)
            {
                if (property.AssetName == AutoNamePlaceHolder)
                {
                    property.AssetName = GetAssetAutoName(property.Asset.Name, usedAssetNames);
                }

                if (property.GettingRequestEntityName == AutoNamePlaceHolder)
                {
                    property.GettingRequestEntityName = GetEntityAutoName(_result.ClassNameBase, property.AssetName, "GettingRequest",
                        usedClassNames);
                }
                if (property.GettingResponseEntityName == AutoNamePlaceHolder)
                {
                    property.GettingResponseEntityName = GetEntityAutoName(_result.ClassNameBase, property.AssetName, "GettingResponse",
                        usedClassNames);
                }
                if (property.SettingRequestEntityName == AutoNamePlaceHolder)
                {
                    property.SettingRequestEntityName = GetEntityAutoName(_result.ClassNameBase, property.AssetName, "SettingRequest",
                        usedClassNames);
                }
                if (property.SettingResponseEntityName == AutoNamePlaceHolder)
                {
                    property.SettingResponseEntityName = GetEntityAutoName(_result.ClassNameBase, property.AssetName, "SettingResponse",
                        usedClassNames);
                }
            }
        }

        TValue GetValueFromAttribute<TAttribute, TValue>(ICustomAttributeProvider memberInfo, Func<TAttribute, TValue> selector, out TAttribute attribute,
            TValue defaultValue = default)
            where TAttribute : Attribute
        {
            attribute = memberInfo.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>().FirstOrDefault();
            if (attribute == null)
                return defaultValue;
            else
                return selector(attribute);
        }
        
        TValue GetValueFromAttribute<TAttribute, TValue>(ParameterInfo parameterInfo,
            Func<TAttribute, TValue> selector, out TAttribute attribute, Dictionary<string, TAttribute> overrides,
            TValue defaultValue = default)
            where TAttribute : Attribute
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            if (overrides == null || overrides.TryGetValue(parameterInfo.Name, out attribute))
                attribute = parameterInfo.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>()
                    .FirstOrDefault();
            if (attribute == null)
                return defaultValue;
            else
                return selector(attribute);
        }

        TValue GetValueFromAttribute<TAttribute, TValue>(EventInfo memberInfo, Type @delegate, Func<TAttribute, TValue> selector, out TAttribute attribute,
            TValue defaultValue = default)
            where TAttribute : Attribute
        {
            attribute = memberInfo.GetCustomAttribute<TAttribute>();
            if (attribute == null)
                attribute = @delegate.GetCustomAttribute<TAttribute>();
            if (attribute == null)
                return defaultValue;
            else
                return selector(attribute);
        }

        string GetAssetNameSpecified(MemberInfo memberInfo, Stack<MemberInfo> memberParentPath, HashSet<string> used, string defaultValue)
        {
            var attribute = memberInfo.GetCustomAttribute<CustomizedAssetNameAttribute>();
            if (attribute == null || string.IsNullOrEmpty(attribute.AssetName))
                return defaultValue;
            if (used.Contains(attribute.AssetName))
            {
                throw new AssetNameConflictException($"The asset name specified conflicts with others.",
                    attribute, memberInfo, memberParentPath);
            }
            used.Add(attribute.AssetName);
            return attribute.AssetName;
        }

        
        static string GetDefaultEntityTypeName(string classNameBase, string assetName, string usage)
        {
            return $"{classNameBase}_{assetName}_{usage}";
        }

        string GetEntityAutoName(string classNameBase, string assetName, string usage, HashSet<string> used)
            => GetAutoName(GetDefaultEntityTypeName(classNameBase, assetName, usage), used);

        string GetAssetAutoName(string assetName, HashSet<string> used)
            => GetAutoName(assetName, used);

        string GetPropertyAutoName(string name, HashSet<string> used)
        {
            if (char.IsLower(name[0]))
            {
                name = char.ToUpper(name[0]) + name.Substring(1);
            }
            return GetAutoName(name, used);
        }
            

        string GetAutoName(string nameBase, HashSet<string> used)
        {
            if (used.Contains(nameBase))
            {
                int index = 1;
                while (true)
                {
                    var name = $"{nameBase}_{index}";
                    if (used.Add(name))
                    {
                        return name;
                    }
                }
            }
            else
            {
                return nameBase;
            }
        }
    }
}