using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies property name for return value in entity class.
    /// </summary>
    /// <remarks>
    /// <para>When this attribute is not present, or <see cref="EntityPropertyName"/> is set to <see langword="null"/> or empty string, the property name is chosen automatically.</para>
    /// <para>The one marked on the event has higher priority than the one marked on the delegate of the same event.</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Event | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
    public class CustomizedReturnEntityPropertyNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the property name in entity class.
        /// </summary>
        /// <remarks>When the value is <see langword="null"/> or empty string, name is chosen automatically.</remarks>
        public string EntityPropertyName { get; }

        /// <summary>
        /// Initializes an instance of the CustomizedReturnEntityPropertyNameAttribute.
        /// </summary>
        /// <param name="entityPropertyName">Property name in entity class. When the value is <see langword="null"/> or empty string, name is chosen automatically.</param>
        public CustomizedReturnEntityPropertyNameAttribute(string entityPropertyName)
        {
            EntityPropertyName = entityPropertyName;
        }
    }
}
