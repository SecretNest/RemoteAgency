using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    abstract class RemoteAgencyManagingObject : IDisposable
    {
        public abstract void OnProxiesDisposed(Guid siteId);
        public abstract void OnProxyDisposed(Guid siteId, Guid proxyInstanceId);

        public abstract void Dispose();

        public ThreadLockMode ThreadLockMode { get; }
        public string ThreadLockTaskSchedulerName { get; }

        
    }

    abstract partial class RemoteAgencyManagingObject<TEntityBase> : RemoteAgencyManagingObject
    {



    }
}
