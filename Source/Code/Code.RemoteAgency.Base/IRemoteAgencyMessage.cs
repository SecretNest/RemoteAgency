using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    public interface IRemoteAgencyMessage<TSerialized>
    {
        MessageType MessageType { get; set; }

        string AssetName { get; set; }

        Guid MessageId { get; set; }

        WrappedException WrappedException { get; set; }

        bool IsOneWay { get; set; }
    }
}
