using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.EventHelper
{
    /// <summary>
    /// Defines a helper class to be implanted into built assembly for event servicing in service wrapper.
    /// </summary>
    public class ServiceWrapperEventHelper
    {
        /// <summary>
        /// Gets or sets the service object.
        /// </summary>
        public object ServiceObject { get; set; }

        Dictionary<Guid, Dictionary<Guid, Dictionary<string, List<EventRouterBase>>>> _routers
            = new Dictionary<Guid, Dictionary<Guid, Dictionary<string, List<EventRouterBase>>>>();

        /// <summary>
        /// Processes an event adding.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        public void ProcessEventAddingMessage(IRemoteAgencyMessage message)
        {
            if (_builders.TryGetValue(message.AssetName, out var builder))
            {
                var router = builder(ServiceObject);
                router.SendEventMessageCallback = SendEventMessageCallback;
                router.SendOneWayEventMessageCallback = SendOneWayEventMessageCallback;
                router.AssetName = message.AssetName;
                router.RemoteSiteId = message.SenderSiteId;
                router.RemoteInstanceId = message.SenderInstanceId;
                try
                {
                    router.AddHandler();
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
                        routersPerSite = new Dictionary<Guid, Dictionary<string, List<EventRouterBase>>>();
                        _routers[message.SenderSiteId] = routersPerSite;
                    }
                    if (!routersPerSite.TryGetValue(message.SenderInstanceId, out var routerPerInstance))
                    {
                        routerPerInstance = new Dictionary<string, List<EventRouterBase>>();
                        routersPerSite[message.SenderInstanceId] = routerPerInstance;
                    }
                    if (!routerPerInstance.TryGetValue(message.AssetName, out var routerPerAsset))
                    {
                        routerPerAsset = new List<EventRouterBase>();
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
                    router.RemoveHandler();
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
                        List<Exception> exceptions = new List<Exception>();
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
                    List<Exception> exceptions = new List<Exception>();
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

        void RemoveRouterPerInstance(Dictionary<string, List<EventRouterBase>> routerPerInstance, List<Exception> exceptions)
        {
            foreach(var i in routerPerInstance.Values)
            foreach (var router in i)
            {
                try
                {
                    router.RemoveHandler();
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
                finally
                {
                    router.SendEventMessageCallback = null;
                    router.SendOneWayEventMessageCallback = null;
                }
            }
        }

        /// <summary>
        /// Sends messages to all relevant objects and closes the functions of this object.
        /// </summary>
        /// <exception cref="AggregateException">When exceptions occurred.</exception>
        public void CloseRequestedByManagingObject()
        {
            _builders.Clear();
            _builders = null;

            List<Exception> exceptions = new List<Exception>();

            lock (_routers)
            {
                foreach (var i in _routers.Values)
                foreach (var j in i.Values)
                    RemoveRouterPerInstance(j, exceptions);
                _routers = null;
            }

            if (exceptions.Count > 0)
            {
                throw new AggregateException(exceptions);
            }
        }

        /// <summary>
        /// Will be called while an event raising message need to be sent to a remote site and get response of it.
        /// </summary>
        public SendTwoWayMessageCallback SendEventMessageCallback { get; set; }

        /// <summary>
        /// Will be called while an event raising message need to be sent to a remote site without getting response.
        /// </summary>
        public SendOneWayMessageCallback SendOneWayEventMessageCallback { get; set; }

        private Dictionary<string, Func<object, EventRouterBase>> _builders = new Dictionary<string, Func<object, EventRouterBase>>();

        /// <summary>
        /// Adds a builder callback.
        /// </summary>
        /// <param name="assetName">Name of the event.</param>
        /// <param name="callback">Callback for creating an instance of a derived class of EventRouterBase.</param>
        public void AddBuilder(string assetName, Func<object, EventRouterBase> callback)
        {
            _builders[assetName] = callback;
        }
    }

    /// <summary>
    /// Defines a helper class to be implanted into built assembly for handling an event handler in service wrapper. This is an abstract class.
    /// </summary>
    public abstract class EventRouterBase
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
        /// Will be called while an event raising message need to be sent to a remote site and get response of it.
        /// </summary>
        public SendTwoWayMessageCallback SendEventMessageCallback { get; set; }

        /// <summary>
        /// Will be called while an event raising message need to be sent to a remote site without getting response.
        /// </summary>
        public SendOneWayMessageCallback SendOneWayEventMessageCallback { get; set; }

        /// <summary>
        /// Sends message to relevant object and closes the functions of this object.
        /// </summary>
        public void CloseRequestedByManagingObject()
        {
            try
            {
                RemoveHandler();
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
        public abstract void RemoveHandler();

        /// <summary>
        /// Adds the handler.
        /// </summary>
        public abstract void AddHandler();

    }

    /// <summary>
    /// Defines a helper class to be implanted into built assembly for handling an two way event handler in service wrapper. This is an abstract class.
    /// </summary>
    public abstract class TwoWayEventRouterBase : EventRouterBase
    {

    }
    
    /// <summary>
    /// Defines a helper class to be implanted into built assembly for handling an one way event handler in service wrapper. This is an abstract class.
    /// </summary>
    public abstract class OneWayEventRouterBase : EventRouterBase
    {

    }
}
