using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretNest.RemoteAgency.Helper
{
    /// <summary>
    /// Defines a helper class to be implanted into built assembly for event servicing in service wrapper.
    /// </summary>
    /// <typeparam name="TServiceContractInterface">Service contract interface type.</typeparam>
    public class ServiceWrapperEventHelper<TServiceContractInterface>
    {
        /// <summary>
        /// Gets or sets the service object.
        /// </summary>
        public TServiceContractInterface ServiceObject { get; set; }

        //siteid, instanceid, asset, routers
        Dictionary<Guid, Dictionary<Guid, Dictionary<string, List<ServiceWrapperEventRouterBase<TServiceContractInterface>>>>> _routers
            = new Dictionary<Guid, Dictionary<Guid, Dictionary<string, List<ServiceWrapperEventRouterBase<TServiceContractInterface>>>>>();

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
        /// <param name="proxyInstanceId">The instance id of the closing proxy. When set as null, all proxies from the site specified by <paramref name="siteId" /> will be unlinked.</param>
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

        void RemoveRouterPerInstance(Dictionary<string, List<ServiceWrapperEventRouterBase<TServiceContractInterface>>> routerPerInstance, List<Exception> exceptions)
        {
            foreach(var i in routerPerInstance.Values)
            foreach (var router in i)
            {
                try
                {
                    router.CloseRequestedByManagingObject(ServiceObject);
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
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

        private Dictionary<string, Func<ServiceWrapperEventRouterBase<TServiceContractInterface>>> _builders = new Dictionary<string, Func<ServiceWrapperEventRouterBase<TServiceContractInterface>>>();

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

    /// <summary>
    /// Defines a helper class to be implanted into built assembly for handling an event handler in service wrapper. This is an abstract class.
    /// </summary>
    /// <typeparam name="TServiceContractInterface">Service contract interface type.</typeparam>
    public abstract class ServiceWrapperEventRouterBase<TServiceContractInterface>
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

    /// <summary>
    /// Defines a helper class to be implanted into built assembly for handling an one-way event handler in service wrapper. This is an abstract class.
    /// </summary>
    /// <typeparam name="TServiceContractInterface">Service contract interface type.</typeparam>
    /// <typeparam name="TParameterEntity">Parameter entity type.</typeparam>
    public abstract class
        ServiceWrapperEventRouterBase<TServiceContractInterface, TParameterEntity> : ServiceWrapperEventRouterBase<
            TServiceContractInterface>
        where TParameterEntity : IRemoteAgencyMessage
    {
        private protected void SendMessage(TParameterEntity message)
        {
            SetMessageProperties(message);
            SendOneWayEventMessageCallback(message);
        }
    }

    /// <summary>
    /// Defines a helper class to be implanted into built assembly for handling a two-way event handler in service wrapper. This is an abstract class.
    /// </summary>
    /// <typeparam name="TServiceContractInterface">Service contract interface type.</typeparam>
    /// <typeparam name="TParameterEntity">Parameter entity type.</typeparam>
    /// <typeparam name="TReturnValueEntity">Return value entity type.</typeparam>
    public abstract class
        ServiceWrapperEventRouterBase<TServiceContractInterface, TParameterEntity, TReturnValueEntity> :
            ServiceWrapperEventRouterBase<TServiceContractInterface>
        where TParameterEntity : IRemoteAgencyMessage
        where TReturnValueEntity : IRemoteAgencyMessage
    {
        private readonly int _timeout;

        /// <summary>
        /// Initialize an instance of ServiceWrapperEventRouterBase.
        /// </summary>
        /// <param name="timeout">Timeout for waiting for the response of event raising.</param>
        protected ServiceWrapperEventRouterBase(int timeout)
        {
            _timeout = timeout;
        }

        private protected TReturnValueEntity SendMessageAndGetResponse(TParameterEntity message, out bool isEmptyMessage, out Exception exception)
        {
            SetMessageProperties(message);
            var response = SendEventMessageCallback(message, _timeout);
            var responseMessage = (IRemoteAgencyMessage) response;
            isEmptyMessage = responseMessage.IsEmptyMessage;
            exception = responseMessage.Exception;
            return (TReturnValueEntity) response;
        }
    }
}
