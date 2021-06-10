using System;

namespace SecretNest.RemoteAgency.Injection.EventHelper
{
    /// <summary>
    /// Defines a helper class to be implanted into built assembly for handling a two-way event handler in service wrapper. This is an abstract class.
    /// </summary>
    /// <typeparam name="TServiceContractInterface">Service contract interface type.</typeparam>
    /// <typeparam name="TParameterEntity">Parameter entity type.</typeparam>
    /// <typeparam name="TReturnValueEntity">Return value entity type.</typeparam>
    internal abstract class ServiceWrapperEventRouterBase<TServiceContractInterface, TParameterEntity, TReturnValueEntity>
        : ServiceWrapperEventRouterBase<TServiceContractInterface>
        where TParameterEntity : IRemoteAgencyMessage
        where TReturnValueEntity : IRemoteAgencyMessage
    {
        private readonly int _timeout;

        /// <summary>
        /// Initialize an instance of ServiceWrapperEventRouterBase.
        /// </summary>
        /// <param name="timeout">Timeout for waiting for the response of event raising.</param>
        protected ServiceWrapperEventRouterBase(int timeout)
        {
            _timeout = timeout;
        }

        private protected TReturnValueEntity SendMessageAndGetResponse(TParameterEntity message, out Exception exception)
        {
            SetMessageProperties(message);
            var responseMessage = SendEventMessageCallback(message, _timeout);
            exception = responseMessage.Exception;
            return (TReturnValueEntity) responseMessage;
        }
    }
}
