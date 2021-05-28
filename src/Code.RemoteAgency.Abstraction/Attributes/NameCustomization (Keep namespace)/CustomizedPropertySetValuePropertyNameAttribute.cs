using System;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies property name in entity class generated for holding the request of setting this property.
    /// </summary>
    /// <remarks>When this attribute is not present, or <see cref="CustomizedParameterEntityPropertyNameAttribute.EntityPropertyName"/> is set to <see langword="null"/> or empty string, the property name is chosen automatically.</remarks>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#CustomizedName" />
    /// <conceptualLink target="beb637a2-3887-49ff-93f3-1f71b095aa7e" />
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class CustomizedPropertySetValuePropertyNameAttribute : CustomizedParameterEntityPropertyNameAttribute
    {
        /// <summary>
        /// Initializes an instance of the CustomizedPropertySetValuePropertyNameAttribute.
        /// </summary>
        /// <param name="entityPropertyName">Property name in entity class. When the value is <see langword="null"/> or empty string, name is chosen automatically.</param>
        public CustomizedPropertySetValuePropertyNameAttribute(string entityPropertyName) : base(entityPropertyName)
        {
        }
    }
}
