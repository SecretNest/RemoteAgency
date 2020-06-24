﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies an attribute should be marked at the same place of built proxy or service wrapper class.
    /// </summary>
    /// <remarks><para>Use this attribute to mark an attribute at the same place in the created class.</para>
    /// <para>When using the parameterless constructor of target attribute, <see cref="AttributeId"/>, <see cref="AttributeConstructorParameterTypes"/> and <see cref="AttributeConstructorParameters"/> are not required.</para>
    /// <para>When using a constructor with all parameters specified in orders, sets types of parameters of constructor to <see cref="AttributeConstructorParameterTypes"/> and values to <see cref="AttributeConstructorParameters"/>. When the length of <see cref="AttributeConstructorParameters"/> is smaller than the length of <see cref="AttributeConstructorParameterTypes"/>, the missing parameters will not be passed into the constructor. <see cref="AttributeId"/> is not required.</para>
    /// <para>When named parameter is required for constructing, sets types of full form parameters of constructor to <see cref="AttributeConstructorParameterTypes"/> and leading values to <see cref="AttributeConstructorParameters"/>. Marks the rest one by one using <see cref="AttributePassThroughIndexBasedParameterAttribute"/>. The <see cref="AttributePassThroughIndexBasedParameterAttribute.AttributeId"/> should have the same value as <see cref="AttributeId"/>.</para>
    /// <para>When properties need to be set while initializing, marks the properties one by one using <see cref="AttributePassThroughPropertyAttribute"/>. The <see cref="AttributePassThroughPropertyAttribute.AttributeId"/> should have the same value as <see cref="AttributeId"/>.</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Event | AttributeTargets.Method | AttributeTargets.Delegate | AttributeTargets.Property |
                    AttributeTargets.Parameter | AttributeTargets.GenericParameter | AttributeTargets.ReturnValue |
                    AttributeTargets.Interface, Inherited = true, AllowMultiple = true)]
    public class AttributePassThroughAttribute : Attribute
    {
        /// <summary>
        /// Gets the id of the instance of the attribute.
        /// </summary>
        /// <remarks>This id is designed for linking <see cref="AttributePassThroughAttribute"/> with <see cref="AttributePassThroughIndexBasedParameterAttribute"/> and <see cref="AttributePassThroughPropertyAttribute"/>. When no need to mark with <see cref="AttributePassThroughIndexBasedParameterAttribute"/> or <see cref="AttributePassThroughPropertyAttribute"/>, this value is optional.</remarks>
        public Guid? AttributeId { get; }

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
        /// <param name="attributeId"></param>
        public AttributePassThroughAttribute(Type attribute, Type[] attributeConstructorParameterTypes = null, object[] attributeConstructorParameters = null, Guid? attributeId = null)
        {
            AttributeId = attributeId;
            Attribute = attribute;
            if (attributeConstructorParameterTypes == null)
                attributeConstructorParameterTypes = new Type[0];
            AttributeConstructorParameterTypes = attributeConstructorParameterTypes;
            AttributeConstructorParameters = attributeConstructorParameters;
        }
    }
}
