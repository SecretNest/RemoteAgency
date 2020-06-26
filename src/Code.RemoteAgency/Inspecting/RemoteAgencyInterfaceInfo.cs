using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    class RemoteAgencyInterfaceInfo : RemoteAgencyInterfaceBasicInfo
    {
        public List<RemoteAgencyMethodInfo> Methods { get; set; }
        public List<RemoteAgencyEventInfo> Events { get; set; }
        public List<RemoteAgencyPropertyInfo> Properties { get; set; }

        public List<RemoteAgencyAttributePassThrough> InterfaceLevelPassThroughAttributes { get; set; }
        public List<RemoteAgencyGenericParameterInfo> InterfaceLevelGenericParameters { get; set; }

    }
}
