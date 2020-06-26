using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the names of the entity classes generated for holding parameters and return value of this asset.
    /// </summary>
    /// <remarks>When this attribute is not present, or name is set to <see langword="null"/> or empty string, the entity name is chosen automatically.</remarks>
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CustomizedMethodEntityNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the entity class generated for holding parameters of this asset.
        /// </summary>
        /// <remarks>When the value is <see langword="null"/> or empty string, name is chosen automatically.</remarks>
        public string ParameterEntityName { get; }

        /// <summary>
        /// Gets the name of the entity class generated for holding two way parameters, output parameters and return value of this asset.
        /// </summary>
        /// <remarks>When the value is <see langword="null"/> or empty string, name is chosen automatically.</remarks>
        public string ReturnValueEntityName { get; }

        /// <summary>
        /// Initializes an instance of the CustomizedMethodEntityNameAttribute.
        /// </summary>
        /// <param name="parameterEntityName">Name of the entity class generated for holding parameters of this asset. When the value is <see langword="null"/> or empty string, name is chosen automatically.</param>
        /// <param name="returnValueEntityName">Name of the entity class generated for holding two way parameters, output parameters and return value of this asset. When the value is <see langword="null"/> or empty string, name is chosen automatically.</param>
        public CustomizedMethodEntityNameAttribute(string parameterEntityName, string returnValueEntityName)
        {
            ParameterEntityName = parameterEntityName;
            ReturnValueEntityName = returnValueEntityName;
        }
    }
}
