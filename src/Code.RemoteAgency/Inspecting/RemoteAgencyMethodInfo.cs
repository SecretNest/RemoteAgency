using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    class RemoteAgencyMethodInfo : RemoteAgencyAssetInfoBase
    {
        public List<List<RemoteAgencyGenericParameterInfo>> AssetLevelGenericParameters { get; set; } //method only

        public string ParameterEntityName { get; set; }
        public List<RemoteAgencyParameterInfo> ParameterEntityProperties { get; set; }

        public string ReturnValueEntityName { get; set; }
        public List<RemoteAgencyReturnValueInfo> ReturnValueEntityProperties { get; set; }
    }
}
