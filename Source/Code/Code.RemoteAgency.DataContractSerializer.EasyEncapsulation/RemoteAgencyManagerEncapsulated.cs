using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Encapsulated Remote Agency manager using <see cref="DataContractToJsonPackingHelper"/>, <see cref="DataContractSerializerSerializingHelper"/> and <see cref="DataContractSerializerEntityCodeBuilder"/>.
    /// </summary>
    /// <seealso cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}"/>
    public class RemoteAgencyManagerEncapsulated : RemoteAgencyManager<string, string, object>
    {
        static PackingHelperBase<string, string> GeneratePackingHelper() => new DataContractToJsonPackingHelper();
        static SerializingHelperBase<string, object> GenerateSerializingHelper() => new DataContractSerializerSerializingHelper();

        ProxyCreator<string, object> proxyCreator;
        ServiceWrapperCreator<string, object> serviceWrapperCreator;

        /// <summary>
        /// Clear cached assemblies using in Roslyn.
        /// </summary>
        public void ClearBuilderAssemblyCache()
        {
            proxyCreator?.ClearBuilderAssemblyCache();
            serviceWrapperCreator?.ClearBuilderAssemblyCache();
        }
        
        /// <summary>
        /// Occurs when a missing assembly / module, required by creator, needs to be resolved.
        /// </summary>
        public event EventHandler<AssemblyRequestingEventArgs> MissingAssemblyRequesting;

        private void OnMissingAssemblyRequesting(object sender, AssemblyRequestingEventArgs e)
        {
            MissingAssemblyRequesting?.Invoke(this, e);
        }


        /// <summary>
        /// Initializes an instance of the RemoteAgencyManagerEncapsulated.
        /// </summary>
        /// <param name="createProxyCreator">Whether <see cref="ProxyCreator{TSerialized, TEntityBase}"/> object should be prepared.</param>
        /// <param name="createServiceWrapperCreator">Whether <see cref="ServiceWrapperCreator{TSerialized, TEntityBase}"/> object should be prepared.</param>
        /// <param name="siteId">Site id. Will be set to <see cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}.SiteId"/>. A randomized value will be used instead if this parameter absents or is set to null.</param>
        public RemoteAgencyManagerEncapsulated(bool createProxyCreator, bool createServiceWrapperCreator, Guid? siteId = null) : base(GeneratePackingHelper(), GenerateSerializingHelper(), siteId)
        {
            DataContractSerializerEntityCodeBuilder entityBuilder = new DataContractSerializerEntityCodeBuilder();
            if (createProxyCreator)
            {
                proxyCreator = new ProxyCreator<string, object>(entityBuilder, typeof(DataContractSerializerSerializingHelper));
                proxyCreator.MissingAssemblyRequesting += OnMissingAssemblyRequesting;
            }
            if (createServiceWrapperCreator)
            {
                serviceWrapperCreator = new ServiceWrapperCreator<string, object>(entityBuilder, typeof(DataContractSerializerSerializingHelper));
                serviceWrapperCreator.MissingAssemblyRequesting += OnMissingAssemblyRequesting;
            }
        }

        /// <summary>
        /// Will be run for querying the cache for the proxy assembly specified.
        /// </summary>
        /// <seealso cref="IAssemblyCacheOperatings{TSerialized, TEntityBase}.LoadCachedAssemblyCallback"/>
        /// <seealso cref="LoadCachedAssemblyCallback{TSerialized, TEntityBase}"/>
        public LoadCachedAssemblyCallback<string, object> LoadCachedProxyAssemblyCallback
        {
            get
            {
                return proxyCreator?.LoadCachedAssemblyCallback;
            }
            set
            {
                if (proxyCreator != null)
                    proxyCreator.LoadCachedAssemblyCallback = value;
            }
        }

        /// <summary>
        /// Will be run for saving the proxy assembly to cache if necessary.
        /// </summary>
        /// <seealso cref="IAssemblyCacheOperatings{TSerialized, TEntityBase}.SaveCachedAssemblyCallback"/>
        /// <seealso cref="SaveCachedAssemblyCallback{TSerialized, TEntityBase}"/>
        public SaveCachedAssemblyCallback<string, object> SaveCachedProxyAssemblyCallback
        {
            get
            {
                return proxyCreator?.SaveCachedAssemblyCallback;
            }
            set
            {
                if (proxyCreator != null)
                    proxyCreator.SaveCachedAssemblyCallback = value;
            }
        }

        /// <summary>
        /// Will be run for saving the proxy assembly in binary to cache if necessary.
        /// </summary>
        /// <seealso cref="IAssemblyCacheOperatings{TSerialized, TEntityBase}.SaveCachedAssemblyImageCallback"/>
        /// <seealso cref="SaveCachedAssemblyImageCallback{TSerialized, TEntityBase}"/>
        public SaveCachedAssemblyImageCallback<string, object> SaveCachedProxyAssemblyImageCallback
        {
            get
            {
                return proxyCreator?.SaveCachedAssemblyImageCallback;
            }
            set
            {
                if (proxyCreator != null)
                    proxyCreator.SaveCachedAssemblyImageCallback = value;
            }
        }

        /// <summary>
        /// Will be run for querying the cache for the service wrapper assembly specified.
        /// </summary>
        /// <seealso cref="IAssemblyCacheOperatings{TSerialized, TEntityBase}.LoadCachedAssemblyCallback"/>
        /// <seealso cref="LoadCachedAssemblyCallback{TSerialized, TEntityBase}"/>
        public LoadCachedAssemblyCallback<string, object> LoadCachedServiceWrapperAssemblyCallback
        {
            get
            {
                return serviceWrapperCreator?.LoadCachedAssemblyCallback;
            }
            set
            {
                if (serviceWrapperCreator != null)
                    serviceWrapperCreator.LoadCachedAssemblyCallback = value;
            }
        }

        /// <summary>
        /// Will be run for saving the service wrapper assembly to cache if necessary.
        /// </summary>
        /// <seealso cref="IAssemblyCacheOperatings{TSerialized, TEntityBase}.SaveCachedAssemblyCallback"/>
        /// <seealso cref="SaveCachedAssemblyCallback{TSerialized, TEntityBase}"/>
        public SaveCachedAssemblyCallback<string, object> SaveCachedServiceWrapperAssemblyCallback
        {
            get
            {
                return serviceWrapperCreator?.SaveCachedAssemblyCallback;
            }
            set
            {
                if (serviceWrapperCreator != null)
                    serviceWrapperCreator.SaveCachedAssemblyCallback = value;
            }
        }

        /// <summary>
        /// Will be run for saving the service wrapper assembly in binary to cache if necessary.
        /// </summary>
        /// <seealso cref="IAssemblyCacheOperatings{TSerialized, TEntityBase}.SaveCachedAssemblyImageCallback"/>
        /// <seealso cref="SaveCachedAssemblyImageCallback{TSerialized, TEntityBase}"/>
        public SaveCachedAssemblyImageCallback<string, object> SaveCachedServiceWrapperAssemblyImageCallback
        {
            get
            {
                return serviceWrapperCreator?.SaveCachedAssemblyImageCallback;
            }
            set
            {
                if (serviceWrapperCreator != null)
                    serviceWrapperCreator.SaveCachedAssemblyImageCallback = value;
            }
        }

        /// <summary>
        /// Creates a proxy and adds it to this manager with the instance id specified.
        /// </summary>
        /// <typeparam name="TInterfaceContract">Type of service contract interface.</typeparam>
        /// <param name="serviceWrapperInstanceId">Instance id of the target service wrapper.</param>
        /// <param name="proxyInstanceId">Instance id of this proxy object.</param>
        /// <returns>Proxy object.</returns>
        /// <seealso cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}.AddProxy{TInterfaceContract}(ProxyCreator{TSerialized, TEntityBase}, Guid, Guid)"/>
        public TInterfaceContract AddProxy<TInterfaceContract>(Guid serviceWrapperInstanceId, Guid proxyInstanceId) where TInterfaceContract : class
        {
            if (proxyCreator == null)
                throw new InvalidOperationException();
            return AddProxy<TInterfaceContract>(proxyCreator, serviceWrapperInstanceId, proxyInstanceId);
        }

        /// <summary>
        /// Creates a proxy and adds it to this manager with a randomized instance id.
        /// </summary>
        /// <typeparam name="TInterfaceContract">Type of service contract interface.</typeparam>
        /// <param name="serviceWrapperInstanceId">Instance id of the target service wrapper.</param>
        /// <param name="proxyInstanceId">Instance id of this proxy object.</param>
        /// <returns>Proxy object.</returns>
        /// <seealso cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}.AddProxy{TInterfaceContract}(ProxyCreator{TSerialized, TEntityBase}, Guid, out Guid)"/>
        public TInterfaceContract AddProxy<TInterfaceContract>(Guid serviceWrapperInstanceId, out Guid proxyInstanceId) where TInterfaceContract : class
        {
            if (proxyCreator == null)
                throw new InvalidOperationException();
            return AddProxy<TInterfaceContract>(proxyCreator, serviceWrapperInstanceId, out proxyInstanceId);
        }

        /// <summary>
        /// Creates a service wrapper object and adds it to this manager with the instance id specified.
        /// </summary>
        /// <typeparam name="TInterfaceContract">Type of service contract interface.</typeparam>
        /// <param name="serviceObject">Existed service object which will be linked to the new created service wrapper object.</param>
        /// <param name="serviceWrapperInstanceId">Instance id of this service wrapper object.</param>
        /// <returns>Service wrapper object.</returns>
        /// <seealso cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}.AddServiceWrapper{TInterfaceContract}(ServiceWrapperCreator{TSerialized, TEntityBase}, TInterfaceContract, Guid)"/>
        public ICommunicate<string> AddServiceWrapper<TInterfaceContract>(TInterfaceContract serviceObject, Guid serviceWrapperInstanceId) where TInterfaceContract : class
        {
            if (serviceWrapperCreator == null)
                throw new InvalidOperationException();
            return AddServiceWrapper(serviceWrapperCreator, serviceObject, serviceWrapperInstanceId);
        }

        /// <summary>
        /// Creates a service wrapper object and adds it to this manager with a randomized instance id.
        /// </summary>
        /// <typeparam name="TInterfaceContract">Type of service contract interface.</typeparam>
        /// <param name="serviceObject">Existed service object which will be linked to the new created service wrapper object.</param>
        /// <param name="serviceWrapperInstanceId">Instance id of this service wrapper object.</param>
        /// <returns>Service wrapper object.</returns>
        /// <seealso cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}.AddServiceWrapper{TInterfaceContract}(ServiceWrapperCreator{TSerialized, TEntityBase}, TInterfaceContract, out Guid)"/>
        public ICommunicate<string> AddServiceWrapper<TInterfaceContract>(TInterfaceContract serviceObject, out Guid serviceWrapperInstanceId) where TInterfaceContract : class
        {
            if (serviceWrapperCreator == null)
                throw new InvalidOperationException();
            return AddServiceWrapper(serviceWrapperCreator, serviceObject, out serviceWrapperInstanceId);
        }

        /// <summary>
        /// Creates a service wrapper object and adds it to this manager with the instance id specified.
        /// </summary>
        /// <typeparam name="TServiceObject">Type of service object.</typeparam>
        /// <param name="serviceObject">Existed service object which will be linked to the new created service wrapper object.</param>
        /// <param name="interfaceContracts">List of all types of service contract interfaces.</param>
        /// <param name="serviceWrapperInstanceId">Instance id of this service wrapper object.</param>
        /// <returns>Service wrapper object.</returns>
        /// <seealso cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}.AddServiceWrapper{TServiceObject}(ServiceWrapperCreator{TSerialized, TEntityBase}, TServiceObject, IEnumerable{Type}, Guid)"/>
        public ICommunicate<string> AddServiceWrapper<TServiceObject>(TServiceObject serviceObject, List<Type> interfaceContracts, Guid serviceWrapperInstanceId) where TServiceObject : class
        {
            if (serviceWrapperCreator == null)
                throw new InvalidOperationException();
            return AddServiceWrapper(serviceWrapperCreator, serviceObject, interfaceContracts, serviceWrapperInstanceId);
        }

        /// <summary>
        /// Creates a service wrapper object and adds it to this manager with a randomized instance id.
        /// </summary>
        /// <typeparam name="TServiceObject">Type of service object.</typeparam>
        /// <param name="serviceObject">Existed service object which will be linked to the new created service wrapper object.</param>
        /// <param name="interfaceContracts">List of all types of service contract interfaces.</param>
        /// <param name="serviceWrapperInstanceId">Instance id of this service wrapper object.</param>
        /// <returns>Service wrapper object.</returns>
        /// <seealso cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}.AddServiceWrapper{TServiceObject}(ServiceWrapperCreator{TSerialized, TEntityBase}, TServiceObject, IEnumerable{Type}, out Guid)"/>
        public ICommunicate<string> AddServiceWrapper<TServiceObject>(TServiceObject serviceObject, List<Type> interfaceContracts, out Guid serviceWrapperInstanceId) where TServiceObject : class
        {
            if (serviceWrapperCreator == null)
                throw new InvalidOperationException();
            return AddServiceWrapper(serviceWrapperCreator, serviceObject, interfaceContracts, out serviceWrapperInstanceId);
        }
    }
}
