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
    public class MessageProcessTerminatedException : Exception
    {
        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        /// <returns>A string representation of the current exception.</returns>
        public override string ToString() => Message;

        /// <summary>
        /// Initializes an instance of the MessageProcessTerminatedException.
        /// </summary>
        public MessageProcessTerminatedException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the MessageProcessTerminatedException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        protected MessageProcessTerminatedException(SerializationInfo info, StreamingContext context) : base(info,
            context)
        { }
    }
}
