using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    abstract partial class RemoteAgency
    {
        /// <summary>
        /// Creates an instance of Remote Agency.
        /// </summary>
        /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
        /// <typeparam name="TEntityBase">Type of the parent class of all entities.</typeparam>
        /// <param name="serializingHelper">Serializer helper.</param>
        /// <param name="entityTypeBuilder">Entity type builder.</param>
        /// <param name="siteId">Site id. A randomized value is used when it is set as <see cref="Guid"/>.Empty or absent.</param>
        /// <returns>Created Remote Agency instance.</returns>
        public static RemoteAgency<TSerialized, TEntityBase> Create<TSerialized, TEntityBase>(
            SerializingHelperBase<TSerialized, TEntityBase> serializingHelper, EntityTypeBuilderBase entityTypeBuilder, Guid? siteId = null)
        {
            if (serializingHelper == null)
                throw new ArgumentNullException(nameof(serializingHelper));
            if (entityTypeBuilder == null)
                throw new ArgumentNullException(nameof(entityTypeBuilder));

            return CreateWithoutCheck(serializingHelper, entityTypeBuilder, siteId);
        }

        static RemoteAgency<TSerialized, TEntityBase> CreateWithoutCheck<TSerialized, TEntityBase>(
            SerializingHelperBase<TSerialized, TEntityBase> serializingHelper, EntityTypeBuilderBase entityTypeBuilder, Guid? siteId)
        {
            return new RemoteAgency<TSerialized, TEntityBase>(serializingHelper, entityTypeBuilder, siteId ?? Guid.Empty);
        }
    }
}
