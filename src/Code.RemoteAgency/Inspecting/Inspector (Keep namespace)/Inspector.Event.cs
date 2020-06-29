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
            var timeoutTime = GetValueFromAttribute<OperatingTimeoutTimeAttribute, OperatingTimeoutTimeAttribute>(
                eventInfo, @event.Delegate,
                i => i, out _);
            @event.EventAddingTimeout = timeoutTime?.EventAddingTimeout ?? interfaceLevelEventAddingTimeout;
            @event.EventRemovingTimeout = timeoutTime?.EventRemovingTimeout ?? interfaceLevelEventRemovingTimeout;
            @event.EventRaisingTimeout = timeoutTime?.EventRaisingTimeout ?? interfaceLevelEventRaisingTimeout;

            //asset level pass through attributes
            @event.AssetLevelPassThroughAttributes = GetAttributePassThrough(eventInfo,
                (m, a) => new InvalidAttributeDataException(m, a, memberPath));

            if (@event.IsIgnored)
            {
                @event.RaisingNotificationEntityProperties = new List<RemoteAgencyParameterInfo>();
                if (@event.WillThrowExceptionWhileCalling)
                {
                    @event.RaisingFeedbackEntityProperties = new List<RemoteAgencyReturnValueInfoBase>();
                }
                else
                {
                    GetParameterAndReturnValueOfEvent(@event, out var parameterInfo, out var returnType, out _);
                    ProcessParameterAndReturnValueForIgnoredAsset(parameterInfo, returnType, out var returnValues);
                    @event.RaisingFeedbackEntityProperties = returnValues;
                }
            }
            else
            {
                GetParameterAndReturnValueOfEvent(@event, out var parameterInfo, out var returnType, out var delegateMethod);
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
                    ProcessParameterAndReturnValueForOneWayAsset(parameterInfo, returnType, memberPath,
                        out var parameters, out var returnValues, eventLevelParameterIgnoredAttributes,
                        eventLevelParameterEntityPropertyNameAttributes);
                    @event.RaisingNotificationEntityProperties = parameters;
                    @event.RaisingFeedbackEntityProperties = returnValues;
                }
                else
                {
                    Dictionary<string, ParameterTwoWayAttribute> eventLevelParameterTwoWayAttributes =
                        new Dictionary<string, ParameterTwoWayAttribute>();
                    foreach (var attribute in eventInfo.GetCustomAttributes<EventParameterTwoWayAttribute>())
                    {
                        eventLevelParameterTwoWayAttributes.TryAdd(attribute.ParameterName, attribute);
                    }

                    ProcessParameterAndReturnValueForNormalAsset(parameterInfo, returnType, memberPath,
                        new[] {eventInfo, delegateMethod.ReturnTypeCustomAttributes, delegateMethod},
                        out var parameters, out var returnValues, eventLevelParameterIgnoredAttributes,
                        eventLevelParameterEntityPropertyNameAttributes, eventLevelParameterTwoWayAttributes);
                    @event.RaisingNotificationEntityProperties = parameters;
                    @event.RaisingFeedbackEntityProperties = returnValues;
                }
            }

        }

        void GetParameterAndReturnValueOfEvent(RemoteAgencyEventInfo @event, out ParameterInfo[] parameters,
            out Type returnType, out MethodInfo delegateMethod)
        {
            delegateMethod = @event.Delegate.GetMethod("Invoke");
            // ReSharper disable once PossibleNullReferenceException
            parameters = delegateMethod.GetParameters();
            returnType = delegateMethod.ReturnType;
        }
    }
}