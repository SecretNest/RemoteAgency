﻿using System;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies a field value to be passed while initializing an instance of attribute.
    /// </summary>
    /// <remarks>This attribute works with <see cref="AttributePassThroughAttribute"/> with the value <see cref="AttributePassThroughAttribute.AttributeId"/> is the same as <see cref="AttributePassThroughAdditionalDataAttributeBase.AttributeId"/>.</remarks>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#AttributePassThrough" />
    /// <conceptualLink target="0276dae1-94a2-4e9c-87ab-e3b371f41104" />
    [AttributeUsage(AttributeTargets.Event | AttributeTargets.Method | AttributeTargets.Property |
                    AttributeTargets.Parameter | AttributeTargets.GenericParameter | AttributeTargets.ReturnValue |
                    AttributeTargets.Interface, Inherited = true, AllowMultiple = true)]
    public sealed class AttributePassThroughFieldAttribute : AttributePassThroughAdditionalDataAttributeBase
    {
        /// <summary>
        /// Gets the order for setting the field. All setting operations are performed sequentially. Default value is 0.
        /// </summary>
        /// <remarks></remarks>
        public int Order { get; }

        /// <summary>
        /// Gets the name of the field.
        /// </summary>
        public string FieldName { get; }

        /// <summary>
        /// Gets the value of the field.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Initializes an instance of AttributePassThroughFieldAttribute.
        /// </summary>
        /// <param name="attributeId">Id of the instance of the attribute. This value should be same as the <see cref="AttributePassThroughAttribute.AttributeId"/> marked at the same place for the same instance of attribute.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <param name="value">Value of the field.</param>
        /// <param name="order">Order for setting the field. All setting operations are performed sequentially. Default value is 0.</param>
        public AttributePassThroughFieldAttribute(string attributeId, string fieldName, object value, int order = 0) : base(attributeId)
        {
            FieldName = fieldName ?? throw new ArgumentNullException(nameof(fieldName));
            Value = value;
            Order = order;
        }
    }
}
