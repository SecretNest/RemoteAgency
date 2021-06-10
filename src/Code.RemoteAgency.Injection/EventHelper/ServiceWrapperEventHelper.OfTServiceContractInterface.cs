using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretNest.RemoteAgency.Injection.EventHelper
{
    /// <summary>
    /// Defines a helper class to be implanted into built assembly for event servicing in service wrapper.
    /// </summary>
    /// <typeparam name="TServiceContractInterface">Service contract interface type.</typeparam>
    internal class ServiceWrapperEventHelper<TServiceContractInterface>
    {
        /// <summary>
        /// Gets or sets the service object.
        /// </summary>
        public TServiceContractInterface ServiceObject { get; set; }

        //siteid, instanceid, asset, routers
        Dictionary<Guid, Dictionary<Guid, Dictionary<string, List<ServiceWrapperEventRouterBase<TServiceContractInterface>>>>> _routers = new ();

        /// <summary>
        /// Processes an event adding.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        public void ProcessEventAddingMessage(IRemoteAgencyMessage message)
        {
            if (_builders.TryGetValue(message.AssetName, out var builder))
            {
                var router = builder();
                router.SendEventMessageCallback = SendEventMessageCallback;
                router.SendOneWayEventMessageCallback = SendOneWayEventMessageCallback;
                router.AssetName = message.AssetName;
                router.RemoteSiteId = message.SenderSiteId;
                router.RemoteInstanceId = message.SenderInstanceId;
                try
                {
                    router.AddHandler(ServiceObject);
                }
                catch 
                {
                    router.SendEventMessageCallback = null;
                    router.SendOneWayEventMessageCallback = null;
                    throw;
                }

                //adding router
                lock (_routers)
                {
                    if (!_routers.TryGetValue(message.SenderSiteId, out var routersPerSite))
                    {
                        routersPerSite = new Dictionary<Guid, Dictionary<string, List<ServiceWrapperEventRouterBase<TServiceContractInterface>>>>();
                        _routers[message.SenderSiteId] = routersPerSite;
                    }
                    if (!routersPerSite.TryGetValue(message.SenderInstanceId, out var routerPerInstance))
                    {
                        routerPerInstance = new Dictionary<string, List<ServiceWrapperEventRouterBase<TServiceContractInterface>>>();
                        routersPerSite[message.SenderInstanceId] = routerPerInstance;
                    }
                    if (!routerPerInstance.TryGetValue(message.AssetName, out var routerPerAsset))
                    {
                        routerPerAsset = new List<ServiceWrapperEventRouterBase<TServiceContractInterface>>();
                        routerPerInstance[message.AssetName] = routerPerAsset;
                    }
                    routerPerAsset.Add(router);
                }
            }
            else
            {
                throw new AssetNotFoundException(message);
            }
        }

        /// <summary>
        /// Processes an event removing.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        public void ProcessEventRemovingMessage(IRemoteAgencyMessage message)
        {
            lock (_routers)
            {
                if (!_routers.TryGetValue(message.SenderSiteId, out var routersPerSite))
                {
                    return;
                }
                if (!routersPerSite.TryGetValue(message.SenderInstanceId, out var routerPerInstance))
                {
                    return;
                }
                if (!routerPerInstance.TryGetValue(message.AssetName, out var routerPerAsset))
                {
                    return;
                }

                var router = routerPerAsset[0];

                try
                {
                    router.RemoveHandler(ServiceObject);
                }
                finally
                {
                    router.SendEventMessageCallback = null;
                    router.SendOneWayEventMessageCallback = null;

                    if (routerPerAsset.Count > 1)
                    {
                        routerPerAsset.RemoveAt(0);
                    }
                    else if (routerPerInstance.Count > 1)
                    {
                        routerPerInstance.Remove(message.AssetName);
                    }
                    else if (routersPerSite.Count > 1)
                    {
                        routersPerSite.Remove(message.SenderInstanceId);
                    }
                    else
                    {
                        _routers.Remove(message.SenderSiteId);
                    }
                }
            }
        }

        /// <summary>
        /// Unlinks specified remote proxy from the event registered in service wrapper objects.
        /// </summary>
        /// <param name="siteId">The site id of the instance of the Remote Agency which managing the closing proxy.</param>
        /// <param name="proxyInstanceId">The instance id of the closing proxy. When set to null, all proxies from the site specified by <paramref name="siteId" /> will be unlinked.</param>
        /// <exception cref="AggregateException">When exceptions occurred.</exception>
        public void OnRemoteProxyClosing(Guid siteId, Guid? proxyInstanceId)
        {
            lock (_routers)
            {
                if (!_routers.TryGetValue(siteId, out var routersPerSite))
                {
                    return;
                }

                if (proxyInstanceId.HasValue)
                {
                    if (routersPerSite.TryGetValue(proxyInstanceId.Value, out var routerPerInstance))
                    {
                        var exceptions = new List<Exception>();
                        RemoveRouterPerInstance(routerPerInstance, exceptions);

                        if (routersPerSite.Count > 1)
                        {
                            routersPerSite.Remove(proxyInstanceId.Value);
                        }
                        else
                        {
                            _routers.Remove(siteId);
                        }

                        if (exceptions.Count > 0)
                        {
                            throw new AggregateException(exceptions);
                        }
                    }
                }
                else
                {
                    var exceptions = new List<Exception>();
                    foreach (var value in routersPerSite.Values)
                    {
                        RemoveRouterPerInstance(value, exceptions);
                    }

                    _routers.Remove(siteId);
                    if (exceptions.Count > 0)
                    {
                        throw new AggregateException(exceptions);
                    }
                }
            }
        }

        void RemoveRouterPerInstance(
            Dictionary<string, List<ServiceWrapperEventRouterBase<TServiceContractInterface>>> routerPerInstance,
            List<Exception> exceptions)
        {
            foreach (var router in routerPerInstance.Values.SelectMany(i => i))
            {
                try
                {
                    router.CloseRequestedByManagingObject(ServiceObject);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
#pragma warning restore CA1031 // Do not catch general exception types
            }
        }

        /// <summary>
        /// Sends messages to all relevant objects and closes the functions of this object.
        /// </summary>
        /// <param name="sendSpecialCommand">Whether need to send special command.</param>
        /// <exception cref="AggregateException">When exceptions occurred.</exception>
        public void CloseRequestedByManagingObject(bool sendSpecialCommand)
        {
            _builders.Clear();
            _builders = null;

            var exceptions = new List<Exception>();
            Guid[] sites;

            lock (_routers)
            {
                if (sendSpecialCommand)
                    sites = _routers.Keys.ToArray();
                else
                    sites = null;

                foreach (var i in _routers.Values)
                foreach (var j in i.Values)
                    RemoveRouterPerInstance(j, exceptions);
                _routers = null;
            }

            SendEventMessageCallback = null;
            SendOneWayEventMessageCallback = null;

            if (sendSpecialCommand)
            {
                var tasks = Array.ConvertAll(sites,
                    siteId => Task.Run(() =>
                    {
                        var message = CreateEmptyMessageCallback();
                        message.AssetName = Const.SpecialCommandServiceWrapperDisposed;
                        message.TargetInstanceId = siteId;
                        SendOneWaySpecialCommandMessageCallback(message);
                    }));
                try
                {
                    Task.WaitAll(tasks);
                }
                catch (AggregateException e)
                {
                    exceptions.AddRange(e.InnerExceptions);
                }
            }

            CreateEmptyMessageCallback = null;
            SendOneWaySpecialCommandMessageCallback = null;

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }

        /// <summary>
        /// Gets or sets the callback for a delegate which will be called while an event raising message need to be sent to a remote site and get response of it.
        /// </summary>
        public SendTwoWayMessageCallback SendEventMessageCallback { get; set; }

        /// <summary>
        /// Gets or sets the callback for a delegate which will be called while an event raising message need to be sent to a remote site without getting response.
        /// </summary>
        public SendOneWayMessageCallback SendOneWayEventMessageCallback { get; set; }

        /// <summary>
        /// Gets or sets the callback for a delegate which will be called while a special command message need to be sent to a remote site without getting response.
        /// </summary>
        public SendOneWayMessageCallback SendOneWaySpecialCommandMessageCallback { get; set; }
        //Const.SpecialCommandServiceWrapperDisposed

        /// <summary>
        /// Gets or sets the callback for a delegate which will be called while an empty message need to be created.
        /// </summary>
        private CreateEmptyMessageCallback CreateEmptyMessageCallback { get; set; }

        private Dictionary<string, Func<ServiceWrapperEventRouterBase<TServiceContractInterface>>> _builders = new ();

        /// <summary>
        /// Adds a builder callback.
        /// </summary>
        /// <param name="assetName">Name of the event.</param>
        /// <param name="callback">Callback for creating an instance of a derived class of EventRouterBase.</param>
        public void AddBuilder(string assetName, Func<ServiceWrapperEventRouterBase<TServiceContractInterface>> callback)
        {
            _builders[assetName] = callback;
        }
    }
}
