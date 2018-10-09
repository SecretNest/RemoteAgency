using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Represents a message to be checked for sending or processing after received. This is an abstract class.
    /// </summary>
    /// <seealso cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}.BeforeMessageSending"/>
    /// <seealso cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}.AfterMessageReceived"/>
    public abstract class BeforeMessageProcessingEventArgsBase : EventArgs
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
        public abstract bool IsException { get; }

        MessageFurtherProcessing furtherProcessing;
        /// <summary>
        /// Defines the further processing of this message.
        /// </summary>
        public MessageFurtherProcessing FurtherProcessing
        {
            get { return furtherProcessing; }
            set
            {
                if (IsOneWay && value == MessageFurtherProcessing.TerminateWithExceptionReturned)
                {
                    furtherProcessing = MessageFurtherProcessing.TerminateSilently;
                }
                else
                {
                    furtherProcessing = value;
                }
            }
        }

        /// <summary>
        /// Initializes an instance of BeforeMessageProcessingEventArgsBase.
        /// </summary>
        /// <param name="senderSiteId">Sender site id.</param>
        /// <param name="senderInstanceId">Sender instance id.</param>
        /// <param name="targetSiteId">Target site id.</param>
        /// <param name="targetInstanceId">Target instance id.</param>
        /// <param name="messageType">Message type.</param>
        /// <param name="assetName">Asset Name.</param>
        /// <param name="messageId">Message id.</param>
        /// <param name="isOneWay">Whether this is a one way message. When set to true, no response should be returned from the target site.</param>
        protected BeforeMessageProcessingEventArgsBase(Guid senderSiteId, Guid senderInstanceId, Guid targetSiteId, Guid targetInstanceId, MessageType messageType, string assetName, Guid messageId, bool isOneWay)
        {
            SenderSiteId = senderSiteId;
            SenderInstanceId = senderInstanceId;
            TargetSiteId = targetSiteId;
            TargetInstanceId = targetInstanceId;
            MessageType = messageType;
            AssetName = assetName;
            MessageId = messageId;
            IsOneWay = isOneWay;
            furtherProcessing = MessageFurtherProcessing.Continue;
        }
    }

    /// <summary>
    /// Defines the further processing of this message.
    /// </summary>
    public enum MessageFurtherProcessing
    {
        /// <summary>
        /// Continue.
        /// </summary>
        Continue,
        /// <summary>
        /// Terminate this sending process and send an instance of <see cref="MessageProcessTerminatedException" /> back to the sender. Cannot be set when <see cref="BeforeMessageProcessingEventArgsBase.IsOneWay"/> is set to true.
        /// </summary>
        TerminateWithExceptionReturned,
        /// <summary>
        /// Terminate this sending process.
        /// </summary>
        TerminateSilently
    }

    /// <summary>
    /// Represents a message to be checked for sending or processing after received. This is an abstract class.
    /// </summary>
    /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
    public abstract class BeforeMessageProcessingEventArgsBase<TSerialized> : BeforeMessageProcessingEventArgsBase
    {
        /// <summary>
        /// Gets the serialized message.
        /// </summary>
        public TSerialized SerializedMessage { get; }

        /// <summary>
        /// Initializes an instance of BeforeMessageProcessingEventArgsBase.
        /// </summary>
        /// <param name="senderSiteId">Sender site id.</param>
        /// <param name="senderInstanceId">Sender instance id.</param>
        /// <param name="targetSiteId">Target site id.</param>
        /// <param name="targetInstanceId">Target instance id.</param>
        /// <param name="messageType">Message type.</param>
        /// <param name="assetName">Asset Name.</param>
        /// <param name="messageId">Message id.</param>
        /// <param name="isOneWay">Whether this is a one way message. When set to true, no response should be returned from the target site.</param>
        /// <param name="serializedMessage">Serialized message.</param>
        protected BeforeMessageProcessingEventArgsBase(Guid senderSiteId, Guid senderInstanceId, Guid targetSiteId, Guid targetInstanceId, MessageType messageType, string assetName, Guid messageId, bool isOneWay, TSerialized serializedMessage)
            : base(senderSiteId, senderInstanceId, targetSiteId, targetInstanceId, messageType, assetName, messageId, isOneWay)
        {
            SerializedMessage = serializedMessage;
        }
    }

    /// <summary>
    /// Represents an exception message to be checked for sending or processing after received.
    /// </summary>
    /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
    public class BeforeExceptionMessageProcessingEventArgs<TSerialized> : BeforeMessageProcessingEventArgsBase<TSerialized>
    {
        /// <summary>
        /// Gets whether this is an exception message. Will always return true.
        /// </summary>
        public override bool IsException => true;
        /// <summary>
        /// Exception type wrapped in this message.
        /// </summary>
        public Type ExceptionType { get; }

        internal BeforeExceptionMessageProcessingEventArgs(Guid senderSiteId, Guid senderInstanceId, Guid targetSiteId, Guid targetInstanceId, MessageType messageType, string assetName, Guid messageId, TSerialized serializedException, Type exceptionType)
            : base(senderSiteId, senderInstanceId, targetSiteId, targetInstanceId, messageType, assetName, messageId, true, serializedException)
        {
            ExceptionType = exceptionType;
        }
    }

    /// <summary>
    /// Represents a normal message (not an exception) to be checked for sending or processing after received.
    /// </summary>
    /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
    public class BeforeMessageProcessingEventArgs<TSerialized> : BeforeMessageProcessingEventArgsBase<TSerialized>
    {
        /// <summary>
        /// Gets whether this is an exception message. Will always return false.
        /// </summary>
        public override bool IsException => false;
        /// <summary>
        /// Generic arguments used in this message.
        /// </summary>
        public Type[] GenericArguments { get; }

        internal BeforeMessageProcessingEventArgs(Guid senderSiteId, Guid senderInstanceId, Guid targetSiteId, Guid targetInstanceId, MessageType messageType, string assetName, Guid messageId, bool isOneWay, TSerialized serializedMessage, Type[] genericArguments)
            : base(senderSiteId, senderInstanceId, targetSiteId, targetInstanceId, messageType, assetName, messageId, isOneWay, serializedMessage)
        {
            GenericArguments = genericArguments;
        }
    }
}
