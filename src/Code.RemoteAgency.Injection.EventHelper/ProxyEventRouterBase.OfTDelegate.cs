using System;
using System.Collections.Generic;
using System.Linq;

namespace SecretNest.RemoteAgency.Injection.EventHelper
{
    /// <summary>
    /// Defines a helper class to be implanted into built assembly for handling an event handler in proxy. This is an abstract class.
    /// </summary>
    /// <typeparam name="TDelegate">Delegate of event.</typeparam>
    internal abstract class ProxyEventRouterBase<TDelegate>
        : ProxyEventRouterBase
    {
        private readonly int _addingTimeout, _removingTimeout;

        private List<EventTarget> _targetSiteIdAndInstanceId = new ();

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
}
