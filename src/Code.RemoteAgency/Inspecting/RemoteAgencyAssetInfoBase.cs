using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    abstract class RemoteAgencyAssetInfoBase
    {
        public string AssetName { get; set; }
        public MemberInfo Asset { get; set; }

        public bool IsIgnored { get; set; }
        public bool WillThrowException { get; set; }
        
        public bool IsOneWay { get; set; }

        public List<RemoteAgencyAttributePassThrough> AssetLevelPassThroughAttributes { get; set; }


    }
}
