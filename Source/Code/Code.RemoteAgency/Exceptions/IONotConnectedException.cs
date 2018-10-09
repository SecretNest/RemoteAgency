using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// The exception that is thrown when IO is not connected at the local <see cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}"/> instance.
    /// </summary>
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

        internal IONotConnectedException() { }
    }
}
