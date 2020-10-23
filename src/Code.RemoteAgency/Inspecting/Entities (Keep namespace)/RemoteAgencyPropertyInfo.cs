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

        public bool IsGettable { get; set; }
        public bool IsSettable { get; set; }

        public RemoteAgencyMethodBodyInfo GettingMethodBodyInfo { get; set; }
        public RemoteAgencyMethodBodyInfo SettingMethodBodyInfo { get; set; }

        public List<RemoteAgencyAttributePassThrough> GettingMethodPassThroughAttributes { get; set; }
        public List<Attribute> GettingMethodSerializerAssetLevelAttributes { get; set; }
        public List<RemoteAgencyAttributePassThrough> SettingMethodPassThroughAttributes { get; set; }
        public List<Attribute> SettingMethodSerializerAssetLevelAttributes { get; set; }

        public Dictionary<string, List<RemoteAgencyAttributePassThrough>> MethodParameterPassThroughAttributes { get; set; }
        public List<RemoteAgencyAttributePassThrough> GettingMethodReturnValuePassThroughAttributes { get; set; }

        public override IEnumerable<EntityBuildingExtended> GetEntities(List<Attribute> interfaceLevelAttributes, Type[] interfaceLevelGenericParameters,
            Dictionary<string, List<RemoteAgencyAttributePassThrough>> interfaceLevelGenericParameterPassThroughAttributes)
        {
            if (IsGettable)
            {
                List<Attribute> serializerAssetLevelAttributes = SerializerAssetLevelAttributes
                    .Concat(GettingMethodSerializerAssetLevelAttributes).ToList();

                if (!string.IsNullOrEmpty(GettingMethodBodyInfo.ParameterEntityName))
                {
                    List<EntityProperty> properties = GettingMethodBodyInfo.ParameterEntityProperties.Select(i =>
                            new EntityProperty(i.DataType, i.PropertyName,
                                i.SerializerParameterLevelAttributes
                                    .Select(j => new EntityPropertyAttribute(AttributePosition.AssetProperty, j)).ToList()))
                        .ToList();

                    EntityBuildingExtended entity = new EntityBuildingExtended(
                        GettingMethodBodyInfo.ParameterEntityName, properties,
                        interfaceLevelAttributes, serializerAssetLevelAttributes, null,
                        interfaceLevelGenericParameters, interfaceLevelGenericParameterPassThroughAttributes,
                        type => GettingMethodBodyInfo.ParameterEntity = type);

                    yield return entity;
                }

                if (!string.IsNullOrEmpty(GettingMethodBodyInfo.ReturnValueEntityName))
                {
                    List<EntityProperty> properties = GettingMethodBodyInfo.ReturnValueEntityProperties
                        .Where(i => i.IsIncludedInEntity)
                        .Select(i =>
                            new EntityProperty(i.DataType, i.PropertyName, i.GetEntityPropertyAttributes().ToList()))
                        .ToList();

                    EntityBuildingExtended entity = new EntityBuildingExtended(
                        GettingMethodBodyInfo.ReturnValueEntityName, properties,
                        interfaceLevelAttributes, serializerAssetLevelAttributes, null,
                        interfaceLevelGenericParameters, interfaceLevelGenericParameterPassThroughAttributes,
                        type => GettingMethodBodyInfo.ReturnValueEntity = type);

                    yield return entity;
                }
            }

            if (IsSettable)
            {
                List<Attribute> serializerAssetLevelAttributes = SerializerAssetLevelAttributes
                    .Concat(SettingMethodSerializerAssetLevelAttributes).ToList();

                if (!string.IsNullOrEmpty(SettingMethodBodyInfo.ParameterEntityName))
                {
                    List<EntityProperty> properties = SettingMethodBodyInfo.ParameterEntityProperties.Select(i =>
                            new EntityProperty(i.DataType, i.PropertyName,
                                i.SerializerParameterLevelAttributes
                                    .Select(j => new EntityPropertyAttribute(AttributePosition.AssetProperty, j)).ToList()))
                        .ToList();

                    EntityBuildingExtended entity = new EntityBuildingExtended(
                        SettingMethodBodyInfo.ParameterEntityName, properties,
                        interfaceLevelAttributes, serializerAssetLevelAttributes, null,
                        interfaceLevelGenericParameters, interfaceLevelGenericParameterPassThroughAttributes,
                        type => SettingMethodBodyInfo.ParameterEntity = type);

                    yield return entity;
                }

                if (!string.IsNullOrEmpty(SettingMethodBodyInfo.ReturnValueEntityName))
                {
                    List<EntityProperty> properties = SettingMethodBodyInfo.ReturnValueEntityProperties
                        .Where(i => i.IsIncludedInEntity)
                        .Select(i =>
                            new EntityProperty(i.DataType, i.PropertyName, i.GetEntityPropertyAttributes().ToList()))
                        .ToList();

                    EntityBuildingExtended entity = new EntityBuildingExtended(
                        SettingMethodBodyInfo.ReturnValueEntityName, properties,
                        interfaceLevelAttributes, serializerAssetLevelAttributes, null,
                        interfaceLevelGenericParameters, interfaceLevelGenericParameterPassThroughAttributes,
                        type => SettingMethodBodyInfo.ReturnValueEntity = type);

                    yield return entity;
                }
            }
        }
    }
}
