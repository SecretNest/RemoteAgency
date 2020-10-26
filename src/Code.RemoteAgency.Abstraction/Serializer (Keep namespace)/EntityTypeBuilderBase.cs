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
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#SerializerAdditional" />
    public abstract class EntityTypeBuilderBase
    {
        /// <summary>
        /// Builds an entity class type.
        /// </summary>
        /// <param name="typeBuilder">Builder of the entity class.</param>
        /// <param name="entityBuilding">Info of entity to be built in this method.</param>
        public abstract void BuildEntity(TypeBuilder typeBuilder, EntityBuilding entityBuilding);

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
        /// <remarks>The parameter level attributes will be searched from parameter of method, parameter of delegate and property itself.</remarks>
        public abstract Type ParameterLevelAttributeBaseType { get; }

        /// <summary>
        /// Creates an empty message which is allowed to be serialized.
        /// </summary>
        /// <returns>Empty message.</returns>
        public abstract IRemoteAgencyMessage CreateEmptyMessage();
    }
}
