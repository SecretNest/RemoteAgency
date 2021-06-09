using System;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Represents the instance of the created proxy.
    /// </summary>
    public class CreatedProxy
    {
        /// <summary>
        /// Initializes an instance of CreatedProxy.
        /// </summary>
        /// <param name="instanceId">Id of the proxy instance.</param>
        /// <param name="proxy">Proxy created.</param>
        public CreatedProxy(Guid instanceId, object proxy)
        {
            InstanceId = instanceId;
            Proxy = proxy;
        }

        /// <summary>
        /// Gets the id of the proxy instance.
        /// </summary>
        public Guid InstanceId { get; }

        /// <summary>
        /// Gets the proxy created.
        /// </summary>
        public object Proxy { get; }
    }

    /// <summary>
    /// Represents the instance of the created proxy, created based on the service contract specified by <typeparamref name="TInterface"/>.
    /// </summary>
    /// <typeparam name="TInterface">Service contract interface to be implemented by this proxy.</typeparam>
    public class CreatedProxy<TInterface> : CreatedProxy
    {
        /// <summary>
        /// Initializes an instance of CreatedProxy.
        /// </summary>
        /// <param name="instanceId">Id of the proxy instance.</param>
        /// <param name="proxy">Proxy created.</param>
        public CreatedProxy(Guid instanceId, TInterface proxy) : base(instanceId, proxy)
        {
            ProxyGeneric = proxy;
        }

        /// <summary>
        /// Gets the proxy created.
        /// </summary>
        public TInterface ProxyGeneric { get; }
    }
}
