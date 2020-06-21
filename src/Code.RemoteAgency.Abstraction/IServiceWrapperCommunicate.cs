using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Represents a created service wrapper object that can communicate with Remote Agency.
    /// </summary>
    public interface IServiceWrapperCommunicate
    {
        /// <summary>
        /// Processes a method calling message and returns response.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        /// <param name="exception">Exception thrown while running user code.</param>
        /// <returns>Message contains the data to be returned.</returns>
        IRemoteAgencyMessage ProcessMethodMessage(IRemoteAgencyMessage message, out Exception exception);

        /// <summary>
        /// Processes a one way method calling message.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        void ProcessOneWayMethodMessage(IRemoteAgencyMessage message);

        /// <summary>
        /// Processes an event adding.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        void ProcessEventAddingMessage(IRemoteAgencyMessage message);
        
        /// <summary>
        /// Processes an event removing.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        void ProcessEventRemovingMessage(IRemoteAgencyMessage message);

        /// <summary>
        /// Will be called while an event raising message need to be sent to a remote site and get response of it.
        /// </summary>
        SendMessageAndGetReturnCallback SendEventMessageCallback { get; set; }

        /// <summary>
        /// Will be called while an event raising message need to be sent to a remote site without getting response.
        /// </summary>
        SendMessageWithoutReturnCallback SendOneWayEventMessageCallback { get; set; }

        /// <summary>
        /// Processes a property getting message and returns response.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        /// <param name="exception">Exception thrown while running user code.</param>
        /// <returns>Message contains the data to be returned.</returns>
        IRemoteAgencyMessage ProcessPropertyGettingMessage(IRemoteAgencyMessage message, out Exception exception);

        /// <summary>
        /// Processes a property setting message and returns response.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        /// <param name="exception">Exception thrown while running user code.</param>
        /// <returns>Message contains the data to be returned.</returns>
        IRemoteAgencyMessage ProcessPropertySettingMessage(IRemoteAgencyMessage message, out Exception exception);

        /// <summary>
        /// Processes a property setting message.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        void ProcessOneWayPropertySettingMessage(IRemoteAgencyMessage message);
    }
}
