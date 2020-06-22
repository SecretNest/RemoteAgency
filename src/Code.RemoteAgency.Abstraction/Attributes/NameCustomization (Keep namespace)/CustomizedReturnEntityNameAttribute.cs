using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the entity class name which will be generated for holding return values of this asset.
    /// </summary>
    /// <remarks>When this attribute is not present, or <see cref="EntityName"/> is set to <see langword="null"/> or empty string, the entity name will be chosen automatically.</remarks>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Event, Inherited = true, AllowMultiple = false)]
    public class CustomizedReturnEntityNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the entity class name.
        /// </summary>
        public string EntityName { get; }

        /// <summary>
        /// Initializes an instance of the CustomizedReturnEntityNameAttribute.
        /// </summary>
        /// <param name="entityName">Entity class name.</param>
        public CustomizedReturnEntityNameAttribute(string entityName)
        {
            EntityName = entityName;
        }
    }
}
