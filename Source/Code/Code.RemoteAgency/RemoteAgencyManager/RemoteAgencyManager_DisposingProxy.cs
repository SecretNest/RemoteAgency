using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretNest.RemoteAgency
{
    public partial class RemoteAgencyManager<TNetworkMessage, TSerialized, TEntityBase> : IDisposable where TEntityBase : class
    {
        ConcurrentDictionary<Guid, Tuple<Guid, Guid>> disposingRequiredProxies = new ConcurrentDictionary<Guid, Tuple<Guid, Guid>>(); //proxyInstanceId, siteId+serviceWrapperInstanceId;

        /// <summary>
        /// Occurs when a proxy, which needs sending disposing message, is found.
        /// </summary>
        public event EventHandler<DisposingMessageRequiredProxyEventArgs> DisposingMessageRequiredProxyAdded;

        /// <summary>
        /// Occurs when a proxy, which needs sending disposing message, is removed.
        /// </summary>
        public event EventHandler<DisposingMessageRequiredProxyEventArgs> DisposingMessageRequiredProxyRemoved;

        void OnDisposingMessageRequiredProxyAdded(Guid siteId, Guid proxyInstanceId, Guid serviceWrapperInstanceId)
        {
            if (disposingRequiredProxies.TryAdd(proxyInstanceId, new Tuple<Guid, Guid>(siteId, serviceWrapperInstanceId)))
            {
                DisposingMessageRequiredProxyAdded?.Invoke(this, new DisposingMessageRequiredProxyEventArgs(siteId, proxyInstanceId));
            }
        }

        void OnDisposingMessageRequiredProxyRemoved(Guid siteId, Guid proxyInstanceId)
        {
            if (disposingRequiredProxies.TryRemove(proxyInstanceId, out _))
            {
                DisposingMessageRequiredProxyRemoved?.Invoke(this, new DisposingMessageRequiredProxyEventArgs(siteId, proxyInstanceId));
            }
        }

        /// <summary>
        /// Should be called when a proxy disposing happened without message sent through <see cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}.ProcessPackagedMessage(TNetworkMessage)"/>.
        /// </summary>
        /// <param name="instanceId">Instance id of the proxy.</param>
        public void OnProxyDisposed(Guid instanceId)
        {
            if (disposingRequiredProxies.TryRemove(instanceId, out var values))
            {
                if (managingObjects.TryGetValue(values.Item2, out var managingObject))
                {
                    managingObject.TargetProxyDisposed(instanceId);
                }
                DisposingMessageRequiredProxyRemoved?.Invoke(this, new DisposingMessageRequiredProxyEventArgs(values.Item1, instanceId));
            }
        }

        /// <summary>
        /// Should be called when disposing of all proxies of a site happened without message sent through <see cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}.ProcessPackagedMessage(TNetworkMessage)"/>.
        /// </summary>
        /// <param name="siteId">Id of the site.</param>
        public void OnProxiesOfSiteDisposed(Guid siteId)
        {
            foreach(var instanceId in disposingRequiredProxies.Where(i=>i.Value.Item1 == siteId).Select(i=>i.Key))
            {
                OnProxyDisposed(instanceId);
            }
        }

        /// <summary>
        /// Gets all proxies id which needs sending disposing message.
        /// </summary>
        /// <returns>All proxies id which needs sending disposing message.</returns>
        public IEnumerable<DisposingMessageRequiredProxyQueryResult> GetAllDisposingRequiredProxies()
        {
            return disposingRequiredProxies.Select(i => new DisposingMessageRequiredProxyQueryResult(i.Value.Item1, i.Key));
        }
    }

    /// <summary>
    /// Represents an event argument of <see cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}.DisposingMessageRequiredProxyAdded"/> or <see cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}.DisposingMessageRequiredProxyRemoved"/>.
    /// </summary>
    public class DisposingMessageRequiredProxyEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the site id of the proxy.
        /// </summary>
        public Guid SiteId { get; }

        /// <summary>
        /// Gets the instance id of the proxy.
        /// </summary>
        public Guid InstanceId { get; }

        /// <summary>
        /// Initializes an instance of the DisposingMessageRequiredProxyEventArgs.
        /// </summary>
        /// <param name="siteId">Site id of the proxy.</param>
        /// <param name="instanceId">Instance id of the proxy.</param>
        public DisposingMessageRequiredProxyEventArgs(Guid siteId, Guid instanceId)
        {
            SiteId = siteId;
            InstanceId = instanceId;
        }
    }

    /// <summary>
    /// Represents a record of the result of <see cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}.GetAllDisposingRequiredProxies"/>.
    /// </summary>
    public class DisposingMessageRequiredProxyQueryResult
    {
        /// <summary>
        /// Gets the site id of the proxy.
        /// </summary>
        public Guid SiteId { get; }

        /// <summary>
        /// Gets the instance id of the proxy.
        /// </summary>
        public Guid InstanceId { get; }

        /// <summary>
        /// Initializes an instance of the DisposingMessageRequiredProxyEventArgs.
        /// </summary>
        /// <param name="siteId">Site id of the proxy.</param>
        /// <param name="instanceId">Instance id of the proxy.</param>
        public DisposingMessageRequiredProxyQueryResult(Guid siteId, Guid instanceId)
        {
            SiteId = siteId;
            InstanceId = instanceId;
        }
    }
}
