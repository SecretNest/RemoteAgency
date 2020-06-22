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
    public interface IProxyCommunicate : IManagedObjectCommunicate
    {
        /// <summary>
        /// Gets or sets the id of this proxy instance.
        /// </summary>
        Guid InstanceId { get; set; }

        /// <summary>
        /// Will be called for resetting the proxy sticky target site to the original state.
        /// </summary>
        Action ProxyStickyTargetSiteResetCallback { get; set; }

        /// <summary>
        /// Will be called for querying the proxy sticky target site state.
        /// </summary>
        ProxyStickyTargetSiteQueryCallback ProxyStickyTargetSiteQueryCallback { get; set; }

        /// <summary>
        /// Will be called while a method calling message need to be sent to a remote site and get response of it.
        /// </summary>
        SendTwoWayMessageCallback SendMethodMessageCallback { get; set; }

        /// <summary>
        /// Will be called while a method calling message need to be sent to a remote site without getting response.
        /// </summary>
        SendOneWayMessageCallback SendOneWayMethodMessageCallback { get; set; }

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
        /// <param name="localExceptionHandlingMode">Local exception handling mode.</param>
        /// <returns>Message contains the data to be returned.</returns>
        IRemoteAgencyMessage ProcessEventRaisingMessage(IRemoteAgencyMessage message, out Exception exception, out LocalExceptionHandlingMode localExceptionHandlingMode);
        
        /// <summary>
        /// Processes an event raising message.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        /// <param name="localExceptionHandlingMode">Local exception handling mode.</param>
        void ProcessOneWayEventRaisingMessage(IRemoteAgencyMessage message, out LocalExceptionHandlingMode localExceptionHandlingMode);

        /// <summary>
        /// Will be called while a property getting message need to be sent to a remote site and get response of it.
        /// </summary>
        SendTwoWayMessageCallback SendPropertyGetMessageCallback { get; set; }

        /// <summary>
        /// Will be called while a property getting message need to be sent to a remote site without getting response.
        /// </summary>
        SendOneWayMessageCallback SendOneWayPropertyGetMessageCallback { get; set; }

        /// <summary>
        /// Will be called while a property setting message need to be sent to a remote site and get response of it.
        /// </summary>
        SendTwoWayMessageCallback SendPropertySetMessageCallback { get; set; }

        /// <summary>
        /// Will be called while a property setting message need to be sent to a remote site without getting response.
        /// </summary>
        SendOneWayMessageCallback SendOneWayPropertySetMessageCallback { get; set; }
    }
}
