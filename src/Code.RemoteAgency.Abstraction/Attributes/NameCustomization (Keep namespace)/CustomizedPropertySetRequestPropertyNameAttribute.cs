using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies property name in entity class which will be generated for holding the request of setting this property.
    /// </summary>
    /// <remarks>When this attribute is not present, or <see cref="EntityPropertyName"/> is set to <see langword="null"/> or empty string, the property name will be chosen automatically.</remarks>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class CustomizedPropertySetRequestPropertyNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the property name in entity class.
        /// </summary>
        public string EntityPropertyName { get; }

        /// <summary>
        /// Initializes an instance of the CustomizedPropertySetRequestPropertyNameAttribute.
        /// </summary>
        /// <param name="entityPropertyName">Property name in entity class.</param>
        public CustomizedPropertySetRequestPropertyNameAttribute(string entityPropertyName)
        {
            EntityPropertyName = entityPropertyName;
        }
    }
}
