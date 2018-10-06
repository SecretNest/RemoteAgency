using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// The exception that is thrown when the packaged message received cannot be recognized or decoded.
    /// </summary>
    /// <typeparam name="TNetworkMessage">Type of the message for transporting.</typeparam>
    /// <seealso cref="PackingHelperBase{TNetworkMessage, TSerialized}"/>
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
        public PackagedMessageNotRecognizedException(TNetworkMessage packagedMessage)
        {
            PackagedMessage = packagedMessage;
        }

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
