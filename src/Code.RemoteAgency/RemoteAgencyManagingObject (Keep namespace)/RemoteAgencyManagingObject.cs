using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    abstract partial class RemoteAgencyManagingObject : IDisposable
    {
        public virtual void OnProxyClosing(Guid siteId, Guid? proxyInstanceId)
        {
        }

        public virtual void OnServiceWrapperClosing(Guid siteId, Guid? serviceWrapperInstanceId)
        {
        }

        public virtual void Dispose()
        {
            CloseManagingObject();
            DisposeThreadLock();
        }

        protected abstract void CloseManagingObject();

        public Guid InstanceId { get; }
        protected abstract Guid TargetSiteId { get; }
        public abstract Guid DefaultTargetSiteId { get; }
        public abstract Guid DefaultTargetInstanceId { get; }

        private readonly Action<IRemoteAgencyMessage> _sendMessageToManagerCallback;
        private readonly Action<Exception> _sendExceptionToManagerCallback;
        private readonly Dictionary<string, LocalExceptionHandlingMode> _localExceptionHandlingAssets; //Key: AssetName

        private RemoteAgencyManagingObject(ref Guid instanceId,
            Action<IRemoteAgencyMessage> sendMessageToManagerCallback, Action<Exception> sendExceptionToManagerCallback,
            Dictionary<string, LocalExceptionHandlingMode> localExceptionHandlingAssets)
        {
            if (instanceId == Guid.Empty)
            {
                instanceId = Guid.NewGuid();
            }

            InstanceId = instanceId;
            _sendMessageToManagerCallback = sendMessageToManagerCallback;
            _sendExceptionToManagerCallback = sendExceptionToManagerCallback;
            _localExceptionHandlingAssets = localExceptionHandlingAssets;
        }

        protected RemoteAgencyManagingObject(ref Guid instanceId, ThreadLockMode threadLockMode,
            Action<IRemoteAgencyMessage> sendMessageToManagerCallback, Action<Exception> sendExceptionToManagerCallback,
            Dictionary<string, LocalExceptionHandlingMode> localExceptionHandlingAssets)
            : this(ref instanceId, sendMessageToManagerCallback, sendExceptionToManagerCallback,
                localExceptionHandlingAssets)
        {
            InitializeThreadLock(threadLockMode);
        }

        protected RemoteAgencyManagingObject(ref Guid instanceId, string threadLockTaskSchedulerName,
            TryGetTaskSchedulerCallback tryGetTaskSchedulerCallback,
            Action<IRemoteAgencyMessage> sendMessageToManagerCallback, Action<Exception> sendExceptionToManagerCallback,
            Dictionary<string, LocalExceptionHandlingMode> localExceptionHandlingAssets)
            : this(ref instanceId, sendMessageToManagerCallback, sendExceptionToManagerCallback,
                localExceptionHandlingAssets)
        {
            InitializeThreadLock(threadLockTaskSchedulerName, tryGetTaskSchedulerCallback);
        }
    }

    abstract partial class RemoteAgencyManagingObject<TEntityBase> : RemoteAgencyManagingObject
    {
        protected RemoteAgencyManagingObject(ref Guid instanceId, ThreadLockMode threadLockMode,
            Action<IRemoteAgencyMessage> sendMessageToManagerCallback, Action<Exception> sendExceptionToManagerCallback,
            Dictionary<string, LocalExceptionHandlingMode> localExceptionHandlingAssets)
            : base(ref instanceId, threadLockMode, sendMessageToManagerCallback, sendExceptionToManagerCallback,
                localExceptionHandlingAssets)
        {
        }

        protected RemoteAgencyManagingObject(ref Guid instanceId, string threadLockTaskSchedulerName,
            TryGetTaskSchedulerCallback tryGetTaskSchedulerCallback,
            Action<IRemoteAgencyMessage> sendMessageToManagerCallback, Action<Exception> sendExceptionToManagerCallback,
            Dictionary<string, LocalExceptionHandlingMode> localExceptionHandlingAssets)
            : base(ref instanceId, threadLockTaskSchedulerName, tryGetTaskSchedulerCallback,
                sendMessageToManagerCallback, sendExceptionToManagerCallback, localExceptionHandlingAssets)
        {
        }
    }

    partial class RemoteAgencyManagingObjectProxy<TEntityBase> : RemoteAgencyManagingObject<TEntityBase>
    {
        private IProxyCommunicate _proxyObject;
        private readonly bool _isStickyModeEnabled;
        private Guid? _stickyTargetSiteId = null;

        public override void OnServiceWrapperClosing(Guid siteId, Guid? serviceWrapperInstanceId)
        {
            if (_isStickyModeEnabled && _stickyTargetSiteId == siteId)
            {
                if (!serviceWrapperInstanceId.HasValue || serviceWrapperInstanceId == DefaultTargetInstanceId)
                {
                    _stickyTargetSiteId = null;
                }
            }
        }

        protected override void CloseManagingObject()
        {
            CallClose();
            _proxyObject.SendEventAddingMessageCallback = null;
            _proxyObject.GetSiteIdCallback = null;
            _proxyObject.ProxyStickyTargetSiteResetCallback = null;
            _proxyObject.ProxyStickyTargetSiteQueryCallback = null;
            _proxyObject.SendEventRemovingMessageCallback = null;
            _proxyObject.SendMethodMessageCallback = null;
            _proxyObject.SendOneWayMethodMessageCallback = null;
            _proxyObject.SendOneWayPropertySetMessageCallback = null;
            _proxyObject.SendPropertyGetMessageCallback = null;
            _proxyObject.SendPropertySetMessageCallback = null;
            _proxyObject = null;
        }

        public RemoteAgencyManagingObjectProxy(IProxyCommunicate proxyObject, ref Guid instanceId,
            Guid defaultTargetSiteId, Guid defaultTargetInstanceId, ThreadLockMode threadLockMode,
            Action<IRemoteAgencyMessage> sendMessageToManagerCallback, Action<Exception> sendExceptionToManagerCallback,
            Dictionary<string, LocalExceptionHandlingMode> localExceptionHandlingAssets, bool isStickyModeEnabled)
            : base(ref instanceId, threadLockMode, sendMessageToManagerCallback, sendExceptionToManagerCallback,
                localExceptionHandlingAssets)
        {
            _proxyObject = proxyObject;
            _proxyObject.ProxyStickyTargetSiteResetCallback = ResetProxyStickyTargetSite;
            _proxyObject.ProxyStickyTargetSiteQueryCallback = ProxyStickyTargetSiteQuery;
            DefaultTargetSiteId = defaultTargetSiteId;
            DefaultTargetInstanceId = defaultTargetInstanceId;
            _isStickyModeEnabled = isStickyModeEnabled;
        }

        public RemoteAgencyManagingObjectProxy(IProxyCommunicate proxyObject, ref Guid instanceId,
            Guid defaultTargetSiteId, Guid defaultTargetInstanceId, string threadLockTaskSchedulerName,
            TryGetTaskSchedulerCallback tryGetTaskSchedulerCallback,
            Action<IRemoteAgencyMessage> sendMessageToManagerCallback, Action<Exception> sendExceptionToManagerCallback,
            Dictionary<string, LocalExceptionHandlingMode> localExceptionHandlingAssets, bool isStickyModeEnabled)
            : base(ref instanceId, threadLockTaskSchedulerName, tryGetTaskSchedulerCallback,
                sendMessageToManagerCallback, sendExceptionToManagerCallback, localExceptionHandlingAssets)
        {
            _proxyObject = proxyObject;
            _proxyObject.ProxyStickyTargetSiteResetCallback = ResetProxyStickyTargetSite;
            _proxyObject.ProxyStickyTargetSiteQueryCallback = ProxyStickyTargetSiteQuery;
            DefaultTargetSiteId = defaultTargetSiteId;
            DefaultTargetInstanceId = defaultTargetInstanceId;
            _isStickyModeEnabled = isStickyModeEnabled;
        }

        void ResetProxyStickyTargetSite()
        {
            if (_isStickyModeEnabled)
                _stickyTargetSiteId = null;
        }

        void ProxyStickyTargetSiteQuery(out bool isEnabled, out Guid defaultTargetSiteId,
            out Guid? stickyTargetSiteId)
        {
            isEnabled = _isStickyModeEnabled;
            defaultTargetSiteId = DefaultTargetSiteId;
            stickyTargetSiteId = _stickyTargetSiteId;
        }

        protected override Guid TargetSiteId
        {
            get
            {
                if (_isStickyModeEnabled && _stickyTargetSiteId.HasValue)
                {
                    return _stickyTargetSiteId.Value;
                }
                else
                {
                    return DefaultTargetSiteId;
                }
            }
        }

        public override Guid DefaultTargetSiteId { get; }

        public override Guid DefaultTargetInstanceId { get; }
    }

    partial class RemoteAgencyManagingObjectServiceWrapper<TEntityBase> : RemoteAgencyManagingObject<TEntityBase>
    {
        private IServiceWrapperCommunicate _serviceWrapperObject;
        private Func<IRemoteAgencyMessage> _createEmptyMessageCallback;

        public override void OnProxyClosing(Guid siteId, Guid? proxyInstanceId)
        {
            CallOnClosing(_serviceWrapperObject.OnRemoteProxyClosing, siteId, proxyInstanceId);
        }

        protected override void CloseManagingObject()
        {
            CallClose();
            _serviceWrapperObject.GetSiteIdCallback = null;
            _serviceWrapperObject.SendEventMessageCallback = null;
            _serviceWrapperObject.SendOneWayEventMessageCallback = null;
            _serviceWrapperObject = null;
            _createEmptyMessageCallback = null;
        }

        protected override Guid TargetSiteId => throw new InvalidOperationException();
        public override Guid DefaultTargetSiteId => throw new InvalidOperationException();
        public override Guid DefaultTargetInstanceId => throw new InvalidOperationException();

        public RemoteAgencyManagingObjectServiceWrapper(IServiceWrapperCommunicate serviceWrapperObject,
            ref Guid instanceId, ThreadLockMode threadLockMode,
            Action<IRemoteAgencyMessage> sendMessageToManagerCallback, Action<Exception> sendExceptionToManagerCallback,
            Dictionary<string, LocalExceptionHandlingMode> localExceptionHandlingAssets,
            Func<IRemoteAgencyMessage> createEmptyMessageCallback)
            : base(ref instanceId, threadLockMode, sendMessageToManagerCallback, sendExceptionToManagerCallback,
                localExceptionHandlingAssets)
        {
            _serviceWrapperObject = serviceWrapperObject;
            _createEmptyMessageCallback = createEmptyMessageCallback;
        }

        public RemoteAgencyManagingObjectServiceWrapper(IServiceWrapperCommunicate serviceWrapperObject,
            ref Guid instanceId, string threadLockTaskSchedulerName,
            TryGetTaskSchedulerCallback tryGetTaskSchedulerCallback,
            Action<IRemoteAgencyMessage> sendMessageToManagerCallback, Action<Exception> sendExceptionToManagerCallback,
            Dictionary<string, LocalExceptionHandlingMode> localExceptionHandlingAssets,
            Func<IRemoteAgencyMessage> createEmptyMessageCallback)
            : base(ref instanceId, threadLockTaskSchedulerName, tryGetTaskSchedulerCallback,
                sendMessageToManagerCallback, sendExceptionToManagerCallback, localExceptionHandlingAssets)
        {
            _serviceWrapperObject = serviceWrapperObject;
            _createEmptyMessageCallback = createEmptyMessageCallback;
        }
    }
}
