using SecretNest.RemoteAgency.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SecretNest.RemoteAgency
{
    public partial class ServiceWrapperCreator<TSerialized, TEntityBase> : IAssemblyCacheOperatings<TSerialized, TEntityBase> where TEntityBase : class
    {
        /// <summary>
        /// Will be run for querying the cache for the assembly specified.
        /// </summary>
        /// <seealso cref="IAssemblyCacheOperatings{TSerialized, TEntityBase}.LoadCachedAssemblyCallback"/>
        /// <seealso cref="LoadCachedAssemblyCallback{TSerialized, TEntityBase}"/>
        public LoadCachedAssemblyCallback<TSerialized, TEntityBase> LoadCachedAssemblyCallback { get; set; }

        /// <summary>
        /// Will be run for saving the assembly to cache if necessary.
        /// </summary>
        /// <seealso cref="IAssemblyCacheOperatings{TSerialized, TEntityBase}.SaveCachedAssemblyCallback"/>
        /// <seealso cref="SaveCachedAssemblyCallback{TSerialized, TEntityBase}"/>
        public SaveCachedAssemblyCallback<TSerialized, TEntityBase> SaveCachedAssemblyCallback { get; set; }

        /// <summary>
        /// Will be run for saving the assembly to cache if necessary.
        /// </summary>
        public SaveCachedAssemblyImageCallback<TSerialized, TEntityBase> SaveCachedAssemblyImageCallback { get; set; }

        Assembly LoadAssembly(Type[] interfaceType, TypeInfo[] interfaceTypeInfo, Type serviceObjectType, out bool disposeRequired)
        {
            var cacheable = serviceObjectType.GetTypeInfo().GetCustomAttribute<ServiceWrapperCacheableAttribute>() != null;
            if (cacheable && LoadCachedAssemblyCallback != null)
            {
                var loaded = LoadCachedAssemblyCallback(serviceObjectType, out disposeRequired);
                if (loaded != null) return loaded;
            }
            var created = CreateServiceWrapperAssembly(interfaceType, interfaceTypeInfo, out disposeRequired, out var image);
            if (cacheable)
            {
                SaveCachedAssemblyCallback?.Invoke(serviceObjectType, disposeRequired, created);
                SaveCachedAssemblyImageCallback?.Invoke(serviceObjectType, disposeRequired, image);
            }
            return created;
        }
    }
}
