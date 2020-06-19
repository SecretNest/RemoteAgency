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
        /// <param name="exception">Exception thrown while running user code.</param>
        /// <returns>Message contains the data to be returned.</returns>
        IRemoteAgencyMessage ProcessMethodCallingMessageWithReturn(IRemoteAgencyMessage message, out Exception exception);

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
        /// <param name="exception">Exception thrown while running user code.</param>
        /// <returns>Message contains the data to be returned.</returns>
        IRemoteAgencyMessage ProcessEventRaisingMessageWithReturn(IRemoteAgencyMessage message, out Exception exception);
        
        /// <summary>
        /// Processes an event raising message.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        void ProcessEventRaisingMessageWithoutReturn(IRemoteAgencyMessage message);

        /// <summary>
        /// Processes a property getting message and get the data returned.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        /// <param name="exception">Exception thrown while running user code.</param>
        /// <returns>Message contains the data to be returned.</returns>
        IRemoteAgencyMessage ProcessPropertyGettingMessage(IRemoteAgencyMessage message, out Exception exception);

        /// <summary>
        /// Processes a property setting message and get the data returned.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        /// <param name="exception">Exception thrown while running user code.</param>
        /// <returns>Message contains the data to be returned.</returns>
        IRemoteAgencyMessage ProcessPropertySettingMessageWithReturn(IRemoteAgencyMessage message, out Exception exception);

        /// <summary>
        /// Processes a property setting message.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        void ProcessPropertySettingMessageWithoutReturn(IRemoteAgencyMessage message);

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
}
