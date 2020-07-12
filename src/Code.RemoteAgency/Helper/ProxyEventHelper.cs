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
        /// Unlinks specified remote service wrapper from the event registered in proxy objects when the service wrapper is closing.
        /// </summary>
        /// <param name="siteId">The site id of the instance of the Remote Agency which managing the closing service wrapper.</param>
        /// <param name="serviceWrapperInstanceId">The instance id of the closing service wrapper. When set to null, all proxies with sticky target site specified by <paramref name="siteId" /> will be reset. Default value is null.</param>
        /// <exception cref="AggregateException">When exceptions occurred.</exception>
        public void OnRemoteServiceWrapperClosing(Guid siteId, Guid? serviceWrapperInstanceId = null)
        {

        }

        /// <summary>
        /// Sends messages to all relevant objects and closes the functions of this object.
        /// </summary>
        /// <param name="sendSpecialCommand">Whether need to send special command.</param>
        /// <exception cref="AggregateException">When exceptions occurred.</exception>
        public void CloseRequestedByManagingObject(bool sendSpecialCommand)
        {

        }

        /// <summary>
        /// Adds a builder.
        /// </summary>
        /// <param name="assetName">Name of the event.</param>
        /// <param name="router">An instance of a derived class of ProxyEventRouterBase.</param>
        public void AddRouter(string assetName, ProxyEventRouterBase router)
        {
        }
    }

    /// <summary>
    /// Defines a helper class to be implanted into built assembly for handling an event handler in proxy. This is an abstract class.
    /// </summary>
    public abstract class ProxyEventRouterBase
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
        public void ProcessOneWayEventRaisingMessage(IRemoteAgencyMessage message)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Defines a helper class to be implanted into built assembly for handling an event handler in proxy. This is an abstract class.
    /// </summary>
    /// <typeparam name="TDelegate">Delegate of event.</typeparam>
    public abstract class ProxyEventRouterBase<TDelegate>: ProxyEventRouterBase
    {


        /// <summary>
        /// Processes an event adding.
        /// </summary>
        /// <param name="value">Handler.</param>
        public void ProcessEventAdding(TDelegate value)
        {

        }

        /// <summary>
        /// Processes an event removing.
        /// </summary>
        /// <param name="value">Handler.</param>
        public void ProcessEventRemoving(TDelegate value)
        {

        }
    }
    
    /// <summary>
    /// Defines a helper class to be implanted into built assembly for handling an one way event handler in proxy. This is an abstract class.
    /// </summary>
    /// <typeparam name="TDelegate">Delegate of event.</typeparam>
    /// <typeparam name="TParameterEntity">Parameter entity type.</typeparam>
    public abstract class ProxyEventRouterBase<TDelegate, TParameterEntity> : ProxyEventRouterBase<TDelegate>
    {

    }

    /// <summary>
    /// Defines a helper class to be implanted into built assembly for handling an two way event handler in proxy. This is an abstract class.
    /// </summary>
    /// <typeparam name="TDelegate">Delegate of event.</typeparam>
    /// <typeparam name="TParameterEntity">Parameter entity type.</typeparam>
    /// <typeparam name="TReturnValueEntity">Return value entity type.</typeparam>
    public abstract class ProxyEventRouterBase<TDelegate, TParameterEntity, TReturnValueEntity> : ProxyEventRouterBase<TDelegate>
    {
        private readonly int _timeout;

        /// <summary>
        /// Initialize an instance of ProxyEventRouterBase.
        /// </summary>
        /// <param name="timeout">Timeout for waiting for the response of event raising.</param>
        protected ProxyEventRouterBase(int timeout)
        {
            _timeout = timeout;
        }


    }
}
