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

        public override IEnumerable<EntityBuildingExtended> GetEntities(List<Attribute> interfaceLevelAttributes, List<RemoteAgencyGenericParameterInfo> interfaceLevelGenericParameters)
        {
            if (!string.IsNullOrEmpty(GettingRequestEntityName))
            {
                List<EntityProperty> properties = new List<EntityProperty>();

                EntityBuildingExtended entity = new EntityBuildingExtended(GettingRequestEntityName, properties, interfaceLevelAttributes, SerializerAssetLevelAttributes, null, interfaceLevelGenericParameters);

                yield return entity;
            }

            if (!string.IsNullOrEmpty(GettingResponseEntityName))
            {
                List<EntityProperty> properties = new List<EntityProperty>()
                {
                    new EntityProperty(GettingResponseEntityProperty.DataType, GettingResponseEntityProperty.PropertyName, GettingResponseEntityProperty.GetEntityPropertyAttributes().ToList())
                };
                //when GettingResponseEntityName is not null nor empty, the type of GettingResponseEntityProperty will be RemoteAgencyReturnValueInfoFromAssetPropertyReturnValue.

                EntityBuildingExtended entity = new EntityBuildingExtended(GettingResponseEntityName, properties, interfaceLevelAttributes, SerializerAssetLevelAttributes, null, interfaceLevelGenericParameters);

                yield return entity;
            }

            if (!string.IsNullOrEmpty(SettingRequestEntityName))
            {
                List<EntityProperty> properties = new List<EntityProperty>()
                {
                    new EntityProperty(DataType, SettingRequestEntityValuePropertyName, SerializerParameterLevelAttributes.Select(i=>new EntityPropertyAttribute(AttributePosition.AssetProperty, i)).ToList())
                };

                EntityBuildingExtended entity = new EntityBuildingExtended(SettingRequestEntityName, properties, interfaceLevelAttributes, SerializerAssetLevelAttributes, null, interfaceLevelGenericParameters);

                yield return entity;
            }

            if (!string.IsNullOrEmpty(SettingResponseEntityName))
            {
                List<EntityProperty> properties = SettingResponseEntityProperties.Select(i =>
                    new EntityProperty(i.DataType, i.PropertyName, i.GetEntityPropertyAttributes().ToList())).ToList();

                EntityBuildingExtended entity = new EntityBuildingExtended(SettingResponseEntityName, properties, interfaceLevelAttributes, SerializerAssetLevelAttributes, null, interfaceLevelGenericParameters);

                yield return entity;
            }
        }
    }
}
