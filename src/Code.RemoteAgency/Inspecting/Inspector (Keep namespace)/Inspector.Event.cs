using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
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
            var memberPath = new Stack<MemberInfo>();
            memberPath.Push(@interface);
            memberPath.Push(@event.Asset);

            var eventInfo = (EventInfo) @event.Asset;
            @event.LocalExceptionHandlingMode =
                eventInfo.GetValueFromAttribute<LocalExceptionHandlingAttribute, LocalExceptionHandlingMode>(@event.Delegate,
                    i => i.LocalExceptionHandlingMode, out _, interfaceLevelLocalExceptionHandlingMode);

            if (_serializerAssetLevelAttributeBaseType != null)
            {
                @event.SerializerAssetLevelAttributes =
                    eventInfo.GetCustomAttributes(_serializerAssetLevelAttributeBaseType, true).Cast<Attribute>().ToList();
            }

            if (_serializerDelegateLevelAttributeBaseType != null)
            {
                @event.SerializerDelegateLevelAttributes =
                    eventInfo.EventHandlerType!.GetCustomAttributes(_serializerDelegateLevelAttributeBaseType, true).Cast<Attribute>().ToList();
            }

            //asset level pass through attributes
            @event.AssetLevelPassThroughAttributes = _includesProxyOnlyInfo
                ? eventInfo.GetAttributePassThrough((m, a) => new InvalidAttributeDataException(m, a, memberPath))
                : new List<CustomAttributeBuilder>();

            var raiseMethod = eventInfo.GetRaiseMethod();

            if (@event.IsIgnored)
            {
                ProcessMethodBodyForIgnoredAsset(raiseMethod, raiseMethod!.ReturnType, @event.WillThrowExceptionWhileCalling,
                    @event.RaisingMethodBodyInfo, AsyncMethodOriginalReturnValueDataTypeClass.NotAsyncMethod);
            }
            else
            {
                var eventLevelParameterIgnoredAttributes =
                    new Dictionary<string, ParameterIgnoredAttribute>();
                foreach (var attribute in eventInfo.GetCustomAttributes<EventParameterIgnoredAttribute>())
                {
                    eventLevelParameterIgnoredAttributes.TryAdd(attribute.ParameterName, attribute);
                }

                var eventLevelParameterEntityPropertyNameAttributes =
                    new Dictionary<string, CustomizedParameterEntityPropertyNameAttribute>();
                foreach (var attribute in eventInfo.GetCustomAttributes<CustomizedEventParameterEntityPropertyNameAttribute>())
                {
                    eventLevelParameterEntityPropertyNameAttributes.TryAdd(attribute.ParameterName, attribute);
                }

                if (@event.IsOneWay)
                {
                    ProcessMethodBodyForOneWayAsset(raiseMethod, raiseMethod!.ReturnType, memberPath, _includesServiceWrapperOnlyInfo,
                        @event.RaisingMethodBodyInfo, AsyncMethodOriginalReturnValueDataTypeClass.NotAsyncMethod, eventLevelParameterIgnoredAttributes,
                        eventLevelParameterEntityPropertyNameAttributes);
                }
                else
                {
                    //normal
                    var delegateMethod = @event.Delegate.GetMethod("Invoke");
                    
                    var timeoutTime = eventInfo.GetValueFromAttribute<OperatingTimeoutTimeAttribute, OperatingTimeoutTimeAttribute>(
                        @event.Delegate, i => i, out _);
                    @event.AddingMethodBodyInfo.Timeout = timeoutTime?.EventAddingTimeout ?? interfaceLevelEventAddingTimeout;
                    @event.RemovingMethodBodyInfo.Timeout = timeoutTime?.EventRemovingTimeout ?? interfaceLevelEventRemovingTimeout;
                    var eventRaisingTimeout = timeoutTime?.EventRaisingTimeout ?? interfaceLevelEventRaisingTimeout;

                    var eventLevelParameterReturnRequiredAttributes =
                        new Dictionary<string, ParameterReturnRequiredAttribute>();
                    foreach (var attribute in eventInfo.GetCustomAttributes<EventParameterReturnRequiredAttribute>())
                    {
                        eventLevelParameterReturnRequiredAttributes.TryAdd(attribute.ParameterName, attribute);
                    }

                    var eventLevelParameterReturnRequiredPropertyAttributes =
                        new Dictionary<string, List<ParameterReturnRequiredPropertyAttribute>>();
                    foreach (var attribute in eventInfo
                        .GetCustomAttributes<EventParameterReturnRequiredPropertyAttribute>())
                    {
                        if (!eventLevelParameterReturnRequiredPropertyAttributes.TryGetValue(attribute.ParameterName,
                            out var list))
                        {
                            list = new List<ParameterReturnRequiredPropertyAttribute>();
                            eventLevelParameterReturnRequiredPropertyAttributes[attribute.ParameterName] = list;
                        }
                        list.Add(attribute);
                    }

                    var isReturnValueIgnored =
                        eventInfo.GetValueFromAttribute<ReturnIgnoredAttribute, bool>(i => i.IsIgnored,
                            out var returnIgnoredAttribute);
                    if (returnIgnoredAttribute == null)
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        isReturnValueIgnored = delegateMethod.ReturnTypeCustomAttributes.GetValueFromAttribute(
                            i => i.IsIgnored, out returnIgnoredAttribute);
                           
                    }
                    if (returnIgnoredAttribute == null)
                    {
                        isReturnValueIgnored =
                            delegateMethod.GetValueFromAttribute(i => i.IsIgnored, out returnIgnoredAttribute);
                    }

                    var returnValuePropertyNameSpecifiedByAttribute =
                        eventInfo.GetValueFromAttribute<CustomizedReturnValueEntityPropertyNameAttribute, string>(
                            i => i.EntityPropertyName,
                            out var customizedReturnValueEntityPropertyNameAttribute);
                    if (customizedReturnValueEntityPropertyNameAttribute == null)
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        returnValuePropertyNameSpecifiedByAttribute = delegateMethod.ReturnTypeCustomAttributes.GetValueFromAttribute(
                            i => i.EntityPropertyName, out customizedReturnValueEntityPropertyNameAttribute);
                    }
                    if (customizedReturnValueEntityPropertyNameAttribute == null)
                    {
                        returnValuePropertyNameSpecifiedByAttribute = delegateMethod.GetValueFromAttribute(
                            i => i.EntityPropertyName, out customizedReturnValueEntityPropertyNameAttribute);
                    }

                    ProcessMethodBodyForNormalAsset(raiseMethod, raiseMethod!.ReturnType, memberPath,
                        // ReSharper disable once PossibleNullReferenceException
                        eventRaisingTimeout, isReturnValueIgnored, returnValuePropertyNameSpecifiedByAttribute,
                        customizedReturnValueEntityPropertyNameAttribute,
                        @event.RaisingMethodBodyInfo, AsyncMethodOriginalReturnValueDataTypeClass.NotAsyncMethod,
                        eventLevelParameterIgnoredAttributes, eventLevelParameterEntityPropertyNameAttributes,
                        eventLevelParameterReturnRequiredAttributes,
                        eventLevelParameterReturnRequiredPropertyAttributes);
                }
            }

        }
    }
}