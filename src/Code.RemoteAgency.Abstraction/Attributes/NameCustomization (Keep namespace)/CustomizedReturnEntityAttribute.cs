using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the entity class name which will be generated for holding returns of this asset. If this attribute absent, the name will be chosen automatically.
    /// </summary>
    /// <seealso cref="CustomizedEventParameterEntityPropertyNameAttribute"/>
    /// <seealso cref="CustomizedParameterEntityAttribute"/>
    /// <seealso cref="CustomizedParameterEntityPropertyNameAttribute"/>
    /// <seealso cref="CustomizedReturnEntityPropertyNameAttribute"/>
    /// <seealso cref="CustomizedPropertyGetRequestEntityAttribute"/>
    /// <seealso cref="CustomizedPropertyGetResponseEntityAttribute"/>
    /// <seealso cref="CustomizedPropertyGetResponsePropertyNameAttribute"/>
    /// <seealso cref="CustomizedPropertySetRequestEntityAttribute"/>
    /// <seealso cref="CustomizedPropertySetRequestPropertyNameAttribute"/>
    /// <seealso cref="CustomizedPropertySetResponseEntityAttribute"/>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Event, Inherited = true, AllowMultiple = false)]
    public class CustomizedReturnEntityAttribute : Attribute
    {
        /// <summary>
        /// Gets the entity class name.
        /// </summary>
        public string EntityName { get; }

        /// <summary>
        /// Initializes an instance of the CustomizedReturnEntityAttribute.
        /// </summary>
        /// <param name="entityName">Entity class name.</param>
        public CustomizedReturnEntityAttribute(string entityName)
        {
            EntityName = entityName;
        }
    }
}
