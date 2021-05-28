using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency.Inspecting
{
    abstract class RemoteAgencyAssetInfoBase : IIsIgnored, IIsOneWay
    {
        public string AssetName { get; set; }
        public MemberInfo Asset { get; set; }

        public bool IsIgnored { get; set; }
        public bool WillThrowExceptionWhileCalling { get; set; }
        
        public bool IsOneWay { get; set; }

        public List<CustomAttributeBuilder> AssetLevelPassThroughAttributes { get; set; }
        public List<Attribute> SerializerAssetLevelAttributes { get; set; }
        
        public LocalExceptionHandlingMode LocalExceptionHandlingMode { get; set; }

        public abstract IEnumerable<EntityBuildingExtended> GetEntities(List<Attribute> interfaceLevelAttributes,
            Type[] interfaceLevelGenericParameters,
            Dictionary<string, List<CustomAttributeBuilder>>
                interfaceLevelGenericParameterPassThroughAttributes);
    }
}