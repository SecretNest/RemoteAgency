using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Declares this class is designed for cache handling.
    /// </summary>
    /// <remarks>Cache will not be used while this attribute absent. Due to this cache function is based on class instead of the interfaces directly, the calling time for generating cache, the 1st time usually, should contains all supported interfaces.</remarks>
    /// <seealso cref="LoadCachedAssemblyCallback{TSerialized, TEntityBase}"/>
    /// <seealso cref="SaveCachedAssemblyCallback{TSerialized, TEntityBase}"/>
    /// <seealso cref="IAssemblyCacheOperatings{TSerialized, TEntityBase}"/>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class ServiceWrapperCacheableAttribute : Attribute
    {
    }
}
