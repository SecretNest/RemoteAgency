using System;
using System.Collections.Generic;
using System.Text;

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

        //TODO: call InitializeThreadLock in constructor
      
    }

    abstract partial class RemoteAgencyManagingObject<TEntityBase> : RemoteAgencyManagingObject
    {

    }

    partial class RemoteAgencyManagingObjectProxy<TEntityBase> : RemoteAgencyManagingObject<TEntityBase>
    {

    }

    partial class RemoteAgencyManagingObjectServiceWrapper<TEntityBase> : RemoteAgencyManagingObject<TEntityBase>
    {
        public override void OnProxiesDisposed(Guid siteId)
        {
            throw new NotImplementedException();
        }

        public override void OnProxyDisposed(Guid siteId, Guid proxyInstanceId)
        {
            throw new NotImplementedException();
        }
    }
}
