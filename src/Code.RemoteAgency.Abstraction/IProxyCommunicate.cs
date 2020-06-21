using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Represents a created proxy object that can communicate with Remote Agency.
    /// </summary>
    public interface IProxyCommunicate
    {
        /// <summary>
        /// Will be called after this object is linked to a Remote Agency.
        /// </summary>
        void AfterInitialized();

        /// <summary>
        /// Will be called when disposing.
        /// </summary>
        ProxyDisposingCallback DisposingRequestedCallback { get; set; }

        /// <summary>
        /// Will be called while a method calling message need to be sent to a remote site and get response of it.
        /// </summary>
        SendMessageAndGetReturnCallback SendMethodMessageCallback { get; set; }

        /// <summary>
        /// Will be called while a method calling message need to be sent to a remote site without getting response.
        /// </summary>
        SendMessageWithoutReturnCallback SendOneWayMethodMessageCallback { get; set; }

        /// <summary>
        /// Will be called while an event adding is requested.
        /// </summary>
        SendEmptyMessageCallback SendEventAddingMessageCallback { get; set; }

        /// <summary>
        /// Will be called while an event removing is requested.
        /// </summary>
        SendEmptyMessageCallback SendEventRemovingMessageCallback { get; set; }
        
        /// <summary>
        /// Processes an event raising message and returns response.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        /// <param name="exception">Exception thrown while running user code.</param>
        /// <returns>Message contains the data to be returned.</returns>
        IRemoteAgencyMessage ProcessEventRaisingMessage(IRemoteAgencyMessage message, out Exception exception);
        
        /// <summary>
        /// Processes an event raising message.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        void ProcessOneWayEventRaisingMessage(IRemoteAgencyMessage message);

        /// <summary>
        /// Will be called while a property getting message need to be sent to a remote site and get response of it.
        /// </summary>
        SendMessageAndGetReturnCallback SendPropertyGetMessageCallback { get; set; }

        /// <summary>
        /// Will be called while a property setting message need to be sent to a remote site and get response of it.
        /// </summary>
        SendMessageAndGetReturnCallback SendPropertySetMessageCallback { get; set; }

        /// <summary>
        /// Will be called while a property setting message need to be sent to a remote site without getting response.
        /// </summary>
        SendMessageAndGetReturnCallback SendOneWayPropertySetMessageCallback { get; set; }
    }

}
