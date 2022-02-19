using System;

namespace SecretNest.RemoteAgency.Attributes
{ 
    /// <summary>
    /// An abstract class of all attributes for providing additional data to attribute pass through.
    /// </summary>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#AttributePassThrough" />
    /// <conceptualLink target="0276dae1-94a2-4e9c-87ab-e3b371f41104" />
    public abstract class AttributePassThroughAdditionalDataAttributeBase : Attribute
    {
        /// <summary>
        /// Gets the id of the instance of the attribute.
        /// </summary>
        /// <remarks>This value should be same as the <see cref="AttributePassThroughAttribute.AttributeId"/> marked at the same place for the same instance of attribute.</remarks>
        public string AttributeId { get; }

        /// <summary>
        /// Initializes an instance of AttributePassThroughAdditionalDataAttributeBase.
        /// </summary>
        /// <param name="attributeId">Id of the instance of the attribute. This value should be same as the <see cref="AttributePassThroughAttribute.AttributeId"/> marked at the same place for the same instance of attribute.</param>
        protected AttributePassThroughAdditionalDataAttributeBase(string attributeId)
        {
            AttributeId = attributeId ?? throw new ArgumentNullException(nameof(attributeId));
        }
    }
}
