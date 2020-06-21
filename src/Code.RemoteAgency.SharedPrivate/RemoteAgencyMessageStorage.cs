using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    class RemoteAgencyMessageStorage
    {
        public IRemoteAgencyMessage RequestMessage { get; set; }
        public IRemoteAgencyMessage ResponseMessage { get; set; }
    }
}
