using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the property get operating should be one-way. No response is required.
    /// </summary>
    /// <remarks>
    /// <para>When <see cref="IsOneWay"/> is set as <see langword="true"/>, the getting operating return the default value to the caller due to lack of response. Any exception raised from the user code on target site will not be transferred to the caller site.</para>
    /// <para>By specifying this on properties, only getting operating is affected. If one-way setting operating is expected, mark the property with <see cref="AssetOneWayOperatingAttribute"/>.</para>
    /// <para>When this attribute is absent, the one-way mode on property getting operating of this asset is disabled.</para>
    /// </remarks>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#AssetLevel" />
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class PropertyGetOneWayOperatingAttribute : Attribute
    {
        /// <summary>
        /// Gets whether the property get operating is one-way.
        /// </summary>
        public bool IsOneWay { get; }

        /// <summary>
        /// Initializes an instance of PropertyGetOneWayOperatingAttribute.
        /// </summary>
        /// <param name="isOneWay">Whether the property get operating is one-way. Default value is true.</param>
        public PropertyGetOneWayOperatingAttribute(bool isOneWay = true)
        {
            IsOneWay = isOneWay;
        }
    }
}
