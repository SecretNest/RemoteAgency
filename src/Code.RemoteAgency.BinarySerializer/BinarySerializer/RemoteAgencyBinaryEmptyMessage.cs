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
        private Guid _senderSiteId, _targetSiteId, _senderInstanceId, _targetInstanceId, _messageId;
        private MessageType _messageType;
        private string _assetName;
        private Exception _exception;
        private bool _isOneWay/*, _isEmptyMessage*/;
#pragma warning restore CA2235 // Mark all non-serializable fields
#pragma warning restore IDE0079 // Remove unnecessary suppression

        Guid IRemoteAgencyMessage.SenderSiteId { get => _senderSiteId; set => _senderSiteId = value; }
        Guid IRemoteAgencyMessage.TargetSiteId { get => _targetSiteId; set => _targetSiteId = value; }
        Guid IRemoteAgencyMessage.SenderInstanceId { get => _senderInstanceId; set => _senderInstanceId = value; }
        Guid IRemoteAgencyMessage.TargetInstanceId { get => _targetInstanceId; set => _targetInstanceId = value; }
        MessageType IRemoteAgencyMessage.MessageType { get => _messageType; set => _messageType = value; }
        string IRemoteAgencyMessage.AssetName { get => _assetName; set => _assetName = value; }
        Guid IRemoteAgencyMessage.MessageId { get => _messageId; set => _messageId = value; }
        Exception IRemoteAgencyMessage.Exception { get => _exception; set => _exception = value; }
        bool IRemoteAgencyMessage.IsOneWay { get => _isOneWay; set => _isOneWay = value; }
        bool IRemoteAgencyMessage.IsEmptyMessage { get => true; set => _ = value; }

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