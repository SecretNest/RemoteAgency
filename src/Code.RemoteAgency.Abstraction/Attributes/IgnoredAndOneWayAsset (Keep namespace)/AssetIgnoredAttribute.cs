﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the asset is ignored from type building.
    /// </summary>
    /// <remarks><para>When <see cref="IsIgnored"/> is set to <see langword="true"/>, any access to this asset will not be transferred and relayed. When <see cref="WillThrowException"/> is set to <see langword="true"/>, a <see cref="NotImplementedException"/> will be thrown when accessing, or default value will be used as returning like one way operating enabled.</para>
    /// <para>When this attribute is absent, the ignored mode is disabled.</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
    public class AssetIgnoredAttribute : Attribute
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
        /// <param name="isIgnored">Whether the asset is ignored. Default value is true.</param>
        /// <param name="willThrownException">Whether an exception should be thrown while accessing this asset. Default value is true.</param>
        public AssetIgnoredAttribute(bool isIgnored = true, bool willThrownException = true)
        {
            IsIgnored = isIgnored;
            WillThrowException = willThrownException;
        }
    }
}