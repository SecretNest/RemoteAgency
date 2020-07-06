using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies property name for a parameter in entity class of the event.
    /// </summary>
    /// <remarks><para><see cref="CustomizedParameterEntityPropertyNameAttribute"/> can be marked on parameters of the delegate related to this event, with lower priority than <see cref="CustomizedEventParameterEntityPropertyNameAttribute"/>.</para>
    /// <para>For the parameter is not described by either <see cref="CustomizedEventParameterEntityPropertyNameAttribute"/> or <see cref="CustomizedParameterEntityPropertyNameAttribute"/>, or <see cref="CustomizedParameterEntityPropertyNameAttribute.EntityPropertyName"/> is set to <see langword="null"/> or empty string, the property name is chosen automatically.</para>
    /// </remarks>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#CustomizedName" />
    [AttributeUsage(AttributeTargets.Event, Inherited = true, AllowMultiple = true)]
    public class CustomizedEventParameterEntityPropertyNameAttribute : CustomizedParameterEntityPropertyNameAttribute
    {
        /// <summary>
        /// Gets the parameter name of the event.
        /// </summary>
        public string ParameterName { get; }

        /// <summary>
        /// Initializes an instance of the CustomizedEventParameterEntityPropertyNameAttribute.
        /// </summary>
        /// <param name="parameterName">Parameter name of the event.</param>
        /// <param name="entityPropertyName">Property name in entity class. When the value is <see langword="null"/> or empty string, name is chosen automatically.</param>
        public CustomizedEventParameterEntityPropertyNameAttribute(string parameterName, string entityPropertyName) :
            base(entityPropertyName)
        {
            ParameterName = parameterName;
        }
    }
}
