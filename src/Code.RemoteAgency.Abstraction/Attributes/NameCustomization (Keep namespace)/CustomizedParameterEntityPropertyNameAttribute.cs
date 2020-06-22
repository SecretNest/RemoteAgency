using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies property name for the parameter in entity class.
    /// </summary>
    /// <remarks>When this attribute is not present, or <see cref="EntityPropertyName"/> is set to <see langword="null"/> or empty string, the property name will be chosen automatically.</remarks>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
    public class CustomizedParameterEntityPropertyNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the property name in entity class.
        /// </summary>
        public string EntityPropertyName { get; }

        /// <summary>
        /// Initializes an instance of the CustomizedParameterEntityPropertyNameAttribute.
        /// </summary>
        /// <param name="entityPropertyName">Property name in entity class.</param>
        public CustomizedParameterEntityPropertyNameAttribute(string entityPropertyName)
        {
            EntityPropertyName = entityPropertyName;
        }
    }
}
