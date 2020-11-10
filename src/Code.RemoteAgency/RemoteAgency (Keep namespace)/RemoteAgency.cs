using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Remote Agency from SecretNest.info. This is an abstract class.
    /// </summary>
    /// <seealso cref="RemoteAgency{TSerialized, TEntityBase}"/>
    public abstract partial class RemoteAgencyBase : IDisposable
    {
        /// <summary>
        /// Gets or sets the site id of this instance.
        /// </summary>
        /// <remarks>SiteId is used to identify the instance of Remote Agency when routing messages on network.</remarks>
        public Guid SiteId { get; set; }

        /// <summary>
        /// Instance of entity type builder.
        /// </summary>
        protected readonly EntityTypeBuilderBase EntityTypeBuilder;

        /// <summary>
        /// Initializes the instance of Remote Agency.
        /// </summary>
        /// <param name="entityTypeBuilder">Entity type builder.</param>
        /// <param name="siteId">Site id. A randomized value is used when it is set as <see cref="Guid"/>.Empty.</param>
        /// <param name="entityBase">Type of the entity base.</param>
        protected RemoteAgencyBase(EntityTypeBuilderBase entityTypeBuilder, Guid siteId, Type entityBase)
        {
            SiteId = siteId == Guid.Empty ? Guid.NewGuid() : siteId;
            EntityTypeBuilder = entityTypeBuilder;
            _entityBase = entityBase;
        }

        private readonly Type _entityBase;

        #region IDisposable Support
        private bool _disposedValue;

        /// <summary>
        /// Disposes of the resources (other than memory) used by this instance.
        /// </summary>
        /// <param name="disposing">True: release both managed and unmanaged resources; False: release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    CloseAllInstances();
                }

                _disposedValue = true;
            }
        }

        /// <summary>
        /// Releases all resources used by this instance.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
        }
        #endregion
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
        /// Initializes an instance of Remote Agency.
        /// </summary>
        /// <param name="serializingHelper">Serializer helper.</param>
        /// <param name="entityTypeBuilder">Entity type builder.</param>
        /// <param name="siteId">Site id. A randomized value is used when it is set as <see cref="Guid"/>.Empty.</param>
        public RemoteAgency(SerializingHelperBase<TSerialized, TEntityBase> serializingHelper, EntityTypeBuilderBase entityTypeBuilder, Guid siteId) : base(entityTypeBuilder, siteId, typeof(TEntityBase))
        {
            _serializingHelper = serializingHelper;
        }
    }

    //public class RemoteAgency<TSerialized>
}
