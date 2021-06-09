using System;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Remote Agency from SecretNest.info. This is an abstract class.
    /// </summary>
    /// <seealso cref="RemoteAgency{TSerialized, TEntityBase}"/>
    public abstract partial class RemoteAgencyBase
    {
        /// <summary>
        /// Gets or sets the site id of this instance.
        /// </summary>
        /// <value>The site id of this instance.</value>
        /// <remarks>SiteId is used to identify the instance of Remote Agency when routing messages on network.</remarks>
        public Guid SiteId { get; set; }

        private protected abstract Type EntityBase { get; }

        /// <summary>
        /// Initializes the instance of Remote Agency.
        /// </summary>
        /// <param name="entityTypeBuilder">Entity type builder.</param>
        /// <param name="siteId">Site id. A randomized value is used when it is set to <see cref="Guid"/>.Empty.</param>
        private protected RemoteAgencyBase(EntityTypeBuilderBase entityTypeBuilder, Guid siteId)
        {
            SiteId = siteId == Guid.Empty ? Guid.NewGuid() : siteId;
            EntityTypeBuilder = entityTypeBuilder;
        }
    }

    /// <summary>
    /// Remote Agency from SecretNest.info. 
    /// </summary>
    /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
    /// <typeparam name="TEntityBase">Type of the parent class of all entities.</typeparam>
    public partial class RemoteAgency<TSerialized, TEntityBase> : RemoteAgencyBase
    {
        private readonly SerializingHelperBase<TSerialized, TEntityBase> _serializingHelper;

        /// <summary>
        /// Initializes an instance of Remote Agency with types of serialized data and parent class of all entities specified.
        /// </summary>
        /// <param name="serializingHelper">Serializer helper.</param>
        /// <param name="entityTypeBuilder">Entity type builder.</param>
        /// <param name="siteId">Site id. A randomized value is used when it is set to <see cref="Guid"/>.Empty.</param>
        internal RemoteAgency(SerializingHelperBase<TSerialized, TEntityBase> serializingHelper, EntityTypeBuilderBase entityTypeBuilder, Guid siteId) : base(entityTypeBuilder, siteId)
        {
            _serializingHelper = serializingHelper;
        }

        private protected override Type EntityBase => typeof(TEntityBase);
    }
}
