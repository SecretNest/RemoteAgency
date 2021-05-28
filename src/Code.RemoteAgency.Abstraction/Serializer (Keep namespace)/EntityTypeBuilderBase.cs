﻿using System;
using System.Reflection.Emit;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Provides code generation for entity classes. This is an abstract class.
    /// </summary>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#SerializerAdditional" />
    /// <conceptualLink target="37179c21-8267-47da-83c5-cb71adfc1287" />
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
        /// <conceptualLink target="37179c21-8267-47da-83c5-cb71adfc1287" />
        public abstract Type InterfaceLevelAttributeBaseType { get; }

        /// <summary>
        /// Gets the type of the base class of attributes which are used to mark metadata on asset level.
        /// </summary>
        /// <conceptualLink target="37179c21-8267-47da-83c5-cb71adfc1287" />
        public abstract Type AssetLevelAttributeBaseType { get; }

        /// <summary>
        /// Gets the type of the base class of attributes which are used to mark metadata on delegate level. Only works with processing events.
        /// </summary>
        /// <conceptualLink target="37179c21-8267-47da-83c5-cb71adfc1287" />
        public abstract Type DelegateLevelAttributeBaseType { get; }

        /// <summary>
        /// Gets the type of the base class of attributes which are used to mark metadata on parameter level.
        /// </summary>
        /// <remarks>The parameter level attributes will be searched from parameter of method, parameter of delegate and property itself.</remarks>
        /// <conceptualLink target="37179c21-8267-47da-83c5-cb71adfc1287" />
        public abstract Type ParameterLevelAttributeBaseType { get; }

        /// <summary>
        /// Creates an empty message which is allowed to be serialized.
        /// </summary>
        /// <returns>Empty message.</returns>
        public abstract IRemoteAgencyMessage CreateEmptyMessage();
    }
}
