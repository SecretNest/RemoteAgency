using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    abstract partial class RemoteAgencyManagingObject : IDisposable
    {
        public virtual void OnProxiesDisposed(Guid siteId)
        {
        }

        public virtual void OnProxyDisposed(Guid siteId, Guid proxyInstanceId)
        {
        }

        public virtual void Dispose()
        {
            DisposeThreadLock();
        }

        public Guid InstanceId { get; }
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
        private readonly IProxyCommunicate _proxyObject;

        public RemoteAgencyManagingObjectProxy(IProxyCommunicate proxyObject, ref Guid instanceId,
            Guid defaultTargetSiteId, Guid defaultTargetInstanceId, ThreadLockMode threadLockMode,
            Action<IRemoteAgencyMessage> sendMessageToManagerCallback, Action<Exception> sendExceptionToManagerCallback,
            Dictionary<string, LocalExceptionHandlingMode> localExceptionHandlingAssets)
            : base(ref instanceId, threadLockMode, sendMessageToManagerCallback, sendExceptionToManagerCallback,
                localExceptionHandlingAssets)
        {
            _proxyObject = proxyObject;
            DefaultTargetSiteId = defaultTargetSiteId;
            DefaultTargetInstanceId = defaultTargetInstanceId;
        }

        public RemoteAgencyManagingObjectProxy(IProxyCommunicate proxyObject, ref Guid instanceId,
            Guid defaultTargetSiteId, Guid defaultTargetInstanceId, string threadLockTaskSchedulerName,
            TryGetTaskSchedulerCallback tryGetTaskSchedulerCallback,
            Action<IRemoteAgencyMessage> sendMessageToManagerCallback, Action<Exception> sendExceptionToManagerCallback,
            Dictionary<string, LocalExceptionHandlingMode> localExceptionHandlingAssets)
            : base(ref instanceId, threadLockTaskSchedulerName, tryGetTaskSchedulerCallback,
                sendMessageToManagerCallback, sendExceptionToManagerCallback, localExceptionHandlingAssets)
        {
            _proxyObject = proxyObject;
            DefaultTargetSiteId = defaultTargetSiteId;
            DefaultTargetInstanceId = defaultTargetInstanceId;
        }

        public override Guid DefaultTargetSiteId { get; }

        public override Guid DefaultTargetInstanceId { get; }
    }

    partial class RemoteAgencyManagingObjectServiceWrapper<TEntityBase> : RemoteAgencyManagingObject<TEntityBase>
    {
        private readonly IServiceWrapperCommunicate _serviceWrapperObject;
        private Func<IRemoteAgencyMessage> _createEmptyMessageCallback;

        public override void OnProxiesDisposed(Guid siteId)
        {
            throw new NotImplementedException();


        }

        public override void OnProxyDisposed(Guid siteId, Guid proxyInstanceId)
        {
            throw new NotImplementedException();


        }

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
