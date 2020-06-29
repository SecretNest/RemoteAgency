using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    class RemoteAgencyMethodInfo : RemoteAgencyAssetInfoBase
    {
        public List<RemoteAgencyGenericArgumentInfo> AssetLevelGenericArguments { get; set; }
        public List<RemoteAgencyAttributePassThrough> ReturnValuePassThroughAttributes { get; set; }

        public string ParameterEntityName { get; set; }
        public List<RemoteAgencyParameterInfo> ParameterEntityProperties { get; set; }

        public string ReturnValueEntityName { get; set; }
        public List<RemoteAgencyReturnValueInfoBase> ReturnValueEntityProperties { get; set; }

        public int MethodCallingTimeout { get; set; }

    }
}
