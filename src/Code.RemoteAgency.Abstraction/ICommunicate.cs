using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Represents a created object that can communicate with Remote Agency Manager.
    /// </summary>
    public interface ICommunicate
    {
        /// <summary>
        /// Will be called after this object is linked to a Remote Agency Manager.
        /// </summary>
        void AfterInitialized();

        /// <summary>
        /// Processes a message.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        void ProcessMessage(IRemoteAgencyMessage message);

        /// <summary>
        /// Should be called while an exception is raised in user code.
        /// </summary>
        /// <seealso cref="LocalExceptionHandlingAttribute"/>
        /// <remarks>This will only be called when <see cref="LocalExceptionHandlingAttribute"/> exists and the <see cref="LocalExceptionHandlingMode"/> is set to Redirect. When it's not set, the exception will be suppressed.</remarks>
        RedirectedExceptionRaisedCallback RedirectedExceptionRaisedCallback { get; set; }
        /// <summary>
        /// Should be called while waiting for response is timed out.
        /// </summary>
        /// <seealso cref="CustomizedOperatingTimeoutTimeAttribute"/>
        MessageWaitingTimedOutCallback MessageWaitingTimedOutCallback { get; set; }
        /// <summary>
        /// Should be called while a message should be sent to a remote site.
        /// </summary>
        SendMessageCallback SendMessageCallback { get; set; }
    }

    /// <summary>
    /// Sends a message out.
    /// </summary>
    /// <param name="message">Message to be sent.</param>
    /// <seealso cref="SendMessageCallback"/>
    public delegate void SendMessageCallback(IRemoteAgencyMessage message);

    /// <summary>
    /// Notifies a message waiting is timed out.
    /// </summary>
    /// <param name="messageId">Id of the message.</param>
    /// <seealso cref="MessageWaitingTimedOutCallback"/>
    /// <seealso cref="CustomizedOperatingTimeoutTimeAttribute"/>
    public delegate void MessageWaitingTimedOutCallback(Guid messageId);

    /// <summary>
    /// Redirects the exception raised in user code.
    /// </summary>
    /// <param name="messageType"><see cref="MessageType">Message type.</see></param>
    /// <param name="assetName">Asset name.</param>
    /// <param name="messageId">Id of the message.</param>
    /// <param name="interfaceType">The type of the interface related to this asset.</param>
    /// <param name="exception">Raised exception object.</param>
    /// <seealso cref="LocalExceptionHandlingAttribute"/>
    /// <seealso cref="RedirectedExceptionRaisedCallback"/>
    public delegate void RedirectedExceptionRaisedCallback(MessageType messageType, string assetName, Guid messageId, Type interfaceType, Exception exception);

}
