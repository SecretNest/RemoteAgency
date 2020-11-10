using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the name of the asset.
    /// </summary>
    /// <remarks>When this attribute is not present, or <see cref="AssetName"/> is set to <see langword="null"/> or empty string, the asset name is chosen automatically.</remarks>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#CustomizedName" />
    /// <conceptualLink target="beb637a2-3887-49ff-93f3-1f71b095aa7e" />
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event, Inherited = true, AllowMultiple = false)]
    public sealed class CustomizedAssetNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the asset name. When the value is <see langword="null"/> or empty string, the name of the asset is the same as the name of Method, Event or Property.
        /// </summary>
        public string AssetName { get; }

        /// <summary>
        /// Initializes an instance of the CustomizedAssetNameAttribute.
        /// </summary>
        /// <param name="assetName">Asset name. When the value is <see langword="null"/> or empty string, name is chosen automatically.</param>
        public CustomizedAssetNameAttribute(string assetName)
        {
            AssetName = assetName;
        }
    }
}
