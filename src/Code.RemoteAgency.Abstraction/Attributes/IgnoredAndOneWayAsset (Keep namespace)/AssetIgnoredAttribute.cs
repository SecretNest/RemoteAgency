using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the asset is ignored from type building.
    /// </summary>
    /// <remarks><para>When <see cref="IsIgnored"/> is set to <see langword="true"/>, any access to this asset will not be transferred or relayed. When <see cref="WillThrowException"/> is set to <see langword="true"/>, a <see cref="NotImplementedException"/> will be thrown when accessing, or default value will be used as returning like one-way operating enabled.</para>
    /// <para>When <see cref="IsIgnored"/> is set to <see langword="true"/>, all other attributes except <see cref="AttributePassThroughAttribute"/>, <see cref="AttributePassThroughIndexBasedParameterAttribute"/> and <see cref="AttributePassThroughPropertyAttribute"/> on or in the same asset, and the delegate related to the asset if the asset is an event, will be ignored.</para>
    /// <para>When this attribute is absent, the ignored mode is disabled.</para>
    /// </remarks>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#AssetLevel" />
    /// <conceptualLink target="e7b65736-b2df-4aa9-897a-3a3050d3cceb" />
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
    public sealed class AssetIgnoredAttribute : Attribute
    {
        /// <summary>
        /// Gets whether the asset is ignored.
        /// </summary>
        public bool IsIgnored { get; }

        /// <summary>
        /// Gets whether an exception should be thrown while accessing this asset.
        /// </summary>
        public bool WillThrowException { get; }

        /// <summary>
        /// Initializes an instance of AssetIgnoredAttribute.
        /// </summary>
        /// <param name="isIgnored">Whether the asset is ignored. Default value is <see langword="true"/>.</param>
        /// <param name="willThrownException">Whether an exception should be thrown while accessing this asset. Default value is <see langword="true"/>.</param>
        public AssetIgnoredAttribute(bool isIgnored = true, bool willThrownException = true)
        {
            IsIgnored = isIgnored;
            WillThrowException = willThrownException;
        }
    }
}
