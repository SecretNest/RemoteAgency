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
        /// <param name="proxyInstanceId">The instance id of the closing proxy. When set as null, all proxies from the site specified by <paramref name="siteId" /> will be unlinked. Default value is null.</param>
        /// <remarks>
        /// <para>A message to notify the closing is sent automatically while remote proxy closing. Call this method manually if exception is thrown while processing the closing of the remote proxy.</para>
        /// <para>Service wrapper manages links of all proxies which need to handle events. When remote proxy is closed, message for removing event handlers is sent to the service wrapper. But when something wrong happened, network disconnected or proxy crashed for example, the crucial messages may not be able to transferred correctly. In this case, this method need to be called, or the obsolete links will stay in service wrapper which may cause lags or exceptions while processing events.</para></remarks>
        public abstract void OnRemoteProxyClosing(Guid siteId, Guid? proxyInstanceId = null);

        /// <summary>
        /// Resets sticky target site of all affected proxies and unlinks specified remote service wrapper from the event registered in proxy objects when the service wrapper is closing.
        /// </summary>
        /// <param name="siteId">The site id of the instance of the Remote Agency which managing the closing service wrapper.</param>
        /// <param name="serviceWrapperInstanceId">The instance id of the closing service wrapper. When set as <see langword="null"/>, all proxies with sticky target site specified by <paramref name="siteId" /> will be reset. Default value is <see langword="null"/>.</param>
        /// <remarks>
        /// <para>A message to notify the closing is sent automatically while remote service wrapper closing. Call this method manually if exception is thrown while processing the closing of the remote service wrapper.</para>
        /// <para>Proxy manages links of all service wrappers which is registered as an event raiser. When remote service wrapper is closed, message for removing the link of the event handler is sent to the proxy. But when something wrong happened, network disconnected or service wrapper crashed for example, the crucial messages may not be able to transferred correctly. In this case, this method need to be called, or the obsolete links will stay in proxy which may cause lags or exceptions while processing events.</para>
        /// <para>Also, the sticky target site will be reset for the proxies which is pointing to this service wrapper.</para>
        /// </remarks>
        /// <seealso cref="ProxyStickyTargetSiteAttribute"/>
        public abstract void OnRemoteServiceWrapperClosing(Guid siteId, Guid? serviceWrapperInstanceId);

        /// <summary>
        /// Resets sticky target site of the proxy specified.
        /// </summary>
        /// <param name="proxy">Proxy to be reset.</param>
        /// <remarks>Proxy sticky site will be automatically reset when the target remote site is closing. Call this method manually if exception is thrown while processing the closing of the target service wrapper, or while looking forward to choosing another target site.</remarks>
        /// <seealso cref="ProxyStickyTargetSiteAttribute"/>
        public void ResetProxyStickyTargetSite(object proxy)
        {
            var obj = proxy as IProxyCommunicate;
            if (obj == null)
                throw new ArgumentNullException(nameof(proxy), $"Argument {nameof(proxy)} is not set as a proxy object.");

            obj.ProxyStickyTargetSiteResetCallback();
        }

        /// <summary>
        /// Resets sticky target site of the proxy specified.
        /// </summary>
        /// <param name="instanceId">Instance id of the proxy to be queried.</param>
        /// <remarks>Proxy sticky site will be automatically reset when the target remote site is closing. Call this method manually if exception is thrown while processing the disposing of the target service wrapper.</remarks>
        /// <seealso cref="ProxyStickyTargetSiteAttribute"/>
        public abstract void ResetProxyStickyTargetSite(Guid instanceId);

        /// <summary>
        /// Queries the proxy sticky target site setting state.
        /// </summary>
        /// <param name="proxy">Proxy to be reset.</param>
        /// <param name="isEnabled">Will be set as whether this function is enabled on this proxy.</param>
        /// <param name="defaultTargetSiteId">Will be set as default target site id.</param>
        /// <param name="stickyTargetSiteId">Will be set as sticky target site id. Value will be set as <see langword="null"/> if no sticky target set yet.</param>
        public void ProxyStickyTargetSiteQuery(object proxy, out bool isEnabled, out Guid defaultTargetSiteId,
            out Guid? stickyTargetSiteId)
        {
            var obj = proxy as IProxyCommunicate;
            if (obj == null)
                throw new ArgumentNullException(nameof(proxy), $"Argument {nameof(proxy)} is not set as a proxy object.");

            obj.ProxyStickyTargetSiteQueryCallback(out isEnabled, out defaultTargetSiteId, out stickyTargetSiteId);
        }

        /// <summary>
        /// Queries the proxy sticky target site setting state.
        /// </summary>
        /// <param name="instanceId">Instance id of the proxy to be queried.</param>
        /// <param name="isEnabled">Will be set as whether this function is enabled on this proxy.</param>
        /// <param name="defaultTargetSiteId">Will be set as default target site id.</param>
        /// <param name="stickyTargetSiteId">Will be set as sticky target site id. Value will be set as <see langword="null"/> if no sticky target set yet.</param>
        public abstract void ProxyStickyTargetSiteQuery(Guid instanceId, out bool isEnabled, out Guid defaultTargetSiteId,
            out Guid? stickyTargetSiteId);
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

        /// <inheritdoc />
        public override void ResetProxyStickyTargetSite(Guid instanceId)
        {
            if (_managingObjects.TryGetValue(instanceId, out var managingObject))
            {
                var obj = managingObject as RemoteAgencyManagingObjectProxy<TEntityBase>;
                if (obj == null)
                {
                    throw new ArgumentException(nameof(instanceId), $"Object specified by argument {nameof(instanceId)} is not a proxy object.");
                }
                else
                {
                    obj.ResetProxyStickyTargetSite();
                }
            }
            else
            {
                throw new ArgumentException(nameof(instanceId), $"Object specified by argument {nameof(instanceId)} is not found.");
            }
        }

        /// <inheritdoc />
        public override void ProxyStickyTargetSiteQuery(Guid instanceId, out bool isEnabled, out Guid defaultTargetSiteId,
            out Guid? stickyTargetSiteId)
        {
            if (_managingObjects.TryGetValue(instanceId, out var managingObject))
            {
                var obj = managingObject as RemoteAgencyManagingObjectProxy<TEntityBase>;
                if (obj == null)
                {
                    throw new ArgumentException(nameof(instanceId), $"Object specified by argument {nameof(instanceId)} is not a proxy object.");
                }
                else
                {
                    obj.ProxyStickyTargetSiteQuery(out isEnabled, out defaultTargetSiteId, out stickyTargetSiteId);
                }
            }
            else
            {
                throw new ArgumentException(nameof(instanceId), $"Object specified by argument {nameof(instanceId)} is not found.");
            }
        }
    }
}
