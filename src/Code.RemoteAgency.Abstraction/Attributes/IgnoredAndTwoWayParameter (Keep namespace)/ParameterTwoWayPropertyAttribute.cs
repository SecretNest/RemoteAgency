using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the parameter contains a settable property or field which value should be send back to the caller.
    /// </summary>
    /// <remarks><para>When a parameter contains properties or fields which may be changed on the target site and need to be sent back to the caller, use <see cref="ParameterTwoWayPropertyAttribute"/> on related properties.</para>
    /// <para>When a parameter marked with "ref / ByRef", the value of the parameter will be passed back to the caller. Due to lack of tracking information, regardless of whether this parameter contains changed properties or fields, the whole object will be transferred and replaced. If this is not the expected operation, use <see cref="ParameterTwoWayPropertyAttribute"/> on related properties and fields instead of marking "ref / ByRef".</para>
    /// <para><see cref="EventParameterTwoWayPropertyAttribute"/> can be marked on event, with higher priority than <see cref="ParameterTwoWayPropertyAttribute"/> with the same parameter.</para>
    /// <para>Without <see cref="EventParameterTwoWayPropertyAttribute"/> or <see cref="ParameterTwoWayPropertyAttribute"/> specified, properties will not be send back to the caller unless the parameter is marked with "ref / ByRef".</para>
    /// <para>By specifying this on properties, only set operating will be affected.</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public class ParameterTwoWayPropertyAttribute : Attribute
    {
        /// <summary>
        /// Gets whether this is in simple mode.
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        ///     <term>True</term>
        ///     <description><see cref="PropertyInParameter"/> is the name of property or field of the parameter entity.</description>
        /// </item>
        /// <item>
        ///     <term>False</term>
        ///     <description><see cref="PropertyInParameter"/> is the path, starts with ".", from the parameter entity.</description>
        /// </item>
        /// </list>
        /// </remarks>
        public bool IsSimpleMode { get; }

        /// <summary>
        /// Gets the property in the parameter.
        /// </summary>
        /// <remarks>
        /// Based on the value of <see cref="IsSimpleMode"/>:
        /// <list type="bullet">
        /// <item>
        ///     <term>True</term>
        ///     <description>The name of property or field of the parameter entity.</description>
        /// </item>
        /// <item>
        ///     <term>False</term>
        ///     <description>The path, starts with ".", from the parameter entity.</description>
        /// </item>
        /// </list>
        /// </remarks>
        public string PropertyInParameter { get; }

        /// <summary>
        /// Gets the type of the property or field specified by <see cref="PropertyInParameter"/>.
        /// </summary>
        /// <remarks>When <see cref="IsSimpleMode"/> is set to true, this value will be ignored.</remarks>
        public Type ElementType { get; }

        /// <summary>
        /// Gets the preferred property name in entity.
        /// </summary>
        /// <remarks>If it's set to null (default), the property name will be chosen automatically.</remarks>
        public string EntityPropertyName { get; }

        /// <summary>
        /// Gets whether this property should be included in return entity when exception thrown by the user code on the remote site.
        /// </summary>
        public bool IsIncludedWhenExceptionThrown { get; }

        /// <summary>
        /// Initializes an instance of the ParameterTwoWayPropertyAttribute. <see cref="IsSimpleMode"/> will be set to false.
        /// </summary>
        /// <param name="propertyPathInParameter">The path, starts with ".", from the parameter entity.</param>
        /// <param name="elementType">The type of the property or field specified by <see cref="PropertyInParameter"/>.</param>
        /// <param name="entityPropertyName">Preferred property name in entity. If it's set to null, the property name will be chosen automatically.</param>
        /// <param name="isIncludedWhenExceptionThrown">Whether this property should be included in return entity when exception thrown by the user code on the remote site. Default value is <see langword="false" />.</param>
        public ParameterTwoWayPropertyAttribute(string propertyPathInParameter, Type elementType, string entityPropertyName, bool isIncludedWhenExceptionThrown = false)
        {
            PropertyInParameter = propertyPathInParameter;
            ElementType = elementType;
            EntityPropertyName = entityPropertyName;
            IsSimpleMode = false;
            IsIncludedWhenExceptionThrown = isIncludedWhenExceptionThrown;
        }

        /// <summary>
        /// Initializes an instance of the ParameterTwoWayPropertyAttribute. <see cref="IsSimpleMode"/> will be set to true.
        /// </summary>
        /// <param name="propertyNameInParameter">The name of property or field of the parameter entity.</param>
        /// <param name="entityPropertyName">Preferred property name in entity. If it's set to null (default), the property name will be chosen automatically.</param>
        /// <param name="isIncludedWhenExceptionThrown">Whether this property should be included in return entity when exception thrown by the user code on the remote site. Default value is <see langword="false" />.</param>
        public ParameterTwoWayPropertyAttribute(string propertyNameInParameter, string entityPropertyName = null, bool isIncludedWhenExceptionThrown = false)
        {
            PropertyInParameter = propertyNameInParameter;
            EntityPropertyName = entityPropertyName;
            IsSimpleMode = true;
            IsIncludedWhenExceptionThrown = isIncludedWhenExceptionThrown;
        }
    }
}
