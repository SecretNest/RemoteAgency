using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    public partial class RemoteAgency<TSerialized, TEntityBase>
    {
        ConcurrentDictionary<Guid, RemoteAgencyManagingObject<TEntityBase>> _managingObjects = new ConcurrentDictionary<Guid, RemoteAgencyManagingObject<TEntityBase>>(); //key: instanceId




    }
}
