using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency.Inspecting
{
    partial class Inspector
    {
        private Type _sourceInterface;
        private TypeInfo _sourceInterfaceTypeInfo;
        private RemoteAgencyInterfaceInfo _result;

        public Inspector(Type sourceInterface)
        {
            _sourceInterface = sourceInterface;
            _sourceInterfaceTypeInfo = sourceInterface.GetTypeInfo();
            _result = new RemoteAgencyInterfaceInfo();
            SetInterfaceTypeBasicInfo(_result, _sourceInterface, _sourceInterfaceTypeInfo);
        }

        public RemoteAgencyInterfaceInfo InterfaceTypeInfo => _result;

        public void Process(bool includeAttributePassThrough)
        {
            //interface level
            Stack<MemberInfo> parentPath = new Stack<MemberInfo>();
            if (includeAttributePassThrough)
                _result.InterfaceLevelPassThroughAttributes =
                    GetAttributePassThrough(_sourceInterfaceTypeInfo, parentPath);
            _result.InterfaceLevelGenericParameters =
                ProcessGenericParameter(_sourceInterface, parentPath, includeAttributePassThrough);

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

            foreach (var method in _sourceInterfaceTypeInfo.GetMethods())
            {
                var item = new RemoteAgencyMethodInfo
                {
                    AssetName = GetAssetNameSpecified(method, parentPath, usedAssetNames)
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
                    Delegate = @event.EventHandlerType
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
                    AssetName = GetAssetNameSpecified(property, parentPath, usedAssetNames)
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

            //autoname



            //methods



            //events


            //properties



            //
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
    }
}