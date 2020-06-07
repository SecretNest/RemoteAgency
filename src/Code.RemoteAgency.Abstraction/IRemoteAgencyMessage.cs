using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Defines the common properties contained in messages.
    /// </summary>
    /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
    public interface IRemoteAgencyMessage<TSerialized>
    {
        /// <summary>
        /// Site id of the source Remote Agency manager
        /// </summary>
        Guid SenderSiteId { get; set; }
        /// <summary>
        /// Site id of the target Remote Agency manager
        /// </summary>
        Guid TargetSiteId { get; set; }
        /// <summary>
        /// Instance id of the source proxy or service wrapper
        /// </summary>
        Guid SenderInstanceId { get; set; }
        /// <summary>
        /// Instance id of the target proxy or service wrapper
        /// </summary>
        Guid TargetInstanceId { get; set; }

        /// <summary>
        /// Message type
        /// </summary>
        MessageType MessageType { get; set; }

        /// <summary>
        /// Asset name
        /// </summary>
        string AssetName { get; set; }

        /// <summary>
        /// Message id
        /// </summary>
        Guid MessageId { get; set; }

        /// <summary>
        /// Exception object
        /// </summary>
        WrappedException WrappedException { get; set; }

        /// <summary>
        /// Whether this message is one way (do not need any response)
        /// </summary>
        bool IsOneWay { get; set; }
    }
}
