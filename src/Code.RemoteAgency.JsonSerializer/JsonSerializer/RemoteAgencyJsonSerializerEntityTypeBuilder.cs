using System;
using System.Reflection.Emit;

namespace SecretNest.RemoteAgency.JsonSerializer
{
    /// <summary>
    /// Provides code generating for entity classes working with <see cref="RemoteAgencyJsonSerializer"/>
    /// </summary>
    /// <remarks><para>This class is not present in Neat release.</para></remarks>
    public class RemoteAgencyJsonSerializerEntityTypeBuilder : EntityTypeBuilderBase
    {
        /// <summary>
        /// Builds an entity class type for json serializer.
        /// </summary>
        /// <param name="typeBuilder">Builder of the entity class.</param>
        /// <param name="entityBuilding">Info of entity to be built in this method.</param>
        /// <remarks><para>This method and this class are not present in Neat release.</para></remarks>
        public override void BuildEntity(TypeBuilder typeBuilder, EntityBuilding entityBuilding)
        {
            typeBuilder.BuildEntity(entityBuilding);
        }

        /// <summary>
        /// Gets the type of the base class of attributes which are used to mark metadata on interface level.
        /// </summary>
        /// <conceptualLink target="37179c21-8267-47da-83c5-cb71adfc1287" />
        /// <remarks><para>This property and this class are not present in Neat release.</para></remarks>
        public override Type InterfaceLevelAttributeBaseType => null;

        /// <summary>
        /// Gets the type of the base class of attributes which are used to mark metadata on asset level.
        /// </summary>
        /// <conceptualLink target="37179c21-8267-47da-83c5-cb71adfc1287" />
        /// <remarks><para>This property and this class are not present in Neat release.</para></remarks>
        public override Type AssetLevelAttributeBaseType => null;

        /// <summary>
        /// Gets the type of the base class of attributes which are used to mark metadata on delegate level. Only works with processing events.
        /// </summary>
        /// <conceptualLink target="37179c21-8267-47da-83c5-cb71adfc1287" />
        /// <remarks><para>This property and this class are not present in Neat release.</para></remarks>
        public override Type DelegateLevelAttributeBaseType => null;

        /// <summary>
        /// Gets the type of the base class of attributes which are used to mark metadata on parameter level.
        /// </summary>
        /// <remarks>The parameter level attributes will be searched from parameter of method, parameter of delegate and property itself.</remarks>
        /// <conceptualLink target="37179c21-8267-47da-83c5-cb71adfc1287" />
        /// <remarks><para>This property and this class are not present in Neat release.</para></remarks>
        public override Type ParameterLevelAttributeBaseType => null;

        /// <summary>
        /// Creates an empty message which is allowed to be serialized.
        /// </summary>
        /// <returns>Empty message.</returns>
        /// <remarks><para>This method and this class are not present in Neat release.</para></remarks>
        public override IRemoteAgencyMessage CreateEmptyMessage()
        {
            return new RemoteAgencyJsonEmptyMessage();
        }
    }
}
