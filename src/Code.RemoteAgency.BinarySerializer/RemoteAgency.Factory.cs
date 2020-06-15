using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    public abstract partial class RemoteAgency
    {
        public static RemoteAgency<byte[], object> CreateWithBinarySerializer()
        {
            throw new NotImplementedException();
        }


    }
}
