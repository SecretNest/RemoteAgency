using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    class InterfaceTypeBasicInfo
    {
        public Type SourceInterface { get; set; }
        public string AssemblyName { get; set; }
        public virtual string ProxyTypeName { get; set; }
        public virtual string ServiceWrapperTypeName { get; set; }
    }
}
