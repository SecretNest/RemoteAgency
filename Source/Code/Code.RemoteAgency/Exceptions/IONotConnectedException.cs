using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// The exception that is thrown when IO is not connected at the local <see cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}"/> instance.
    /// </summary>
    [Serializable]
    public sealed class IONotConnectedException : InvalidOperationException
    {
        /// <summary>
        /// Gets the error message of the current exception.
        /// </summary>
        public override string Message => "RemoteAgency IO not connected.";
        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        /// <returns>A string representation of the current exception.</returns>
        public override string ToString()
        {
            return Message;
        }

        internal IONotConnectedException() : base() { } 

        /// <summary>
        /// Initializes a new instance of the IONotConnectedException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        public IONotConnectedException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }
}
