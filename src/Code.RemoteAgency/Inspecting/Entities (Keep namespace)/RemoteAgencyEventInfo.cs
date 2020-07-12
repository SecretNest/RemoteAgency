using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    class RemoteAgencyEventInfo : RemoteAgencyAssetInfoBase
    {
        public Type Delegate { get; set; }
        public List<Attribute> SerializerDelegateLevelAttributes { get; set; }

        public RemoteAgencySimpleMethodBodyInfo AddingMethodBodyInfo { get; set; }
        public RemoteAgencySimpleMethodBodyInfo RemovingMethodBodyInfo { get; set; }
        public RemoteAgencyMethodBodyInfo RaisingMethodBodyInfo { get; set; }

        public override IEnumerable<EntityBuildingExtended> GetEntities(List<Attribute> interfaceLevelAttributes,
            Type[] interfaceLevelGenericParameters,
            Dictionary<string, List<RemoteAgencyAttributePassThrough>>
                interfaceLevelGenericParameterPassThroughAttributes)
        {
            if (!string.IsNullOrEmpty(RaisingMethodBodyInfo.ParameterEntityName))
            {
                List<EntityProperty> properties = RaisingMethodBodyInfo.ParameterEntityProperties.Select(i =>
                        new EntityProperty(i.DataType, i.PropertyName,
                            i.SerializerParameterLevelAttributes
                                .Select(j => new EntityPropertyAttribute(AttributePosition.Parameter, j)).ToList()))
                    .ToList();

                EntityBuildingExtended entity = new EntityBuildingExtended(
                    RaisingMethodBodyInfo.ParameterEntityName, properties, interfaceLevelAttributes,
                    SerializerAssetLevelAttributes, SerializerDelegateLevelAttributes, interfaceLevelGenericParameters,
                    interfaceLevelGenericParameterPassThroughAttributes,
                    type => RaisingMethodBodyInfo.ParameterEntity = type);

                yield return entity;
            }

            if (!string.IsNullOrEmpty(RaisingMethodBodyInfo.ReturnValueEntityName))
            {
                List<EntityProperty> properties = RaisingMethodBodyInfo.ReturnValueEntityProperties
                    .Where(i => i.IsIncludedInEntity)
                    .Select(i =>
                        new EntityProperty(i.DataType, i.PropertyName, i.GetEntityPropertyAttributes().ToList()))
                    .ToList();

                EntityBuildingExtended entity = new EntityBuildingExtended(
                    RaisingMethodBodyInfo.ReturnValueEntityName, properties, interfaceLevelAttributes,
                    SerializerAssetLevelAttributes, SerializerDelegateLevelAttributes, interfaceLevelGenericParameters,
                    interfaceLevelGenericParameterPassThroughAttributes,
                    type => RaisingMethodBodyInfo.ReturnValueEntity = type);

                yield return entity;
            }
        }
    }
}