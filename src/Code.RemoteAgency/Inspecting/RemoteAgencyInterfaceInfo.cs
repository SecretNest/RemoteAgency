using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency.Inspecting
{
    class RemoteAgencyInterfaceInfo : RemoteAgencyInterfaceBasicInfo
    {
        public List<RemoteAgencyMethodInfo> Methods { get; set; }
        public List<RemoteAgencyEventInfo> Events { get; set; }
        public List<RemoteAgencyPropertyInfo> Properties { get; set; }

        public List<RemoteAgencyAttributePassThrough> InterfaceLevelPassThroughAttributes { get; set; }
        public List<RemoteAgencyGenericParameterInfo> InterfaceLevelGenericParameters { get; set; }

        public ThreadLockMode ThreadLockMode { get; set; }
        public string TaskSchedulerName { get; set; }

        public int DefaultMethodCallingTimeout { get; set; } //set before building
        public int DefaultEventAddingTimeout { get; set; } //set before building
        public int DefaultEventRemovingTimeout { get; set; } //set before building
        public int DefaultEventRaisingTimeout { get; set; } //set before building
        public int DefaultPropertyGettingTimeout { get; set; } //set before building
        public int DefaultPropertySettingTimeout { get; set; } //set before building

    }
}
