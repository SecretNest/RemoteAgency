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
        /// <param name="typeBuilder">Builder of the entity class.</param>
        /// <param name="entityBuilding">Entity to be built in this method.</param>
        /// <returns>Type of the built entity class.</returns>
        public abstract Type BuildEntity(TypeBuilder typeBuilder, EntityBuilding entityBuilding);

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
