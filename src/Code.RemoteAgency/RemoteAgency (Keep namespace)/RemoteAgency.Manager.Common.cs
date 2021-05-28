using System;
using System.Collections.Concurrent;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgency<TSerialized, TEntityBase>
    {
        readonly ConcurrentDictionary<Guid, RemoteAgencyManagingObject<TEntityBase>> _managingObjects = new (); //key: instanceId
    }
}
