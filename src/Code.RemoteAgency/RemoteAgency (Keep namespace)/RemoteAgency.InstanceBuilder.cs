using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.SqlServer.Server;
using SecretNest.RemoteAgency.Helper;
using SecretNest.RemoteAgency.AssemblyBuilding;
using SecretNest.RemoteAgency.Inspecting;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgency
    {
        Guid GetSiteId() => SiteId;

        /// <summary>
        /// Constructs a proxy object of a type.
        /// </summary>
        /// <param name="sourceInterface">Type of the source interface.</param>
        /// <param name="isServiceWrapperIncludedWhileBuildingProxy">Whether the service wrapper should be built into assembly while proxy type building is required.</param>
        /// <returns>Instance of the type constructed.</returns>
        protected object ConstructProxyInstance(Type sourceInterface, bool isServiceWrapperIncludedWhileBuildingProxy)
            => ConstructProxyInstance(Inspector.GetBasicInfo(sourceInterface),
                isServiceWrapperIncludedWhileBuildingProxy);

        object ConstructProxyInstance(InterfaceTypeBasicInfo interfaceTypeBasicInfo,
            bool isServiceWrapperIncludedWhileBuildingProxy)
        {
            if (!TryGetType(interfaceTypeBasicInfo.AssemblyName, interfaceTypeBasicInfo.ProxyTypeName, out var proxyType))
            {
                BuildAssembly(interfaceTypeBasicInfo.SourceInterface, true, isServiceWrapperIncludedWhileBuildingProxy,
                    out proxyType, out _);
            }

            var obj = FastActivator.CreateInstance<IProxyCommunicate>(proxyType);
            obj.GetSiteIdCallback = GetSiteId;
            return obj;
        }

        /// <summary>
        /// Constructs a service wrapper object of a type.
        /// </summary>
        /// <param name="sourceInterface">Type of the source interface.</param>
        /// <param name="serviceObject">Target service object to be wrapped.</param>
        /// <param name="isProxyIncludedWhileBuildingServiceWrapper">Whether the proxy should be built into assembly while service wrapper type building is required.</param>
        /// <returns>Instance of the type constructed.</returns>
        protected object ConstructServiceWrapperInstance(Type sourceInterface, object serviceObject,
            bool isProxyIncludedWhileBuildingServiceWrapper)
            => ConstructServiceWrapperInstance(Inspector.GetBasicInfo(sourceInterface), serviceObject,
                isProxyIncludedWhileBuildingServiceWrapper);

        object ConstructServiceWrapperInstance(InterfaceTypeBasicInfo interfaceTypeBasicInfo, object serviceObject, 
            bool isProxyIncludedWhileBuildingServiceWrapper)
        {
            if (!TryGetType(interfaceTypeBasicInfo.AssemblyName, interfaceTypeBasicInfo.ServiceWrapperTypeName, out var serviceWrapperType))
            {
                BuildAssembly(interfaceTypeBasicInfo.SourceInterface, isProxyIncludedWhileBuildingServiceWrapper, true,
                    out _, out serviceWrapperType);
            }

            var obj = FastActivator<object>.CreateInstance<IServiceWrapperCommunicate>(serviceWrapperType,
                serviceObject);
            obj.GetSiteIdCallback = GetSiteId;
            return obj;
        }
    }
}
