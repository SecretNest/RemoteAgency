using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public Type DataType => ((PropertyInfo) Asset).PropertyType;

        public List<Attribute> SerializerParameterLevelAttributes { get; set; } //will be linked to the *OnAsset property in RemoteAgencyValueInfo* class.

        public string GettingRequestEntityName { get; set; }
        public string GettingResponseEntityName { get; set; }
        public RemoteAgencyReturnValueInfoBase GettingResponseEntityProperty { get; set; }

        public string SettingRequestEntityName { get; set; }
        public string SettingRequestEntityValuePropertyName { get; set; }
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
                List<EntityProperty> properties = new List<EntityProperty>()
                {
                    new EntityProperty(GettingResponseEntityProperty.DataType, GettingResponseEntityProperty.PropertyName, GettingResponseEntityProperty.GetEntityPropertyAttributes().ToList())
                };
                //when GettingResponseEntityName is not null nor empty, the type of GettingResponseEntityProperty will be RemoteAgencyReturnValueInfoFromAssetPropertyReturnValue.

                EntityBuilding entity = new EntityBuilding(GettingResponseEntityName, properties, interfaceLevelAttributes, SerializerAssetLevelAttributes, null);

                yield return entity;
            }

            if (!string.IsNullOrEmpty(SettingRequestEntityName))
            {
                List<EntityProperty> properties = new List<EntityProperty>()
                {
                    new EntityProperty(DataType, SettingRequestEntityValuePropertyName, SerializerParameterLevelAttributes.Select(i=>new EntityPropertyAttribute(AttributePosition.AssetProperty, i)).ToList())
                };

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
