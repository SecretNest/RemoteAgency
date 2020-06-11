using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Declares this interface is designed for cache handling.
    /// </summary>
    /// <remarks>Cache will not be used while this attribute absent.</remarks>
    /// <seealso cref="LoadCachedAssemblyCallback{TSerialized, TEntityBase}"/>
    /// <seealso cref="SaveCachedAssemblyCallback{TSerialized, TEntityBase}"/>
    /// <seealso cref="IAssemblyCacheOperating{TSerialized, TEntityBase}"/>
    [AttributeUsage(AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
    public class ProxyCacheableAttribute : Attribute
    {
    }
}
