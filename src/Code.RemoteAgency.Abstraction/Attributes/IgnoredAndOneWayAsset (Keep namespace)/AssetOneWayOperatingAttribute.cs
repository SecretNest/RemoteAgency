using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the operating should be one way. No response is required.
    /// </summary>
    /// <remarks>
    /// <para>When one way mode enabled, any output parameters and return value will always be set to default value due to lack of response.</para>
    /// <para>When one way mode enabled, any exception raised from the user code on target site will not be transferred to the caller site.</para>
    /// <para>By specifying this on properties, only set operating will be affected. If one way get operating, which gets from a property but ignore the result and exception from the caller, is expected, mark the property with <see cref="PropertyGetOneWayOperatingAttribute"/>.</para>
    /// <para>When this attribute is absent, the one way mode is disabled.</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
    public class AssetOneWayOperatingAttribute : Attribute
    {
        /// <summary>
        /// Gets whether the operating is one way.
        /// </summary>
        public bool IsOneWay { get; }

        /// <summary>
        /// Initializes an instance of AssetOneWayOperatingAttribute.
        /// </summary>
        /// <param name="isOneWay">Whether the operating is one way. Default value is true.</param>
        public AssetOneWayOperatingAttribute(bool isOneWay = true)
        {
            IsOneWay = isOneWay;
        }
    }
}
