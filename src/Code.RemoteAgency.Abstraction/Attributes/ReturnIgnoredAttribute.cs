using System;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the return value of the asset is ignored from type building.
    /// </summary>
    /// <remarks>
    /// <para>When <see cref="IsIgnored"/> is set to <see langword="true"/>, the return value of the asset, if exists, will not be sent back the caller.</para>
    /// <para><see cref="ReturnIgnoredAttribute"/> affect the return value only. For ignoring from all responses, use <see cref="AssetOneWayOperatingAttribute"/> or <see cref="PropertyGetOneWayOperatingAttribute"/> instead.</para>
    /// <para>When <see cref="IsIgnored"/> is set to <see langword="true"/>, <see cref="CustomizedPropertyGetValuePropertyNameAttribute"/> and <see cref="CustomizedReturnValueEntityPropertyNameAttribute"/> on or in the same asset, and the delegate related to the asset if the asset is an event, will be ignored.</para>
    /// <para>By specifying this on properties, only get operating will be affected.</para>
    /// <para>The one marked on the event has higher priority than the one marked on the delegate of the same event.</para>
    /// <para>The one marked on the return value has higher priority than the one marked on the same member (method, event or delegate).</para>
    /// <para>Without <see cref="AssetOneWayOperatingAttribute"/>, <see cref="PropertyGetOneWayOperatingAttribute"/> or <see cref="ReturnIgnoredAttribute"/> specified, no return value is ignored.</para>
    /// </remarks>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#AssetLevel" />
    /// <conceptualLink target="168d9d48-771b-4912-9bcd-880f1d65c090" />
    [AttributeUsage(AttributeTargets.Event | AttributeTargets.Method | AttributeTargets.Delegate | AttributeTargets.Property | AttributeTargets.ReturnValue, Inherited = true, AllowMultiple = false)]
    public sealed class ReturnIgnoredAttribute : Attribute
    {
        /// <summary>
        /// Gets whether the return value is ignored.
        /// </summary>
        public bool IsIgnored { get; }

        /// <summary>
        /// Initializes an instance of ReturnIgnoredAttribute.
        /// </summary>
        /// <param name="isIgnored">Whether the return value is ignored. Default value is <see langword="true"/>.</param>
        public ReturnIgnoredAttribute(bool isIgnored = true)
        {
            IsIgnored = isIgnored;
        }
    }
}
