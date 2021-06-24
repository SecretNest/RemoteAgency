using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency.Injection.EventHelper
{
    /// <summary>
    /// Defines a helper class to be implanted into built assembly for event servicing in proxy.
    /// </summary>
    internal class ProxyEventHelper
    {
        private Dictionary<string, ProxyEventRouterBase> _routers = new ();

        /// <summary>
        /// Gets or sets the callback for a delegate which will be called while an event adding is requested.
        /// </summary>
        public SendTwoWayMessageCallback SendEventAddingMessageCallback { get; set; }

        /// <summary>
        /// Gets or sets the callback for a delegate which will be called while an event removing is requested.
        /// </summary>
        public SendTwoWayMessageCallback SendEventRemovingMessageCallback { get; set; }

        /// <summary>
        /// Gets or sets the callback for a delegate which will be called while a special command message need to be sent to a remote site without getting response.
        /// </summary>
        public SendOneWayMessageCallback SendOneWaySpecialCommandMessageCallback { get; set; }
        //Const.SpecialCommandProxyDisposed

        /// <summary>
        /// Gets or sets the callback for a delegate which will be called while an empty message need to be created.
        /// </summary>
        public CreateEmptyMessageCallback CreateEmptyMessageCallback { get; set; }

        /// <summary>
        /// Unlinks specified remote service wrapper from the event registered in proxy objects when the service wrapper is closing.
        /// </summary>
        /// <param name="siteId">The site id of the instance of the Remote Agency which managing the closing service wrapper.</param>
        /// <param name="serviceWrapperInstanceId">The instance id of the closing service wrapper. When set to <see langword="null"/>, all proxies with sticky target site specified by <paramref name="siteId" /> will be reset. Default value is null.</param>
        /// <exception cref="AggregateException">When exceptions occurred.</exception>
        public void OnRemoteServiceWrapperClosing(Guid siteId, Guid? serviceWrapperInstanceId = null)
        {
            foreach (var router in _routers.Values)
            {
                router.OnRemoteServiceWrapperClosing(siteId, serviceWrapperInstanceId);
            }
        }

        /// <summary>
        /// Sends messages to all relevant objects and closes the functions of this object.
        /// </summary>
        /// <param name="sendSpecialCommand">Whether need to send special command.</param>
        /// <exception cref="AggregateException">When exceptions occurred.</exception>
        public void CloseRequestedByManagingObject(bool sendSpecialCommand)
        {
            if (sendSpecialCommand)
            {
                var targets = new Dictionary<Guid, HashSet<Guid>>(); //site id, instance id
                foreach (var router in _routers.Values)
                foreach (var id in router.GetTargetSiteIdAndInstanceIdThenClose())
                {
                    if (!targets.TryGetValue(id.Item1, out var hash))
                    {
                        hash = new HashSet<Guid>();
                        targets[id.Item1] = hash;
                    }

                    hash.Add(id.Item2);
                }

                _routers.Clear();
                _routers = null;

                var ids = targets.SelectMany(s => s.Value.Select(i => new Tuple<Guid, Guid>(s.Key, i))).ToArray();

                var tasks = Array.ConvertAll(ids, i => Task.Run(() =>
                {
                    var message = CreateEmptyMessageCallback();
                    message.AssetName = Const.SpecialCommandProxyDisposed;
                    message.TargetSiteId = i.Item1;
                    message.TargetInstanceId = i.Item2;
                    SendOneWaySpecialCommandMessageCallback(message);
                }));

                Task.WaitAll(tasks); //AggregateException
            }
            else
            {
                foreach (var router in _routers.Values)
                {
                    router.Close();
                }
                _routers.Clear();
                _routers = null;
            }
        }

        /// <summary>
        /// Adds a builder.
        /// </summary>
        /// <param name="router">An instance of a derived class of ProxyEventRouterBase.</param>
        public void AddRouter(ProxyEventRouterBase router)
        {
            _routers[router.AssetName] = router;
            router.ProxyEventHelper = this;
        }

        /// <summary>
        /// Processes an event raising message and returns response.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        /// <param name="exception">Exception thrown while running user code.</param>
        /// <param name="localExceptionHandlingMode">Local exception handling mode.</param>
        /// <returns>Message contains the data to be returned.</returns>
        public IRemoteAgencyMessage ProcessEventRaisingMessage(IRemoteAgencyMessage message,
            out Exception exception, out LocalExceptionHandlingMode localExceptionHandlingMode)
        {
            if (_routers.TryGetValue(message.AssetName, out var router))
            {
                localExceptionHandlingMode = router.LocalExceptionHandlingMode;
                return router.ProcessEventRaisingMessage(message, out exception);
            }
            else
            {
                localExceptionHandlingMode = LocalExceptionHandlingMode.Redirect;
                var result = CreateEmptyMessageCallback();
                //for sending a feedback, no property of message need to be assigned here.

                exception = new AssetNotFoundException(message);

                return result;
            }
        }

        /// <summary>
        /// Processes an event raising message.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        /// <param name="localExceptionHandlingMode">Local exception handling mode.</param>
        public virtual void ProcessOneWayEventRaisingMessage(IRemoteAgencyMessage message,
            out LocalExceptionHandlingMode localExceptionHandlingMode)
        {
            if (_routers.TryGetValue(message.AssetName, out var router))
            {
                localExceptionHandlingMode = router.LocalExceptionHandlingMode;
                router.ProcessOneWayEventRaisingMessage(message);
            }
            else
            {
                localExceptionHandlingMode = LocalExceptionHandlingMode.Redirect;
            }
        }
    }
}
