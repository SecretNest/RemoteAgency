using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// The exception that is thrown when <see cref="BeforeMessageProcessingEventArgsBase.FurtherProcessing"/> is set as TerminateWithExceptionReturned.
    /// </summary>
    /// <seealso cref="BeforeMessageProcessingEventArgs{TSerialized}"/>
    /// <seealso cref="BeforeExceptionMessageProcessingEventArgs{TSerialized}"/>
    /// <seealso cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}.BeforeMessageSending"/>
    /// <seealso cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}.AfterMessageReceived"/>
    [Serializable]
    public class MessageProcessTerminatedException : Exception
    {
        /// <summary>
        /// Gets the error message of the current exception.
        /// </summary>
        public override string Message => "Remote Agency Manager terminated this message processing due to user request.";
        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        /// <returns>A string representation of the current exception.</returns>
        public override string ToString()
        {
            return Message;
        }

        /// <summary>
        /// Initializes an instance of the MessageProcessTerminatedException.
        /// </summary>
        public MessageProcessTerminatedException() : base() { }

        /// <summary>
        /// Initializes a new instance of the MessageProcessTerminatedException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        public MessageProcessTerminatedException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
