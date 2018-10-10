using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Represents metadata of one message instance.
    /// </summary>
    public class MessageInstanceMetadata
    {
        /// <summary>
        /// Gets the sender site id.
        /// </summary>
        public Guid SenderSiteId { get; }
        /// <summary>
        /// Gets the sender instance id.
        /// </summary>
        public Guid SenderInstanceId { get; }
        /// <summary>
        /// Gets the target site id.
        /// </summary>
        public Guid TargetSiteId { get; }
        /// <summary>
        /// Gets the target instance id.
        /// </summary>
        public Guid TargetInstanceId { get; }
        /// <summary>
        /// Gets the message type.
        /// </summary>
        public MessageType MessageType { get; }
        /// <summary>
        /// Gets the asset name.
        /// </summary>
        public string AssetName { get; }
        /// <summary>
        /// Gets the message id.
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// Gets whether this is a one way message. When set to true, no response should be returned from the target site.
        /// </summary>
        public bool IsOneWay { get; }
        /// <summary>
        /// Gets whether this is an exception message.
        /// </summary>
        public bool IsException { get; }

        /// <summary>
        /// Initializes an instance of MessageInstanceMetadata.
        /// </summary>
        /// <param name="senderSiteId">Sender site id.</param>
        /// <param name="senderInstanceId">Sender instance id.</param>
        /// <param name="targetSiteId">Target site id.</param>
        /// <param name="targetInstanceId">Target instance id.</param>
        /// <param name="messageType">Message type.</param>
        /// <param name="assetName">Asset name.</param>
        /// <param name="messageId">Message id.</param>
        /// <param name="isOneWay">Whether this is a one way message. When set to true, no response should be returned from the target site.</param>
        /// <param name="isException">Whether this is an exception message.</param>
        public MessageInstanceMetadata(Guid senderSiteId, Guid senderInstanceId, Guid targetSiteId, Guid targetInstanceId, MessageType messageType, string assetName, Guid messageId, bool isOneWay, bool isException)
        {
            SenderSiteId = senderSiteId;
            SenderInstanceId = senderInstanceId;
            TargetSiteId = targetSiteId;
            TargetInstanceId = targetInstanceId;
            MessageType = messageType;
            AssetName = assetName;
            MessageId = messageId;
            IsOneWay = isOneWay;
            IsException = isException;
        }
    }
}
