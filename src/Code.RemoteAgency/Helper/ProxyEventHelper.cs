using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency.Helper
{
    /// <summary>
    /// Defines a helper class to be implanted into built assembly for event servicing in proxy.
    /// </summary>
    public class ProxyEventHelper
    {
        private Dictionary<string, ProxyEventRouterBase> _routers = new Dictionary<string, ProxyEventRouterBase>();

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
        /// <param name="serviceWrapperInstanceId">The instance id of the closing service wrapper. When set as <see langword="null"/>, all proxies with sticky target site specified by <paramref name="siteId" /> will be reset. Default value is null.</param>
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
                Dictionary<Guid, HashSet<Guid>> targets = new Dictionary<Guid, HashSet<Guid>>(); //site id, instance id
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

    /// <summary>
    /// Defines a helper class to be implanted into built assembly for handling an event handler in proxy. This is an abstract class.
    /// </summary>
    public abstract class ProxyEventRouterBase
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
        /// <param name="serviceWrapperInstanceId">The instance id of the closing service wrapper. When set as <see langword="null"/>, all proxies with sticky target site specified by <paramref name="siteId" /> will be reset. Default value is null.</param>
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

    /// <summary>
    /// Defines a helper class to be implanted into built assembly for handling an event handler in proxy. This is an abstract class.
    /// </summary>
    /// <typeparam name="TDelegate">Delegate of event.</typeparam>
    public abstract class ProxyEventRouterBase<TDelegate>: ProxyEventRouterBase
    {
        private readonly int _addingTimeout, _removingTimeout;

        private List<EventTarget> _targetSiteIdAndInstanceId = new List<EventTarget>();

        class EventTarget
        {
            public Guid SiteId;
            public Guid InstanceId;
            public bool TargetDisposed;
            public TDelegate Delegate;
        }

        /// <summary>
        /// Initializes an instance of ProxyEventRouterBase.
        /// </summary>
        /// <param name="addingTimeout">Timeout for waiting for the response of event adding.</param>
        /// <param name="removingTimeout">Timeout for waiting for the response of event removing.</param>
        protected ProxyEventRouterBase(int addingTimeout, int removingTimeout)
        {
            _addingTimeout = addingTimeout;
            _removingTimeout = removingTimeout;
        }

        /// <summary>
        /// Processes an event adding.
        /// </summary>
        /// <param name="value">Handler.</param>
        public void ProcessEventAdding(TDelegate value)
        {
            var message = ProxyEventHelper.CreateEmptyMessageCallback();
            message.AssetName = AssetName;

            var response = ProxyEventHelper.SendEventAddingMessageCallback(message, _addingTimeout);
            if (response.Exception != null)
                throw response.Exception;
            else
            {
                lock (_targetSiteIdAndInstanceId)
                {
                    _targetSiteIdAndInstanceId.Add(new EventTarget()
                        {SiteId = response.SenderSiteId, InstanceId = response.SenderInstanceId, Delegate = value});
                }
            }
        }

        /// <summary>
        /// Processes an event removing.
        /// </summary>
        /// <param name="value">Handler.</param>
        public void ProcessEventRemoving(TDelegate value)
        {
            lock (_targetSiteIdAndInstanceId)
            {
                // ReSharper disable once ReplaceWithSingleCallToFirstOrDefault
                var target = _targetSiteIdAndInstanceId.Where(i => i.Delegate.Equals(value)).FirstOrDefault();
                if (target == null)
                    return;
                if (!target.TargetDisposed)
                {
                    var message = ProxyEventHelper.CreateEmptyMessageCallback();
                    message.AssetName = AssetName;
                    message.TargetSiteId = target.SiteId;
                    message.TargetInstanceId = target.InstanceId;
                    var response = ProxyEventHelper.SendEventRemovingMessageCallback(message, _removingTimeout);
                    if (response.Exception != null)
                        throw response.Exception;
                }

                _targetSiteIdAndInstanceId.Remove(target);
            }
        }

        /// <inheritdoc />
        public sealed override void OnRemoteServiceWrapperClosing(Guid siteId, Guid? serviceWrapperInstanceId = null)
        {
            lock (_targetSiteIdAndInstanceId)
            {
                if (serviceWrapperInstanceId.HasValue)
                {
                    foreach (var item in _targetSiteIdAndInstanceId)
                    {
                        if (item.SiteId == siteId && item.InstanceId == serviceWrapperInstanceId)
                        {
                            item.TargetDisposed = true;
                        }
                    }
                }
                else
                {
                    foreach (var item in _targetSiteIdAndInstanceId)
                    {
                        if (item.SiteId == siteId)
                        {
                            item.TargetDisposed = true;
                        }
                    }
                }
            }
        }

        /// <inheritdoc />
        public sealed override List<Tuple<Guid, Guid>> GetTargetSiteIdAndInstanceIdThenClose()
        {
            List<Tuple<Guid, Guid>> result;
            lock (_targetSiteIdAndInstanceId)
            {
                result = _targetSiteIdAndInstanceId.Where(i => !i.TargetDisposed)
                    .Select(i => new Tuple<Guid, Guid>(i.SiteId, i.InstanceId)).ToList();
            }
            Close();
            return result;
        }

        /// <inheritdoc />
        public sealed override void Close()
        {
            lock (_targetSiteIdAndInstanceId)
            {
                _targetSiteIdAndInstanceId.Clear();
            }

            // ReSharper disable once InconsistentlySynchronizedField
            _targetSiteIdAndInstanceId = null;

            ProxyEventHelper = null;
        }
    }

    /// <summary>
    /// Defines a helper class to be implanted into built assembly for handling an one-way event handler in proxy. This is an abstract class.
    /// </summary>
    /// <typeparam name="TDelegate">Delegate of event.</typeparam>
    /// <typeparam name="TParameterEntity">Parameter entity type.</typeparam>
    public abstract class ProxyEventRouterBase<TDelegate, TParameterEntity> : ProxyEventRouterBase<TDelegate>
        where TParameterEntity : IRemoteAgencyMessage
    {
        /// <summary>
        /// Initializes an instance of ProxyEventRouterBase.
        /// </summary>
        /// <param name="addingTimeout">Timeout for waiting for the response of event adding.</param>
        /// <param name="removingTimeout">Timeout for waiting for the response of event removing.</param>
        protected ProxyEventRouterBase(int addingTimeout, int removingTimeout) : base(addingTimeout, removingTimeout)
        {
        }

        /// <inheritdoc />
        public sealed override void ProcessOneWayEventRaisingMessage(IRemoteAgencyMessage message)
        {
            Process((TParameterEntity) message);
        }

        private protected abstract void Process(TParameterEntity message);
    }

    /// <summary>
    /// Defines a helper class to be implanted into built assembly for handling an two-way event handler in proxy. This is an abstract class.
    /// </summary>
    /// <typeparam name="TDelegate">Delegate of event.</typeparam>
    /// <typeparam name="TParameterEntity">Parameter entity type.</typeparam>
    /// <typeparam name="TReturnValueEntity">Return value entity type.</typeparam>
    public abstract class
        ProxyEventRouterBase<TDelegate, TParameterEntity, TReturnValueEntity> : ProxyEventRouterBase<TDelegate>
        where TParameterEntity : IRemoteAgencyMessage
        where TReturnValueEntity : IRemoteAgencyMessage
    {
        private readonly int _timeout;

        /// <summary>
        /// Initialize an instance of ProxyEventRouterBase.
        /// </summary>
        /// <param name="addingTimeout">Timeout for waiting for the response of event adding.</param>
        /// <param name="removingTimeout">Timeout for waiting for the response of event removing.</param>
        /// <param name="raisingTimeout">Timeout for waiting for the response of event raising.</param>
        protected ProxyEventRouterBase(int addingTimeout, int removingTimeout, int raisingTimeout) : base(addingTimeout,
            removingTimeout)
        {
            _timeout = raisingTimeout;
        }

        /// <inheritdoc />
        public sealed override IRemoteAgencyMessage ProcessEventRaisingMessage(IRemoteAgencyMessage message,
            out Exception exception)
        {
            var response = (IRemoteAgencyMessage) Process((TParameterEntity) message, out exception);
            response.AssetName = AssetName;
            return response;
        }

        /// <inheritdoc />
        public override void ProcessOneWayEventRaisingMessage(IRemoteAgencyMessage message)
        {
            Process((TParameterEntity) message, out var exception);
            if (exception != null)
                throw exception;
        }

        private protected abstract TReturnValueEntity Process(TParameterEntity message, out Exception exception);
    }
}
