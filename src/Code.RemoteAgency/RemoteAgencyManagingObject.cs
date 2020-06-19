using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    abstract class RemoteAgencyManagingObject : IDisposable
    {
        public abstract void OnProxiesDisposed(Guid siteId);
        public abstract void OnProxyDisposed(Guid siteId, Guid proxyInstanceId);

        public abstract void Dispose();



        
    }

    abstract partial class RemoteAgencyManagingObject<TEntityBase> : RemoteAgencyManagingObject
    {



    }
}
