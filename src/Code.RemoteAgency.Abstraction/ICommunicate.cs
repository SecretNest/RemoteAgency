using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    //TODO: need to clean

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
        /// Processes a method calling message and get the data returned.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        /// <returns>Message contains the data to be returned.</returns>
        IRemoteAgencyMessage ProcessMethodCallingMessageWithReturn(IRemoteAgencyMessage message);

        /// <summary>
        /// Processes a method calling message.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        void ProcessMethodCallingMessageWithoutReturn(IRemoteAgencyMessage message);

        /// <summary>
        /// Processes an event adding message.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        void ProcessEventAddingMessage(IRemoteAgencyMessage message);
        
        /// <summary>
        /// Processes an event removing message.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        void ProcessEventRemovingMessage(IRemoteAgencyMessage message);
        
        /// <summary>
        /// Processes an event raising message.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        /// <returns>Message contains the data to be returned.</returns>
        IRemoteAgencyMessage ProcessEventRaisingMessageWithReturn(IRemoteAgencyMessage message);
        
        /// <summary>
        /// Processes an event raising message.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        void ProcessEventRaisingMessageWithoutReturn(IRemoteAgencyMessage message);

        /// <summary>
        /// Processes a property getting message and get the data returned.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        /// <returns>Message contains the data to be returned.</returns>
        IRemoteAgencyMessage ProcessPropertyGettingMessage(IRemoteAgencyMessage message);

        /// <summary>
        /// Processes a property setting message and get the data returned.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        /// <returns>Message contains the data to be returned.</returns>
        IRemoteAgencyMessage ProcessPropertySettingMessageWithReturn(IRemoteAgencyMessage message);

        /// <summary>
        /// Processes a property setting message.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        void ProcessPropertySettingMessageWithoutReturn(IRemoteAgencyMessage message);

        ///// <summary>
        ///// Should be called while an exception is raised in user code.
        ///// </summary>
        ///// <seealso cref="LocalExceptionHandlingAttribute"/>
        ///// <remarks>This will only be called when <see cref="LocalExceptionHandlingAttribute"/> exists and the <see cref="LocalExceptionHandlingMode"/> is set to Redirect. When it's not set, the exception will be suppressed.</remarks>
        //RedirectedExceptionRaisedCallback RedirectedExceptionRaisedCallback { get; set; }
        ///// <summary>
        ///// Should be called while waiting for response is timed out.
        ///// </summary>
        ///// <seealso cref="CustomizedOperatingTimeoutTimeAttribute"/>
        //MessageWaitingTimedOutCallback MessageWaitingTimedOutCallback { get; set; }
        /// <summary>
        /// Should be called while a message should be sent to a remote site.
        /// </summary>
        SendMessageCallback SendMessageCallback { get; set; }
    }

    /// <summary>
    /// Sends a message out.
    /// </summary>
    /// <param name="message">Message to be sent.</param>
    /// <returns>Message returned. Value <see langword="null" /> will be returned if no return required by <paramref name="message"/>.</returns>
    /// <exception cref="TimeoutException">Thrown when timed out.</exception>
    public delegate IRemoteAgencyMessage SendMessageCallback(IRemoteAgencyMessage message);

    ///// <summary>
    ///// Redirects the exception raised in user code.
    ///// </summary>
    ///// <param name="messageType"><see cref="MessageType">Message type.</see></param>
    ///// <param name="assetName">Asset name.</param>
    ///// <param name="messageId">Id of the message.</param>
    ///// <param name="interfaceType">The type of the interface related to this asset.</param>
    ///// <param name="exception">Raised exception object.</param>
    ///// <seealso cref="LocalExceptionHandlingAttribute"/>
    ///// <seealso cref="RedirectedExceptionRaisedCallback"/>
    //public delegate void RedirectedExceptionRaisedCallback(MessageType messageType, string assetName, Guid messageId, Type interfaceType, Exception exception);

}
