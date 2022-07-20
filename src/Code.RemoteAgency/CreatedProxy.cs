using System;
using System.Collections.Generic;
using System.Linq;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Contains the instance of the created proxy and id of it.
    /// </summary>
    public class CreatedProxy
    {
        /// <summary>
        /// Initializes an instance of CreatedProxy.
        /// </summary>
        /// <param name="instanceId">Id of the proxy instance.</param>
        /// <param name="proxy">Proxy created.</param>
        /// <param name="interfaceType">Type of the interface.</param>
        public CreatedProxy(Guid instanceId, object proxy, Type interfaceType)
        {
            InstanceId = instanceId;
            Proxy = proxy;
            InterfaceType = interfaceType;
        }

        /// <summary>
        /// Initializes an instance of CreatedProxy.
        /// </summary>
        /// <param name="instanceId">Id of the proxy instance.</param>
        /// <param name="proxy">Proxy created.</param>
        protected CreatedProxy(Guid instanceId, object proxy)
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

        /// <summary>
        /// Gets the type of the interface.
        /// </summary>
        public virtual Type InterfaceType { get; }

        private bool _proxyInitPropertyPrepared;
        private HashSet<string> _proxyInitPropertyNames;

        private void PrepareProxyInitProperty()
        {
            var proxy = (IProxyCommunicate) Proxy;
            var returned = proxy.GetInitOnlyPropertyNames();
            _proxyInitPropertyNames = returned != null ? new HashSet<string>(returned) : new HashSet<string>();
        }

        /// <summary>
        /// Sets value for the property marked with init only setter.
        /// </summary>
        /// <param name="name">Name of the property marked with init only setter.</param>
        /// <param name="value">The value to be set to the property.</param>
        /// <exception cref="InvalidOperationException">Thrown when the property is not found or not marked with init only setter.</exception>
        public void SetInitOnlyPropertyValue(string name, object value)
        {
            lock (Proxy)
            {
                if (!_proxyInitPropertyPrepared)
                {
                    PrepareProxyInitProperty();
                    
                    _proxyInitPropertyPrepared = true;
                }
            }

            if (_proxyInitPropertyNames.Contains(name))
            {
                ((IProxyCommunicate)Proxy).SetInitOnlyPropertyValue(name, value);
            }
            else
            {
                throw new InvalidOperationException($"Property with name {name} is absent or not marked with init only setter.");
            }
        }
    }

    /// <summary>
    /// Represents the instance of the created proxy and id of it, created based on the service contract specified by <typeparamref name="TInterface"/>.
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

        /// <inheritdoc />
        public override Type InterfaceType => typeof(TInterface);
    }
}
