using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// The exception that is thrown when the accessing is timed out.
    /// </summary>
    [Serializable]
    public sealed class AccessingTimeOutException : TimeoutException
    {
        /// <summary>
        /// Initializes an instance of AccessingTimeOutException.
        /// </summary>
        public AccessingTimeOutException()
        { }

        /// <summary>
        /// Initializes a new instance of the AccessingTimeOutException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        private AccessingTimeOutException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
