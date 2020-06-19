using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the name of the asset.
    /// </summary>
    /// <remarks>By default, the name of the asset will be the same as the name of Method, Event or Property.</remarks>
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
        /// <param name="assetName">Asset name.</param>
        public CustomizedAssetNameAttribute(string assetName)
        {
            AssetName = assetName;
        }
    }
}
