using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the names of the entity classes generated for holding parameters and return value of this asset.
    /// </summary>
    /// <remarks>When this attribute is not present, or name is set as <see langword="null"/> or empty string, the entity name is chosen automatically.</remarks>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#CustomizedName" />
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class CustomizedMethodEntityNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the entity class generated for holding parameters of this asset.
        /// </summary>
        /// <remarks>When the value is <see langword="null"/> or empty string, name is chosen automatically.</remarks>
        public string ParameterEntityName { get; }

        /// <summary>
        /// Gets the name of the entity class generated for holding return required parameters, output parameters and return value of this asset.
        /// </summary>
        /// <remarks>When the value is <see langword="null"/> or empty string, name is chosen automatically.</remarks>
        public string ReturnValueEntityName { get; }

        /// <summary>
        /// Initializes an instance of the CustomizedMethodEntityNameAttribute.
        /// </summary>
        /// <param name="parameterEntityName">Name of the entity class generated for holding parameters of this asset. When the value is <see langword="null"/> or empty string, name is chosen automatically.</param>
        /// <param name="returnValueEntityName">Name of the entity class generated for holding return required parameters, output parameters and return value of this asset. When the value is <see langword="null"/> or empty string, name is chosen automatically.</param>
        public CustomizedMethodEntityNameAttribute(string parameterEntityName, string returnValueEntityName)
        {
            ParameterEntityName = parameterEntityName;
            ReturnValueEntityName = returnValueEntityName;
        }
    }
}
