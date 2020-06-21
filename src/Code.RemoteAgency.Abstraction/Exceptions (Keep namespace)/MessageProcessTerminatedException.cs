using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// The exception to indicate a message processing is terminated.
    /// </summary>
    [Serializable]
    public sealed class MessageProcessTerminatedException : Exception
    {
        /// <summary>
        /// Gets the terminated message.
        /// </summary>
        public IRemoteAgencyMessage TerminatedMessage { get; }

        /// <summary>
        /// Gets the position where the message is terminated.
        /// </summary>
        public MessageProcessTerminatedPosition TerminatedPosition { get; }

        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        /// <returns>A string representation of the current exception.</returns>
        public override string ToString() => Message;

        /// <summary>
        /// Initializes an instance of the MessageProcessTerminatedException.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="terminatedPosition">Position where the message is terminated.</param>
        /// <param name="terminatedMessage">Terminated message.</param>
        public MessageProcessTerminatedException(string message, MessageProcessTerminatedPosition terminatedPosition, IRemoteAgencyMessage terminatedMessage) : base(message)
        {
            TerminatedMessage = terminatedMessage;
            TerminatedPosition = terminatedPosition;
        }

        /// <summary>
        /// Initializes a new instance of the MessageProcessTerminatedException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        private MessageProcessTerminatedException(SerializationInfo info, StreamingContext context) : base(info,
            context)
        {
            var terminatedMessageType = (Type)info.GetValue("TerminatedMessageType", typeof(Type));
            TerminatedMessage = (IRemoteAgencyMessage)info.GetValue("TerminatedMessage", terminatedMessageType);
            TerminatedPosition = (MessageProcessTerminatedPosition)info.GetValue("TerminatedPosition", typeof(MessageProcessTerminatedPosition));
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("TerminatedMessageType", TerminatedMessage.GetType());
            info.AddValue("TerminatedMessage", TerminatedMessage);
            info.AddValue("TerminatedPosition", TerminatedPosition);
        }
    }

    /// <summary>
    /// Defines the position where the message can be terminated.
    /// </summary>
    [Serializable]
    public enum MessageProcessTerminatedPosition
    {
        /// <summary>
        /// Before sending
        /// </summary>
        BeforeSending,
        /// <summary>
        /// After received
        /// </summary>
        AfterReceived
    }
}
