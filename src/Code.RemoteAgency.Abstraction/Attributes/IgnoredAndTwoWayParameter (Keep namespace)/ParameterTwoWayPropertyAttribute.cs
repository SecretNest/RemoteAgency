using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the parameter contains a property or field which value should be send back to the caller.
    /// </summary>
    /// <remarks>
    /// <para>When a parameter contains properties or fields which may be changed on the target site and need to be sent back to the caller, use <see cref="ParameterTwoWayPropertyAttribute"/> on related properties.</para>
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
        /// <remarks>When the value is <see langword="true"/>, property specified by <see cref="PropertyNameInParameter"/> need to be sent back to the caller; when the value is <see langword="false"/>, properties marked with <see cref="TwoWayHelperAttribute"/> and <see cref="TwoWayHelperAttribute.IsTwoWay"/> set to <see langword="true"/> are used as the helper fow two way property accessing.</remarks>
        public bool IsSimpleMode { get; }

        /// <summary>
        /// Gets the property name in the parameter which need to be sent back to the caller.
        /// </summary>
        /// <remarks>Only valid when <see cref="IsSimpleMode"/> is set to <see langword="true"/>.</remarks>
        public string PropertyNameInParameter { get; }

        /// <summary>
        /// Gets the type of the helper class.
        /// </summary>
        /// <remarks><para>Only valid when <see cref="IsSimpleMode"/> is set to <see langword="false"/>.</para>
        /// <para>The helper class should have a public constructor with one parameter in the same type of the parameter marked with this attribute. All properties in the helper class marked with <see cref="TwoWayHelperAttribute"/> and <see cref="TwoWayHelperAttribute.IsTwoWay"/> set to <see langword="true"/> are used as the helper fow two way property accessing.</para></remarks>
        public Type HelperClass { get; }

        /// <summary>
        /// Gets the preferred property name in response entity.
        /// </summary>
        /// <remarks><para>Only valid when <see cref="IsSimpleMode"/> is set to <see langword="false"/>.</para>
        /// <para>When the value is <see langword="null"/> or empty string, name is chosen automatically.</para></remarks>
        public string ResponseEntityPropertyName { get; }

        /// <summary>
        /// Gets whether this property should be included in return entity when exception thrown by the user code on the remote site.
        /// </summary>
        /// <remarks>Only valid when <see cref="IsSimpleMode"/> is set to <see langword="false"/>.</remarks>
        public bool IsIncludedWhenExceptionThrown { get; }

        /// <summary>
        /// Initializes an instance of the ParameterTwoWayPropertyAttribute. <see cref="IsSimpleMode"/> will be set to <see langword="false"/>.
        /// </summary>
        /// <param name="helperClass">Type of the helper class.</param>
        /// <seealso cref="HelperClass"/>
        public ParameterTwoWayPropertyAttribute(Type helperClass)
        {
            HelperClass = helperClass;
            IsSimpleMode = false;
        }

        /// <summary>
        /// Initializes an instance of the ParameterTwoWayPropertyAttribute. <see cref="IsSimpleMode"/> will be set to <see langword="true"/>.
        /// </summary>
        /// <param name="propertyNameInParameter">The name of property or field of the parameter entity.</param>
        /// <param name="responseEntityPropertyName">Preferred property name in response entity. When the value is <see langword="null"/> or empty string, name is chosen automatically.</param>
        /// <param name="isIncludedWhenExceptionThrown">Whether this property should be included in return entity when exception thrown by the user code on the remote site. Default value is <see langword="false" />.</param>
        public ParameterTwoWayPropertyAttribute(string propertyNameInParameter, string responseEntityPropertyName = null, bool isIncludedWhenExceptionThrown = false)
        {
            PropertyNameInParameter = propertyNameInParameter;
            ResponseEntityPropertyName = responseEntityPropertyName;
            IsSimpleMode = true;
            IsIncludedWhenExceptionThrown = isIncludedWhenExceptionThrown;
        }
    }
}
