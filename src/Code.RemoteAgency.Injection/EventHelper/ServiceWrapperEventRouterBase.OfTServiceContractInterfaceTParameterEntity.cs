namespace SecretNest.RemoteAgency.Injection.EventHelper
{
    /// <summary>
    /// Defines a helper class to be implanted into built assembly for handling an one-way event handler in service wrapper. This is an abstract class.
    /// </summary>
    /// <typeparam name="TServiceContractInterface">Service contract interface type.</typeparam>
    /// <typeparam name="TParameterEntity">Parameter entity type.</typeparam>
    internal abstract class ServiceWrapperEventRouterBase<TServiceContractInterface, TParameterEntity>
        : ServiceWrapperEventRouterBase<TServiceContractInterface>
        where TParameterEntity : IRemoteAgencyMessage
    {
        private protected void SendMessage(TParameterEntity message)
        {
            SetMessageProperties(message);
            SendOneWayEventMessageCallback(message);
        }
    }
}
