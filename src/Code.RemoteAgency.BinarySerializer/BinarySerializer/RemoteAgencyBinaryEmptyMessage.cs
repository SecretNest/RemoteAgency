using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;

namespace SecretNest.RemoteAgency.BinarySerializer
{
    /// <summary>
    /// Defines an empty message with binary serialization support.
    /// </summary>
    [Serializable]
    public class RemoteAgencyBinaryEmptyMessage : IRemoteAgencyMessage
    {
#pragma warning disable CA2235 // Mark all non-serializable fields
        Guid IRemoteAgencyMessage.SenderSiteId { get; set; }
        Guid IRemoteAgencyMessage.TargetSiteId { get; set; }
        Guid IRemoteAgencyMessage.SenderInstanceId { get; set; }
        Guid IRemoteAgencyMessage.TargetInstanceId { get; set; }
        MessageType IRemoteAgencyMessage.MessageType { get; set; }
        string IRemoteAgencyMessage.AssetName { get; set; }
        Guid IRemoteAgencyMessage.MessageId { get; set; }
        Exception IRemoteAgencyMessage.Exception { get; set; }
        bool IRemoteAgencyMessage.IsOneWay { get; set; }
        bool IRemoteAgencyMessage.IsEmptyMessage { get; set; }
#pragma warning restore CA2235 // Mark all non-serializable fields

        /// <summary>
        /// Initializes an instance of RemoteAgencyBinaryEmptyMessage.
        /// </summary>
        public RemoteAgencyBinaryEmptyMessage()
        {
            ((IRemoteAgencyMessage) this).IsEmptyMessage = true;
        }
    }
}