using System;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies an attribute should be marked at the same place of built proxy class.
    /// </summary>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#AttributePassThrough" />
    /// <conceptualLink target="0276dae1-94a2-4e9c-87ab-e3b371f41104" />
    [AttributeUsage(AttributeTargets.Event | AttributeTargets.Method | AttributeTargets.Property |
                    AttributeTargets.Parameter | AttributeTargets.GenericParameter | AttributeTargets.ReturnValue | 
                    AttributeTargets.Interface, Inherited = true, AllowMultiple = true)]
    public sealed class AttributePassThroughAttribute : Attribute
    {
        /// <summary>
        /// Gets the id of the instance of the attribute.
        /// </summary>
        /// <remarks><para>This id is designed for linking <see cref="AttributePassThroughAttribute"/> with <see cref="AttributePassThroughIndexBasedParameterAttribute"/>, <see cref="AttributePassThroughPropertyAttribute"/> and <see cref="AttributePassThroughFieldAttribute"/> on the same member. When no need to mark with <see cref="AttributePassThroughIndexBasedParameterAttribute"/> or <see cref="AttributePassThroughPropertyAttribute"/>, this value is optional.</para>
        /// </remarks>
        public string AttributeId { get; }

        /// <summary>
        /// Gets the type of the attribute.
        /// </summary>
        public Type Attribute { get; }

        /// <summary>
        /// Gets the types to identify the constructor of attribute.
        /// </summary>
        public Type[] AttributeConstructorParameterTypes { get; }

        /// <summary>
        /// Gets the parameters used in constructor. The length can not exceed the length of <see cref="AttributeConstructorParameterTypes"/>.
        /// </summary>
        public object[] AttributeConstructorParameters { get; }

        /// <summary>
        /// Initializes an instance of AttributePassThroughAttribute.
        /// </summary>
        /// <param name="attribute">Type of the attribute.</param>
        /// <param name="attributeConstructorParameterTypes">Types to identify the constructor of attribute. Default value is <see langword="null"/>. Set the value to <see langword="null"/> or empty array to use parameterless constructor.</param>
        /// <param name="attributeConstructorParameters">Parameters used in constructor. The length can not exceed the length of <paramref name="attributeConstructorParameterTypes"/>.</param>
        /// <param name="attributeId">Id of the instance of the attribute.</param>
        /// <remarks><para>Use this attribute to mark an attribute at the same place in the created class.</para>
        /// <para>When using the parameterless constructor of target attribute, <paramref name="attributeConstructorParameterTypes"/> and <paramref name="attributeConstructorParameters"/> are not required.</para>
        /// <para>When using a constructor with all parameters specified in order, sets types of parameters of constructor to <paramref name="attributeConstructorParameterTypes"/> and values to <paramref name="attributeConstructorParameters"/>. When the length of <paramref name="attributeConstructorParameters"/> is smaller than the length of <paramref name="attributeConstructorParameterTypes"/>, the missing parameters will be filled by <see langword="null"/>.</para>
        /// <para>When named parameter is required for constructing, sets types of full form parameters of constructor to <paramref name="attributeConstructorParameterTypes"/> and leading values to <paramref name="attributeConstructorParameters"/>. Marks the rest one by one using <see cref="AttributePassThroughIndexBasedParameterAttribute"/>. The <see cref="AttributePassThroughIndexBasedParameterAttribute.AttributeId"/> should have the same value as <paramref name="attributeId"/>.</para>
        /// <para>When properties need to be set while initializing, marks the properties one by one using <see cref="AttributePassThroughPropertyAttribute"/>. The <see cref="AttributePassThroughPropertyAttribute.AttributeId"/> should have the same value as <paramref name="attributeId"/>.</para>
        /// <para>When fields need to be set while initializing, marks the fields one by one using <see cref="AttributePassThroughFieldAttribute"/>. The <see cref="AttributePassThroughFieldAttribute.AttributeId"/> should have the same value as <paramref name="attributeId"/>.</para>
        /// </remarks>
        public AttributePassThroughAttribute(Type attribute, Type[] attributeConstructorParameterTypes = null, object[] attributeConstructorParameters = null, string attributeId = null)
        {
            AttributeId = attributeId;
            Attribute = attribute;

            if (attributeConstructorParameterTypes == null)
                attributeConstructorParameterTypes = Type.EmptyTypes;
            AttributeConstructorParameterTypes = attributeConstructorParameterTypes;

            if (attributeConstructorParameters != null &&
                attributeConstructorParameters.Length > attributeConstructorParameterTypes.Length)
                throw new ArgumentException(nameof(attributeConstructorParameters),
                    $"The length can not exceed the length of {nameof(attributeConstructorParameterTypes)}.");
            AttributeConstructorParameters = attributeConstructorParameters;
        }
    }
}
