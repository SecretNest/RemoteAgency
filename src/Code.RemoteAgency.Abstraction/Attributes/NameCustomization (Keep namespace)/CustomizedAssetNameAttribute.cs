using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the name of the asset.
    /// </summary>
    /// <remarks>When this attribute is not present, or <see cref="AssetName"/> is set to <see langword="null"/> or empty string, the name of the asset will be the same as the name of Method, Event or Property.</remarks>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event, Inherited = true, AllowMultiple = false)]
    public class CustomizedAssetNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the asset name.
        /// </summary>
        public string AssetName { get; }

        /// <summary>
        /// Initializes an instance of the CustomizedAssetNameAttribute.
        /// </summary>
        /// <param name="assetName">Asset name. Set to <see langword="null"/> or empty string to use the default value.</param>
        public CustomizedAssetNameAttribute(string assetName)
        {
            AssetName = assetName;
        }
    }
}
