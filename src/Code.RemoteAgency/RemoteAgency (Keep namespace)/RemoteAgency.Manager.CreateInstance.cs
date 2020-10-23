using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.RemoteAgency.AssemblyBuilding;
using SecretNest.RemoteAgency.Attributes;
using SecretNest.RemoteAgency.Inspecting;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgency
    {
        /// <summary>
        /// Creates proxy of the interface specified.
        /// </summary>
        /// <param name="sourceInterface">Type of the service contract interface to be implemented by this proxy.</param>
        /// <param name="targetSiteId">Target site id of the created proxy instance.</param>
        /// <param name="targetInstanceId">Target instance id of the created proxy instance.</param>
        /// <param name="instanceId">Id of the created proxy instance. A new id is generated if value is set as <see cref="Guid"/>.Empty.</param>
        /// <param name="defaultTimeout">Default timeout in milliseconds for all operations; or -1 to indicate that the waiting does not time out. Value cannot be 0. Default value is 90000 (90 sec).</param>
        /// <param name="buildServiceWrapperWithProxy">When building is required, builds service wrapper and its required entities in the same assembly. Default value is <see langword="true"/>.</param>
        /// <returns>Proxy instance.</returns>
        /// <remarks>The types required will be created when necessary.</remarks>
        /// <event cref="RemoteAgency.BeforeTypeCreated">Raised before type building finished when a type is required for building.</event>
        /// <event cref="RemoteAgency.BeforeAssemblyCreated">Raised before module and assembly building finished when a type is required for building.</event>
        /// <event cref="RemoteAgency.AfterTypeAndAssemblyBuilt">Raised after the assembly built when a type is required for building.</event>
        public abstract object CreateProxy(Type sourceInterface, Guid targetSiteId, Guid targetInstanceId,
            ref Guid instanceId, int defaultTimeout = 90000,
            bool buildServiceWrapperWithProxy = true);


        /// <summary>
        /// Creates service wrapper of the interface and the service object specified.
        /// </summary>
        /// <param name="sourceInterface">Type of service contract interface to be implemented by this service wrapper and have been implemented by the <paramref name="serviceObject"/>.</param>
        /// <param name="serviceObject">The service object to be wrapped.</param>
        /// <param name="instanceId">Id of the created service wrapper instance. A new id is generated if value is set as <see cref="Guid"/>.Empty.</param>
        /// <param name="defaultTimeout">Default timeout in milliseconds for all operations; or -1 to indicate that the waiting does not time out. Value cannot be 0. Default value is 90000 (90 sec).</param>
        /// <param name="buildProxyWithServiceWrapper">When building is required, builds proxy and its required entities in the same assembly. Default value is <see langword="true"/>.</param>
        /// <returns>The id of the service wrapper instance created.</returns>
        /// <remarks>The types required will be created when necessary.</remarks>
        /// <event cref="RemoteAgency.BeforeTypeCreated">Raised before type building finished when a type is required for building.</event>
        /// <event cref="RemoteAgency.BeforeAssemblyCreated">Raised before module and assembly building finished when a type is required for building.</event>
        /// <event cref="RemoteAgency.AfterTypeAndAssemblyBuilt">Raised after the assembly built when a type is required for building.</event>
        public abstract Guid CreateServiceWrapper(Type sourceInterface, object serviceObject, ref Guid instanceId,
            int defaultTimeout = 90000, bool buildProxyWithServiceWrapper = true);
    }

    partial class RemoteAgency<TSerialized, TEntityBase>
    {
        /// <summary>
        /// Creates proxy of the interface specified.
        /// </summary>
        /// <typeparam name="TInterface">Service contract interface to be implemented by this proxy.</typeparam>
        /// <param name="targetSiteId">Target site id of the created proxy instance.</param>
        /// <param name="targetInstanceId">Target instance id of the created proxy instance.</param>
        /// <param name="instanceId">Id of the created proxy instance. A new id is generated if value is set as <see cref="Guid"/>.Empty.</param>
        /// <param name="defaultTimeout">Default timeout in milliseconds for all operations; or -1 to indicate that the waiting does not time out. Value cannot be 0. Default value is 90000 (90 sec).</param>
        /// <param name="buildServiceWrapperWithProxy">When building is required, builds service wrapper and its required entities in the same assembly. Default value is <see langword="true"/>.</param>
        /// <returns>Proxy instance.</returns>
        /// <remarks>The types required will be created when necessary.</remarks>
        /// <event cref="RemoteAgency.BeforeTypeCreated">Raised before type building finished when a type is required for building.</event>
        /// <event cref="RemoteAgency.BeforeAssemblyCreated">Raised before module and assembly building finished when a type is required for building.</event>
        /// <event cref="RemoteAgency.AfterTypeAndAssemblyBuilt">Raised after the assembly built when a type is required for building.</event>
        public TInterface CreateProxy<TInterface>(Guid targetSiteId, Guid targetInstanceId, ref Guid instanceId,
            int defaultTimeout = 90000,
            bool buildServiceWrapperWithProxy = true)
            => (TInterface) CreateProxy(typeof(TInterface), targetSiteId, targetInstanceId, ref instanceId,
                defaultTimeout, buildServiceWrapperWithProxy);

        /// <inheritdoc />
        public override object CreateProxy(Type sourceInterface, Guid targetSiteId, Guid targetInstanceId, ref Guid instanceId, int defaultTimeout = 90000,
            bool buildServiceWrapperWithProxy = true)
        {
            if (defaultTimeout == 0 || defaultTimeout < -1)
                throw new ArgumentOutOfRangeException(nameof(defaultTimeout));

            if (_managingObjects.ContainsKey(instanceId))
            {
                throw new ArgumentException("Instance id specified exists in this Remote Agency instance.", nameof(instanceId));
            }

            var serviceInterface = sourceInterface.IsConstructedGenericType ? sourceInterface.GetGenericTypeDefinition() : sourceInterface;
            var basicInfo = Inspector.GetBasicInfo(serviceInterface, true);
            
            BuildAssembly(basicInfo, true, buildServiceWrapperWithProxy, out Type builtProxy, out _);

            Type proxy;

            if (sourceInterface.IsConstructedGenericType)
            {
                proxy = builtProxy.MakeGenericType(basicInfo.SourceInterfaceGenericArguments);
            }
            else
            {
                proxy = builtProxy;
            }

            var item = FastActivator.CreateInstance(proxy);

            RemoteAgencyManagingObjectProxy<TEntityBase> managingObject;

            if (basicInfo.ThreadLockMode == ThreadLockMode.TaskSchedulerSpecified)
            {
                managingObject = new RemoteAgencyManagingObjectProxy<TEntityBase>((IProxyCommunicate) item,
                    ref instanceId, targetSiteId, targetInstanceId, basicInfo.TaskSchedulerName, TryGetTaskScheduler,
                    ProcessMessageReceivedFromInside, 
                    (id, assetName, exception) =>
                        RedirectException(sourceInterface, id, assetName, exception),
                    EntityTypeBuilder.CreateEmptyMessage, defaultTimeout, GetWaitingTimeForDisposing);
            }
            else
            {
                managingObject = new RemoteAgencyManagingObjectProxy<TEntityBase>((IProxyCommunicate) item,
                    ref instanceId, targetSiteId, targetInstanceId, basicInfo.ThreadLockMode,
                    ProcessMessageReceivedFromInside, 
                    (id, assetName, exception) =>
                        RedirectException(sourceInterface, id, assetName, exception),
                    EntityTypeBuilder.CreateEmptyMessage, defaultTimeout, GetWaitingTimeForDisposing);
            }

            if (_managingObjects.TryAdd(instanceId, managingObject))
            {
                return item;
            }
            else
            {
                throw new ArgumentException("Instance id specified exists in this Remote Agency instance.", nameof(instanceId));
            }
        }

        /// <summary>
        /// Creates service wrapper of the interface and the service object specified.
        /// </summary>
        /// <typeparam name="TInterface">Service contract interface of the service to be implemented by this service wrapper and have been implemented by the <paramref name="serviceObject"/>.</typeparam>
        /// <param name="serviceObject">The service object to be wrapped.</param>
        /// <param name="instanceId">Id of the created service wrapper instance. A new id is generated if value is set as <see cref="Guid"/>.Empty.</param>
        /// <param name="defaultTimeout">Default timeout in milliseconds for all operations; or -1 to indicate that the waiting does not time out. Value cannot be 0. Default value is 90000 (90 sec).</param>
        /// <param name="buildProxyWithServiceWrapper">When building is required, builds proxy and its required entities in the same assembly. Default value is <see langword="true"/>.</param>
        /// <returns>The id of the service wrapper instance created.</returns>
        /// <remarks>The types required will be created when necessary.</remarks>
        /// <event cref="RemoteAgency.BeforeTypeCreated">Raised before type building finished when a type is required for building.</event>
        /// <event cref="RemoteAgency.BeforeAssemblyCreated">Raised before module and assembly building finished when a type is required for building.</event>
        /// <event cref="RemoteAgency.AfterTypeAndAssemblyBuilt">Raised after the assembly built when a type is required for building.</event>
        public Guid CreateServiceWrapper<TInterface>(TInterface serviceObject, ref Guid instanceId,
            int defaultTimeout = 90000, bool buildProxyWithServiceWrapper = true)
            => (Guid) CreateServiceWrapperInternal(typeof(TInterface), serviceObject, ref instanceId, defaultTimeout,
                buildProxyWithServiceWrapper);

        /// <inheritdoc />
        public override Guid CreateServiceWrapper(Type sourceInterface, object serviceObject, ref Guid instanceId, int defaultTimeout = 90000, bool buildProxyWithServiceWrapper = true)
        {
            if (sourceInterface.IsInstanceOfType(serviceObject))
            {
                return CreateServiceWrapperInternal(sourceInterface, serviceObject, ref instanceId, defaultTimeout,
                    buildProxyWithServiceWrapper);
            }
            else
            {
                throw new ArgumentException($"Service object should implement the interface specified ({sourceInterface.Name}.", nameof(serviceObject));                
            }
        }

        Guid CreateServiceWrapperInternal(Type sourceInterface, object serviceObject, ref Guid instanceId, int defaultTimeout = 90000,
            bool buildProxyWithServiceWrapper = true)
        {
            if (defaultTimeout == 0 || defaultTimeout < -1)
                throw new ArgumentOutOfRangeException(nameof(defaultTimeout));

            if (_managingObjects.ContainsKey(instanceId))
            {
                throw new ArgumentException("Instance id specified exists in this Remote Agency instance.", nameof(instanceId));
            }

            var serviceInterface = sourceInterface.IsConstructedGenericType ? sourceInterface.GetGenericTypeDefinition() : sourceInterface;
            var basicInfo = Inspector.GetBasicInfo(serviceInterface, buildProxyWithServiceWrapper);

            BuildAssembly(basicInfo, buildProxyWithServiceWrapper, true, out _, out Type builtServiceWrapper);

            Type serviceWrapper;

            if (sourceInterface.IsConstructedGenericType)
            {
                serviceWrapper = builtServiceWrapper.MakeGenericType(basicInfo.SourceInterfaceGenericArguments);
            }
            else
            {
                serviceWrapper = builtServiceWrapper;
            }

            var item = FastActivator<object>.CreateInstance(serviceWrapper, serviceObject);

            RemoteAgencyManagingObjectServiceWrapper<TEntityBase> managingObject;

            if (basicInfo.ThreadLockMode == ThreadLockMode.TaskSchedulerSpecified)
            {
                managingObject = new RemoteAgencyManagingObjectServiceWrapper<TEntityBase>(
                    (IServiceWrapperCommunicate) item, ref instanceId, basicInfo.TaskSchedulerName, TryGetTaskScheduler,
                    ProcessMessageReceivedFromInside,
                    (id, assetName, exception) =>
                        RedirectException(sourceInterface, id, assetName, exception),
                    EntityTypeBuilder.CreateEmptyMessage, defaultTimeout, GetWaitingTimeForDisposing);
            }
            else
            {
                managingObject = new RemoteAgencyManagingObjectServiceWrapper<TEntityBase>(
                    (IServiceWrapperCommunicate) item, ref instanceId, basicInfo.ThreadLockMode,
                    ProcessMessageReceivedFromInside, 
                    (id, assetName, exception) =>
                        RedirectException(sourceInterface, id, assetName, exception),
                    EntityTypeBuilder.CreateEmptyMessage, defaultTimeout, GetWaitingTimeForDisposing);
            }

            if (_managingObjects.TryAdd(instanceId, managingObject))
            {
                return instanceId;
            }
            else
            {
                throw new ArgumentException("Instance id specified exists in this Remote Agency instance.", nameof(instanceId));
            }
        }

    }
}
