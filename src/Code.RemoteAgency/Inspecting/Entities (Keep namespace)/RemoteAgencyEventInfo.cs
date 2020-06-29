﻿using System;
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

        public override IEnumerable<EntityBuilding> GetEntities(Type entityClassParentClass, Type entityClassInterface,
            List<Attribute> interfaceLevelAttributes)
        {
            if (!string.IsNullOrEmpty(RaisingNotificationEntityName))
            {
                List<EntityProperty> properties = RaisingNotificationEntityProperties.Select(i =>
                        new EntityProperty(i.DataType, i.PropertyName,
                            i.SerializerParameterLevelAttributes
                                .Select(j => new EntityPropertyAttribute(AttributePosition.Parameter, j)).ToList()))
                    .ToList();

                EntityBuilding entity = new EntityBuilding(RaisingNotificationEntityName, entityClassParentClass,
                    entityClassInterface, properties, interfaceLevelAttributes, SerializerAssetLevelAttributes, SerializerDelegateLevelAttributes);

                yield return entity;
            }

            if (!string.IsNullOrEmpty(RaisingFeedbackEntityName))
            {
                List<EntityProperty> properties = RaisingFeedbackEntityProperties.Select(i =>
                    new EntityProperty(i.DataType, i.PropertyName, i.GetEntityPropertyAttributes().ToList())).ToList();

                EntityBuilding entity = new EntityBuilding(RaisingFeedbackEntityName, entityClassParentClass,
                    entityClassInterface, properties, interfaceLevelAttributes, SerializerAssetLevelAttributes, SerializerDelegateLevelAttributes);

                yield return entity;
            }
        }
    }
}
