using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.MessageFiltering
{
    /// <summary>
    /// Represents a message to be checked for sending or processing after received. This is an abstract class.
    /// </summary>
    /// <seealso cref="BeforeMessageProcessingEventArgs{TEntityBase}"/>
    /// <seealso cref="RemoteAgency{TSerialized, TEntityBase}.BeforeMessageSending"/>
    /// <seealso cref="RemoteAgency{TSerialized, TEntityBase}.AfterMessageReceived"/>
    /// <seealso cref="MessageProcessTerminatedException"/>
    public abstract class BeforeMessageProcessingEventArgsBase
    {
        #region FurtherProcessing
        MessageFurtherProcessing _furtherProcessing;
        
        /// <summary>
        /// Defines the further processing of this message.
        /// </summary>
        public MessageFurtherProcessing FurtherProcessing
        {
            get => _furtherProcessing;
            set
            {
                if (IsOneWay && value == MessageFurtherProcessing.TerminateAndReturnException)
                {
                    _furtherProcessing = MessageFurtherProcessing.TerminateSilently;
                }
                else
                {
                    _furtherProcessing = value;
                }
            }
        }

        /// <summary>
        /// Gets text will be used as <see cref="MessageProcessTerminatedException.Message"/>.
        /// </summary>
        public string MessageOfMessageProcessTerminatedException { get; private set; }

        /// <summary>
        /// Lets Remote Agency continue processing.
        /// </summary>
        public void SetToContinue() => _furtherProcessing = MessageFurtherProcessing.Continue;
        
        /// <summary>
        /// Terminates this process and send an instance of <see cref="MessageProcessTerminatedException" /> back to the sender. Cannot be used when <see cref="BeforeMessageProcessingEventArgsBase.IsOneWay"/> is <see langword="true" />.
        /// </summary>
        /// <param name="message">Message of the exception.</param>
        public void SetToTerminateAndReturnException(
            string message = "Remote Agency Manager terminated this message processing due to user request.")
        {
            if (IsOneWay)
            {
                throw new InvalidOperationException("This method is not allowed for processing one-way messages.");
            }

            _furtherProcessing = MessageFurtherProcessing.TerminateAndReturnException;
            MessageOfMessageProcessTerminatedException = message;
        }

        /// <summary>
        /// Replaces this message by an instance of <see cref="MessageProcessTerminatedException" /> then sends it to the receiver. Cannot be used when <see cref="MessageDirection"/> is <see cref="SecretNest.RemoteAgency.MessageFiltering.MessageDirection.Sending"/>.
        /// </summary>
        /// <param name="message">Message of the exception.</param>
        /// <remarks>Caution: This may cause the sender throw <see cref="TimeoutException"/> if <see cref="IsOneWay"/> is set to <see langword="false"/>.</remarks>
        public void SetToReplaceWithException(
            string message = "Remote Agency Manager terminated this message processing due to user request.")
        {
            if (MessageDirection == MessageDirection.Sending)
            {
                throw new InvalidOperationException("This method is not allowed for processing messages for sending.");
            }

            _furtherProcessing = MessageFurtherProcessing.ReplaceWithException;
            MessageOfMessageProcessTerminatedException = message;
        }

        /// <summary>
        /// Replaces this message by an instance of <see cref="MessageProcessTerminatedException" /> then sends it to the receiver and the sender.
        /// </summary>
        /// <param name="message"></param>
        public void SetToReplaceWithExceptionAndReturn(
            string message = "Remote Agency Manager terminated this message processing due to user request.")
        {
            if (IsOneWay)
            {
                throw new InvalidOperationException("This method is not allowed for processing one-way messages.");
            }
            if (MessageDirection == MessageDirection.Sending)
            {
                throw new InvalidOperationException("This method is not allowed for processing messages for sending.");
            }

            _furtherProcessing = MessageFurtherProcessing.ReplaceWithExceptionAndReturn;
            MessageOfMessageProcessTerminatedException = message;
        }
        
        /// <summary>
        /// Terminates this process silently.
        /// </summary>
        /// <remarks>Caution: This may cause the sender throw <see cref="TimeoutException"/>.</remarks>
        public void SetToTerminateSilently()
        {
            _furtherProcessing = MessageFurtherProcessing.TerminateSilently;
        }
        #endregion

        /// <summary>
        /// Gets the direction of the message.
        /// </summary>
        public MessageDirection MessageDirection { get; }

        /// <summary>
        /// Gets the message body.
        /// </summary>
        public abstract IRemoteAgencyMessage MessageBodyGeneric { get; }

        #region Members from message
        /// <summary>
        /// Site id of the source Remote Agency manager.
        /// </summary>
        public Guid SenderSiteId => MessageBodyGeneric.SenderSiteId;

        /// <summary>
        /// Site id of the target Remote Agency manager.
        /// </summary>
        public Guid TargetSiteId => MessageBodyGeneric.TargetSiteId;

        /// <summary>
        /// Instance id of the source proxy or service wrapper.
        /// </summary>
        public Guid SenderInstanceId => MessageBodyGeneric.SenderInstanceId;

        /// <summary>
        /// Instance id of the target proxy or service wrapper.
        /// </summary>
        public Guid TargetInstanceId => MessageBodyGeneric.TargetInstanceId;

        /// <summary>
        /// Message type.
        /// </summary>
        public MessageType MessageType => MessageBodyGeneric.MessageType;

        /// <summary>
        /// Asset name.
        /// </summary>
        public string AssetName => MessageBodyGeneric.AssetName;

        /// <summary>
        /// Message id.
        /// </summary>
        public Guid MessageId => MessageBodyGeneric.MessageId;

        /// <summary>
        /// Exception object.
        /// </summary>
        public Exception Exception => MessageBodyGeneric.Exception;

        /// <summary>
        /// Whether this message is one way (do not need any response).
        /// </summary>
        public bool IsOneWay => MessageBodyGeneric.IsOneWay;
        #endregion

        /// <summary>
        /// Initializes an instance of BeforeMessageProcessingEventArgsBase.
        /// </summary>
        /// <param name="messageDirection">Direction of the message.</param>
        protected BeforeMessageProcessingEventArgsBase(MessageDirection messageDirection)
        {
            _furtherProcessing = MessageFurtherProcessing.Continue;
            MessageDirection = messageDirection;
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
        /// Terminates this process and send an instance of <see cref="MessageProcessTerminatedException" /> back to the sender. Cannot be used when <see cref="BeforeMessageProcessingEventArgsBase.IsOneWay"/> is <see langword="true" />.
        /// </summary>
        TerminateAndReturnException,
        /// <summary>
        /// Replaces this message by an instance of <see cref="MessageProcessTerminatedException" /> then sends it to the receiver. Cannot be used when <see cref="BeforeMessageProcessingEventArgsBase.MessageDirection"/> is <see cref="MessageDirection.Sending"/>.
        /// </summary>
        ReplaceWithException,
        /// <summary>
        /// Replaces this message by an instance of <see cref="MessageProcessTerminatedException" /> then sends it to the receiver and the sender. Cannot be used when <see cref="BeforeMessageProcessingEventArgsBase.MessageDirection"/> is <see cref="MessageDirection.Sending"/> or <see cref="BeforeMessageProcessingEventArgsBase.IsOneWay"/> is <see langword="true" />. 
        /// </summary>
        ReplaceWithExceptionAndReturn,
        /// <summary>
        /// Terminates this process silently.
        /// </summary>
        TerminateSilently
    }

    /// <summary>
    /// Indicates the direction of the message.
    /// </summary>
    public enum MessageDirection
    {
        /// <summary>
        /// Indicates this message is received from object outside this instance of Remote Agency.
        /// </summary>
        Receiving,
        /// <summary>
        /// Indicates this message is prepared to send to a target instance of Remote Agency.
        /// </summary>
        Sending
    }

    /// <summary>
    /// Represents a message to be checked for sending or processing after received.
    /// </summary>
    /// <typeparam name="TEntityBase">Type of the parent class of all entities.</typeparam>
    /// <seealso cref="RemoteAgency{TSerialized, TEntityBase}.BeforeMessageSending"/>
    /// <seealso cref="RemoteAgency{TSerialized, TEntityBase}.AfterMessageReceived"/>
    /// <seealso cref="MessageProcessTerminatedException"/>
    public class BeforeMessageProcessingEventArgs<TEntityBase> : BeforeMessageProcessingEventArgsBase
    {
        /// <summary>
        /// Gets the message body.
        /// </summary>
        public TEntityBase MessageBody { get; }

        /// <summary>
        /// Initializes an instance of BeforeMessageProcessingEventArgsBase.
        /// </summary>
        /// <param name="messageDirection">Direction of the message.</param>
        /// <param name="messageBody">Message.</param>
        public BeforeMessageProcessingEventArgs(MessageDirection messageDirection, TEntityBase messageBody) : base(messageDirection)
        {
            MessageBody = messageBody;
        }

        /// <inheritdoc />
        public override IRemoteAgencyMessage MessageBodyGeneric => (IRemoteAgencyMessage) MessageBody;
    }
}
