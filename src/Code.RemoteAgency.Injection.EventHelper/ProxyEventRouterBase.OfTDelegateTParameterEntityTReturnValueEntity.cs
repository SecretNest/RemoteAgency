using System;

namespace SecretNest.RemoteAgency.Injection.EventHelper
{
    /// <summary>
    /// Defines a helper class to be implanted into built assembly for handling an two-way event handler in proxy. This is an abstract class.
    /// </summary>
    /// <typeparam name="TDelegate">Delegate of event.</typeparam>
    /// <typeparam name="TParameterEntity">Parameter entity type.</typeparam>
    /// <typeparam name="TReturnValueEntity">Return value entity type.</typeparam>
    internal abstract class ProxyEventRouterBase<TDelegate, TParameterEntity, TReturnValueEntity>
        : ProxyEventRouterBase<TDelegate>
        where TParameterEntity : IRemoteAgencyMessage
        where TReturnValueEntity : IRemoteAgencyMessage
    {
        //private readonly int _timeout;

        /// <summary>
        /// Initialize an instance of ProxyEventRouterBase.
        /// </summary>
        /// <param name="addingTimeout">Timeout for waiting for the response of event adding.</param>
        /// <param name="removingTimeout">Timeout for waiting for the response of event removing.</param>
        ///// <param name="raisingTimeout">Timeout for waiting for the response of event raising.</param>
        protected ProxyEventRouterBase(int addingTimeout, int removingTimeout/*, int raisingTimeout*/) : base(addingTimeout,
            removingTimeout)
        {
            //_timeout = raisingTimeout;
        }

        /// <inheritdoc />
        public sealed override IRemoteAgencyMessage ProcessEventRaisingMessage(IRemoteAgencyMessage message,
            out Exception exception)
        {
            var response = (IRemoteAgencyMessage) Process((TParameterEntity) message, out exception);
            response.AssetName = AssetName;
            return response;
        }

        /// <inheritdoc />
        public override void ProcessOneWayEventRaisingMessage(IRemoteAgencyMessage message)
        {
            Process((TParameterEntity) message, out var exception);
            if (exception != null)
                throw exception;
        }

        private protected abstract TReturnValueEntity Process(TParameterEntity message, out Exception exception);
    }
}
