using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the proxy built from this interface is target site sticky or not.
    /// </summary>
    /// <remarks><para>Due to routing mechanism, like load-balancing, the actual target site may not the same as requested.</para>
    /// <para>When sticky mode disabled, every message is sent with the target site id set as the parameter when adding the proxy object to the Remote Agency instance. This will make the load-balancing works well and distribute accesses among target sites.</para>
    /// <para>When sticky mode enabled, source site id of the first response message received is stored and used as the target site id for later messages. This will make all accesses stick to one of the target sites. It should be noted that when the chosen site is no longer responding, the proxy should be reset to restore it to the original state.</para>
    /// <para>When this attribute is absent, the target site sticky mode is off.</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Interface, Inherited = true, AllowMultiple = false)]
    public class ProxyStickyTargetSiteAttribute : Attribute
    {
        /// <summary>
        /// Gets whether the proxy built from this interface is target site sticky.
        /// </summary>
        public bool IsSticky { get; }

        /// <summary>
        /// Initializes an instance of ProxyTargetSiteStickyAttribute.
        /// </summary>
        /// <param name="isSticky">Whether the proxy built from this interface is target site sticky. Default value is true.</param>
        public ProxyStickyTargetSiteAttribute(bool isSticky = true)
        {
            IsSticky = isSticky;
        }
    }
}
