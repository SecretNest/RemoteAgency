using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgency
    {
        private int _waitingTimeForDisposing = 90000; //90 secs

        private protected int GetWaitingTimeForDisposing() => _waitingTimeForDisposing;

        /// <summary>
        /// Gets or sets the waiting time in milliseconds for waiting a managing object to complete all communication operations before being disposed.
        /// </summary>
        /// <remarks>The code waiting for response will throw a <see cref="ObjectDisposedException"/> when the communication operation is halt due to disposing.</remarks>
        public int WaitingTimeForDisposing
        {
            get => _waitingTimeForDisposing;
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException();
                _waitingTimeForDisposing = value;
            }
        }

        /// <summary>
        /// Unlinks specified remote proxy from the event registered in service wrapper objects.
        /// </summary>
        /// <param name="siteId">The site id of the instance of the Remote Agency which managing the closing proxy.</param>
        /// <param name="proxyInstanceId">The instance id of the closing proxy. When set to null, all proxies from the site specified by <paramref name="siteId" /> will be unlinked. Default value is null.</param>
        /// <remarks><para>Should be called when a remote proxy closing happened without messages routed to <see cref="RemoteAgency{TSerialized, TEntityBase}.ProcessReceivedMessage(TEntityBase)"/> or <see cref="RemoteAgency{TSerialized, TEntityBase}.ProcessReceivedMessage(IRemoteAgencyMessage)"/>.</para>
        /// <para>Service wrapper object manages links of all proxies which need to handle events. When remote proxy is disposed, messages for removing event handlers are sent to the service wrapper object. But when something wrong happened, network disconnected or proxy object crashed for example, the crucial messages may not be able to transferred correctly. In this case, this method need to be called, or the obsolete links will stay in service wrapper object which may cause lags or exceptions while processing events.</para></remarks>
        public abstract void OnRemoteProxyClosing(Guid siteId, Guid? proxyInstanceId = null);

        /// <summary>
        /// Resets sticky target site of all affected proxies and unlinks specified remote service wrapper from the event registered in proxy objects when the service wrapper is closing.
        /// </summary>
        /// <param name="siteId">The site id of the instance of the Remote Agency which managing the closing service wrapper.</param>
        /// <param name="serviceWrapperInstanceId">The instance id of the closing service wrapper. When set to null, all proxies with sticky target site specified by <paramref name="siteId" /> will be reset. Default value is null.</param>
        /// <remarks>Should be called when a service wrapper is closing and some proxies managed by the local Remote Agency instance have the sticky target site pointed to the site managing the closing service wrapper.</remarks>
        /// <seealso cref="ProxyStickyTargetSiteAttribute"/>
        public abstract void OnRemoteServiceWrapperClosing(Guid siteId, Guid? serviceWrapperInstanceId);

        /// <summary>
        /// Resets sticky target site of the proxy specified.
        /// </summary>
        /// <param name="proxy">Proxy to be reset.</param>
        /// <remarks>Should be called when a service wrapper is closing and this proxy has the sticky target site pointed to the site managing the closing service wrapper.</remarks>
        /// <seealso cref="ProxyStickyTargetSiteAttribute"/>
        public void ResetProxyStickyTargetSite(object proxy)
        {
            var obj = proxy as IProxyCommunicate;
            if (obj == null)
                throw new ArgumentNullException(nameof(proxy), $"Argument {nameof(proxy)} is not set as a proxy object.");

            obj.ProxyStickyTargetSiteResetCallback();
        }

        /// <summary>
        /// Queries the proxy sticky target site setting state.
        /// </summary>
        /// <param name="proxy">Proxy to be reset.</param>
        /// <param name="isEnabled">Will be set as whether this function is enabled on this proxy.</param>
        /// <param name="defaultTargetSiteId">Will be set as default target site id.</param>
        /// <param name="stickyTargetSiteId">Will be set as sticky target site id. Value will be set to <see langword="null"/> if no sticky target set yet.</param>
        public void ProxyStickyTargetSiteQuery(object proxy, out bool isEnabled, out Guid defaultTargetSiteId,
            out Guid? stickyTargetSiteId)
        {
            var obj = proxy as IProxyCommunicate;
            if (obj == null)
                throw new ArgumentNullException(nameof(proxy), $"Argument {nameof(proxy)} is not set as a proxy object.");

            obj.ProxyStickyTargetSiteQueryCallback(out isEnabled, out defaultTargetSiteId, out stickyTargetSiteId);
        }
    }

    partial class RemoteAgency<TSerialized, TEntityBase>
    {
        /// <inheritdoc />
        public override void OnRemoteProxyClosing(Guid siteId, Guid? proxyInstanceId = null)
        {
            List<Exception> exceptions = new List<Exception>();
            foreach (var remoteAgencyManagingObject in _managingObjects.Values)
            {
                try
                {
                    remoteAgencyManagingObject.OnProxyClosing(siteId, proxyInstanceId);
                }
                catch (AggregateException e)
                {
                    exceptions.AddRange(e.InnerExceptions);
                }
            }

            if (exceptions.Count == 0)
                return;
            else if (exceptions.Count == 1)
                throw exceptions[0];
            else
            {
                throw new AggregateException(exceptions);
            }
        }

        /// <inheritdoc />
        public override void OnRemoteServiceWrapperClosing(Guid siteId, Guid? serviceWrapperInstanceId)
        {
            foreach (var remoteAgencyManagingObject in _managingObjects.Values)
            {
                remoteAgencyManagingObject.OnServiceWrapperClosing(siteId, serviceWrapperInstanceId);
            }
        }
    }
}
