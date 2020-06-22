using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the entity class name which will be generated for holding the response of setting this property.
    /// </summary>
    /// <remarks>When this attribute is not present, or <see cref="EntityName"/> is set to <see langword="null"/> or empty string, the entity name will be chosen automatically.</remarks>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class CustomizedPropertySetResponseEntityAttribute : Attribute
    {
        /// <summary>
        /// Gets the entity class name.
        /// </summary>
        public string EntityName { get; }
        /// <summary>
        /// Initializes an instance of the CustomizedPropertySetResponseEntityAttribute.
        /// </summary>
        /// <param name="entityName">Entity class name.</param>
        public CustomizedPropertySetResponseEntityAttribute(string entityName)
        {
            EntityName = entityName;
        }
    }
}
