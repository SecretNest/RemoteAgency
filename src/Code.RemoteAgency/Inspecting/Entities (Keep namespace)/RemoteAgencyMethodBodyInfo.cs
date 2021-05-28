using System;
using System.Collections.Generic;
using System.Reflection;

namespace SecretNest.RemoteAgency.Inspecting
{
    class RemoteAgencySimpleMethodBodyInfo
    {
        public int Timeout { get; set; }
    }

    class RemoteAgencyMethodBodyInfo : RemoteAgencySimpleMethodBodyInfo
    {
        public Type ParameterEntity { get; set; }
        public Type ReturnValueEntity { get; set; }

        public List<RemoteAgencyParameterInfo> ParameterEntityProperties { get; set; }
        public List<RemoteAgencyReturnValueInfoBase> ReturnValueEntityProperties { get; set; }
        public string ParameterEntityName { get; set; }
        public string ReturnValueEntityName { get; set; }

        public ParameterInfo[] Parameters { get; set; }
        public Type ReturnType { get; set; }
    }
}
