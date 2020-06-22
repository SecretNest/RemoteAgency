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
    /// Sends an empty message out.
    /// </summary>
    /// <param name="messageType">Message type.</param>
    /// <param name="assetName">Asset name.</param>
    /// <param name="timeout">Timeout in millisecond.</param>
    /// <exception cref="AccessingTimeOutException">Thrown when timed out.</exception>
    public delegate void SendEmptyMessageCallback(MessageType messageType, string assetName, int timeout);

    /// <summary>
    /// Queries the proxy sticky target site setting state.
    /// </summary>
    /// <param name="isEnabled">Will be set as whether this function is enabled on this proxy.</param>
    /// <param name="defaultTargetSiteId">Will be set as default target site id.</param>
    /// <param name="stickyTargetSiteId">Will be set as sticky target site id. Value will be set to <see langword="null"/> if no sticky target set yet.</param>
    public delegate void ProxyStickyTargetSiteQueryCallback(out bool isEnabled, out Guid defaultTargetSiteId,
        out Guid? stickyTargetSiteId);

}
