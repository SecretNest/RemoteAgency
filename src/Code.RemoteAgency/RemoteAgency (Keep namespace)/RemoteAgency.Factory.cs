using System;

namespace SecretNest.RemoteAgency
{
    abstract partial class RemoteAgencyBase
    {
        /// <summary>
        /// Creates an instance of Remote Agency.
        /// </summary>
        /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
        /// <typeparam name="TEntityBase">Type of the parent class of all entities.</typeparam>
        /// <param name="serializingHelper">Serializer helper.</param>
        /// <param name="entityTypeBuilder">Entity type builder.</param>
        /// <param name="siteId">Site id. A randomized value is used when it is set to <see cref="Guid"/>.Empty or absent.</param>
        /// <returns>Created Remote Agency instance.</returns>
        /// <remarks>The function of this method is the same as <see cref="M:SecretNest.RemoteAgency.RemoteAgency`2.Create(SecretNest.RemoteAgency.SerializingHelperBase{`0,`1},SecretNest.RemoteAgency.EntityTypeBuilderBase,System.Nullable{System.Guid})" />.</remarks>
        public static RemoteAgency<TSerialized, TEntityBase> Create<TSerialized, TEntityBase>(
            SerializingHelperBase<TSerialized, TEntityBase> serializingHelper, EntityTypeBuilderBase entityTypeBuilder, Guid? siteId = null)
        {
            if (serializingHelper == null)
                throw new ArgumentNullException(nameof(serializingHelper));
            if (entityTypeBuilder == null)
                throw new ArgumentNullException(nameof(entityTypeBuilder));

            return CreateWithoutCheck(serializingHelper, entityTypeBuilder, siteId);
        }

        private static RemoteAgency<TSerialized, TEntityBase> CreateWithoutCheck<TSerialized, TEntityBase>(
            SerializingHelperBase<TSerialized, TEntityBase> serializingHelper, EntityTypeBuilderBase entityTypeBuilder, Guid? siteId)
        {
            return new (serializingHelper, entityTypeBuilder, siteId ?? Guid.Empty);
        }
    }

    partial class RemoteAgency<TSerialized, TEntityBase>
    {
        /// <summary>
        /// Creates an instance of Remote Agency.
        /// </summary>
        /// <param name="serializingHelper">Serializer helper.</param>
        /// <param name="entityTypeBuilder">Entity type builder.</param>
        /// <param name="siteId">Site id. A randomized value is used when it is set to <see cref="Guid"/>.Empty or absent.</param>
        /// <returns>Created Remote Agency instance.</returns>
        /// <remarks>The function of this method is the same as <see cref="M:SecretNest.RemoteAgency.RemoteAgencyBase.Create``2(SecretNest.RemoteAgency.SerializingHelperBase{``0,``1},SecretNest.RemoteAgency.EntityTypeBuilderBase,System.Nullable{System.Guid})" />.</remarks>
        public static RemoteAgency<TSerialized, TEntityBase> Create(
            SerializingHelperBase<TSerialized, TEntityBase> serializingHelper, EntityTypeBuilderBase entityTypeBuilder, Guid? siteId = null)
        {
            return Create<TSerialized, TEntityBase>(serializingHelper, entityTypeBuilder, siteId);
        }
    }
}
