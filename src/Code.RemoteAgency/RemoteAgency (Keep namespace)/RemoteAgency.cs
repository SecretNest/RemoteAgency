using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Remote Agency from SecretNest.info. 
    /// </summary>
    /// <seealso cref="RemoteAgency{TSerialized, TEntityBase}"/>
    public abstract partial class RemoteAgency
    {
        /// <summary>
        /// Gets or sets the site id of this instance.
        /// </summary>
        /// <remarks>SiteId is used to identify the instance of Remote Agency when routing messages on network.</remarks>
        public Guid SiteId { get; set; }

        /// <summary>
        /// Initializes the instance of Remote Agency.
        /// </summary>
        /// <param name="siteId">Site id. A randomized value is used when it is set to <see cref="Guid.Empty"/>.</param>
        protected RemoteAgency(Guid siteId)
        {
            InitializeAssemblyBuilder();

            SiteId = siteId == Guid.Empty ? Guid.NewGuid() : siteId;

            EnableInMemoryTypeCache();
        }
    }

    /// <summary>
    /// Remote Agency from SecretNest.info. 
    /// </summary>
    /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
    /// <typeparam name="TEntityBase">Type of the parent class of all entities.</typeparam>
    public sealed partial class RemoteAgency<TSerialized, TEntityBase> : RemoteAgency, IDisposable
    {
        private readonly SerializingHelperBase<TSerialized, TEntityBase> _serializingHelper;
        private readonly EntityCodeBuilderBase _entityCodeBuilder;

        /// <summary>
        /// Initializes an instance of Remote Agency.
        /// </summary>
        /// <param name="serializingHelper">Serializer helper.</param>
        /// <param name="entityCodeBuilder">Entity code builder.</param>
        /// <param name="siteId">Site id. A randomized value is used when it is set to <see cref="Guid.Empty"/>.</param>
        public RemoteAgency(SerializingHelperBase<TSerialized, TEntityBase> serializingHelper, EntityCodeBuilderBase entityCodeBuilder, Guid siteId) : base(siteId)
        {
            this._serializingHelper = serializingHelper;
            this._entityCodeBuilder = entityCodeBuilder;



        }


        /// <summary>
        /// Placeholder
        /// </summary>
        /// <param name="cancelMessagesInWaiting"></param>
        public void RemoveAllManagingObjects(bool cancelMessagesInWaiting)
        {
        }



        #region IDisposable Support
        private bool _disposedValue;

        /// <summary>
        /// Disposes of the resources (other than memory) used by this instance.
        /// </summary>
        /// <param name="disposing">True: release both managed and unmanaged resources; False: release only unmanaged resources.</param>
        void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    RemoveAllManagingObjects(true);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
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
}
