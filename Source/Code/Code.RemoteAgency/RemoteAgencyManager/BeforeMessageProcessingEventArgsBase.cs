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
        MessageInstanceMetadata metadata;
        /// <summary>
        /// Gets the metadata of this message instance.
        /// </summary>
        public MessageInstanceMetadata MessageInstanceMetadata => metadata;

        /// <summary>
        /// Gets the sender site id.
        /// </summary>
        public Guid SenderSiteId => metadata.SenderSiteId;
        /// <summary>
        /// Gets the sender instance id.
        /// </summary>
        public Guid SenderInstanceId => metadata.SenderInstanceId;
        /// <summary>
        /// Gets the target site id.
        /// </summary>
        public Guid TargetSiteId => metadata.TargetSiteId;
        /// <summary>
        /// Gets the target instance id.
        /// </summary>
        public Guid TargetInstanceId => metadata.TargetInstanceId;
        /// <summary>
        /// Gets the message type.
        /// </summary>
        public MessageType MessageType => metadata.MessageType;
        /// <summary>
        /// Gets the asset name.
        /// </summary>
        public string AssetName => metadata.AssetName;
        /// <summary>
        /// Gets the message id.
        /// </summary>
        public Guid MessageId => metadata.MessageId;

        /// <summary>
        /// Gets whether this is a one way message. When set to true, no response should be returned from the target site.
        /// </summary>
        public bool IsOneWay => metadata.IsOneWay;
        /// <summary>
        /// Gets whether this is an exception message.
        /// </summary>
        public bool IsException => metadata.IsException;

        MessageFurtherProcessing furtherProcessing;
        /// <summary>
        /// Defines the further processing of this message.
        /// </summary>
        public MessageFurtherProcessing FurtherProcessing
        {
            get { return furtherProcessing; }
            set
            {
                if (metadata.IsOneWay && value == MessageFurtherProcessing.TerminateWithExceptionReturned)
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
        /// <param name="metadata">Metadata of this message instance.</param>
        protected BeforeMessageProcessingEventArgsBase(MessageInstanceMetadata metadata)
        {
            this.metadata = metadata;
            furtherProcessing = MessageFurtherProcessing.Continue;
        }
    }

    /// <summary>
    /// Defines the further processing of this message.
    /// </summary>
    public enum MessageFurtherProcessing
    {
        /// <summary>
        /// Continues.
        /// </summary>
        Continue,
        /// <summary>
        /// Terminates this sending process and send an instance of <see cref="MessageProcessTerminatedException" /> back to the sender. Cannot be set when <see cref="BeforeMessageProcessingEventArgsBase.IsOneWay"/> is set to true.
        /// </summary>
        TerminateWithExceptionReturned,
        /// <summary>
        /// Replaces this message by an instance of <see cref="MessageProcessTerminatedException" /> then sends it to the receiver.
        /// </summary>
        ReplacedWithException,
        /// <summary>
        /// Terminates this sending process.
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
        /// <param name="metadata">Metadata of this message instance.</param>
        /// <param name="serializedMessage">Serialized message.</param>
        protected BeforeMessageProcessingEventArgsBase(MessageInstanceMetadata metadata, TSerialized serializedMessage)
            : base(metadata)
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
        /// Exception type wrapped in this message.
        /// </summary>
        public Type ExceptionType { get; }

        internal BeforeExceptionMessageProcessingEventArgs(MessageInstanceMetadata metadata, TSerialized serializedException, Type exceptionType)
            : base(metadata, serializedException)
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
        /// Generic arguments used in this message.
        /// </summary>
        public Type[] GenericArguments { get; }

        internal BeforeMessageProcessingEventArgs(MessageInstanceMetadata metadata, TSerialized serializedMessage, Type[] genericArguments)
            : base(metadata, serializedMessage)
        {
            GenericArguments = genericArguments;
        }
    }
}
