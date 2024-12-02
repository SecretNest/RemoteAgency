using System;
using System.Collections.Concurrent;

namespace SecretNest.RemoteAgency.AssemblyBuilding
{
    static class CreateInstanceExtensions
    {
        //This extension caches delegates of ctor of create types. The delegates are created by Emit (FastActivatorMethod), as a quick way to replace Activator.

        //For Proxy
        private static readonly ConcurrentDictionary<Type, Func<IProxyCommunicate>> ProxyConstructorCache = new ();

        internal static IProxyCommunicate CreateProxyInstance(this Type type)
        {
            var constructor = ProxyConstructorCache.GetOrAdd(type,
                i => i.BuildConstructorDelegate<Func<IProxyCommunicate>>());
            return constructor();
        }

        //For ServiceWrapper
        private static readonly ConcurrentDictionary<Type, Func<object, IServiceWrapperCommunicate>> ServiceWrapperConstructorCache = new ();

        internal static IServiceWrapperCommunicate CreateServiceWrapperInstance(this Type type, object serviceObject)
        {
            var constructor = ServiceWrapperConstructorCache.GetOrAdd(type,
                i => i.BuildConstructorDelegate<Func<object, IServiceWrapperCommunicate>>(typeof(object)));
            return constructor(serviceObject);
        }
    }
}
