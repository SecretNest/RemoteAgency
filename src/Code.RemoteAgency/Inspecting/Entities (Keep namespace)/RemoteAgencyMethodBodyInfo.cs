using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    class RemoteAgencySimpleMethodBodyInfo
    {
        public int Timeout { get; set; }
    }

    class RemoteAgencyMethodBodyInfo : RemoteAgencySimpleMethodBodyInfo
    {
        public List<RemoteAgencyParameterInfo> ParameterEntityProperties { get; set; }
        public List<RemoteAgencyReturnValueInfoBase> ReturnValueEntityProperties { get; set; }
        public string ParameterEntityName { get; set; }
        public string ReturnValueEntityName { get; set; }
    }
}
