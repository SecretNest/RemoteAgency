using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    class RemoteAgencyMethodInfo : RemoteAgencyAssetInfoBase
    {
        public bool IsGenericMethod => !IsIgnored && AssetLevelGenericParameters.Length > 0;

        public Type[] AssetLevelGenericParameters { get; set; }
        public Dictionary<string, List<RemoteAgencyAttributePassThrough>> AssetLevelGenericParameterPassThroughAttributes { get; set; }

        public Dictionary<string, List<RemoteAgencyAttributePassThrough>> ParameterPassThroughAttributes { get; set; }
        public List<RemoteAgencyAttributePassThrough> ReturnValuePassThroughAttributes { get; set; }

        public RemoteAgencyMethodBodyInfo MethodBodyInfo { get; set; }

        public override IEnumerable<EntityBuildingExtended> GetEntities(List<Attribute> interfaceLevelAttributes,
            Type[] interfaceLevelGenericParameters,
            Dictionary<string, List<RemoteAgencyAttributePassThrough>>
                interfaceLevelGenericParameterPassThroughAttributes)
        {
            Type[] methodGenericParameters =
                new Type[interfaceLevelGenericParameters.Length + AssetLevelGenericParameters.Length];
            Array.Copy(interfaceLevelGenericParameters, methodGenericParameters,
                interfaceLevelGenericParameters.Length);
            Array.Copy(AssetLevelGenericParameters, 0, methodGenericParameters, interfaceLevelGenericParameters.Length,
                AssetLevelGenericParameters.Length);

            var methodGenericParameterPassThroughAttributes =
                interfaceLevelGenericParameterPassThroughAttributes.Concat(
                    AssetLevelGenericParameterPassThroughAttributes).ToDictionary(i => i.Key, i => i.Value);

            if (!string.IsNullOrEmpty(MethodBodyInfo.ParameterEntityName))
            {
                List<EntityProperty> properties = MethodBodyInfo.ParameterEntityProperties.Select(i =>
                        new EntityProperty(i.DataType, i.PropertyName,
                            i.SerializerParameterLevelAttributes
                                .Select(j => new EntityPropertyAttribute(AttributePosition.Parameter, j)).ToList()))
                    .ToList();

                EntityBuildingExtended entity = new EntityBuildingExtended(MethodBodyInfo.ParameterEntityName,
                    properties, interfaceLevelAttributes, SerializerAssetLevelAttributes, null, methodGenericParameters,
                    methodGenericParameterPassThroughAttributes,
                    type => MethodBodyInfo.ParameterEntity = type);

                yield return entity;
            }

            if (!string.IsNullOrEmpty(MethodBodyInfo.ReturnValueEntityName))
            {
                List<EntityProperty> properties = MethodBodyInfo.ReturnValueEntityProperties
                    .Where(i => i.IsIncludedInEntity)
                    .Select(i =>
                        new EntityProperty(i.DataType, i.PropertyName, i.GetEntityPropertyAttributes().ToList()))
                    .ToList();

                EntityBuildingExtended entity = new EntityBuildingExtended(MethodBodyInfo.ReturnValueEntityName,
                    properties, interfaceLevelAttributes, SerializerAssetLevelAttributes, null, methodGenericParameters,
                    methodGenericParameterPassThroughAttributes,
                    type => MethodBodyInfo.ReturnValueEntity = type);

                yield return entity;
            }
        }
    }
}
