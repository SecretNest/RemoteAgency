using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Queries the cache for the assembly specified.
    /// </summary>
    /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
    /// <typeparam name="TEntityBase">Type of the parent class of all entities.</typeparam>
    /// <param name="type">Type related to this request.</param>
    /// <param name="disposeRequired">Whether the objects created from this class need to be disposed.</param>
    /// <returns>The cached assembly.</returns>
    /// <remarks>The value of <paramref name="type" /> is:
    /// <list type="bullet"><item><term>The type of interface.</term><description>When this request is related to a proxy.</description></item>
    /// <item><term>The type of class of service object.</term><description>When this request is related to a service wrapper.</description></item></list></remarks>
    /// <seealso cref="Attributes.ProxyCacheableAttribute"/>
    /// <seealso cref="Attributes.ServiceWrapperCacheableAttribute"/>
    public delegate Assembly LoadCachedAssemblyCallback<TSerialized, TEntityBase>(Type type, out bool disposeRequired)
        where TEntityBase : class, IRemoteAgencyMessage<TSerialized>, new();

    /// <summary>
    /// Saves the assembly to cache if necessary.
    /// </summary>
    /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
    /// <typeparam name="TEntityBase">Type of the parent class of all entities.</typeparam>
    /// <param name="type">Type related to this request.</param>
    /// <param name="disposeRequired">Whether the objects created from this class need to be disposed.</param>
    /// <param name="assembly">The assembly created which contains the class generated.</param>
    /// <remarks>The value of <paramref name="type" /> is:
    /// <list type="bullet"><item><term>The type of interface.</term><description>When this request is related to a proxy.</description></item>
    /// <item><term>The type of class of service object.</term><description>When this request is related to a service wrapper.</description></item></list></remarks>
    /// <seealso cref="Attributes.ProxyCacheableAttribute"/>
    /// <seealso cref="Attributes.ServiceWrapperCacheableAttribute"/>
    public delegate void SaveCachedAssemblyCallback<TSerialized, TEntityBase>(Type type, bool disposeRequired, Assembly assembly)
        where TEntityBase : class, IRemoteAgencyMessage<TSerialized>, new();

    /// <summary>
    /// Saves the assembly in binary to cache if necessary.
    /// </summary>
    /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
    /// <typeparam name="TEntityBase">Type of the parent class of all entities.</typeparam>
    /// <param name="type">Type related to this request.</param>
    /// <param name="disposeRequired">Whether the objects created from this class need to be disposed.</param>
    /// <param name="image">The image of assembly created which contains the class generated.</param>
    /// <remarks>The value of <paramref name="type" /> is:
    /// <list type="bullet"><item><term>The type of interface.</term><description>When this request is related to a proxy.</description></item>
    /// <item><term>The type of class of service object.</term><description>When this request is related to a service wrapper.</description></item></list></remarks>
    /// <seealso cref="Attributes.ProxyCacheableAttribute"/>
    /// <seealso cref="Attributes.ServiceWrapperCacheableAttribute"/>
    public delegate void SaveCachedAssemblyImageCallback<TSerialized, TEntityBase>(Type type, bool disposeRequired, byte[] image)
        where TEntityBase : class, IRemoteAgencyMessage<TSerialized>, new();

    /// <summary>
    /// Represents a object that can support the assembly caching function.
    /// </summary>
    /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
    /// <typeparam name="TEntityBase">Type of the parent class of all entities.</typeparam>
    /// <seealso cref="Attributes.ProxyCacheableAttribute"/>
    /// <seealso cref="Attributes.ServiceWrapperCacheableAttribute"/>
    /// <seealso cref="LoadCachedAssemblyCallback{TSerialized, TEntityBase}"/>
    /// <seealso cref="SaveCachedAssemblyCallback{TSerialized, TEntityBase}"/>
    /// <seealso cref="SaveCachedAssemblyImageCallback{TSerialized, TEntityBase}"/>
    public interface IAssemblyCacheOperating<TSerialized, TEntityBase>
        where TEntityBase : class, IRemoteAgencyMessage<TSerialized>, new()
    {
        /// <summary>
        /// Should be called when an assembly cache querying is undertaken.
        /// </summary>
        LoadCachedAssemblyCallback<TSerialized, TEntityBase> LoadCachedAssemblyCallback { get; set; }

        /// <summary>
        /// Should be called when a new created assembly is ready for saving to cache.
        /// </summary>
        SaveCachedAssemblyCallback<TSerialized, TEntityBase> SaveCachedAssemblyCallback { get; set; }

        /// <summary>
        /// Should be called when a new created assembly is ready for saving to cache.
        /// </summary>
        SaveCachedAssemblyImageCallback<TSerialized, TEntityBase> SaveCachedAssemblyImageCallback { get; set; }
    }
}
