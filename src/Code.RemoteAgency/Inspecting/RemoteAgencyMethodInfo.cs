using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    class RemoteAgencyMethodInfo : RemoteAgencyAssetInfoBase
    {
        public List<RemoteAgencyGenericParameterInfo> AssetLevelGenericParameters { get; set; }

        public string ParameterEntityName { get; set; }
        public List<RemoteAgencyParameterInfo> ParameterEntityProperties { get; set; }

        public string ReturnValueEntityName { get; set; }
        public List<RemoteAgencyReturnValueInfo> ReturnValueEntityProperties { get; set; }

        public int MethodCallingTimeout { get; set; }
    }
}
