using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Represents an entity class to be created.
    /// </summary>
    public class EntityBuilding
    {
        /// <summary>
        /// Gets the name of the entity class.
        /// </summary>
        public string EntityClassName { get; }

        /// <summary>
        /// Gets the generic parameters of this entity class.
        /// </summary>
        public Type[] GenericParameters { get; }

        /// <summary>
        /// Gets properties other than in interface.
        /// </summary>
        public IReadOnlyList<EntityProperty> Properties { get; }

        /// <summary>
        /// Gets the metadata objects marked with derived class specified by <see cref="EntityTypeBuilderBase.InterfaceLevelAttributeBaseType"/> in interface level.
        /// </summary>
        /// <remarks>This will be set to <see langword="null"/> when <see cref="EntityTypeBuilderBase.InterfaceLevelAttributeBaseType"/> is set to <see langword="null"/>.</remarks>
        /// <conceptualLink target="37179c21-8267-47da-83c5-cb71adfc1287" />
        public IReadOnlyList<Attribute> InterfaceLevelAttributes { get; }

        /// <summary>
        /// Gets the metadata objects marked with derived class specified by <see cref="EntityTypeBuilderBase.AssetLevelAttributeBaseType"/> in asset level.
        /// </summary>
        /// <remarks>This will be set to <see langword="null"/> when <see cref="EntityTypeBuilderBase.AssetLevelAttributeBaseType"/> is set to <see langword="null"/>.</remarks>
        /// <conceptualLink target="37179c21-8267-47da-83c5-cb71adfc1287" />
        public IReadOnlyList<Attribute> AssetLevelAttributes { get; }

        /// <summary>
        /// Gets the metadata objects marked with derived class specified by <see cref="EntityTypeBuilderBase.DelegateLevelAttributeBaseType"/> for the delegate of event. Only available when processing events.
        /// </summary>
        /// <remarks>This will be set to <see langword="null"/> when <see cref="EntityTypeBuilderBase.DelegateLevelAttributeBaseType"/> is set to <see langword="null"/>.</remarks>
        /// <conceptualLink target="37179c21-8267-47da-83c5-cb71adfc1287" />
        public IReadOnlyList<Attribute> DelegateLevelAttributes { get; }

        /// <summary>
        /// Initializes an instance of EntityBuilding.
        /// </summary>
        /// <param name="entityClassName">Name of the entity class.</param>
        /// <param name="properties">Properties other than in interface.</param>
        /// <param name="interfaceLevelAttributes">Metadata objects marked with derived class specified by <see cref="EntityTypeBuilderBase.InterfaceLevelAttributeBaseType"/> in interface level. This will be set to <see langword="null"/> when <see cref="EntityTypeBuilderBase.InterfaceLevelAttributeBaseType"/> is set to <see langword="null"/>.</param>
        /// <param name="assetLevelAttributes">Metadata objects marked with derived class specified by <see cref="EntityTypeBuilderBase.AssetLevelAttributeBaseType"/> in asset level. This will be set to <see langword="null"/> when <see cref="EntityTypeBuilderBase.AssetLevelAttributeBaseType"/> is set to <see langword="null"/>.</param>
        /// <param name="delegateLevelAttributes">Metadata objects marked with derived class specified by <see cref="EntityTypeBuilderBase.DelegateLevelAttributeBaseType"/> for the delegate of event. Only available when processing events. This will be set to <see langword="null"/> when <see cref="EntityTypeBuilderBase.DelegateLevelAttributeBaseType"/> is set to <see langword="null"/>.</param>
        /// <param name="genericParameters">Generic parameters of this entity class.</param>
        public EntityBuilding(string entityClassName, IReadOnlyList<EntityProperty> properties, IReadOnlyList<Attribute> interfaceLevelAttributes,
            IReadOnlyList<Attribute> assetLevelAttributes, IReadOnlyList<Attribute> delegateLevelAttributes, Type[] genericParameters)
        {
            EntityClassName = entityClassName;
            Properties = properties;
            InterfaceLevelAttributes = interfaceLevelAttributes;
            AssetLevelAttributes = assetLevelAttributes;
            DelegateLevelAttributes = delegateLevelAttributes;
            GenericParameters = genericParameters;
        }
    }
}
