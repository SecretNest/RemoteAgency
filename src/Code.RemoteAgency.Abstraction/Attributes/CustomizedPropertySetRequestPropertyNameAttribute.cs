using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies property name in entity class which will be generated for holding the request of setting this property. If this attribute absent, the name will be set to "Value".
    /// </summary>
    /// <seealso cref="CustomizedEventParameterEntityPropertyNameAttribute"/>
    /// <seealso cref="CustomizedParameterEntityAttribute"/>
    /// <seealso cref="CustomizedParameterEntityPropertyNameAttribute"/>
    /// <seealso cref="CustomizedReturnEntityAttribute"/>
    /// <seealso cref="CustomizedReturnEntityPropertyNameAttribute"/>
    /// <seealso cref="CustomizedPropertyGetRequestEntityAttribute"/>
    /// <seealso cref="CustomizedPropertyGetResponseEntityAttribute"/>
    /// <seealso cref="CustomizedPropertyGetResponsePropertyNameAttribute"/>
    /// <seealso cref="CustomizedPropertySetRequestEntityAttribute"/>
    /// <seealso cref="CustomizedPropertySetResponseEntityAttribute"/>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class CustomizedPropertySetRequestPropertyNameAttribute : Attribute
    {
        /// <summary>
        /// Property name in entity class.
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
