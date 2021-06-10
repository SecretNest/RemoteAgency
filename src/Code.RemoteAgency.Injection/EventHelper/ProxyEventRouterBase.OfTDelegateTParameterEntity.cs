namespace SecretNest.RemoteAgency.Injection.EventHelper
{
    /// <summary>
    /// Defines a helper class to be implanted into built assembly for handling an one-way event handler in proxy. This is an abstract class.
    /// </summary>
    /// <typeparam name="TDelegate">Delegate of event.</typeparam>
    /// <typeparam name="TParameterEntity">Parameter entity type.</typeparam>
    internal abstract class ProxyEventRouterBase<TDelegate, TParameterEntity>
        : ProxyEventRouterBase<TDelegate>
        where TParameterEntity : IRemoteAgencyMessage
    {
        /// <summary>
        /// Initializes an instance of ProxyEventRouterBase.
        /// </summary>
        /// <param name="addingTimeout">Timeout for waiting for the response of event adding.</param>
        /// <param name="removingTimeout">Timeout for waiting for the response of event removing.</param>
        protected ProxyEventRouterBase(int addingTimeout, int removingTimeout) : base(addingTimeout, removingTimeout)
        {
        }

        /// <inheritdoc />
        public sealed override void ProcessOneWayEventRaisingMessage(IRemoteAgencyMessage message)
        {
            Process((TParameterEntity) message);
        }

        private protected abstract void Process(TParameterEntity message);
    }
}
