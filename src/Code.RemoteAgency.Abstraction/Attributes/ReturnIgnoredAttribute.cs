using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the return value of the asset is ignored from type building.
    /// </summary>
    /// <remarks>
    /// <para>When <see cref="IsIgnored"/> is set to <see langword="true"/>, the return value of the asset, if exists, will not be sent back the caller.</para>
    /// <para><see cref="ReturnIgnoredAttribute"/> affect the return value only. For ignoring from all responses, use <see cref="AssetOneWayOperatingAttribute"/> or <see cref="PropertyGetOneWayOperatingAttribute"/> instead.</para>
    /// <para>When <see cref="IsIgnored"/> is set to <see langword="true"/>, <see cref="CustomizedPropertyGetResponsePropertyNameAttribute"/> and <see cref="CustomizedReturnEntityPropertyNameAttribute"/> on or in the same asset, and the delegate related to the asset if the asset is an event, will be ignored.</para>
    /// <para>By specifying this on properties, only get operating will be affected.</para>
    /// <para>The one marked on the event has higher priority than the one marked on the delegate of the same event.</para>
    /// <para>Without <see cref="AssetOneWayOperatingAttribute"/>, <see cref="PropertyGetOneWayOperatingAttribute"/> or <see cref="ReturnIgnoredAttribute"/> specified, no return value is ignored.</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Event | AttributeTargets.Method | AttributeTargets.Delegate | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ReturnIgnoredAttribute : Attribute
    {
        /// <summary>
        /// Gets whether the return value is ignored.
        /// </summary>
        public bool IsIgnored { get; }

        /// <summary>
        /// Initializes an instance of ReturnIgnoredAttribute.
        /// </summary>
        /// <param name="isIgnored">Whether the return value is ignored. Default value is true.</param>
        public ReturnIgnoredAttribute(bool isIgnored = true)
        {
            IsIgnored = isIgnored;
        }
    }
}
