using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the entity class name which will be generated for holding the request of getting this property. If this attribute absent, the name will be chosen automatically.
    /// </summary>
    /// <seealso cref="CustomizedEventParameterEntityPropertyNameAttribute"/>
    /// <seealso cref="CustomizedParameterEntityAttribute"/>
    /// <seealso cref="CustomizedParameterEntityPropertyNameAttribute"/>
    /// <seealso cref="CustomizedReturnEntityAttribute"/>
    /// <seealso cref="CustomizedReturnEntityPropertyNameAttribute"/>
    /// <seealso cref="CustomizedPropertyGetResponseEntityAttribute"/>
    /// <seealso cref="CustomizedPropertyGetResponsePropertyNameAttribute"/>
    /// <seealso cref="CustomizedPropertySetRequestEntityAttribute"/>
    /// <seealso cref="CustomizedPropertySetRequestPropertyNameAttribute"/>
    /// <seealso cref="CustomizedPropertySetResponseEntityAttribute"/>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class CustomizedPropertyGetRequestEntityAttribute : Attribute
    {
        /// <summary>
        /// Entity class name.
        /// </summary>
        public string EntityName { get; }
        /// <summary>
        /// Initializes an instance of the CustomizedPropertyGetRequestEntityAttribute.
        /// </summary>
        /// <param name="entityName">Entity class name.</param>
        public CustomizedPropertyGetRequestEntityAttribute(string entityName)
        {
            EntityName = entityName;
        }
    }
}
