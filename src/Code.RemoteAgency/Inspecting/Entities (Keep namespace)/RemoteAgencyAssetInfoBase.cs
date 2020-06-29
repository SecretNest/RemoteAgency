using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency.Inspecting
{
    abstract class RemoteAgencyAssetInfoBase
    {
        public string AssetName { get; set; }
        public MemberInfo Asset { get; set; }

        public bool IsIgnored { get; set; }
        public bool WillThrowExceptionWhileCalling { get; set; }
        
        public bool IsOneWay { get; set; }

        public List<RemoteAgencyAttributePassThrough> AssetLevelPassThroughAttributes { get; set; }
        public List<Attribute> SerializerAssetLevelAttributes { get; set; }
        
        public LocalExceptionHandlingMode LocalExceptionHandlingMode { get; set; }

        public abstract IEnumerable<EntityBuilding> GetEntities(List<Attribute> interfaceLevelAttributes);
    }
}
