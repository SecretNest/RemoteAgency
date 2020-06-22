using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies property name for a parameter in entity class of the event.
    /// </summary>
    /// <remarks><para>Marking on delegate of this event with <see cref="CustomizedParameterEntityPropertyNameAttribute"/> is effected when parameter is not described by this attribute on the event.</para>
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

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        /// <remarks>This is calculated based on <see cref="ParameterName"/>.</remarks>
        public override int GetHashCode()
        {
            return ParameterName.GetHashCode();
        }


        /// <summary>
        /// This API supports the product infrastructure and is not intended to be used directly from your code. Returns a value that indicates whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An object to compare with this instance or null.</param>
        /// <returns>true if obj equals the type and value of this instance; otherwise, false.</returns>
        /// <remarks>This comparer is based on <see cref="ParameterName"/>.</remarks>
        public override bool Equals(object obj)
        {
            return obj is CustomizedEventParameterEntityPropertyNameAttribute target && target.ParameterName == ParameterName;
        }
    }
}
