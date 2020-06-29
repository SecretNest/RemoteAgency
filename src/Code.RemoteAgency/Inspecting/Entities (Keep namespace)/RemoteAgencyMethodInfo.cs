using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    class RemoteAgencyMethodInfo : RemoteAgencyAssetInfoBase
    {
        public List<RemoteAgencyGenericArgumentInfo> AssetLevelGenericArguments { get; set; }
        public Dictionary<string, List<RemoteAgencyAttributePassThrough>> ParameterPassThroughAttributes { get; set; }
        public List<RemoteAgencyAttributePassThrough> ReturnValuePassThroughAttributes { get; set; }

        public string ParameterEntityName { get; set; }
        public List<RemoteAgencyParameterInfo> ParameterEntityProperties { get; set; }

        public string ReturnValueEntityName { get; set; }
        public List<RemoteAgencyReturnValueInfoBase> ReturnValueEntityProperties { get; set; }

        public int MethodCallingTimeout { get; set; }

        public override IEnumerable<EntityBuilding> GetEntities(List<Attribute> interfaceLevelAttributes)
        {
            if (!string.IsNullOrEmpty(ParameterEntityName))
            {
                List<EntityProperty> properties = ParameterEntityProperties.Select(i =>
                        new EntityProperty(i.DataType, i.PropertyName,
                            i.SerializerParameterLevelAttributes
                                .Select(j => new EntityPropertyAttribute(AttributePosition.Parameter, j)).ToList()))
                    .ToList();

                EntityBuilding entity = new EntityBuilding(ParameterEntityName, properties, interfaceLevelAttributes, SerializerAssetLevelAttributes, null);

                yield return entity;
            }

            if (!string.IsNullOrEmpty(ReturnValueEntityName))
            {
                List<EntityProperty> properties = ReturnValueEntityProperties.Select(i =>
                    new EntityProperty(i.DataType, i.PropertyName, i.GetEntityPropertyAttributes().ToList())).ToList();

                EntityBuilding entity = new EntityBuilding(ReturnValueEntityName, properties, interfaceLevelAttributes, SerializerAssetLevelAttributes, null);

                yield return entity;
            }
        }
    }
}
