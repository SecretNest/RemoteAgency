using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the name of the entity class generated for holding the response of setting this property.
    /// </summary>
    /// <remarks>When this attribute is not present, or <see cref="EntityName"/> is set to <see langword="null"/> or empty string, the entity name is chosen automatically.</remarks>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class CustomizedPropertySetResponseEntityNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the entity class name.
        /// </summary>
        /// <remarks>When the value is <see langword="null"/> or empty string, name will be generate automatically.</remarks>
        public string EntityName { get; }

        /// <summary>
        /// Initializes an instance of the CustomizedPropertySetResponseEntityNameAttribute.
        /// </summary>
        /// <param name="entityName">Entity class name. When the value is <see langword="null"/> or empty string, name is chosen automatically.</param>
        public CustomizedPropertySetResponseEntityNameAttribute(string entityName)
        {
            EntityName = entityName;
        }
    }
}
