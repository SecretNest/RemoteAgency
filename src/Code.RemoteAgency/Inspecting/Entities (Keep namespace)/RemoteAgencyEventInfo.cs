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

        public string AddingRequestEntityName { get; set; }
        public string AddingResponseEntityName { get; set; }

        public string RemovingRequestEntityName { get; set; }
        public string RemovingResponseEntityName { get; set; }

        public string RaisingNotificationEntityName { get; set; }
        public List<RemoteAgencyParameterInfo> RaisingNotificationEntityProperties { get; set; }
        public string RaisingFeedbackEntityName { get; set; }
        public List<RemoteAgencyReturnValueInfoBase> RaisingFeedbackEntityProperties { get; set; }

        public int EventAddingTimeout { get; set; }
        public int EventRemovingTimeout { get; set; }
        public int EventRaisingTimeout { get; set; }

        public override IEnumerable<EntityBuildingExtended> GetEntities(List<Attribute> interfaceLevelAttributes, List<RemoteAgencyGenericParameterInfo> interfaceLevelGenericParameters)
        {
            if (!string.IsNullOrEmpty(RaisingNotificationEntityName))
            {
                List<EntityProperty> properties = RaisingNotificationEntityProperties.Select(i =>
                        new EntityProperty(i.DataType, i.PropertyName,
                            i.SerializerParameterLevelAttributes
                                .Select(j => new EntityPropertyAttribute(AttributePosition.Parameter, j)).ToList()))
                    .ToList();

                EntityBuildingExtended entity = new EntityBuildingExtended(RaisingNotificationEntityName, properties, interfaceLevelAttributes, SerializerAssetLevelAttributes, SerializerDelegateLevelAttributes, interfaceLevelGenericParameters);

                yield return entity;
            }

            if (!string.IsNullOrEmpty(RaisingFeedbackEntityName))
            {
                List<EntityProperty> properties = RaisingFeedbackEntityProperties.Where(i => i.IsIncludedInEntity)
                    .Select(i =>
                        new EntityProperty(i.DataType, i.PropertyName, i.GetEntityPropertyAttributes().ToList()))
                    .ToList();

                EntityBuildingExtended entity = new EntityBuildingExtended(RaisingFeedbackEntityName, properties, interfaceLevelAttributes, SerializerAssetLevelAttributes, SerializerDelegateLevelAttributes, interfaceLevelGenericParameters);

                yield return entity;
            }
        }
    }
}
