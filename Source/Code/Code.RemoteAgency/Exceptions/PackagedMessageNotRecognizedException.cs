using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// The exception that is thrown when the packaged message received cannot be recognized or decoded.
    /// </summary>
    /// <typeparam name="TNetworkMessage">Type of the message for transporting.</typeparam>
    /// <seealso cref="PackingHelperBase{TNetworkMessage, TSerialized}"/>
    [Serializable]
    public sealed class PackagedMessageNotRecognizedException<TNetworkMessage> : FormatException
    {
        /// <summary>
        /// Packaged message that cannot be recognized.
        /// </summary>
        public TNetworkMessage PackagedMessage { get; }

        /// <summary>
        /// Initializes an instance of the PackagedMessageNotRecognizedException.
        /// </summary>
        /// <param name="packagedMessage">Packaged message that cannot be recognized.</param>
        internal PackagedMessageNotRecognizedException(TNetworkMessage packagedMessage)
        {
            PackagedMessage = packagedMessage;
        }


        /// <summary>
        /// Initializes a new instance of the PackagedMessageNotRecognizedException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        public PackagedMessageNotRecognizedException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }

        /// <summary>
        /// Gets the error message of the current exception.
        /// </summary>
        public override string Message => string.Format("Packaged message not recognized.\n\nPackaged Message:\n{0}", PackagedMessage);

        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        /// <returns>A string representation of the current exception.</returns>
        public override string ToString()
        {
            return Message;
        }
    }
}
