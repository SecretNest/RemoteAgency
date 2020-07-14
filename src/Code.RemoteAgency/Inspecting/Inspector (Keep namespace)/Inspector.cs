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
        private readonly TypeInfo _sourceInterfaceTypeInfo;
        private readonly RemoteAgencyInterfaceInfo _result;
        private readonly bool _includesProxyOnlyInfo;
        private readonly bool _includesServiceWrapperOnlyInfo;

        private readonly Type _serializerInterfaceLevelAttributeBaseType;
        private readonly Type _serializerAssetLevelAttributeBaseType;
        private readonly Type _serializerDelegateLevelAttributeBaseType;
        private readonly Type _serializerParameterLevelAttributeBaseType;

        public Inspector(RemoteAgencyInterfaceBasicInfo basicInfo, bool includesProxyOnlyInfo, bool includesServiceWrapperOnlyInfo, Type serializerInterfaceLevelAttributeBaseType, Type serializerAssetLevelAttributeBaseType, Type serializerDelegateLevelAttributeBaseType, Type serializerParameterLevelAttributeBaseType)
        {
            _sourceInterfaceTypeInfo = _result.SourceInterface.GetTypeInfo();
            _result = new RemoteAgencyInterfaceInfo(basicInfo);
            _includesProxyOnlyInfo = includesProxyOnlyInfo;
            _includesServiceWrapperOnlyInfo = includesServiceWrapperOnlyInfo;
            _serializerInterfaceLevelAttributeBaseType = serializerInterfaceLevelAttributeBaseType;
            _serializerAssetLevelAttributeBaseType = serializerAssetLevelAttributeBaseType;
            _serializerDelegateLevelAttributeBaseType = serializerDelegateLevelAttributeBaseType;
            _serializerParameterLevelAttributeBaseType = serializerParameterLevelAttributeBaseType;
        }

        public RemoteAgencyInterfaceInfo InterfaceTypeInfo => _result;

        public void Process()
        {
            //interface level
            Stack<MemberInfo> memberPath = new Stack<MemberInfo>();
            memberPath.Push(_result.SourceInterface);

            if (_includesProxyOnlyInfo)
                _result.InterfaceLevelPassThroughAttributes =
                    GetAttributePassThrough(_sourceInterfaceTypeInfo,
                        (m, a) => new InvalidAttributeDataException(m, a, memberPath));

            var interfaceLevelLocalExceptionHandlingMode =
                GetValueFromAttribute<LocalExceptionHandlingAttribute, LocalExceptionHandlingMode>(_result.SourceInterface,
                    i => i.LocalExceptionHandlingMode, out _, LocalExceptionHandlingMode.Redirect);
            var interfaceLevelOperatingTimeoutTimeAttribute =
                _result.SourceInterface.GetCustomAttribute<OperatingTimeoutTimeAttribute>();
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

            if (_serializerInterfaceLevelAttributeBaseType != null)
            {
                _result.SerializerInterfaceLevelAttributes =
                    _result.SourceInterface.GetCustomAttributes(_serializerInterfaceLevelAttributeBaseType, true).Cast<Attribute>().ToList();
            }

            _result.InterfaceLevelGenericParameters = _result.SourceInterface.GetGenericArguments();
            _result.InterfaceLevelGenericParameterPassThroughAttributes =
                FillAttributePassThroughOnGenericParameters(_result.InterfaceLevelGenericParameters,
                    (m, a, t) => new InvalidAttributeDataException(m, a, t, memberPath));

            //assets start
            var usedClassNames = new HashSet<string>
            {
                _result.ProxyTypeName, _result.ServiceWrapperTypeName
            };
            var usedAssetNames = new HashSet<string>();

            _result.Methods = new List<RemoteAgencyMethodInfo>();
            _result.Events = new List<RemoteAgencyEventInfo>();
            _result.Properties = new List<RemoteAgencyPropertyInfo>();

            //foreach asset: get ignore, one-way, preset asset name and entity name
            ReadOriginalAsset(usedAssetNames, usedClassNames, memberPath);

            //foreach asset: auto naming
            AutoNaming(usedAssetNames, usedClassNames, memberPath);

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

        //foreach asset: get ignore, one-way, preset asset name and entity name
        void ReadOriginalAsset(HashSet<string> usedAssetNames, HashSet<string> usedClassNames,
            Stack<MemberInfo> parentPath)
        {
            foreach (var method in _sourceInterfaceTypeInfo.GetMethods())
            {
                var item = new RemoteAgencyMethodInfo
                {
                    AssetName = GetAssetNameSpecified(method, parentPath, usedAssetNames, AutoNamePlaceHolder),
                    Asset = method,
                    MethodBodyInfo = new RemoteAgencyMethodBodyInfo()
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

                            item.MethodBodyInfo.ParameterEntityName = customizedEntityName.ParameterEntityName;
                        }
                        else
                        {
                            item.MethodBodyInfo.ParameterEntityName = AutoNamePlaceHolder;
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

                                item.MethodBodyInfo.ReturnValueEntityName = customizedEntityName.ReturnValueEntityName;
                            }
                            else
                            {
                                item.MethodBodyInfo.ReturnValueEntityName = AutoNamePlaceHolder;
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
                    Asset = @event,
                    RaisingMethodBodyInfo = new RemoteAgencyMethodBodyInfo(),
                    AddingMethodBodyInfo = new RemoteAgencySimpleMethodBodyInfo(),
                    RemovingMethodBodyInfo = new RemoteAgencySimpleMethodBodyInfo()
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
                        if (!string.IsNullOrEmpty(customizedEntityName.RaisingNotificationEntityName))
                        {
                            if (!usedClassNames.Add(customizedEntityName.RaisingNotificationEntityName))
                            {
                                throw new EntityNameConflictException(
                                    $"The entity name specified for notification of event raising conflicts with others.",
                                    customizedEntityName, current, parentPath);
                            }

                            item.RaisingMethodBodyInfo.ParameterEntityName = customizedEntityName.RaisingNotificationEntityName;
                        }
                        else
                        {
                            item.RaisingMethodBodyInfo.ParameterEntityName = AutoNamePlaceHolder;
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

                                item.RaisingMethodBodyInfo.ReturnValueEntityName = customizedEntityName.RaisingFeedbackEntityName;
                            }
                            else
                            {
                                item.RaisingMethodBodyInfo.ReturnValueEntityName = AutoNamePlaceHolder;
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
                    Asset = property,
                    GettingMethodBodyInfo = new RemoteAgencyMethodBodyInfo(),
                    SettingMethodBodyInfo = new RemoteAgencyMethodBodyInfo()
                };

                if (GetValueFromAttribute<AssetIgnoredAttribute, bool>(property, i => i.IsIgnored, out var assetIgnored))
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

                            item.GettingMethodBodyInfo.ParameterEntityName = customizedGetEntityName.RequestEntityName;
                        }
                        else
                        {
                            item.GettingMethodBodyInfo.ParameterEntityName = AutoNamePlaceHolder;
                        }

                        if (!item.IsGettingOneWay)
                        {
                            if (!string.IsNullOrEmpty(customizedGetEntityName.ResponseEntityName))
                            {
                                if (!usedClassNames.Add(customizedGetEntityName.ResponseEntityName))
                                {
                                    throw new EntityNameConflictException(
                                        $"The entity name specified for response of getting conflicts with others.",
                                        customizedGetEntityName, property, parentPath);
                                }

                                item.GettingMethodBodyInfo.ReturnValueEntityName = customizedGetEntityName.ResponseEntityName;
                            }
                            else
                            {
                                item.GettingMethodBodyInfo.ReturnValueEntityName = AutoNamePlaceHolder;
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

                            item.SettingMethodBodyInfo.ParameterEntityName = customizedSetEntityName.RequestEntityName;
                        }
                        else
                        {
                            item.SettingMethodBodyInfo.ParameterEntityName = AutoNamePlaceHolder;
                        }

                        if (!item.IsSettingOneWay)
                        {
                            if (!string.IsNullOrEmpty(customizedSetEntityName.ResponseEntityName))
                            {
                                if (!usedClassNames.Add(customizedSetEntityName.ResponseEntityName))
                                {
                                    throw new EntityNameConflictException(
                                        $"The entity name specified for response of setting conflicts with others.",
                                        customizedSetEntityName, property, parentPath);
                                }

                                item.SettingMethodBodyInfo.ReturnValueEntityName = customizedSetEntityName.ResponseEntityName;
                            }
                            else
                            {
                                item.SettingMethodBodyInfo.ReturnValueEntityName = AutoNamePlaceHolder;
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

                if (method.MethodBodyInfo.ParameterEntityName == AutoNamePlaceHolder)
                {
                    method.MethodBodyInfo.ParameterEntityName = GetEntityAutoName(_result.ClassNameBase, method.AssetName, "Parameter",
                        usedClassNames);
                }
                if (method.MethodBodyInfo.ReturnValueEntityName == AutoNamePlaceHolder)
                {
                    method.MethodBodyInfo.ReturnValueEntityName = GetEntityAutoName(_result.ClassNameBase, method.AssetName, "ReturnValue",
                        usedClassNames);
                }
            }

            foreach (var @event in _result.Events)
            {
                if (@event.AssetName == AutoNamePlaceHolder)
                {
                    @event.AssetName = GetAssetAutoName(@event.Asset.Name, usedAssetNames);
                }

                if (@event.RaisingMethodBodyInfo.ParameterEntityName == AutoNamePlaceHolder)
                {
                    @event.RaisingMethodBodyInfo.ParameterEntityName = GetEntityAutoName(_result.ClassNameBase, @event.AssetName, "RaisingNotification",
                        usedClassNames);
                }
                if (@event.RaisingMethodBodyInfo.ReturnValueEntityName == AutoNamePlaceHolder)
                {
                    @event.RaisingMethodBodyInfo.ReturnValueEntityName = GetEntityAutoName(_result.ClassNameBase, @event.AssetName, "RaisingFeedback",
                        usedClassNames);
                }
            }
           
            foreach (var property in _result.Properties)
            {
                if (property.AssetName == AutoNamePlaceHolder)
                {
                    property.AssetName = GetAssetAutoName(property.Asset.Name, usedAssetNames);
                }

                if (property.GettingMethodBodyInfo.ParameterEntityName == AutoNamePlaceHolder)
                {
                    property.GettingMethodBodyInfo.ParameterEntityName = GetEntityAutoName(_result.ClassNameBase, property.AssetName, "GettingRequest",
                        usedClassNames);
                }
                if (property.GettingMethodBodyInfo.ReturnValueEntityName == AutoNamePlaceHolder)
                {
                    property.GettingMethodBodyInfo.ReturnValueEntityName = GetEntityAutoName(_result.ClassNameBase, property.AssetName, "GettingResponse",
                        usedClassNames);
                }
                if (property.SettingMethodBodyInfo.ParameterEntityName == AutoNamePlaceHolder)
                {
                    property.SettingMethodBodyInfo.ParameterEntityName = GetEntityAutoName(_result.ClassNameBase, property.AssetName, "SettingRequest",
                        usedClassNames);
                }
                if (property.SettingMethodBodyInfo.ReturnValueEntityName == AutoNamePlaceHolder)
                {
                    property.SettingMethodBodyInfo.ReturnValueEntityName = GetEntityAutoName(_result.ClassNameBase, property.AssetName, "SettingResponse",
                        usedClassNames);
                }
            }
        }

        static TValue GetValueFromAttribute<TAttribute, TValue>(ICustomAttributeProvider memberInfo, Func<TAttribute, TValue> selector, out TAttribute attribute,
            TValue defaultValue = default)
            where TAttribute : Attribute
        {
            attribute = memberInfo.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>().FirstOrDefault();
            if (attribute == null)
                return defaultValue;
            else
                return selector(attribute);
        }
        
        static TValue GetValueFromAttribute<TAttribute, TValue>(ParameterInfo parameterInfo,
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

        static TValue GetValueFromAttribute<TAttribute, TValue>(EventInfo memberInfo, Type @delegate, Func<TAttribute, TValue> selector, out TAttribute attribute,
            TValue defaultValue = default)
            where TAttribute : Attribute
        {
            attribute = memberInfo.GetCustomAttribute<TAttribute>();
            if (attribute == null)
            {
                return GetValueFromAttribute(@delegate, selector, out attribute, defaultValue);
            }
            else
            {
                var value = selector(attribute);
                if (value.Equals(defaultValue))
                {
                    return GetValueFromAttribute(@delegate, selector, out attribute, defaultValue);
                }
                else
                {
                    return value;
                }
            }
        }



        static string GetAssetNameSpecified(MemberInfo memberInfo, Stack<MemberInfo> memberParentPath, HashSet<string> used, string defaultValue)
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

        static   string GetEntityAutoName(string classNameBase, string assetName, string usage, HashSet<string> used)
            => GetAutoName(GetDefaultEntityTypeName(classNameBase, assetName, usage), used);

        static string GetAssetAutoName(string assetName, HashSet<string> used)
            => GetAutoName(assetName, used);

        static string GetPropertyAutoName(string name, HashSet<string> used)
        {
            if (char.IsLower(name[0]))
            {
                name = char.ToUpper(name[0]) + name.Substring(1);
            }
            return GetAutoName(name, used);
        }
            
        static string GetAutoName(string nameBase, HashSet<string> used)
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