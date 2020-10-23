using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// The exception that is thrown when an asset marked by <see cref="AssetIgnoredAttribute"/> with <see cref="AssetIgnoredAttribute.WillThrowException"/> is set as <see langword="true" />.
    /// </summary>
    [Serializable]
    public sealed class IgnoredAssetException : InvalidOperationException
    {

        /// <inheritdoc />
        public override string Message => "The asset accessing is marked by AssetIgnoredAttribute with WillThrowException set.";

        /// <summary>
        /// Initializes a new instance of the AccessingTimeOutException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        private IgnoredAssetException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
