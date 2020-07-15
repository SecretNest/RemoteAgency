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
        /// Gets or sets the callback for a delegate which will be called while a method calling message need to be sent to a remote site and get response of it.
        /// </summary>
        SendTwoWayMessageCallback SendMethodMessageCallback { get; set; }

        /// <summary>
        /// Gets or sets the callback for a delegate which will be called while a method calling message need to be sent to a remote site without getting response.
        /// </summary>
        SendOneWayMessageCallback SendOneWayMethodMessageCallback { get; set; }

        /// <summary>
        /// Gets or sets the callback for a delegate which will be called while an event adding is requested.
        /// </summary>
        SendTwoWayMessageCallback SendEventAddingMessageCallback { get; set; }

        /// <summary>
        /// Gets or sets the callback for a delegate which will be called while an event removing is requested.
        /// </summary>
        SendTwoWayMessageCallback SendEventRemovingMessageCallback { get; set; }
        
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
        /// Gets or sets the callback for a delegate which will be called while a property getting message need to be sent to a remote site and get response of it.
        /// </summary>
        SendTwoWayMessageCallback SendPropertyGetMessageCallback { get; set; }

        /// <summary>
        /// Gets or sets the callback for a delegate which will be called while a property getting message need to be sent to a remote site without getting response.
        /// </summary>
        SendOneWayMessageCallback SendOneWayPropertyGetMessageCallback { get; set; }

        /// <summary>
        /// Gets or sets the callback for a delegate which will be called while a property setting message need to be sent to a remote site and get response of it.
        /// </summary>
        SendTwoWayMessageCallback SendPropertySetMessageCallback { get; set; }

        /// <summary>
        /// Gets or sets the callback for a delegate which will be called while a property setting message need to be sent to a remote site without getting response.
        /// </summary>
        SendOneWayMessageCallback SendOneWayPropertySetMessageCallback { get; set; }

        /// <summary>
        /// Unlinks specified remote service wrapper from the event registered in proxy objects when the service wrapper is closing.
        /// </summary>
        /// <param name="siteId">The site id of the instance of the Remote Agency which managing the closing service wrapper.</param>
        /// <param name="serviceWrapperInstanceId">The instance id of the closing service wrapper. When set as <see langword="null"/>, all proxies with sticky target site specified by <paramref name="siteId" /> will be reset. Default value is <see langword="null"/>.</param>
        /// <exception cref="AggregateException">When exceptions occurred.</exception>
        void OnRemoteServiceWrapperClosing(Guid siteId, Guid? serviceWrapperInstanceId = null);
    }
}
