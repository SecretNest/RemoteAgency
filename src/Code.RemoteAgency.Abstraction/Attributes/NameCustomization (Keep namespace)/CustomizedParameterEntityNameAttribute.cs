using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the entity class name which will be generated for holding parameters of this asset.
    /// </summary>
    /// <remarks>When this attribute is not present, or <see cref="EntityName"/> is set to <see langword="null"/> or empty string, the entity name will be chosen automatically.</remarks>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Event, Inherited = true, AllowMultiple = false)]
    public class CustomizedParameterEntityNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the entity class name.
        /// </summary>
        public string EntityName { get; }

        /// <summary>
        /// Initializes an instance of the CustomizedParameterEntityNameAttribute.
        /// </summary>
        /// <param name="entityName">Entity class name.</param>
        public CustomizedParameterEntityNameAttribute(string entityName)
        {
            EntityName = entityName;
        }
    }
}
