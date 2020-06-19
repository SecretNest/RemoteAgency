using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Sends a message out and gets a response message.
    /// </summary>
    /// <param name="message">Message to be sent.</param>
    /// <returns>Response message. Value <see langword="null" /> will be returned if no return required by <paramref name="message"/>.</returns>
    /// <exception cref="AccessingTimeOutException">Thrown when timed out.</exception>
    public delegate IRemoteAgencyMessage SendMessageAndGetReturnCallback(IRemoteAgencyMessage message);

    /// <summary>
    /// Sends a message out.
    /// </summary>
    /// <param name="message">Message to be sent.</param>
    public delegate void SendMessageWithoutReturnCallback(IRemoteAgencyMessage message);

    /// <summary>
    /// Sends an empty message out.
    /// </summary>
    /// <param name="messageType">Message type.</param>
    /// <param name="assetName">Asset name.</param>
    /// <exception cref="AccessingTimeOutException">Thrown when timed out.</exception>
    public delegate void SendEmptyMessageCallback(MessageType messageType, string assetName);

    /// <summary>
    /// Notifies the proxy is disposing.
    /// </summary>
    public delegate void ProxyDisposingCallback();
}
