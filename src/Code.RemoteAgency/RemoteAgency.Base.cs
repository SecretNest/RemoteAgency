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
        /// Creates an instance of Remote Agency without entity code builder.
        /// </summary>
        /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
        /// <typeparam name="TEntityBase">Type of the parent class of all entities.</typeparam>
        /// <param name="serializingHelper">Serializer helper.</param>
        /// <returns>Created Remote Agency instance.</returns>
        /// <remarks>Due to lack of entity code builder, this instance cannot create any constructed type.</remarks>
        public static RemoteAgency<TSerialized, TEntityBase> Create<TSerialized, TEntityBase>(
            SerializingHelperBase<TSerialized, TEntityBase> serializingHelper)
        {
            if (serializingHelper == null)
                throw new ArgumentNullException(nameof(serializingHelper));


            return CreateWithoutCheck(serializingHelper, null);
        }

        /// <summary>
        /// Creates an instance of Remote Agency.
        /// </summary>
        /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
        /// <typeparam name="TEntityBase">Type of the parent class of all entities.</typeparam>
        /// <param name="serializingHelper">Serializer helper.</param>
        /// <param name="entityCodeBuilder">Entity code builder.</param>
        /// <returns>Created Remote Agency instance.</returns>
        public static RemoteAgency<TSerialized, TEntityBase> Create<TSerialized, TEntityBase>(
            SerializingHelperBase<TSerialized, TEntityBase> serializingHelper, EntityCodeBuilderBase entityCodeBuilder)
        {
            if (serializingHelper == null)
                throw new ArgumentNullException(nameof(serializingHelper));
            if (entityCodeBuilder == null)
                throw new ArgumentNullException(nameof(entityCodeBuilder));

            return CreateWithoutCheck(serializingHelper, entityCodeBuilder);
        }

        static RemoteAgency<TSerialized, TEntityBase> CreateWithoutCheck<TSerialized, TEntityBase>(
            SerializingHelperBase<TSerialized, TEntityBase> serializingHelper, EntityCodeBuilderBase entityCodeBuilder)
        {
            return new RemoteAgency<TSerialized, TEntityBase>(serializingHelper, entityCodeBuilder);
        }

        /// <summary>
        /// Initializes the instance of Remote Agency.
        /// </summary>
        protected RemoteAgency()
        {
            InitializeAssemblyBuilder();
        }
    }

    /// <summary>
    /// Remote Agency from SecretNest.info. 
    /// </summary>
    /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
    /// <typeparam name="TEntityBase">Type of the parent class of all entities.</typeparam>
    public partial class RemoteAgency<TSerialized, TEntityBase> : IDisposable
    {
        private readonly SerializingHelperBase<TSerialized, TEntityBase> serializingHelper;
        private readonly EntityCodeBuilderBase entityCodeBuilder; //NOTE: this could be null.

        /// <summary>
        /// Initializes an instance of Remote Agency.
        /// </summary>
        /// <param name="serializingHelper">Serializer helper.</param>
        /// <param name="entityCodeBuilder">Entity code builder.</param>
        public RemoteAgency(SerializingHelperBase<TSerialized, TEntityBase> serializingHelper, EntityCodeBuilderBase entityCodeBuilder)
        {
            this.serializingHelper = serializingHelper;
            this.entityCodeBuilder = entityCodeBuilder;



        }

        /// <summary>
        /// Placeholder
        /// </summary>
        /// <param name="force"></param>
        public void Disconnect(bool force)
        {
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
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    Disconnect(true);
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
