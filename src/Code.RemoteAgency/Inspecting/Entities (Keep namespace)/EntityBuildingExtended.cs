using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    class EntityBuildingExtended : EntityBuilding
    {
        public List<RemoteAgencyGenericParameterInfo> GenericParametersInfo { get; }

        public EntityBuildingExtended(string entityClassName, IReadOnlyList<EntityProperty> properties,
            IReadOnlyList<Attribute> interfaceLevelAttributes, IReadOnlyList<Attribute> assetLevelAttributes,
            IReadOnlyList<Attribute> delegateLevelAttributes, List<RemoteAgencyGenericParameterInfo> genericParameters) : base(entityClassName,
            properties, interfaceLevelAttributes, assetLevelAttributes, delegateLevelAttributes, GetGenericTypes(genericParameters))
        {
            GenericParametersInfo = genericParameters;
        }

        static Type[] GetGenericTypes(List<RemoteAgencyGenericParameterInfo> genericParameters)
        {
            return genericParameters.Select(i => i.GenericParameter).ToArray();
        }
    }
}
