using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace SecretNest.RemoteAgency.BinarySerializer
{
    /// <summary>
    /// Provides code generating for entity classes working with <see cref="RemoteAgencyBinarySerializer"/>
    /// </summary>
    public class RemoteAgencyBinarySerializerEntityTypeBuilder : EntityTypeBuilderBase
    {
        /// <summary>
        /// Builds an entity class type.
        /// </summary>
        /// <param name="typeBuilder">Builder of the entity class.</param>
        /// <param name="entityBuilding">Info of entity to be built in this method.</param>
        public override void BuildEntity(TypeBuilder typeBuilder, EntityBuilding entityBuilding)
        {
            SetSerializable(typeBuilder);
            PlainEntityBodyBuilder.BuildEntity(typeBuilder, entityBuilding);
        }

        void SetSerializable(TypeBuilder typeBuilder)
        {
            var ctorInfo = typeof(SerializableAttribute).GetConstructor(Type.EmptyTypes);
            var attributeBuilder = new CustomAttributeBuilder(ctorInfo ?? throw new InvalidOperationException(),
                Array.Empty<object>());
            typeBuilder.SetCustomAttribute(attributeBuilder);
        }

        /// <inheritdoc />
        public override Type InterfaceLevelAttributeBaseType => null;
        /// <inheritdoc />
        public override Type AssetLevelAttributeBaseType => null;
        /// <inheritdoc />
        public override Type DelegateLevelAttributeBaseType => null;
        /// <inheritdoc />
        public override Type ParameterLevelAttributeBaseType => null;

        /// <inheritdoc />
        public override IRemoteAgencyMessage CreateEmptyMessage()
        {
            return new RemoteAgencyBinaryEmptyMessage();
        }
    }
}
