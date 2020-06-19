using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.MessageFiltering
{
    /// <summary>
    /// Represents a message to be checked for sending or processing after received. This is an abstract class.
    /// </summary>
    /// <seealso cref="BeforeMessageProcessingEventArgs{TSerialized, TEntityBase}"/>
    /// <seealso cref="MessageProcessTerminatedException"/>
    public abstract class BeforeMessageProcessingEventArgsBase : MessageBodyEventArgsBase
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
        /// Gets text will be used as the message of <see cref="MessageProcessTerminatedException"/>.
        /// </summary>
        public string MessageOfMessageProcessTerminatedException { get; private set; }

        /// <summary>
        /// Lets Remote Agency continue processing.
        /// </summary>
        public void SetToContinue() => _furtherProcessing = MessageFurtherProcessing.Continue;
        
        /// <summary>
        /// Terminates this process and send an instance of <see cref="MessageProcessTerminatedException" /> back to the sender. Cannot be used when <see cref="MessageBodyEventArgsBase.IsOneWay"/> is <see langword="true" />.
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
        /// <remarks>Caution: This may cause the sender throw <see cref="TimeoutException"/> if <see cref="MessageBodyEventArgsBase.IsOneWay"/> is set to <see langword="false"/>.</remarks>
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
        /// Terminates this process and send an instance of <see cref="MessageProcessTerminatedException" /> back to the sender. Cannot be used when <see cref="MessageBodyEventArgsBase.IsOneWay"/> is <see langword="true" />.
        /// </summary>
        TerminateAndReturnException,
        /// <summary>
        /// Replaces this message by an instance of <see cref="MessageProcessTerminatedException" /> then sends it to the receiver. Cannot be used when <see cref="BeforeMessageProcessingEventArgsBase.MessageDirection"/> is <see cref="MessageDirection.Sending"/>.
        /// </summary>
        ReplaceWithException,
        /// <summary>
        /// Replaces this message by an instance of <see cref="MessageProcessTerminatedException" /> then sends it to the receiver and the sender. Cannot be used when <see cref="BeforeMessageProcessingEventArgsBase.MessageDirection"/> is <see cref="MessageDirection.Sending"/> or <see cref="MessageBodyEventArgsBase.IsOneWay"/> is <see langword="true" />. 
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
    /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
    /// <typeparam name="TEntityBase">Type of the parent class of all entities.</typeparam>
    /// <seealso cref="MessageProcessTerminatedException"/>
    public class BeforeMessageProcessingEventArgs<TSerialized, TEntityBase> : BeforeMessageProcessingEventArgsBase, IMessageBodyGenericEventArgs<TSerialized, TEntityBase>
    {
        /// <summary>
        /// Gets the message body.
        /// </summary>
        public TEntityBase MessageBody { get; }

        readonly Lazy<TSerialized> _serialized;

        /// <inheritdoc />
        public TSerialized Serialize()
        {
            return _serialized.Value;
        }

        /// <inheritdoc />
        public override IRemoteAgencyMessage MessageBodyGeneric => (IRemoteAgencyMessage) MessageBody;

        /// <summary>
        /// Initializes an instance of BeforeMessageProcessingEventArgsBase.
        /// </summary>
        /// <param name="messageDirection">Direction of the message.</param>
        /// <param name="messageBody">Message.</param>
        /// <param name="serializerCallback">Callback for serializing message body.</param>
        public BeforeMessageProcessingEventArgs(MessageDirection messageDirection, TEntityBase messageBody, Func<TEntityBase, TSerialized> serializerCallback) : base(messageDirection)
        {
            MessageBody = messageBody;
            _serialized = new Lazy<TSerialized>(() => serializerCallback(MessageBody));
        }
    }
}
