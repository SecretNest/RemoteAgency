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
            Stack<MemberInfo> parentPath = new Stack<MemberInfo>();
            parentPath.Push(@interface);

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

        }
    }
}