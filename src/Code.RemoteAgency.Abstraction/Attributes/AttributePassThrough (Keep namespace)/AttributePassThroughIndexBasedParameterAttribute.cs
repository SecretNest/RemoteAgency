﻿using System;
namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies a parameter value to be passed while constructing an instance of attribute.
    /// </summary>
    /// <remarks><para>This attribute works with <see cref="AttributePassThroughAttribute"/> with the value <see cref="AttributePassThroughAttribute.AttributeId"/> is the same as <see cref="AttributePassThroughAdditionalDataAttributeBase.AttributeId"/>.</para>
    /// <para>Name of the parameter should be specified by index of the parameter due to lack of parameter name list.</para></remarks>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#AttributePassThrough" />
    /// <conceptualLink target="0276dae1-94a2-4e9c-87ab-e3b371f41104" />
    [AttributeUsage(AttributeTargets.Event | AttributeTargets.Method | AttributeTargets.Property |
                    AttributeTargets.Parameter | AttributeTargets.GenericParameter | AttributeTargets.ReturnValue |
                    AttributeTargets.Interface, Inherited = true, AllowMultiple = true)]
    public sealed class AttributePassThroughIndexBasedParameterAttribute : AttributePassThroughAdditionalDataAttributeBase
    {
        /// <summary>
        /// Gets the index of the parameter.
        /// </summary>
        /// <returns><para>The value cannot be equal or larger than the length of <see cref="AttributePassThroughAttribute.AttributeConstructorParameterTypes"/> marked at the same place with the value <see cref="AttributePassThroughAttribute.AttributeId"/> is the same as <see cref="AttributePassThroughAdditionalDataAttributeBase.AttributeId"/>.</para>
        /// <para>When the index is smaller than the length of <see cref="AttributePassThroughAttribute.AttributeConstructorParameters"/> marked at the same place with the value <see cref="AttributePassThroughAttribute.AttributeId"/> is the same as <see cref="AttributePassThroughAdditionalDataAttributeBase.AttributeId"/>, the parameter in <see cref="AttributePassThroughAttribute.AttributeConstructorParameters"/> with the index specified by <see cref="ParameterIndex"/> is replaced with <see cref="Value"/>.</para></returns>
        public int ParameterIndex { get; }

        /// <summary>
        /// Gets the value of the parameter.
        /// </summary>
        public object Value { get; }

        /// <summary>
        /// Initializes an instance of AttributePassThroughIndexBasedParameterAttribute.
        /// </summary>
        /// <param name="attributeId">Id of the instance of the attribute. This value should be same as the <see cref="AttributePassThroughAttribute.AttributeId"/> marked at the same place for the same instance of attribute.</param>
        /// <param name="parameterIndex">Index of the parameter.</param>
        /// <param name="value">Value of the parameter.</param>
        public AttributePassThroughIndexBasedParameterAttribute(string attributeId, int parameterIndex, object value) : base(attributeId)

        {
            if (parameterIndex < 0) throw new ArgumentOutOfRangeException(nameof(parameterIndex));
            ParameterIndex = parameterIndex;
            Value = value;
        }
    }
}