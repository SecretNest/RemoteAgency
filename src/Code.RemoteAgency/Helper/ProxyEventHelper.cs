using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Helper
{
    /// <summary>
    /// Defines a helper class to be implanted into built assembly for event servicing in proxy.
    /// </summary>
    public class ProxyEventHelper
    {
        /// <summary>
        /// Processes an event raising message and returns response.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        /// <param name="exception">Exception thrown while running user code.</param>
        /// <returns>Message contains the data to be returned.</returns>
        public IRemoteAgencyMessage ProcessEventRaisingMessage(IRemoteAgencyMessage message, out Exception exception)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Processes an event raising message.
        /// </summary>
        /// <param name="message">Message to be processed.</param>
        /// <param name="localExceptionHandlingMode">Local exception handling mode.</param>
        public void ProcessOneWayEventRaisingMessage(IRemoteAgencyMessage message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Will be called while an event adding is requested.
        /// </summary>
        SendEmptyMessageCallback SendEventAddingMessageCallback { get; set; }

        /// <summary>
        /// Will be called while an event removing is requested.
        /// </summary>
        SendEmptyMessageCallback SendEventRemovingMessageCallback { get; set; }

        /// <summary>
        /// Will be called while an event removing is requested. Only for disposing object.
        /// </summary>
        SendOneWayEmptyMessageCallback SendOneWayEventRemovingMessageCallback { get; set; }

        /// <summary>
        /// Will be called when proxy is disposing.
        /// </summary>
        /// <param name="exception">Exception occurred while disposing. When no exception, the value will be set to <see langword="null"/>.</param>
        public void OnDisposing(out Exception exception)
        {
            throw new NotImplementedException();
        }

        public void AddRouter(string assetName, ProxyEventRouterBase router)
        {

        }
    }

    /// <summary>
    /// Defines a helper class to be implanted into built assembly for handling an event handler in proxy. This is an abstract class.
    /// </summary>
    public abstract class ProxyEventRouterBase
    {

    }

    /// <summary>
    /// Defines a helper class to be implanted into built assembly for handling an event handler in proxy. This is an abstract class.
    /// </summary>
    /// <typeparam name="T">Type of the delegate of event.</typeparam>
    public abstract class ProxyEventRouterBase<T> : ProxyEventRouterBase
    {
        /// <summary>
        /// Processes an event adding.
        /// </summary>
        /// <param name="value">Handler.</param>
        public void ProcessEventAdding(T value)
        {

        }

        /// <summary>
        /// Processes an event removing.
        /// </summary>
        /// <param name="value">Handler.</param>
        public void ProcessEventRemoving(T value)
        {

        }
    }
}
