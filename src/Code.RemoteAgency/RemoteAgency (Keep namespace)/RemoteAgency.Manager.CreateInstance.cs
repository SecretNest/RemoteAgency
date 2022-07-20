﻿using System;
using SecretNest.RemoteAgency.AssemblyBuilding;
using SecretNest.RemoteAgency.Attributes;
using SecretNest.RemoteAgency.Inspecting;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyBase
    {
        private protected const int DefaultTimeout = 90000;

        /// <summary>
        /// Creates proxy of the interface and instance id specified.
        /// </summary>
        /// <param name="sourceInterface">Type of the service contract interface to be implemented by this proxy.</param>
        /// <param name="targetSiteId">Target site id of the proxy instance to be created.</param>
        /// <param name="targetInstanceId">Target instance id of the proxy instance to be created.</param>
        /// <param name="instanceId">Id of the proxy instance to be created. Cannot be set to <see cref="Guid"/>.Empty.</param>
        /// <param name="defaultTimeout">Default timeout in milliseconds for all operations; or -1 to indicate that the waiting does not time out. Value cannot be 0. Default value is 90000 (90 sec).</param>
        /// <param name="buildServiceWrapperWithProxy">When building is required, builds service wrapper and its required entities in the same assembly. Default value is <see langword="true"/>.</param>
        /// <returns>The created proxy.</returns>
        /// <remarks>The types required will be created when necessary.</remarks>
        /// <event cref="RemoteAgencyBase.BeforeTypeCreated">Raised before type building finished when a type is required for building.</event>
        /// <event cref="RemoteAgencyBase.BeforeAssemblyCreated">Raised before module and assembly building finished when a type is required for building.</event>
        /// <event cref="RemoteAgencyBase.AfterTypeAndAssemblyBuilt">Raised after the assembly built when a type is required for building.</event>
        public abstract CreatedProxy CreateProxy(Type sourceInterface, Guid targetSiteId, Guid targetInstanceId,
            Guid instanceId, int defaultTimeout = DefaultTimeout,
            bool buildServiceWrapperWithProxy = true);

        /// <summary>
        /// Creates proxy of the interface specified.
        /// </summary>
        /// <param name="sourceInterface">Type of the service contract interface to be implemented by this proxy.</param>
        /// <param name="targetSiteId">Target site id of the proxy instance to be created.</param>
        /// <param name="targetInstanceId">Target instance id of the proxy instance to be created.</param>
        /// <param name="defaultTimeout">Default timeout in milliseconds for all operations; or -1 to indicate that the waiting does not time out. Value cannot be 0. Default value is 90000 (90 sec).</param>
        /// <param name="buildServiceWrapperWithProxy">When building is required, builds service wrapper and its required entities in the same assembly. Default value is <see langword="true"/>.</param>
        /// <returns>The created proxy.</returns>
        /// <remarks>The types required will be created when necessary.</remarks>
        /// <event cref="RemoteAgencyBase.BeforeTypeCreated">Raised before type building finished when a type is required for building.</event>
        /// <event cref="RemoteAgencyBase.BeforeAssemblyCreated">Raised before module and assembly building finished when a type is required for building.</event>
        /// <event cref="RemoteAgencyBase.AfterTypeAndAssemblyBuilt">Raised after the assembly built when a type is required for building.</event>
        public abstract CreatedProxy CreateProxy(Type sourceInterface, Guid targetSiteId, Guid targetInstanceId,
            int defaultTimeout = DefaultTimeout, bool buildServiceWrapperWithProxy = true);

        /// <summary>
        /// Creates service wrapper of the interface, the service object and instance id specified.
        /// </summary>
        /// <param name="sourceInterface">Type of service contract interface to be implemented by this service wrapper and have been implemented by the <paramref name="serviceObject"/>.</param>
        /// <param name="serviceObject">The service object to be wrapped.</param>
        /// <param name="instanceId">Id of the service wrapper instance to be created. Cannot be set to <see cref="Guid"/>.Empty.</param>
        /// <param name="defaultTimeout">Default timeout in milliseconds for all operations; or -1 to indicate that the waiting does not time out. Value cannot be 0. Default value is 90000 (90 sec).</param>
        /// <param name="buildProxyWithServiceWrapper">When building is required, builds proxy and its required entities in the same assembly. Default value is <see langword="true"/>.</param>
        /// <remarks>The types required will be created when necessary.</remarks>
        /// <event cref="RemoteAgencyBase.BeforeTypeCreated">Raised before type building finished when a type is required for building.</event>
        /// <event cref="RemoteAgencyBase.BeforeAssemblyCreated">Raised before module and assembly building finished when a type is required for building.</event>
        /// <event cref="RemoteAgencyBase.AfterTypeAndAssemblyBuilt">Raised after the assembly built when a type is required for building.</event>
        public abstract void CreateServiceWrapper(Type sourceInterface, object serviceObject, Guid instanceId,
            int defaultTimeout = DefaultTimeout, bool buildProxyWithServiceWrapper = true);

        /// <summary>
        /// Creates service wrapper of the interface and the service object specified.
        /// </summary>
        /// <param name="sourceInterface">Type of service contract interface to be implemented by this service wrapper and have been implemented by the <paramref name="serviceObject"/>.</param>
        /// <param name="serviceObject">The service object to be wrapped.</param>
        /// <param name="defaultTimeout">Default timeout in milliseconds for all operations; or -1 to indicate that the waiting does not time out. Value cannot be 0. Default value is 90000 (90 sec).</param>
        /// <param name="buildProxyWithServiceWrapper">When building is required, builds proxy and its required entities in the same assembly. Default value is <see langword="true"/>.</param>
        /// <returns>The id of the created service wrapper instance.</returns>
        /// <remarks>The types required will be created when necessary.</remarks>
        /// <event cref="RemoteAgencyBase.BeforeTypeCreated">Raised before type building finished when a type is required for building.</event>
        /// <event cref="RemoteAgencyBase.BeforeAssemblyCreated">Raised before module and assembly building finished when a type is required for building.</event>
        /// <event cref="RemoteAgencyBase.AfterTypeAndAssemblyBuilt">Raised after the assembly built when a type is required for building.</event>
        public abstract Guid CreateServiceWrapper(Type sourceInterface, object serviceObject,
            int defaultTimeout = DefaultTimeout, bool buildProxyWithServiceWrapper = true);
    }

    partial class RemoteAgency<TSerialized, TEntityBase>
    {
        /// <summary>
        /// Creates proxy of the interface and instance id specified.
        /// </summary>
        /// <typeparam name="TInterface">Service contract interface to be implemented by this proxy.</typeparam>
        /// <param name="targetSiteId">Target site id of the proxy instance to be created.</param>
        /// <param name="targetInstanceId">Target instance id of the proxy instance to be created.</param>
        /// <param name="instanceId">Id of the proxy instance to be created. Cannot be set to <see cref="Guid"/>.Empty.</param>
        /// <param name="defaultTimeout">Default timeout in milliseconds for all operations; or -1 to indicate that the waiting does not time out. Value cannot be 0. Default value is 90000 (90 sec).</param>
        /// <param name="buildServiceWrapperWithProxy">When building is required, builds service wrapper and its required entities in the same assembly. Default value is <see langword="true"/>.</param>
        /// <returns>The created proxy.</returns>
        /// <remarks>The types required will be created when necessary.</remarks>
        /// <event cref="RemoteAgencyBase.BeforeTypeCreated">Raised before type building finished when a type is required for building.</event>
        /// <event cref="RemoteAgencyBase.BeforeAssemblyCreated">Raised before module and assembly building finished when a type is required for building.</event>
        /// <event cref="RemoteAgencyBase.AfterTypeAndAssemblyBuilt">Raised after the assembly built when a type is required for building.</event>
        public CreatedProxy<TInterface> CreateProxy<TInterface>(Guid targetSiteId, Guid targetInstanceId,
            Guid instanceId,
            int defaultTimeout = DefaultTimeout,
            bool buildServiceWrapperWithProxy = true)
        {
            if (instanceId == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(instanceId)} cannot be set to empty.", nameof(instanceId));
            }

            var proxy = (TInterface)CreateProxyWithInstanceInternal(typeof(TInterface), targetSiteId, targetInstanceId, instanceId, defaultTimeout,
                buildServiceWrapperWithProxy);
            return new CreatedProxy<TInterface>(instanceId, proxy);
        }

        /// <summary>
        /// Creates proxy of the interface specified.
        /// </summary>
        /// <typeparam name="TInterface">Service contract interface to be implemented by this proxy.</typeparam>
        /// <param name="targetSiteId">Target site id of the proxy instance to be created.</param>
        /// <param name="targetInstanceId">Target instance id of the proxy instance to be created.</param>
        /// <param name="defaultTimeout">Default timeout in milliseconds for all operations; or -1 to indicate that the waiting does not time out. Value cannot be 0. Default value is 90000 (90 sec).</param>
        /// <param name="buildServiceWrapperWithProxy">When building is required, builds service wrapper and its required entities in the same assembly. Default value is <see langword="true"/>.</param>
        /// <returns>The created proxy.</returns>
        /// <remarks>The types required will be created when necessary.</remarks>
        /// <event cref="RemoteAgencyBase.BeforeTypeCreated">Raised before type building finished when a type is required for building.</event>
        /// <event cref="RemoteAgencyBase.BeforeAssemblyCreated">Raised before module and assembly building finished when a type is required for building.</event>
        /// <event cref="RemoteAgencyBase.AfterTypeAndAssemblyBuilt">Raised after the assembly built when a type is required for building.</event>
        public CreatedProxy<TInterface> CreateProxy<TInterface>(Guid targetSiteId, Guid targetInstanceId,
            int defaultTimeout = DefaultTimeout,
            bool buildServiceWrapperWithProxy = true)
        {
            var instanceId = Guid.NewGuid();

            var proxy = (TInterface)CreateProxyWithInstanceInternal(typeof(TInterface), targetSiteId, targetInstanceId, instanceId,
                defaultTimeout, buildServiceWrapperWithProxy);
            return new CreatedProxy<TInterface>(instanceId, proxy);
        }

        /// <inheritdoc />
        public override CreatedProxy CreateProxy(Type sourceInterface, Guid targetSiteId, Guid targetInstanceId, Guid instanceId, int defaultTimeout = DefaultTimeout,
            bool buildServiceWrapperWithProxy = true)
        {
            if (instanceId == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(instanceId)} cannot be set to empty.", nameof(instanceId));
            }

            var proxy = CreateProxyWithInstanceInternal(sourceInterface, targetSiteId, targetInstanceId, instanceId, defaultTimeout,
                buildServiceWrapperWithProxy);
            return new CreatedProxy(instanceId, proxy, sourceInterface);
        }

        /// <inheritdoc />
        public override CreatedProxy CreateProxy(Type sourceInterface, Guid targetSiteId, Guid targetInstanceId, int defaultTimeout = DefaultTimeout,
            bool buildServiceWrapperWithProxy = true)
        {
            var instanceId = Guid.NewGuid();
            
            var proxy = CreateProxyWithInstanceInternal(sourceInterface, targetSiteId, targetInstanceId, instanceId, defaultTimeout,
                buildServiceWrapperWithProxy);
            return new CreatedProxy(instanceId, proxy, sourceInterface);
        }

        IProxyCommunicate CreateProxyWithInstanceInternal(Type sourceInterface, Guid targetSiteId, Guid targetInstanceId,
            Guid instanceId, int defaultTimeout = DefaultTimeout,
            bool buildServiceWrapperWithProxy = true)
        {
            if (defaultTimeout == 0 || defaultTimeout < -1)
                throw new ArgumentOutOfRangeException(nameof(defaultTimeout));

            if (_managingObjects.ContainsKey(instanceId))
            {
                throw new ArgumentException("Instance id specified already exists in this Remote Agency instance.", nameof(instanceId));
            }

            var serviceInterface = sourceInterface.IsConstructedGenericType ? sourceInterface.GetGenericTypeDefinition() : sourceInterface;
            var basicInfo = serviceInterface.GetBasicInfo(/*true*/);
            
            BuildAssembly(basicInfo, true, buildServiceWrapperWithProxy, out Type builtProxy, out _);

            Type proxyClass;

            if (sourceInterface.IsConstructedGenericType)
            {
                proxyClass = builtProxy.MakeGenericType(basicInfo.SourceInterfaceGenericArguments);
            }
            else
            {
                proxyClass = builtProxy;
            }

            var proxyInstance = proxyClass.CreateProxyInstance();

            RemoteAgencyManagingObjectProxy<TEntityBase> managingObject;

            if (basicInfo.ThreadLockMode == ThreadLockMode.TaskSchedulerSpecified)
            {
                managingObject = new RemoteAgencyManagingObjectProxy<TEntityBase>(proxyInstance,
                    instanceId, targetSiteId, targetInstanceId, basicInfo.TaskSchedulerName, TryGetTaskScheduler,
                    ProcessMessageReceivedFromInside, 
                    (id, assetName, exception) =>
                        RedirectException(sourceInterface, id, assetName, exception),
                    EntityTypeBuilder.CreateEmptyMessage, defaultTimeout, GetWaitingTimeForDisposing);
            }
            else
            {
                managingObject = new RemoteAgencyManagingObjectProxy<TEntityBase>(proxyInstance,
                    instanceId, targetSiteId, targetInstanceId, basicInfo.ThreadLockMode,
                    ProcessMessageReceivedFromInside, 
                    (id, assetName, exception) =>
                        RedirectException(sourceInterface, id, assetName, exception),
                    EntityTypeBuilder.CreateEmptyMessage, defaultTimeout, GetWaitingTimeForDisposing);
            }

            if (_managingObjects.TryAdd(instanceId, managingObject))
            {
                return proxyInstance;
            }
            else
            {
                throw new ArgumentException("Instance id specified already exists in this Remote Agency instance.", nameof(instanceId));
            }
        }

        /// <summary>
        /// Creates service wrapper of the interface, the service object and instance id specified.
        /// </summary>
        /// <typeparam name="TInterface">Service contract interface of the service to be implemented by this service wrapper and have been implemented by the <paramref name="serviceObject"/>.</typeparam>
        /// <param name="serviceObject">The service object to be wrapped.</param>
        /// <param name="instanceId">Id of the service wrapper instance to be created. Cannot be set to <see cref="Guid"/>.Empty.</param>
        /// <param name="defaultTimeout">Default timeout in milliseconds for all operations; or -1 to indicate that the waiting does not time out. Value cannot be 0. Default value is 90000 (90 sec).</param>
        /// <param name="buildProxyWithServiceWrapper">When building is required, builds proxy and its required entities in the same assembly. Default value is <see langword="true"/>.</param>
        /// <remarks>The types required will be created when necessary.</remarks>
        /// <event cref="RemoteAgencyBase.BeforeTypeCreated">Raised before type building finished when a type is required for building.</event>
        /// <event cref="RemoteAgencyBase.BeforeAssemblyCreated">Raised before module and assembly building finished when a type is required for building.</event>
        /// <event cref="RemoteAgencyBase.AfterTypeAndAssemblyBuilt">Raised after the assembly built when a type is required for building.</event>
        public void CreateServiceWrapper<TInterface>(TInterface serviceObject, Guid instanceId,
            int defaultTimeout = DefaultTimeout, bool buildProxyWithServiceWrapper = true)
            => CreateServiceWrapperInternal(typeof(TInterface), serviceObject, instanceId, defaultTimeout,
                buildProxyWithServiceWrapper);

        /// <summary>
        /// Creates service wrapper of the interface and the service object specified.
        /// </summary>
        /// <typeparam name="TInterface">Service contract interface of the service to be implemented by this service wrapper and have been implemented by the <paramref name="serviceObject"/>.</typeparam>
        /// <param name="serviceObject">The service object to be wrapped.</param>
        /// <param name="defaultTimeout">Default timeout in milliseconds for all operations; or -1 to indicate that the waiting does not time out. Value cannot be 0. Default value is 90000 (90 sec).</param>
        /// <param name="buildProxyWithServiceWrapper">When building is required, builds proxy and its required entities in the same assembly. Default value is <see langword="true"/>.</param>
        /// <returns>The id of the created service wrapper instance.</returns>
        /// <remarks>The types required will be created when necessary.</remarks>
        /// <event cref="RemoteAgencyBase.BeforeTypeCreated">Raised before type building finished when a type is required for building.</event>
        /// <event cref="RemoteAgencyBase.BeforeAssemblyCreated">Raised before module and assembly building finished when a type is required for building.</event>
        /// <event cref="RemoteAgencyBase.AfterTypeAndAssemblyBuilt">Raised after the assembly built when a type is required for building.</event>
        public Guid CreateServiceWrapper<TInterface>(TInterface serviceObject, 
            int defaultTimeout = DefaultTimeout, bool buildProxyWithServiceWrapper = true)
            => CreateServiceWrapperInternal(typeof(TInterface), serviceObject, defaultTimeout,
                buildProxyWithServiceWrapper);

        /// <inheritdoc />
        public override void CreateServiceWrapper(Type sourceInterface, object serviceObject, Guid instanceId, int defaultTimeout = DefaultTimeout, bool buildProxyWithServiceWrapper = true)
        {
            if (sourceInterface.IsInstanceOfType(serviceObject))
            {
                CreateServiceWrapperInternal(sourceInterface, serviceObject, instanceId, defaultTimeout, buildProxyWithServiceWrapper);
            }
            else
            {
                throw new ArgumentException($"Service object should implement the interface specified ({sourceInterface.Name}.", nameof(serviceObject));                
            }
        }

        /// <inheritdoc />
        public override Guid CreateServiceWrapper(Type sourceInterface, object serviceObject, int defaultTimeout = DefaultTimeout, bool buildProxyWithServiceWrapper = true)
        {
            if (sourceInterface.IsInstanceOfType(serviceObject))
            {
                return CreateServiceWrapperInternal(sourceInterface, serviceObject, defaultTimeout, buildProxyWithServiceWrapper);
            }
            else
            {
                throw new ArgumentException($"Service object should implement the interface specified ({sourceInterface.Name}.", nameof(serviceObject));                
            }
        }

        void CreateServiceWrapperInternal(Type sourceInterface, object serviceObject, Guid instanceId, int defaultTimeout = DefaultTimeout, bool buildProxyWithServiceWrapper = true)
        {
            if (instanceId == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(instanceId)} cannot be set to empty.", nameof(instanceId));
            }

            CreateServiceWrapperWithInstanceId(sourceInterface, serviceObject, instanceId, defaultTimeout,
                buildProxyWithServiceWrapper);
        }

        Guid CreateServiceWrapperInternal(Type sourceInterface, object serviceObject, int defaultTimeout = DefaultTimeout, bool buildProxyWithServiceWrapper = true)
        {
            var instanceId = Guid.NewGuid();

            CreateServiceWrapperWithInstanceId(sourceInterface, serviceObject, instanceId, defaultTimeout,
                buildProxyWithServiceWrapper);

            return instanceId;
        }
        
        void CreateServiceWrapperWithInstanceId(Type sourceInterface, object serviceObject, Guid instanceId, int defaultTimeout = DefaultTimeout, bool buildProxyWithServiceWrapper = true)
        {
            if (defaultTimeout == 0 || defaultTimeout < -1)
                throw new ArgumentOutOfRangeException(nameof(defaultTimeout));

            if (_managingObjects.ContainsKey(instanceId))
            {
                throw new ArgumentException("Instance id specified already exists in this Remote Agency instance.", nameof(instanceId));
            }

            var serviceInterface = sourceInterface.IsConstructedGenericType ? sourceInterface.GetGenericTypeDefinition() : sourceInterface;
            var basicInfo = serviceInterface.GetBasicInfo(/*buildProxyWithServiceWrapper*/);

            BuildAssembly(basicInfo, buildProxyWithServiceWrapper, true, out _, out Type builtServiceWrapper);

            Type serviceWrapperClass;

            if (sourceInterface.IsConstructedGenericType)
            {
                serviceWrapperClass = builtServiceWrapper.MakeGenericType(basicInfo.SourceInterfaceGenericArguments);
            }
            else
            {
                serviceWrapperClass = builtServiceWrapper;
            }

            var serviceWrapperInstance = serviceWrapperClass.CreateServiceWrapperInstance(serviceObject);

            RemoteAgencyManagingObjectServiceWrapper<TEntityBase> managingObject;

            if (basicInfo.ThreadLockMode == ThreadLockMode.TaskSchedulerSpecified)
            {
                managingObject = new RemoteAgencyManagingObjectServiceWrapper<TEntityBase>(
                    serviceWrapperInstance, instanceId, basicInfo.TaskSchedulerName, TryGetTaskScheduler,
                    ProcessMessageReceivedFromInside,
                    (id, assetName, exception) =>
                        RedirectException(sourceInterface, id, assetName, exception),
                    EntityTypeBuilder.CreateEmptyMessage, defaultTimeout, GetWaitingTimeForDisposing);
            }
            else
            {
                managingObject = new RemoteAgencyManagingObjectServiceWrapper<TEntityBase>(
                    serviceWrapperInstance, instanceId, basicInfo.ThreadLockMode,
                    ProcessMessageReceivedFromInside, 
                    (id, assetName, exception) =>
                        RedirectException(sourceInterface, id, assetName, exception),
                    EntityTypeBuilder.CreateEmptyMessage, defaultTimeout, GetWaitingTimeForDisposing);
            }

            if (!_managingObjects.TryAdd(instanceId, managingObject))
            {
                throw new ArgumentException("Instance id specified already exists in this Remote Agency instance.", nameof(instanceId));
            }
        }

    }
}
