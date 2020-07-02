using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgency<TSerialized, TEntityBase>
    {
        readonly ConcurrentDictionary<Guid, RemoteAgencyManagingObject<TEntityBase>> _managingObjects = new ConcurrentDictionary<Guid, RemoteAgencyManagingObject<TEntityBase>>(); //key: instanceId
    }
}
