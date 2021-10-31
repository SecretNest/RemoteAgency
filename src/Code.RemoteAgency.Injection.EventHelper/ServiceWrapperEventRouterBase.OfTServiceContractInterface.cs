using System;

namespace SecretNest.RemoteAgency.Injection.EventHelper
{
    /// <summary>
    /// Defines a helper class to be implanted into built assembly for handling an event handler in service wrapper. This is an abstract class.
    /// </summary>
    /// <typeparam name="TServiceContractInterface">Service contract interface type.</typeparam>
    internal abstract class ServiceWrapperEventRouterBase<TServiceContractInterface>
    {
        /// <summary>
        /// Gets or sets the asset name.
        /// </summary>
        public string AssetName { get; set; }

        /// <summary>
        /// Gets or sets the remote site id. Remote site is the site contains event handler.
        /// </summary>
        public Guid RemoteSiteId { get; set; }

        /// <summary>
        /// Gets or sets the remote instance id. Remote instance is the instance contains event handler.
        /// </summary>
        public Guid RemoteInstanceId { get; set; }

        /// <summary>
        /// Gets or sets the callback for a delegate which will be called while an event raising message need to be sent to a remote site and get response of it.
        /// </summary>
        public SendTwoWayMessageCallback SendEventMessageCallback { get; set; }

        /// <summary>
        /// Gets or sets the callback for a delegate which will be called while an event raising message need to be sent to a remote site without getting response.
        /// </summary>
        public SendOneWayMessageCallback SendOneWayEventMessageCallback { get; set; }

        private protected void SetMessageProperties(IRemoteAgencyMessage message)
        {
            message.AssetName = AssetName;
            message.TargetInstanceId = RemoteInstanceId;
            message.TargetSiteId = RemoteSiteId;
        }

        /// <summary>
        /// Sends message to relevant object and closes the functions of this object.
        /// </summary>
        /// <param name="serviceObject">Service object.</param>
        public void CloseRequestedByManagingObject(TServiceContractInterface serviceObject)
        {
            try
            {
                RemoveHandler(serviceObject);
            }
            finally
            {
                SendEventMessageCallback = null;
                SendOneWayEventMessageCallback = null;
            }
        }

        /// <summary>
        /// Removes the handler.
        /// </summary>
        /// <param name="serviceObject">Service object.</param>
        public abstract void RemoveHandler(TServiceContractInterface serviceObject);

        /// <summary>
        /// Adds the handler.
        /// </summary>
        /// <param name="serviceObject">Service object.</param>
        public abstract void AddHandler(TServiceContractInterface serviceObject);

    }
}
