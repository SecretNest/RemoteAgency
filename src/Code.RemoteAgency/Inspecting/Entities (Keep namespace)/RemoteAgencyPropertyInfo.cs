using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    class RemoteAgencyPropertyInfo : RemoteAgencyAssetInfoBase
    {
        public bool IsGettingOneWay { get; set; }
        public bool IsSettingOneWay
        {
            get => IsOneWay;
            set => IsOneWay = value;
        }

        public string GettingRequestEntityName { get; set; }
        public string GettingResponseEntityName { get; set; }
        public List<RemoteAgencyReturnValueInfoBase> GettingResponseEntityProperties { get; set; }

        public string SettingRequestEntityName { get; set; }
        public List<RemoteAgencyParameterInfo> SettingRequestEntityProperties { get; set; }
        public string SettingResponseEntityName { get; set; }
        public List<RemoteAgencyReturnValueInfoBase> SettingResponseEntityProperties { get; set; }

        public int PropertyGettingTimeout { get; set; }
        public int PropertySettingTimeout { get; set; }

        public override IEnumerable<EntityBuilding> GetEntities(List<Attribute> interfaceLevelAttributes)
        {
            if (!string.IsNullOrEmpty(GettingRequestEntityName))
            {
                List<EntityProperty> properties = new List<EntityProperty>();

                EntityBuilding entity = new EntityBuilding(GettingRequestEntityName, properties, interfaceLevelAttributes, SerializerAssetLevelAttributes, null);

                yield return entity;
            }

            if (!string.IsNullOrEmpty(GettingResponseEntityName))
            {
                List<EntityProperty> properties = GettingResponseEntityProperties.Select(i =>
                    new EntityProperty(i.DataType, i.PropertyName, i.GetEntityPropertyAttributes().ToList())).ToList();

                EntityBuilding entity = new EntityBuilding(GettingResponseEntityName, properties, interfaceLevelAttributes, SerializerAssetLevelAttributes, null);

                yield return entity;
            }

            if (!string.IsNullOrEmpty(SettingRequestEntityName))
            {
                List<EntityProperty> properties = SettingRequestEntityProperties.Select(i =>
                        new EntityProperty(i.DataType, i.PropertyName,
                            i.SerializerParameterLevelAttributes
                                .Select(j => new EntityPropertyAttribute(AttributePosition.Parameter, j)).ToList()))
                    .ToList();

                EntityBuilding entity = new EntityBuilding(SettingRequestEntityName, properties, interfaceLevelAttributes, SerializerAssetLevelAttributes, null);

                yield return entity;
            }

            if (!string.IsNullOrEmpty(SettingResponseEntityName))
            {
                List<EntityProperty> properties = SettingResponseEntityProperties.Select(i =>
                    new EntityProperty(i.DataType, i.PropertyName, i.GetEntityPropertyAttributes().ToList())).ToList();

                EntityBuilding entity = new EntityBuilding(SettingResponseEntityName, properties, interfaceLevelAttributes, SerializerAssetLevelAttributes, null);

                yield return entity;
            }
        }
    }
}
