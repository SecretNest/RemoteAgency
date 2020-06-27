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
        private readonly bool _includeAttributePassThrough;

        public Inspector(Type sourceInterface, bool includeAttributePassThrough)
        {
            _sourceInterface = sourceInterface;
            _sourceInterfaceTypeInfo = sourceInterface.GetTypeInfo();
            _result = new RemoteAgencyInterfaceInfo();
            _includeAttributePassThrough = includeAttributePassThrough;
            SetInterfaceTypeBasicInfo(_result, _sourceInterface, _sourceInterfaceTypeInfo);
        }

        public RemoteAgencyInterfaceInfo InterfaceTypeInfo => _result;

        public void Process()
        {
            //interface level
            Stack<MemberInfo> parentPath = new Stack<MemberInfo>();
            if (_includeAttributePassThrough)
                _result.InterfaceLevelPassThroughAttributes =
                    GetAttributePassThrough(_sourceInterfaceTypeInfo, parentPath);
            _result.InterfaceLevelGenericParameters =
                ProcessGenericParameter(_sourceInterfaceTypeInfo, _sourceInterface.GetGenericArguments(), parentPath);

            //assets start
            parentPath.Push(_sourceInterface);
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
                var task = Task.Run(() => ProcessMethod(method, _sourceInterfaceTypeInfo));
                tasks[taskIndex++] = task;
            }

            //events
            foreach (var @event in _result.Events)
            {
                var task = Task.Run(() => ProcessEvent(@event, _sourceInterfaceTypeInfo));
                tasks[taskIndex++] = task;
            }

            //properties
            foreach (var property in _result.Properties)
            {
                var task = Task.Run(() => ProcessProperty(property, _sourceInterfaceTypeInfo));
                tasks[taskIndex++] = task;
            }

            Task.WaitAll(tasks);
        }

        //foreach asset: get ignore, one way, preset asset name and entity name
        void ReadOriginalAsset(HashSet<string> usedAssetNames, HashSet<string> usedClassNames, Stack<MemberInfo> parentPath)
        {
            foreach (var method in _sourceInterfaceTypeInfo.GetMethods())
            {
                var item = new RemoteAgencyMethodInfo
                {
                    AssetName = GetAssetNameSpecified(method, parentPath, usedAssetNames),
                    Asset = method
                };

                if (GetValueFromAttribute<AssetIgnoredAttribute, bool>(method, i => i.IsIgnored, out var assetIgnored))
                {
                    item.IsIgnored = true;
                    item.WillThrowException = assetIgnored.WillThrowException;
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
                            if (usedClassNames.Contains(customizedEntityName.ParameterEntityName))
                            {
                                throw new EntityNameConflictException(
                                    $"The entity name specified for parameters conflicts with others.",
                                    customizedEntityName, method, parentPath);
                            }

                            usedClassNames.Add(customizedEntityName.ParameterEntityName);
                            item.ParameterEntityName = customizedEntityName.ParameterEntityName;
                        }

                        if (!item.IsOneWay && !string.IsNullOrEmpty(customizedEntityName.ReturnValueEntityName))
                        {
                            if (usedClassNames.Contains(customizedEntityName.ReturnValueEntityName))
                            {
                                throw new EntityNameConflictException(
                                    $"The entity name specified for return values conflicts with others.",
                                    customizedEntityName, method, parentPath);
                            }

                            usedClassNames.Add(customizedEntityName.ReturnValueEntityName);
                            item.ReturnValueEntityName = customizedEntityName.ReturnValueEntityName;
                        }
                    }
                }

                _result.Methods.Add(item);
            }

            foreach (var @event in _sourceInterfaceTypeInfo.GetEvents())
            {
                var item = new RemoteAgencyEventInfo()
                {
                    AssetName = GetAssetNameSpecified(@event, parentPath, usedAssetNames),
                    Delegate = @event.EventHandlerType,
                    Asset = @event
                };

                if (GetValueFromAttribute<AssetIgnoredAttribute, bool>(@event, item.Delegate, i => i.IsIgnored,
                    out var assetIgnored))
                {
                    item.IsIgnored = true;
                    item.WillThrowException = assetIgnored.WillThrowException;
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
                            if (usedClassNames.Contains(customizedEntityName.AddingRequestEntityName))
                            {
                                throw new EntityNameConflictException(
                                    $"The entity name specified for request of event adding conflicts with others.",
                                    customizedEntityName, current, parentPath);
                            }

                            usedClassNames.Add(customizedEntityName.AddingRequestEntityName);
                            item.AddingRequestEntityName = customizedEntityName.AddingRequestEntityName;
                        }

                        if (!string.IsNullOrEmpty(customizedEntityName.AddingResponseEntityName))
                        {
                            if (usedClassNames.Contains(customizedEntityName.AddingResponseEntityName))
                            {
                                throw new EntityNameConflictException(
                                    $"The entity name specified for response of event adding conflicts with others.",
                                    customizedEntityName, current, parentPath);
                            }

                            usedClassNames.Add(customizedEntityName.AddingResponseEntityName);
                            item.AddingResponseEntityName = customizedEntityName.AddingResponseEntityName;
                        }

                        if (!string.IsNullOrEmpty(customizedEntityName.RemovingRequestEntityName))
                        {
                            if (usedClassNames.Contains(customizedEntityName.RemovingRequestEntityName))
                            {
                                throw new EntityNameConflictException(
                                    $"The entity name specified for request of event removing conflicts with others.",
                                    customizedEntityName, current, parentPath);
                            }

                            usedClassNames.Add(customizedEntityName.RemovingRequestEntityName);
                            item.RemovingRequestEntityName = customizedEntityName.RemovingRequestEntityName;
                        }

                        if (!string.IsNullOrEmpty(customizedEntityName.RemovingResponseEntityName))
                        {
                            if (usedClassNames.Contains(customizedEntityName.RemovingResponseEntityName))
                            {
                                throw new EntityNameConflictException(
                                    $"The entity name specified for response of event removing conflicts with others.",
                                    customizedEntityName, current, parentPath);
                            }

                            usedClassNames.Add(customizedEntityName.RemovingResponseEntityName);
                            item.RemovingResponseEntityName = customizedEntityName.RemovingResponseEntityName;
                        }

                        if (!string.IsNullOrEmpty(customizedEntityName.RaisingNotificationEntityName))
                        {
                            if (usedClassNames.Contains(customizedEntityName.RaisingNotificationEntityName))
                            {
                                throw new EntityNameConflictException(
                                    $"The entity name specified for notification of event raising conflicts with others.",
                                    customizedEntityName, current, parentPath);
                            }

                            usedClassNames.Add(customizedEntityName.RaisingNotificationEntityName);
                            item.RaisingNotificationEntityName = customizedEntityName.RaisingNotificationEntityName;
                        }

                        if (!item.IsOneWay && !string.IsNullOrEmpty(customizedEntityName.RaisingFeedbackEntityName))
                        {
                            if (usedClassNames.Contains(customizedEntityName.RaisingFeedbackEntityName))
                            {
                                throw new EntityNameConflictException(
                                    $"The entity name specified for feedback of event raising conflicts with others.",
                                    customizedEntityName, current, parentPath);
                            }

                            usedClassNames.Add(customizedEntityName.RaisingFeedbackEntityName);
                            item.RaisingFeedbackEntityName = customizedEntityName.RaisingFeedbackEntityName;
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
                    AssetName = GetAssetNameSpecified(property, parentPath, usedAssetNames),
                    Asset = property
                };

                if (GetValueFromAttribute<AssetIgnoredAttribute, bool>(property, i => i.IsIgnored, out var assetIgnored))
                {
                    item.IsIgnored = true;
                    item.WillThrowException = assetIgnored.WillThrowException;
                }
                else
                {
                    item.IsGettingOneWay =
                        GetValueFromAttribute<PropertyGetOneWayOperatingAttribute, bool>(property, i => i.IsOneWay, out _);
                    item.IsSettingOneWay =
                        GetValueFromAttribute<AssetOneWayOperatingAttribute, bool>(property, i => i.IsOneWay, out _);

                    var customizedGetEntityName = property.GetCustomAttribute<CustomizedPropertyGetEntityNameAttribute>();
                    if (customizedGetEntityName != null)
                    {
                        if (!string.IsNullOrEmpty(customizedGetEntityName.RequestEntityName))
                        {
                            if (usedClassNames.Contains(customizedGetEntityName.RequestEntityName))
                            {
                                throw new EntityNameConflictException(
                                    $"The entity name specified for request of getting conflicts with others.",
                                    customizedGetEntityName, property, parentPath);
                            }

                            usedClassNames.Add(customizedGetEntityName.RequestEntityName);
                            item.GettingRequestEntityName = customizedGetEntityName.RequestEntityName;
                        }

                        if (!item.IsOneWay && !string.IsNullOrEmpty(customizedGetEntityName.ResponseEntityName))
                        {
                            if (usedClassNames.Contains(customizedGetEntityName.ResponseEntityName))
                            {
                                throw new EntityNameConflictException(
                                    $"The entity name specified for response of getting conflicts with others.",
                                    customizedGetEntityName, property, parentPath);
                            }

                            usedClassNames.Add(customizedGetEntityName.ResponseEntityName);
                            item.GettingResponseEntityName = customizedGetEntityName.ResponseEntityName;
                        }
                    }

                    var customizedSetEntityName = property.GetCustomAttribute<CustomizedPropertySetEntityNameAttribute>();
                    if (customizedSetEntityName != null)
                    {
                        if (!string.IsNullOrEmpty(customizedSetEntityName.RequestEntityName))
                        {
                            if (usedClassNames.Contains(customizedSetEntityName.RequestEntityName))
                            {
                                throw new EntityNameConflictException(
                                    $"The entity name specified for request of setting conflicts with others.",
                                    customizedSetEntityName, property, parentPath);
                            }

                            usedClassNames.Add(customizedSetEntityName.RequestEntityName);
                            item.SettingRequestEntityName = customizedSetEntityName.RequestEntityName;
                        }

                        if (!item.IsOneWay && !string.IsNullOrEmpty(customizedSetEntityName.ResponseEntityName))
                        {
                            if (usedClassNames.Contains(customizedSetEntityName.ResponseEntityName))
                            {
                                throw new EntityNameConflictException(
                                    $"The entity name specified for response of setting conflicts with others.",
                                    customizedSetEntityName, property, parentPath);
                            }

                            usedClassNames.Add(customizedSetEntityName.ResponseEntityName);
                            item.SettingResponseEntityName = customizedSetEntityName.ResponseEntityName;
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
                if (string.IsNullOrEmpty(method.AssetName))
                {
                    method.AssetName = GetAssetAutoName(method.Asset.Name, usedAssetNames);
                }

                if (string.IsNullOrEmpty(method.ParameterEntityName))
                {
                    method.ParameterEntityName = GetEntityAutoName(_result.ClassNameBase, method.AssetName, "Parameter",
                        usedClassNames);
                }
                if (string.IsNullOrEmpty(method.ReturnValueEntityName))
                {
                    method.ReturnValueEntityName = GetEntityAutoName(_result.ClassNameBase, method.AssetName, "ReturnValue",
                        usedClassNames);
                }
            }

            foreach (var @event in _result.Events)
            {
                if (string.IsNullOrEmpty(@event.AssetName))
                {
                    @event.AssetName = GetAssetAutoName(@event.Asset.Name, usedAssetNames);
                }

                if (string.IsNullOrEmpty(@event.AddingRequestEntityName))
                {
                    @event.AddingRequestEntityName = GetEntityAutoName(_result.ClassNameBase, @event.AssetName, "AddingRequest",
                        usedClassNames);
                }
                if (string.IsNullOrEmpty(@event.AddingResponseEntityName))
                {
                    @event.AddingResponseEntityName = GetEntityAutoName(_result.ClassNameBase, @event.AssetName, "AddingResponse",
                        usedClassNames);
                }
                if (string.IsNullOrEmpty(@event.RemovingRequestEntityName))
                {
                    @event.RemovingRequestEntityName = GetEntityAutoName(_result.ClassNameBase, @event.AssetName, "RemovingRequest",
                        usedClassNames);
                }
                if (string.IsNullOrEmpty(@event.RemovingResponseEntityName))
                {
                    @event.RemovingResponseEntityName = GetEntityAutoName(_result.ClassNameBase, @event.AssetName, "RemovingResponse",
                        usedClassNames);
                }
                if (string.IsNullOrEmpty(@event.RaisingNotificationEntityName))
                {
                    @event.RaisingNotificationEntityName = GetEntityAutoName(_result.ClassNameBase, @event.AssetName, "RaisingNotification",
                        usedClassNames);
                }
                if (string.IsNullOrEmpty(@event.RaisingFeedbackEntityName))
                {
                    @event.RaisingFeedbackEntityName = GetEntityAutoName(_result.ClassNameBase, @event.AssetName, "RaisingFeedback",
                        usedClassNames);
                }
            }
           
            foreach (var property in _result.Properties)
            {
                if (string.IsNullOrEmpty(property.AssetName))
                {
                    property.AssetName = GetAssetAutoName(property.Asset.Name, usedAssetNames);
                }

                if (string.IsNullOrEmpty(property.GettingRequestEntityName))
                {
                    property.GettingRequestEntityName = GetEntityAutoName(_result.ClassNameBase, property.AssetName, "GettingRequest",
                        usedClassNames);
                }
                if (string.IsNullOrEmpty(property.GettingResponseEntityName))
                {
                    property.GettingResponseEntityName = GetEntityAutoName(_result.ClassNameBase, property.AssetName, "GettingResponse",
                        usedClassNames);
                }
                if (string.IsNullOrEmpty(property.SettingRequestEntityName))
                {
                    property.SettingRequestEntityName = GetEntityAutoName(_result.ClassNameBase, property.AssetName, "SettingRequest",
                        usedClassNames);
                }
                if (string.IsNullOrEmpty(property.SettingResponseEntityName))
                {
                    property.SettingResponseEntityName = GetEntityAutoName(_result.ClassNameBase, property.AssetName, "SettingResponse",
                        usedClassNames);
                }
            }
        }

        TValue GetValueFromAttribute<TAttribute, TValue>(MemberInfo memberInfo, Func<TAttribute, TValue> selector, out TAttribute attribute,
            TValue defaultValue = default)
            where TAttribute : Attribute
        {
            attribute = memberInfo.GetCustomAttribute<TAttribute>();
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

        string GetAssetNameSpecified(MemberInfo memberInfo, Stack<MemberInfo> memberParentPath, HashSet<string> used)
        {
            var attribute = memberInfo.GetCustomAttribute<CustomizedAssetNameAttribute>();
            if (attribute == null || string.IsNullOrEmpty(attribute.AssetName))
                return null;
            if (used.Contains(attribute.AssetName))
            {
                throw new AssetNameConflictException($"The asset name specified conflicts with others.",
                    attribute, memberInfo, memberParentPath);
            }
            used.Add(attribute.AssetName);
            return attribute.AssetName;
        }

        string GetEntityAutoName(string classNameBase, string assetName, string usage, HashSet<string> used)
            => GetAutoName(GetDefaultEntityTypeName(classNameBase, assetName, usage), used);

        string GetAssetAutoName(string assetName, HashSet<string> used)
            => GetAutoName(assetName, used);

        string GetPropertyAutoName(string propertyName, HashSet<string> used)
            => GetAutoName(propertyName, used);

        string GetAutoName(string nameBase, HashSet<string> used)
        {
            if (used.Contains(nameBase))
            {
                int index = 1;
                while (true)
                {
                    var name = $"{nameBase}_{index}";
                    if (!used.Contains(name))
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