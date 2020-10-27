using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    abstract partial class RemoteAgencyManagingObject
    {
        public Guid InstanceId { get; }


        private Action<IRemoteAgencyMessage> _sendMessageToManagerCallback; //send message out
        private Action<Guid, string, Exception> _sendExceptionToManagerCallback; //redirect exception in user code, string is the asset name
        private CreateEmptyMessageCallback _createEmptyMessageCallback;
        protected int DefaultTimeOutTime { get; }
        private readonly Func<int> _getWaitingTimeForDisposingCallback; //get WaitingTimeForDisposing.

        #region Constructors

        private RemoteAgencyManagingObject(ref Guid instanceId,
            Action<IRemoteAgencyMessage> sendMessageToManagerCallback, Action<Guid, string, Exception> sendExceptionToManagerCallback,
            CreateEmptyMessageCallback createEmptyMessageCallback, int defaultTimeoutTime, Func<int> getWaitingTimeForDisposingCallback)
        {
            if (instanceId == Guid.Empty)
            {
                instanceId = Guid.NewGuid();
            }

            InstanceId = instanceId;
            _sendMessageToManagerCallback = sendMessageToManagerCallback;
            _sendExceptionToManagerCallback = sendExceptionToManagerCallback;
            _createEmptyMessageCallback = createEmptyMessageCallback;
            DefaultTimeOutTime = defaultTimeoutTime;
            _getWaitingTimeForDisposingCallback = getWaitingTimeForDisposingCallback;
        }

        protected RemoteAgencyManagingObject(ref Guid instanceId, ThreadLockMode threadLockMode,
            Action<IRemoteAgencyMessage> sendMessageToManagerCallback, Action<Guid, string, Exception> sendExceptionToManagerCallback,
            CreateEmptyMessageCallback createEmptyMessageCallback, int defaultTimeoutTime, Func<int> getWaitingTimeForDisposingCallback)
            : this(ref instanceId, sendMessageToManagerCallback, sendExceptionToManagerCallback,
                createEmptyMessageCallback, defaultTimeoutTime, getWaitingTimeForDisposingCallback)
        {
            InitializeThreadLock(threadLockMode);
        }

        protected RemoteAgencyManagingObject(ref Guid instanceId, string threadLockTaskSchedulerName,
            TryGetTaskSchedulerCallback tryGetTaskSchedulerCallback,
            Action<IRemoteAgencyMessage> sendMessageToManagerCallback, Action<Guid, string, Exception> sendExceptionToManagerCallback,
            CreateEmptyMessageCallback createEmptyMessageCallback, int defaultTimeoutTime, Func<int> getWaitingTimeForDisposingCallback)
            : this(ref instanceId, sendMessageToManagerCallback, sendExceptionToManagerCallback,
                createEmptyMessageCallback, defaultTimeoutTime, getWaitingTimeForDisposingCallback)
        {
            InitializeThreadLock(threadLockTaskSchedulerName, tryGetTaskSchedulerCallback);
        }

        #endregion

        public void Dispose(bool sendSpecialCommand)
        {
            try
            {
                CloseManagingObject(sendSpecialCommand);
            }
            finally
            {
                if (!WaitingForRespondersClosed())
                    ForceCloseAllResponders();
                DisposeThreadLock();
                _createEmptyMessageCallback = null;
                _sendMessageToManagerCallback = null;
                _sendExceptionToManagerCallback = null;
            }
        }

        protected IRemoteAgencyMessage CreateEmptyMessage()
            => _createEmptyMessageCallback();

        #region Virtual and abstract methods
        public virtual void OnProxyClosing(Guid siteId, Guid? proxyInstanceId)
        {
        }

        public virtual void OnServiceWrapperClosing(Guid siteId, Guid? serviceWrapperInstanceId)
        {
        }

        protected abstract void CloseManagingObject(bool sendSpecialCommand);

        #endregion
    }

    abstract partial class RemoteAgencyManagingObject<TEntityBase> : RemoteAgencyManagingObject
    {
        protected RemoteAgencyManagingObject(ref Guid instanceId, ThreadLockMode threadLockMode,
            Action<IRemoteAgencyMessage> sendMessageToManagerCallback, Action<Guid, string, Exception> sendExceptionToManagerCallback,
            CreateEmptyMessageCallback createEmptyMessageCallback, int defaultTimeoutTime, Func<int> getWaitingTimeForDisposingCallback)
            : base(ref instanceId, threadLockMode, sendMessageToManagerCallback, sendExceptionToManagerCallback,
                createEmptyMessageCallback, defaultTimeoutTime, getWaitingTimeForDisposingCallback)
        {
        }

        protected RemoteAgencyManagingObject(ref Guid instanceId, string threadLockTaskSchedulerName,
            TryGetTaskSchedulerCallback tryGetTaskSchedulerCallback,
            Action<IRemoteAgencyMessage> sendMessageToManagerCallback, Action<Guid, string, Exception> sendExceptionToManagerCallback,
            CreateEmptyMessageCallback createEmptyMessageCallback, int defaultTimeoutTime, Func<int> getWaitingTimeForDisposingCallback)
            : base(ref instanceId, threadLockTaskSchedulerName, tryGetTaskSchedulerCallback,
                sendMessageToManagerCallback, sendExceptionToManagerCallback,
                createEmptyMessageCallback, defaultTimeoutTime, getWaitingTimeForDisposingCallback)
        {
        }
    }

    partial class RemoteAgencyManagingObjectProxy<TEntityBase> : RemoteAgencyManagingObject<TEntityBase>
    {
        private IProxyCommunicate _proxyObject;

        public Guid DefaultTargetSiteId { get; }

        public Guid DefaultTargetInstanceId { get; }

        //requested by manager: RemoteAgency.OnRemoteServiceWrapperClosing
        public override void OnServiceWrapperClosing(Guid siteId, Guid? serviceWrapperInstanceId) 
        {
            CallOnRemoteClosed(_proxyObject.OnRemoteServiceWrapperClosing, siteId, serviceWrapperInstanceId);
        }

        protected override void CloseManagingObject(bool sendSpecialCommand) //requested by manager
        {
            try
            {
                CallCloseRequestedByManagingObject(_proxyObject.CloseRequestedByManagingObject, sendSpecialCommand);
            }
            catch (AggregateException e)
            {
                if (e.InnerExceptions.Count > 1)
                {
                    throw;
                }
                else
                {
                    throw e.InnerExceptions[0];
                }
            }
            finally
            {
                _proxyObject.SendEventAddingMessageCallback = null;
                //_proxyObject.GetSiteIdCallback = null;
                _proxyObject.SendEventRemovingMessageCallback = null;
                _proxyObject.SendMethodMessageCallback = null;
                _proxyObject.SendOneWayMethodMessageCallback = null;
                _proxyObject.SendOneWayPropertyGetMessageCallback = null;
                _proxyObject.SendOneWayPropertySetMessageCallback = null;
                _proxyObject.SendPropertyGetMessageCallback = null;
                _proxyObject.SendPropertySetMessageCallback = null;
                _proxyObject.SendOneWaySpecialCommandMessageCallback = null;
                _proxyObject.CreateEmptyMessageCallback = null;

                _proxyObject = null;
            }
        }

        #region Constructors

        public RemoteAgencyManagingObjectProxy(IProxyCommunicate proxyObject, ref Guid instanceId,
            Guid defaultTargetSiteId, Guid defaultTargetInstanceId, ThreadLockMode threadLockMode,
            Action<IRemoteAgencyMessage> sendMessageToManagerCallback, Action<Guid, string, Exception> sendExceptionToManagerCallback,
            CreateEmptyMessageCallback createEmptyMessageCallback, 
            int defaultTimeoutTime, Func<int> getWaitingTimeForDisposingCallback)
            : base(ref instanceId, threadLockMode, sendMessageToManagerCallback, sendExceptionToManagerCallback,
                createEmptyMessageCallback, defaultTimeoutTime, getWaitingTimeForDisposingCallback)
        {
            _proxyObject = proxyObject;
            _proxyObject.InstanceId = InstanceId;
            _proxyObject.SendMethodMessageCallback = ProcessMethodMessageReceivedFromInside;
            _proxyObject.SendOneWayMethodMessageCallback = ProcessOneWayMethodMessageReceivedFromInside;
            _proxyObject.SendEventAddingMessageCallback = ProcessEventAddMessageReceivedFromInside;
            _proxyObject.SendEventRemovingMessageCallback = ProcessEventRemoveMessageReceivedFromInside;
            _proxyObject.SendPropertyGetMessageCallback = ProcessPropertyGetMessageReceivedFromInside;
            _proxyObject.SendPropertySetMessageCallback = ProcessPropertySetMessageReceivedFromInside;
            _proxyObject.SendOneWayPropertyGetMessageCallback = ProcessOneWayPropertyGetMessageReceivedFromInside;
            _proxyObject.SendOneWayPropertySetMessageCallback = ProcessOneWayPropertySetMessageReceivedFromInside;
            _proxyObject.SendOneWaySpecialCommandMessageCallback = ProcessOneWaySpecialCommandMessageReceivedFromInside;
            _proxyObject.CreateEmptyMessageCallback = createEmptyMessageCallback;

            DefaultTargetSiteId = defaultTargetSiteId;
            DefaultTargetInstanceId = defaultTargetInstanceId;
        }

        public RemoteAgencyManagingObjectProxy(IProxyCommunicate proxyObject, ref Guid instanceId,
            Guid defaultTargetSiteId, Guid defaultTargetInstanceId, string threadLockTaskSchedulerName,
            TryGetTaskSchedulerCallback tryGetTaskSchedulerCallback,
            Action<IRemoteAgencyMessage> sendMessageToManagerCallback, Action<Guid, string, Exception> sendExceptionToManagerCallback,
            CreateEmptyMessageCallback createEmptyMessageCallback, 
            int defaultTimeoutTime, Func<int> getWaitingTimeForDisposingCallback)
            : base(ref instanceId, threadLockTaskSchedulerName, tryGetTaskSchedulerCallback,
                sendMessageToManagerCallback, sendExceptionToManagerCallback, 
                createEmptyMessageCallback, defaultTimeoutTime, getWaitingTimeForDisposingCallback)
        {
            _proxyObject = proxyObject;
            _proxyObject.InstanceId = InstanceId;
            _proxyObject.SendMethodMessageCallback = ProcessMethodMessageReceivedFromInside;
            _proxyObject.SendOneWayMethodMessageCallback = ProcessOneWayMethodMessageReceivedFromInside;
            _proxyObject.SendEventAddingMessageCallback = ProcessEventAddMessageReceivedFromInside;
            _proxyObject.SendEventRemovingMessageCallback = ProcessEventRemoveMessageReceivedFromInside;
            _proxyObject.SendPropertyGetMessageCallback = ProcessPropertyGetMessageReceivedFromInside;
            _proxyObject.SendPropertySetMessageCallback = ProcessPropertySetMessageReceivedFromInside;
            _proxyObject.SendOneWayPropertyGetMessageCallback = ProcessOneWayPropertyGetMessageReceivedFromInside;
            _proxyObject.SendOneWayPropertySetMessageCallback = ProcessOneWayPropertySetMessageReceivedFromInside;
            _proxyObject.SendOneWaySpecialCommandMessageCallback = ProcessOneWaySpecialCommandMessageReceivedFromInside;
            _proxyObject.CreateEmptyMessageCallback = createEmptyMessageCallback;

            DefaultTargetSiteId = defaultTargetSiteId;
            DefaultTargetInstanceId = defaultTargetInstanceId;
        }

        #endregion
    }

    partial class RemoteAgencyManagingObjectServiceWrapper<TEntityBase> : RemoteAgencyManagingObject<TEntityBase>
    {
        private IServiceWrapperCommunicate _serviceWrapperObject;

        #region Event Cleaning on Proxy Closing

        //requested by manager: RemoteAgency.OnRemoteProxyClosing
        public override void OnProxyClosing(Guid siteId, Guid? proxyInstanceId)
        {
            CallOnRemoteClosed(_serviceWrapperObject.OnRemoteProxyClosing, siteId, proxyInstanceId);
        }

        #endregion

        protected override void CloseManagingObject(bool sendSpecialCommand) //requested by manager
        {
            try
            {
                CallCloseRequestedByManagingObject(_serviceWrapperObject.CloseRequestedByManagingObject, sendSpecialCommand);
            }
            catch (AggregateException e)
            {
                if (e.InnerExceptions.Count > 1)
                {
                    throw;
                }
                else
                {
                    throw e.InnerExceptions[0];
                }
            }
            finally
            {
                //_serviceWrapperObject.GetSiteIdCallback = null;
                _serviceWrapperObject.SendEventMessageCallback = null;
                _serviceWrapperObject.SendOneWayEventMessageCallback = null;
                _serviceWrapperObject.SendOneWaySpecialCommandMessageCallback = null;
                _serviceWrapperObject.CreateEmptyMessageCallback = null;
                _serviceWrapperObject = null;
            }
        }

        #region Constructors

        public RemoteAgencyManagingObjectServiceWrapper(IServiceWrapperCommunicate serviceWrapperObject,
            ref Guid instanceId, ThreadLockMode threadLockMode,
            Action<IRemoteAgencyMessage> sendMessageToManagerCallback, Action<Guid, string, Exception> sendExceptionToManagerCallback,
            CreateEmptyMessageCallback createEmptyMessageCallback, int defaultTimeoutTime, Func<int> getWaitingTimeForDisposingCallback)
            : base(ref instanceId, threadLockMode, sendMessageToManagerCallback, sendExceptionToManagerCallback,
                createEmptyMessageCallback, defaultTimeoutTime, getWaitingTimeForDisposingCallback)
        {
            _serviceWrapperObject = serviceWrapperObject;
            _serviceWrapperObject.SendEventMessageCallback = ProcessEventMessageReceivedFromInside;
            _serviceWrapperObject.SendOneWayEventMessageCallback = ProcessOneWayEventMessageReceivedFromInside;
            _serviceWrapperObject.SendOneWaySpecialCommandMessageCallback = ProcessOneWaySpecialCommandMessageReceivedFromInside;
            _serviceWrapperObject.CreateEmptyMessageCallback = createEmptyMessageCallback;
        }

        public RemoteAgencyManagingObjectServiceWrapper(IServiceWrapperCommunicate serviceWrapperObject,
            ref Guid instanceId, string threadLockTaskSchedulerName,
            TryGetTaskSchedulerCallback tryGetTaskSchedulerCallback,
            Action<IRemoteAgencyMessage> sendMessageToManagerCallback, Action<Guid, string, Exception> sendExceptionToManagerCallback,
            CreateEmptyMessageCallback createEmptyMessageCallback, int defaultTimeoutTime, Func<int> getWaitingTimeForDisposingCallback)
            : base(ref instanceId, threadLockTaskSchedulerName, tryGetTaskSchedulerCallback,
                sendMessageToManagerCallback, sendExceptionToManagerCallback,
                createEmptyMessageCallback, defaultTimeoutTime, getWaitingTimeForDisposingCallback)
        {
            _serviceWrapperObject = serviceWrapperObject;
            _serviceWrapperObject.SendEventMessageCallback = ProcessEventMessageReceivedFromInside;
            _serviceWrapperObject.SendOneWayEventMessageCallback = ProcessOneWayEventMessageReceivedFromInside;
            _serviceWrapperObject.SendOneWaySpecialCommandMessageCallback = ProcessOneWaySpecialCommandMessageReceivedFromInside;
            _serviceWrapperObject.CreateEmptyMessageCallback = createEmptyMessageCallback;
        }
        #endregion
    }
}
