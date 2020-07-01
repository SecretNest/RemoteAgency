using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    class RemoteAgencyInterfaceBasicInfo
    {
        public Type SourceInterface { get; set; }
        public bool IsSourceInterfaceGenericType { get; set; }
        public Type[] SourceInterfaceGenericArguments { get; set; }
        public string AssemblyName { get; set; }
        public string ClassNameBase { get; set; }
        public string ProxyTypeName { get; set; }
        public string ServiceWrapperTypeName { get; set; }
    }
}
