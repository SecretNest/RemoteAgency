using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgency<TSerialized, TEntityBase>
    {
        readonly ConcurrentDictionary<Guid, RemoteAgencyManagingObject<TEntityBase>> _managingObjects = new ConcurrentDictionary<Guid, RemoteAgencyManagingObject<TEntityBase>>(); //key: instanceId

        /// <summary>
        /// Unlinks specified remote proxy from the event registered in service wrapper objects.
        /// </summary>
        /// <param name="siteId">The site id of the instance of the Remote Agency which managing the closing proxy.</param>
        /// <param name="proxyInstanceId">The instance id of the closing proxy. When set to null, all proxies from the site specified by <paramref name="siteId" /> will be unlinked. Default value is null.</param>
        /// <remarks><para>Should be called when a remote proxy closing happened without messages routed to <see cref="ProcessReceivedMessage(TEntityBase)"/> or <see cref="ProcessReceivedMessage(IRemoteAgencyMessage)"/>.</para>
        /// <para>Service wrapper object manages links of all proxies which need to handle events. When remote proxy is disposed, messages for removing event handlers are sent to the service wrapper object. But when something wrong happened, network disconnected or proxy object crashed for example, the crucial messages may not be able to transferred correctly. In this case, this method need to be called, or the obsolete links will stay in service wrapper object which may cause lags or exceptions while processing events.</para></remarks>
        public void OnRemoteProxyClosing(Guid siteId, Guid? proxyInstanceId = null)
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

        /// <summary>
        /// Resets sticky target site of all affected proxies when the service wrapper is closing.
        /// </summary>
        /// <param name="siteId">The site id of the instance of the Remote Agency which managing the closing service wrapper.</param>
        /// <param name="serviceWrapperInstanceId">The instance id of the closing service wrapper. When set to null, all proxies with sticky target site specified by <paramref name="siteId" /> will be reset. Default value is null.</param>
        /// <remarks>Should be called when a service wrapper is closing and some proxies managed by the local Remote Agency instance have the sticky target site pointed to the site managing the closing service wrapper.</remarks>
        /// <seealso cref="ProxyStickyTargetSiteAttribute"/>
        public void OnRemoteServiceWrapperClosing(Guid siteId, Guid? serviceWrapperInstanceId)
        {
            foreach (var remoteAgencyManagingObject in _managingObjects.Values)
            {
                remoteAgencyManagingObject.OnServiceWrapperClosing(siteId, serviceWrapperInstanceId);
            }
        }

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

        /// <summary>
        /// Closes the proxy object.
        /// </summary>
        /// <param name="proxy">Proxy to be closed.</param>
        /// <returns>Result. <see langword="true"/> when instance is located and closed; <see langword="false"/> when instance is not found.</returns>
        public bool CloseProxy(object proxy)
        {
            var obj = proxy as IProxyCommunicate;
            if (obj == null)
                throw new ArgumentNullException(nameof(proxy), $"Argument {nameof(proxy)} is not set as a proxy object.");

            var instanceId = obj.InstanceId;
            return CloseInstance(instanceId);
        }

        /// <summary>
        /// Closes the proxy or service wrapper by instance id.
        /// </summary>
        /// <param name="instanceId">Instance id of the proxy or service wrapper to be closed.</param>
        /// <returns>Result. <see langword="true"/> when instance is located and closed; <see langword="false"/> when instance is not found.</returns>
        public bool CloseInstance(Guid instanceId)
        {
            if (_managingObjects.TryRemove(instanceId, out var removed))
            {
                removed.Dispose();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
