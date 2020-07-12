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
    public interface IServiceWrapperCommunicate : IManagedObjectCommunicate
    {
        /// <summary>
        /// Processes a method calling message and returns response.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        /// <param name="exception">Exception thrown while running user code.</param>
        /// <param name="localExceptionHandlingMode">Local exception handling mode.</param>
        /// <returns>Message contains the data to be returned.</returns>
        IRemoteAgencyMessage ProcessMethodMessage(IRemoteAgencyMessage message, out Exception exception, out LocalExceptionHandlingMode localExceptionHandlingMode);

        /// <summary>
        /// Processes a one way method calling message.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        /// <param name="localExceptionHandlingMode">Local exception handling mode.</param>
        void ProcessOneWayMethodMessage(IRemoteAgencyMessage message, out LocalExceptionHandlingMode localExceptionHandlingMode);

        /// <summary>
        /// Processes an event adding.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        /// <param name="localExceptionHandlingMode">Local exception handling mode.</param>
        void ProcessEventAddingMessage(IRemoteAgencyMessage message, out LocalExceptionHandlingMode localExceptionHandlingMode);
        
        /// <summary>
        /// Processes an event removing.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        /// <param name="localExceptionHandlingMode">Local exception handling mode.</param>
        void ProcessEventRemovingMessage(IRemoteAgencyMessage message, out LocalExceptionHandlingMode localExceptionHandlingMode);

        /// <summary>
        /// Gets or sets the callback for a delegate which will be called while an event raising message need to be sent to a remote site and get response of it.
        /// </summary>
        SendTwoWayMessageCallback SendEventMessageCallback { get; set; }

        /// <summary>
        /// Gets or sets the callback for a delegate which will be called while an event raising message need to be sent to a remote site without getting response.
        /// </summary>
        SendOneWayMessageCallback SendOneWayEventMessageCallback { get; set; }

        /// <summary>
        /// Processes a property getting message and returns response.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        /// <param name="exception">Exception thrown while running user code.</param>
        /// <param name="localExceptionHandlingMode">Local exception handling mode.</param>
        /// <returns>Message contains the data to be returned.</returns>
        IRemoteAgencyMessage ProcessPropertyGettingMessage(IRemoteAgencyMessage message, out Exception exception, out LocalExceptionHandlingMode localExceptionHandlingMode);

        /// <summary>
        /// Processes a property getting message.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        /// <param name="localExceptionHandlingMode">Local exception handling mode.</param>
        void ProcessOneWayPropertyGettingMessage(IRemoteAgencyMessage message, out LocalExceptionHandlingMode localExceptionHandlingMode);

        /// <summary>
        /// Processes a property setting message and returns response.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        /// <param name="exception">Exception thrown while running user code.</param>
        /// <param name="localExceptionHandlingMode">Local exception handling mode.</param>
        /// <returns>Message contains the data to be returned.</returns>
        IRemoteAgencyMessage ProcessPropertySettingMessage(IRemoteAgencyMessage message, out Exception exception, out LocalExceptionHandlingMode localExceptionHandlingMode);

        /// <summary>
        /// Processes a property setting message.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        /// <param name="localExceptionHandlingMode">Local exception handling mode.</param>
        void ProcessOneWayPropertySettingMessage(IRemoteAgencyMessage message, out LocalExceptionHandlingMode localExceptionHandlingMode);

        /// <summary>
        /// Unlinks specified remote proxy from the event registered in service wrapper objects.
        /// </summary>
        /// <param name="siteId">The site id of the instance of the Remote Agency which managing the closing proxy.</param>
        /// <param name="proxyInstanceId">The instance id of the closing proxy. When set to null, all proxies from the site specified by <paramref name="siteId" /> will be unlinked. Default value is null.</param>
        /// <exception cref="AggregateException">When exceptions occurred.</exception>
        void OnRemoteProxyClosing(Guid siteId, Guid? proxyInstanceId = null);
    }
}
