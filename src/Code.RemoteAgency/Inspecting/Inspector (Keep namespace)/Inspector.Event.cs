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
        void ProcessEvent(RemoteAgencyEventInfo @event, TypeInfo @interface,
            LocalExceptionHandlingMode interfaceLevelLocalExceptionHandlingMode,
            int interfaceLevelEventAddingTimeout, int interfaceLevelEventRemovingTimeout,
            int interfaceLevelEventRaisingTimeout)
        {
            Stack<MemberInfo> memberPath = new Stack<MemberInfo>();
            memberPath.Push(@interface);
            memberPath.Push(@event.Asset);

            EventInfo eventInfo = (EventInfo) @event.Asset;
            @event.LocalExceptionHandlingMode =
                GetValueFromAttribute<LocalExceptionHandlingAttribute, LocalExceptionHandlingMode>(eventInfo, @event.Delegate,
                    i => i.LocalExceptionHandlingMode, out _, interfaceLevelLocalExceptionHandlingMode);

            if (_serializerAssetLevelAttributeBaseType != null)
            {
                @event.SerializerAssetLevelAttributes =
                    eventInfo.GetCustomAttributes(_serializerAssetLevelAttributeBaseType, true).Cast<Attribute>().ToList();
            }

            if (_serializerDelegateLevelAttributeBaseType != null)
            {
                @event.SerializerDelegateLevelAttributes =
                    eventInfo.EventHandlerType.GetCustomAttributes(_serializerDelegateLevelAttributeBaseType, true).Cast<Attribute>().ToList();
            }

            //asset level pass through attributes
            if (_includesProxyOnlyInfo)
                @event.AssetLevelPassThroughAttributes = GetAttributePassThrough(eventInfo,
                    (m, a) => new InvalidAttributeDataException(m, a, memberPath));

            var raiseMethod = eventInfo.GetRaiseMethod();

            if (@event.IsIgnored)
            {
                ProcessMethodBodyForIgnoredAsset(raiseMethod, @event.WillThrowExceptionWhileCalling,
                    @event.RaisingMethodBodyInfo);
            }
            else
            {
                Dictionary<string, ParameterIgnoredAttribute> eventLevelParameterIgnoredAttributes =
                    new Dictionary<string, ParameterIgnoredAttribute>();
                foreach (var attribute in eventInfo.GetCustomAttributes<EventParameterIgnoredAttribute>())
                {
                    eventLevelParameterIgnoredAttributes.TryAdd(attribute.ParameterName, attribute);
                }

                Dictionary<string, CustomizedParameterEntityPropertyNameAttribute>
                    eventLevelParameterEntityPropertyNameAttributes =
                        new Dictionary<string, CustomizedParameterEntityPropertyNameAttribute>();
                foreach (var attribute in eventInfo.GetCustomAttributes<CustomizedEventParameterEntityPropertyNameAttribute>())
                {
                    eventLevelParameterEntityPropertyNameAttributes.TryAdd(attribute.ParameterName, attribute);
                }

                if (@event.IsOneWay)
                {
                    ProcessMethodBodyForOneWayAsset(raiseMethod, memberPath, _includesServiceWrapperOnlyInfo,
                        @event.RaisingMethodBodyInfo, eventLevelParameterIgnoredAttributes,
                        eventLevelParameterEntityPropertyNameAttributes);
                }
                else
                {
                    //normal
                    var delegateMethod = @event.Delegate.GetMethod("Invoke");
                    
                    var timeoutTime = GetValueFromAttribute<OperatingTimeoutTimeAttribute, OperatingTimeoutTimeAttribute>(
                        eventInfo, @event.Delegate,
                        i => i, out _);
                    @event.AddingMethodBodyInfo.Timeout = timeoutTime?.EventAddingTimeout ?? interfaceLevelEventAddingTimeout;
                    @event.RemovingMethodBodyInfo.Timeout = timeoutTime?.EventRemovingTimeout ?? interfaceLevelEventRemovingTimeout;
                    var eventRaisingTimeout = timeoutTime?.EventRaisingTimeout ?? interfaceLevelEventRaisingTimeout;

                    Dictionary<string, ParameterReturnRequiredAttribute> eventLevelParameterReturnRequiredAttributes =
                        new Dictionary<string, ParameterReturnRequiredAttribute>();
                    foreach (var attribute in eventInfo.GetCustomAttributes<EventParameterReturnRequiredAttribute>())
                    {
                        eventLevelParameterReturnRequiredAttributes.TryAdd(attribute.ParameterName, attribute);
                    }

                    var isReturnValueIgnored =
                        GetValueFromAttribute<ReturnIgnoredAttribute, bool>(eventInfo, i => i.IsIgnored,
                            out var returnIgnoredAttribute);
                    if (returnIgnoredAttribute == null)
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        isReturnValueIgnored = GetValueFromAttribute(delegateMethod.ReturnTypeCustomAttributes,
                            i => i.IsIgnored, out returnIgnoredAttribute);
                           
                    }
                    if (returnIgnoredAttribute == null)
                    {
                        isReturnValueIgnored =
                            GetValueFromAttribute(delegateMethod, i => i.IsIgnored, out returnIgnoredAttribute);
                    }

                    string returnValuePropertyNameSpecifiedByAttribute =
                        GetValueFromAttribute<CustomizedReturnValueEntityPropertyNameAttribute, string>(
                            eventInfo, i => i.EntityPropertyName,
                            out var customizedReturnValueEntityPropertyNameAttribute);
                    if (customizedReturnValueEntityPropertyNameAttribute == null)
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        returnValuePropertyNameSpecifiedByAttribute = GetValueFromAttribute(delegateMethod.ReturnTypeCustomAttributes,
                            i => i.EntityPropertyName, out customizedReturnValueEntityPropertyNameAttribute);
                    }
                    if (customizedReturnValueEntityPropertyNameAttribute == null)
                    {
                        returnValuePropertyNameSpecifiedByAttribute = GetValueFromAttribute(delegateMethod,
                            i => i.EntityPropertyName, out customizedReturnValueEntityPropertyNameAttribute);
                    }

                    ProcessMethodBodyForNormalAsset(raiseMethod, memberPath,
                        // ReSharper disable once PossibleNullReferenceException
                        eventRaisingTimeout, isReturnValueIgnored, returnValuePropertyNameSpecifiedByAttribute, customizedReturnValueEntityPropertyNameAttribute,
                        @event.RaisingMethodBodyInfo,
                        eventLevelParameterIgnoredAttributes, eventLevelParameterEntityPropertyNameAttributes, eventLevelParameterReturnRequiredAttributes);
                }
            }

        }
    }
}