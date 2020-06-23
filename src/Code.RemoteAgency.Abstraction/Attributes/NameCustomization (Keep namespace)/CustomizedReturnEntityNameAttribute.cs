using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the name of the entity class generated for holding return values of this asset.
    /// </summary>
    /// <remarks>When this attribute is not present, or <see cref="EntityName"/> is set to <see langword="null"/> or empty string, the entity name is chosen automatically.</remarks>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CustomizedReturnEntityNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the entity class name.
        /// </summary>
        /// <remarks>When the value is <see langword="null"/> or empty string, name is chosen automatically.</remarks>
        public string EntityName { get; }

        /// <summary>
        /// Initializes an instance of the CustomizedReturnEntityNameAttribute.
        /// </summary>
        /// <param name="entityName">Entity class name. When the value is <see langword="null"/> or empty string, name is chosen automatically.</param>
        public CustomizedReturnEntityNameAttribute(string entityName)
        {
            EntityName = entityName;
        }
    }
}
