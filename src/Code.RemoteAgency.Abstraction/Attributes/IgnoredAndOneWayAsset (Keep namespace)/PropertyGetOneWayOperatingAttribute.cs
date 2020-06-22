using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the property get operating should be one way. No response is required.
    /// </summary>
    /// <remarks>
    /// <para>When <see cref="IsOneWay"/> is set to <see langword="true"/>, the getting operating will always return the default value to the caller due to lack of response, any exception raised from the user code on target site will not be transferred to the caller site.</para>
    /// <para>By specifying this on properties, only get operating will be affected. If one way set operating is expected, mark the property with <see cref="AssetOneWayOperatingAttribute"/>.</para>
    /// <para>When this attribute is absent, the one way mode on property get operating of this asset is disabled.</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class PropertyGetOneWayOperatingAttribute : Attribute
    {
        /// <summary>
        /// Gets whether the property get operating is one way.
        /// </summary>
        public bool IsOneWay { get; }

        /// <summary>
        /// Initializes an instance of PropertyGetOneWayOperatingAttribute.
        /// </summary>
        /// <param name="isOneWay">Whether the property get operating is one way. Default value is true.</param>
        public PropertyGetOneWayOperatingAttribute(bool isOneWay = true)
        {
            IsOneWay = isOneWay;
        }
    }
}
