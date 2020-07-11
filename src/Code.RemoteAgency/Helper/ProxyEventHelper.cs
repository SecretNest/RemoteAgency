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
        //note: addevent: no need target;
        //note: removevent: need target;
       


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
        public void ProcessOneWayEventRaisingMessage(IRemoteAgencyMessage message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets or sets the callback for a delegate which will be called while an event adding is requested.
        /// </summary>
        SendTwoWayMessageCallback SendEventAddingMessageCallback { get; set; }

        /// <summary>
        /// Gets or sets the callback for a delegate which will be called while an event removing is requested.
        /// </summary>
        SendTwoWayMessageCallback SendEventRemovingMessageCallback { get; set; }

        /// <summary>
        /// Gets or sets the callback for a delegate which will be called while a special command message need to be sent to a remote site without getting response.
        /// </summary>
        SendOneWayMessageCallback SendOneWaySpecialCommandMessageCallback { get; set; }
        //Const.SpecialCommandProxyDisposed

        /// <summary>
        /// Gets or sets the callback for a delegate which will be called while an empty message need to be created.
        /// </summary>
        private CreateEmptyMessageCallback CreateEmptyMessageCallback { get; set; }

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
