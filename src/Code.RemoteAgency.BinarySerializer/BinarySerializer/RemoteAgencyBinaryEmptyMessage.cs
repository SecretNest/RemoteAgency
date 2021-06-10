using System;

namespace SecretNest.RemoteAgency.BinarySerializer
{
    /// <summary>
    /// Defines an empty message with binary serialization support.
    /// </summary>
    /// <remarks><para>This class is not present in Neat release.</para></remarks>
    [Serializable]
    public class RemoteAgencyBinaryEmptyMessage : IRemoteAgencyMessage
    {
#pragma warning disable IDE0079 // Remove unnecessary suppression
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
#pragma warning restore IDE0079 // Remove unnecessary suppression

        /// <summary>
        /// Initializes an instance of RemoteAgencyBinaryEmptyMessage.
        /// </summary>
        /// <remarks><para>This constructor and this class are not present in Neat release.</para></remarks>
        public RemoteAgencyBinaryEmptyMessage()
        {
            ((IRemoteAgencyMessage) this).IsEmptyMessage = true;
        }
    }
}