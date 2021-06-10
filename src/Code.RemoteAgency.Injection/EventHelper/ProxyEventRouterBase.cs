using System;
using System.Collections.Generic;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency.Injection.EventHelper
{
    /// <summary>
    /// Defines a helper class to be implanted into built assembly for handling an event handler in proxy. This is an abstract class.
    /// </summary>
    internal abstract class ProxyEventRouterBase
    {
        /// <summary>
        /// Gets or sets the helper instance.
        /// </summary>
        public ProxyEventHelper ProxyEventHelper { get; set; }

        /// <summary>
        /// Gets the asset name.
        /// </summary>
        public abstract string AssetName { get; }

        /// <summary>
        /// Processes an event raising message and returns response.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        /// <param name="exception">Exception thrown while running user code.</param>
        /// <returns>Message contains the data to be returned.</returns>
        public virtual IRemoteAgencyMessage ProcessEventRaisingMessage(IRemoteAgencyMessage message,
            out Exception exception)
        {
            var result = ProxyEventHelper.CreateEmptyMessageCallback();
            //for sending a feedback, no property of message need to be assigned here.

            exception = new AssetNotFoundException(message);

            return result;
        }

        /// <summary>
        /// Processes an event raising message.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        public abstract void ProcessOneWayEventRaisingMessage(IRemoteAgencyMessage message);

        /// <summary>
        /// Unlinks specified remote service wrapper from the event registered in proxy objects when the service wrapper is closing.
        /// </summary>
        /// <param name="siteId">The site id of the instance of the Remote Agency which managing the closing service wrapper.</param>
        /// <param name="serviceWrapperInstanceId">The instance id of the closing service wrapper. When set to <see langword="null"/>, all proxies with sticky target site specified by <paramref name="siteId" /> will be reset. Default value is null.</param>
        public abstract void OnRemoteServiceWrapperClosing(Guid siteId, Guid? serviceWrapperInstanceId = null);

        /// <summary>
        /// Gets target site id and instance id then closes this object.
        /// </summary>
        /// <returns>Target site id and instance id.</returns>
        public abstract List<Tuple<Guid, Guid>> GetTargetSiteIdAndInstanceIdThenClose();

        /// <summary>
        /// Gets the local exception handling mode setting of this asset.
        /// </summary>
        public abstract LocalExceptionHandlingMode LocalExceptionHandlingMode { get; }

        /// <summary>
        /// Closes this object.
        /// </summary>
        public abstract void Close();
    }
}
