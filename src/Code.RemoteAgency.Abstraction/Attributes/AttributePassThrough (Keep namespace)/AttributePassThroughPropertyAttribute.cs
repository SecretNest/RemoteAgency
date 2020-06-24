using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies a property value to be passed while initializing an instance of attribute.
    /// </summary>
    /// <remarks>This attribute works with <see cref="AttributePassThroughAttribute"/> with the value <see cref="AttributePassThroughAttribute.AttributeId"/> is the same as <see cref="AttributeId"/>.</remarks>
    [AttributeUsage(AttributeTargets.Event | AttributeTargets.Method | AttributeTargets.Delegate | AttributeTargets.Property |
                    AttributeTargets.Parameter | AttributeTargets.GenericParameter | AttributeTargets.ReturnValue |
                    AttributeTargets.Interface, Inherited = true, AllowMultiple = true)]
    public class AttributePassThroughPropertyAttribute : Attribute
    {
        /// <summary>
        /// Gets the id of the instance of the attribute.
        /// </summary>
        /// <remarks>This value should be same as the <see cref="AttributePassThroughAttribute.AttributeId"/> marked at the same place for the same instance of attribute.</remarks>
        public string AttributeId { get; }

        /// <summary>
        /// Gets the order for setting the property. All setting operations are performed sequentially. Default value is 0.
        /// </summary>
        public int Order { get; }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Gets the value of the property.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Initializes an instance of AttributePassThroughPropertyAttribute.
        /// </summary>
        /// <param name="attributeId">Id of the instance of the attribute. This value should be same as the <see cref="AttributePassThroughAttribute.AttributeId"/> marked at the same place for the same instance of attribute.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">Value of the property.</param>
        /// <param name="order">Order for setting the property. All setting operations are performed sequentially. Default value is 0.</param>
        public AttributePassThroughPropertyAttribute(string attributeId, string propertyName, object value, int order = 0)
        {
            AttributeId = attributeId ?? throw new ArgumentNullException(nameof(attributeId));
            PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            Value = value;
            Order = order;
        }
    }
}
