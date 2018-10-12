using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Represents metadata of one message instance.
    /// </summary>
    [DataContract(Namespace = "")]
    [Serializable()]
    public class MessageInstanceMetadata
    {
        /// <summary>
        /// Gets the sender site id.
        /// </summary>
        [DataMember]
        public Guid SenderSiteId { get; }
        /// <summary>
        /// Gets the sender instance id.
        /// </summary>
        [DataMember]
        public Guid SenderInstanceId { get; }
        /// <summary>
        /// Gets the target site id.
        /// </summary>
        [DataMember]
        public Guid TargetSiteId { get; }
        /// <summary>
        /// Gets the target instance id.
        /// </summary>
        [DataMember]
        public Guid TargetInstanceId { get; }
        /// <summary>
        /// Gets the message type.
        /// </summary>
        [DataMember]
        public MessageType MessageType { get; }
        /// <summary>
        /// Gets the asset name.
        /// </summary>
        [DataMember]
        public string AssetName { get; }
        /// <summary>
        /// Gets the message id.
        /// </summary>
        [DataMember]
        public Guid MessageId { get; }

        /// <summary>
        /// Gets whether this is a one way message. When set to true, no response should be returned from the target site.
        /// </summary>
        [DataMember]
        public bool IsOneWay { get; }
        /// <summary>
        /// Gets whether this is an exception message.
        /// </summary>
        [DataMember]
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

        /// <summary>
        /// Initializes an instance of the MessageInstanceMetadata. This method is kept for serializing supporting.
        /// </summary>
        public MessageInstanceMetadata()
        { }

        /// <summary>
        /// Creates and returns a string representation of the current metadata.
        /// </summary>
        /// <returns>A string representation of the current metadata.</returns>
        public override string ToString()
        {
            return string.Format("Sender Site Id: {0}\nSender Instance Id: {1}\nTarget Site Id: {2}\nTarget Instance Id: {3}\nMessage Type: {4}\nAsset Name: {5}\nMessage Id: {6}\nIs One Way: {7}\nIs Exception: {8}",
                SenderSiteId, SenderInstanceId, TargetSiteId, TargetInstanceId, MessageType, AssetName, MessageId, IsOneWay, IsException);
        }
    }
}
