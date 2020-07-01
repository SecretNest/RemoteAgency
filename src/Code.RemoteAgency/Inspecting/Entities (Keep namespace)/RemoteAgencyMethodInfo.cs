using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    class RemoteAgencyMethodInfo : RemoteAgencyAssetInfoBase
    {
        public List<RemoteAgencyGenericParameterInfo> AssetLevelGenericParameters { get; set; }
        public Dictionary<string, List<RemoteAgencyAttributePassThrough>> ParameterPassThroughAttributes { get; set; }
        public List<RemoteAgencyAttributePassThrough> ReturnValuePassThroughAttributes { get; set; }

        public string ParameterEntityName { get; set; }
        public List<RemoteAgencyParameterInfo> ParameterEntityProperties { get; set; }

        public string ReturnValueEntityName { get; set; }
        public List<RemoteAgencyReturnValueInfoBase> ReturnValueEntityProperties { get; set; }

        public int MethodCallingTimeout { get; set; }

        public override IEnumerable<EntityBuildingExtended> GetEntities(List<Attribute> interfaceLevelAttributes, List<RemoteAgencyGenericParameterInfo> interfaceLevelGenericParameters)
        {
            var methodGenericParameters = interfaceLevelGenericParameters.Concat(AssetLevelGenericParameters).ToList();

            if (!string.IsNullOrEmpty(ParameterEntityName))
            {
                List<EntityProperty> properties = ParameterEntityProperties.Select(i =>
                        new EntityProperty(i.DataType, i.PropertyName,
                            i.SerializerParameterLevelAttributes
                                .Select(j => new EntityPropertyAttribute(AttributePosition.Parameter, j)).ToList()))
                    .ToList();

                EntityBuildingExtended entity = new EntityBuildingExtended(ParameterEntityName, properties, interfaceLevelAttributes, SerializerAssetLevelAttributes, null, methodGenericParameters);

                yield return entity;
            }

            if (!string.IsNullOrEmpty(ReturnValueEntityName))
            {
                List<EntityProperty> properties = ReturnValueEntityProperties.Where(i => i.IsIncludedInEntity)
                    .Select(i =>
                        new EntityProperty(i.DataType, i.PropertyName, i.GetEntityPropertyAttributes().ToList()))
                    .ToList();

                EntityBuildingExtended entity = new EntityBuildingExtended(ReturnValueEntityName, properties, interfaceLevelAttributes, SerializerAssetLevelAttributes, null, methodGenericParameters);

                yield return entity;
            }
        }
    }
}
