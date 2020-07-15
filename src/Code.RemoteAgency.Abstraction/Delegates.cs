using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Sends a message out and gets a response message.
    /// </summary>
    /// <param name="message">Message to be sent.</param>
    /// <param name="timeout">Timeout in millisecond.</param>
    /// <returns>Response message. Value <see langword="null" /> will be returned if no return required by <paramref name="message"/>.</returns>
    /// <exception cref="AccessingTimeOutException">Thrown when timed out.</exception>
    public delegate IRemoteAgencyMessage SendTwoWayMessageCallback(IRemoteAgencyMessage message, int timeout);

    /// <summary>
    /// Sends a message out.
    /// </summary>
    /// <param name="message">Message to be sent.</param>
    public delegate void SendOneWayMessageCallback(IRemoteAgencyMessage message);

    /// <summary>
    /// Creates an empty message which is allowed to be serialized.
    /// </summary>
    /// <returns>Empty message.</returns>
    public delegate IRemoteAgencyMessage CreateEmptyMessageCallback();
}
