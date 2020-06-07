using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies property name of a parameter in entity class of the event. If this attribute absent, the name will be chosen automatically.
    /// </summary>
    /// <remarks>You can also use <see cref="CustomizedParameterEntityPropertyNameAttribute"/> in declaration of the delegate related to this event, which has lower priority.</remarks>
    /// <seealso cref="CustomizedParameterEntityAttribute"/>
    /// <seealso cref="CustomizedParameterEntityPropertyNameAttribute"/>
    /// <seealso cref="CustomizedReturnEntityAttribute"/>
    /// <seealso cref="CustomizedReturnEntityPropertyNameAttribute"/>
    /// <seealso cref="CustomizedPropertyGetRequestEntityAttribute"/>
    /// <seealso cref="CustomizedPropertyGetResponseEntityAttribute"/>
    /// <seealso cref="CustomizedPropertyGetResponsePropertyNameAttribute"/>
    /// <seealso cref="CustomizedPropertySetRequestEntityAttribute"/>
    /// <seealso cref="CustomizedPropertySetRequestPropertyNameAttribute"/>
    /// <seealso cref="CustomizedPropertySetResponseEntityAttribute"/>
    [AttributeUsage(AttributeTargets.Event, Inherited = true, AllowMultiple = true)]
    public class CustomizedEventParameterEntityPropertyNameAttribute : Attribute
    {
        /// <summary>
        /// Parameter name of the event.
        /// </summary>
        public string ParameterName { get; }

        /// <summary>
        /// Property name in entity class.
        /// </summary>
        public string EntityPropertyName { get; }

        /// <summary>
        /// Initializes an instance of the CustomizedEventParameterEntityPropertyNameAttribute.
        /// </summary>
        /// <param name="parameterName">Parameter name of the event.</param>
        /// <param name="entityPropertyName">Property name in entity class.</param>
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
            var target = obj as CustomizedEventParameterEntityPropertyNameAttribute;
            return target != null && target.ParameterName == ParameterName;
        }
    }
}
