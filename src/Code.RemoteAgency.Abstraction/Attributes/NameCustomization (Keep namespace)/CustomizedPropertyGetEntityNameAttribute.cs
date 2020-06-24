using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the name of the entity class generated for holding the request and response of getting this property.
    /// </summary>
    /// <remarks>When this attribute is not present, or name is set to <see langword="null"/> or empty string, the entity name is chosen automatically.</remarks>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class CustomizedPropertyGetEntityNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the name entity class generated for holding the request of getting this property.
        /// </summary>
        /// <remarks>When the value is <see langword="null"/> or empty string, name is chosen automatically.</remarks>
        public string RequestEntityName { get; }

        /// <summary>
        /// Gets the name entity class generated for holding the response of getting this property.
        /// </summary>
        /// <remarks>When the value is <see langword="null"/> or empty string, name is chosen automatically.</remarks>
        public string ResponseEntityName { get; }

        /// <summary>
        /// Initializes an instance of the CustomizedPropertyGetEntityNameAttribute.
        /// </summary>
        /// <param name="requestEntityName">Name entity class generated for holding the request of getting this property. When the value is <see langword="null"/> or empty string, name is chosen automatically.</param>
        /// <param name="responseEntityName">Name entity class generated for holding the response of getting this property. When the value is <see langword="null"/> or empty string, name is chosen automatically.</param>
        public CustomizedPropertyGetEntityNameAttribute(string requestEntityName, string responseEntityName)
        {
            RequestEntityName = requestEntityName;
            ResponseEntityName = responseEntityName;
        }
    }
}
