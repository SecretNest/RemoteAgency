using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    public abstract partial class RemoteAgency
    {
        public static RemoteAgency<TSerialized, TEntityBase> Create<TSerialized, TEntityBase>()
        {
            throw new NotImplementedException();
        }


    }
}
