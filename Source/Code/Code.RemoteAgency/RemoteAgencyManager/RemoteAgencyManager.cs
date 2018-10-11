using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Manages proxy and service wrapper objects.
    /// </summary>
    /// <typeparam name="TNetworkMessage">Type of the message for transporting.</typeparam>
    /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
    /// <typeparam name="TEntityBase">Type of the parent class of all entities.</typeparam>
    public partial class RemoteAgencyManager<TNetworkMessage, TSerialized, TEntityBase> : IDisposable where TEntityBase : class
    {

        /// <summary>
        /// Gets or sets the site id of this manager. Will be used to identify remote agency manager.
        /// </summary>
        public Guid SiteId { get; set; }

        /// <summary>
        /// Gets the context id. Will be changed when IO status changed.
        /// </summary>
        /// <seealso cref="ResetContextId()"/>
        /// <seealso cref="ResetContextId(Guid)"/>
        public Guid ContextId { get; private set; }

        /// <summary>
        /// Gets the serializing helper object.
        /// </summary>
        /// <seealso cref="SerializingHelperBase{TSerialized, TEntityBase}"/>
        public SerializingHelperBase<TSerialized, TEntityBase> SerializingHelper { get; }

        /// <summary>
        /// Gets the packing helper object.
        /// </summary>
        /// <seealso cref="PackingHelperBase{TNetworkMessage, TSerialized}"/>
        public PackingHelperBase<TNetworkMessage, TSerialized> PackingHelper { get; }


        /// <summary>
        /// Reset the context id to the value specified.
        /// </summary>
        /// <param name="contextId">Context id.</param>
        /// <seealso cref="ContextId"/>
        public void ResetContextId(Guid contextId)
        {
            ContextId = contextId;
        }

        /// <summary>
        /// Reset the context id to a new randomized value.
        /// </summary>
        /// <seealso cref="ContextId"/>
        public void ResetContextId()
        {
            ResetContextId(Guid.NewGuid());
        }

        /// <summary>
        /// initializes an instance of the RemoteAgencyManager.
        /// </summary>
        /// <param name="packingHelper">Packing helper object.</param>
        /// <param name="serializingHelper">Serializing helper object.</param>
        /// <param name="siteId">Site id. Will be set to <see cref="SiteId"/>. A randomized value will be used instead if this parameter absents or is set to null.</param>
        /// <seealso cref="PackingHelperBase{TNetworkMessage, TSerialized}"/>
        /// <seealso cref="SerializingHelperBase{TSerialized, TEntityBase}"/>
        public RemoteAgencyManager(PackingHelperBase<TNetworkMessage, TSerialized> packingHelper, SerializingHelperBase<TSerialized, TEntityBase> serializingHelper, Guid? siteId = null)
        {
            PackingHelper = packingHelper;
            SerializingHelper = serializingHelper;
            if (siteId.HasValue)
                SiteId = siteId.Value;
            else
                SiteId = Guid.NewGuid();

            messageProcessTerminatedException = new Lazy<TSerialized>
                (() => serializingHelper.SerializeException(new WrappedException<MessageProcessTerminatedException>(new MessageProcessTerminatedException())));
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        /// <summary>
        /// Disposes of the resources (other than memory) used by this instance.
        /// </summary>
        /// <param name="disposing">True: release both managed and unmanaged resources; False: release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Disconnect(true);
                    RemoveAllManagingObjects(true);
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~RemoteAgencyManager() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.

        /// <summary>
        /// Releases all resources used by this instance.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}
