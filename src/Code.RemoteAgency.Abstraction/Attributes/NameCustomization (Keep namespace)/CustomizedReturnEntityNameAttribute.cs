using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the entity class name which will be generated for holding return values of this asset.
    /// </summary>
    /// <remarks><para>When this attribute is not present, or <see cref="EntityName"/> is set to <see langword="null"/> or empty string, the entity name will be chosen automatically.</para>
    /// <para>The one marked on the event has higher priority than the one marked on the delegate of the same event.</para></remarks>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Event | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
    public class CustomizedReturnEntityNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the entity class name.
        /// </summary>
        public string EntityName { get; }

        /// <summary>
        /// Initializes an instance of the CustomizedReturnEntityNameAttribute.
        /// </summary>
        /// <param name="entityName">Entity class name.</param>
        public CustomizedReturnEntityNameAttribute(string entityName)
        {
            EntityName = entityName;
        }
    }
}
