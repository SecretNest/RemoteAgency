using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies property name in entity class which will be generated for holding the response of getting this property. If this attribute absent, the name will be set to "Value".
    /// </summary>
    /// <seealso cref="CustomizedEventParameterEntityPropertyNameAttribute"/>
    /// <seealso cref="CustomizedParameterEntityAttribute"/>
    /// <seealso cref="CustomizedParameterEntityPropertyNameAttribute"/>
    /// <seealso cref="CustomizedReturnEntityAttribute"/>
    /// <seealso cref="CustomizedReturnEntityPropertyNameAttribute"/>
    /// <seealso cref="CustomizedPropertyGetRequestEntityAttribute"/>
    /// <seealso cref="CustomizedPropertyGetResponseEntityAttribute"/>
    /// <seealso cref="CustomizedPropertySetRequestEntityAttribute"/>
    /// <seealso cref="CustomizedPropertySetRequestPropertyNameAttribute"/>
    /// <seealso cref="CustomizedPropertySetResponseEntityAttribute"/>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class CustomizedPropertyGetResponsePropertyNameAttribute : Attribute
    {
        /// <summary>
        /// Property name in entity class.
        /// </summary>
        public string EntityPropertyName { get; }
        /// <summary>
        /// Initializes an instance of the CustomizedPropertyGetResponsePropertyNameAttribute.
        /// </summary>
        /// <param name="entityPropertyName">Property name in entity class.</param>
        public CustomizedPropertyGetResponsePropertyNameAttribute(string entityPropertyName)
        {
            EntityPropertyName = entityPropertyName;
        }
    }
}
