using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Provides code generation for entity classes. This is an abstract class.
    /// </summary>
    public abstract class EntityCodeBuilderBase
    {
        /// <summary>
        /// Builds an entity class type.
        /// </summary>
        /// <param name="targetModule">Module to place the entity class.</param>
        /// <param name="entityClassName">Name of the entity class.</param>
        /// <param name="entityClassParentClass">Type of the parent of entity class.</param>
        /// <param name="entityClassInterface">Type of the interface to be implemented explicitly.</param>
        /// <param name="properties">Properties other than in interface.</param>
        /// <param name="interfaceLevelAttributes">Metadata objects marked with derived class specified by <see cref="InterfaceLevelAttributeBaseType"/> in interface level.<remarks>This will contains nothing when <see cref="InterfaceLevelAttributeBaseType"/> is set to null.</remarks></param>
        /// <param name="assetLevelAttributes">Metadata objects marked with derived class specified by <see cref="AssetLevelAttributeBaseType"/> in asset level.<remarks>This will contains nothing when <see cref="AssetLevelAttributeBaseType"/> is set to null.</remarks></param>
        /// <param name="delegateLevelAttributes">Metadata objects marked with derived class specified by <see cref="DelegateLevelAttributeBaseType"/> for the delegate of event. Only available when processing events.<remarks>This will contains nothing when <see cref="DelegateLevelAttributeBaseType"/> is set to null.</remarks></param>
        /// <returns>Type of the built entity class.</returns>
        public abstract Type BuildEntity(ModuleBuilder targetModule, string entityClassName,
            Type entityClassParentClass, Type entityClassInterface, List<EntityProperty> properties,
            List<Attribute> interfaceLevelAttributes, List<Attribute> assetLevelAttributes,
            List<Attribute> delegateLevelAttributes);

        /// <summary>
        /// Gets the type of the base class of attributes which are used to mark metadata on interface level.
        /// </summary>
        public abstract Type InterfaceLevelAttributeBaseType { get; }

        /// <summary>
        /// Gets the type of the base class of attributes which are used to mark metadata on asset level.
        /// </summary>
        public abstract Type AssetLevelAttributeBaseType { get; }

        /// <summary>
        /// Gets the type of the base class of attributes which are used to mark metadata on delegate level. Only works with processing events.
        /// </summary>
        public abstract Type DelegateLevelAttributeBaseType { get; }

        /// <summary>
        /// Gets the type of the base class of attributes which are used to mark metadata on parameter level.
        /// </summary>
        public abstract Type ParameterLevelAttributeBaseType { get; }

        /// <summary>
        /// Creates an empty message which is allowed to be serialized.
        /// </summary>
        /// <returns>Empty message.</returns>
        public abstract IRemoteAgencyMessage CreateEmptyMessage();
    }
}
