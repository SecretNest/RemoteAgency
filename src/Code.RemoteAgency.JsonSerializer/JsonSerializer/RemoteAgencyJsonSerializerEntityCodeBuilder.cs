using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace SecretNest.RemoteAgency.JsonSerializer
{
    /// <summary>
    /// Provides code generating for entity classes working with <see cref="RemoteAgencyJsonSerializer"/>
    /// </summary>
    public class RemoteAgencyJsonSerializerEntityCodeBuilder : EntityCodeBuilderBase
    {
        /// <inheritdoc />
        public override Type BuildEntity(TypeBuilder typeBuilder, EntityBuilding entityBuilding)
        {
            return PlainEntityBodyBuilder.BuildEntity(typeBuilder, entityBuilding);
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
            return new RemoteAgencyJsonEmptyMessage();
        }
    }
}
