using System;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies property name for return value in entity class.
    /// </summary>
    /// <remarks>
    /// <para>When this attribute is not present, or <see cref="EntityPropertyName"/> is set to <see langword="null"/> or empty string, the property name is chosen automatically.</para>
    /// <para>The one marked on the event has higher priority than the one marked on the delegate of the same event.</para>
    /// <para>The one marked on the return value has higher priority than the one marked on the same member (method, event or delegate).</para>
    /// </remarks>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#CustomizedName" />
    /// <conceptualLink target="beb637a2-3887-49ff-93f3-1f71b095aa7e" />
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Event | AttributeTargets.Delegate | AttributeTargets.ReturnValue, Inherited = true, AllowMultiple = false)]
    public sealed class CustomizedReturnValueEntityPropertyNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the property name in entity class.
        /// </summary>
        /// <remarks>When the value is <see langword="null"/> or empty string, name is chosen automatically.</remarks>
        public string EntityPropertyName { get; }

        /// <summary>
        /// Initializes an instance of the CustomizedReturnValueEntityPropertyNameAttribute.
        /// </summary>
        /// <param name="entityPropertyName">Property name in entity class. When the value is <see langword="null"/> or empty string, name is chosen automatically.</param>
        public CustomizedReturnValueEntityPropertyNameAttribute(string entityPropertyName)
        {
            EntityPropertyName = entityPropertyName;
        }
    }
}
