using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the entity class name which will be generated for holding the request of setting this property.
    /// </summary>
    /// <remarks>When this attribute is not present, or <see cref="EntityName"/> is set to <see langword="null"/> or empty string, the entity name will be chosen automatically.</remarks>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class CustomizedPropertySetRequestEntityAttribute : Attribute
    {
        /// <summary>
        /// Gets the entity class name.
        /// </summary>
        public string EntityName { get; }

        /// <summary>
        /// Initializes an instance of the CustomizedPropertySetRequestEntityAttribute.
        /// </summary>
        /// <param name="entityName">Entity class name.</param>
        public CustomizedPropertySetRequestEntityAttribute(string entityName)
        {
            EntityName = entityName;
        }
    }
}
