using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the name of the entity class generated for holding the request and response of setting this property.
    /// </summary>
    /// <remarks>When this attribute is not present, or name is set to <see langword="null"/> or empty string, the entity name is chosen automatically.</remarks>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#CustomizedName" />
    /// <conceptualLink target="beb637a2-3887-49ff-93f3-1f71b095aa7e" />
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class CustomizedPropertySetEntityNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the name entity class generated for holding the request of setting this property.
        /// </summary>
        /// <remarks>When the value is <see langword="null"/> or empty string, name is chosen automatically.</remarks>
        public string RequestEntityName { get; }

        /// <summary>
        /// Gets the name entity class generated for holding the response of setting this property.
        /// </summary>
        /// <remarks>When the value is <see langword="null"/> or empty string, name is chosen automatically.</remarks>
        public string ResponseEntityName { get; }

        /// <summary>
        /// Initializes an instance of the CustomizedPropertySetEntityNameAttribute.
        /// </summary>
        /// <param name="requestEntityName">Name entity class generated for holding the request of setting this property. When the value is <see langword="null"/> or empty string, name is chosen automatically.</param>
        /// <param name="responseEntityName">Name entity class generated for holding the response of setting this property. When the value is <see langword="null"/> or empty string, name is chosen automatically.</param>
        public CustomizedPropertySetEntityNameAttribute(string requestEntityName, string responseEntityName)
        {
            RequestEntityName = requestEntityName;
            ResponseEntityName = responseEntityName;
        }
    }
}
