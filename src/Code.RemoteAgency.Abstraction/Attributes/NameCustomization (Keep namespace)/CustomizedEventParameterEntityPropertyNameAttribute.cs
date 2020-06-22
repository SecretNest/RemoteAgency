using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies property name for a parameter in entity class of the event.
    /// </summary>
    /// <remarks><para><see cref="CustomizedParameterEntityPropertyNameAttribute"/> can be marked on parameters of the delegate related to this event, with lower priority than <see cref="CustomizedEventParameterEntityPropertyNameAttribute"/>.</para>
    /// <para>For the parameter is not described by either <see cref="CustomizedEventParameterEntityPropertyNameAttribute"/> or <see cref="CustomizedParameterEntityPropertyNameAttribute"/>, or <see cref="EntityPropertyName"/> is set to <see langword="null"/> or empty string, the property name will be chosen automatically.</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Event, Inherited = true, AllowMultiple = true)]
    public class CustomizedEventParameterEntityPropertyNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the parameter name of the event.
        /// </summary>
        public string ParameterName { get; }

        /// <summary>
        /// Gets the property name in entity class.
        /// </summary>
        public string EntityPropertyName { get; }

        /// <summary>
        /// Initializes an instance of the CustomizedEventParameterEntityPropertyNameAttribute.
        /// </summary>
        /// <param name="parameterName">Parameter name of the event.</param>
        /// <param name="entityPropertyName">Property name in entity class. Set to <see langword="null"/> or empty string to use the default value.</param>
        public CustomizedEventParameterEntityPropertyNameAttribute(string parameterName, string entityPropertyName)
        {
            ParameterName = parameterName;
            EntityPropertyName = entityPropertyName;
        }
    }
}
