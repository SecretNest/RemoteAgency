using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies a property or field which value should be send back to the caller is contained in the parameter.
    /// </summary>
    /// <remarks>
    /// <para>When a parameter contains properties or fields which may be changed on the target site and need to be sent back to the caller, use <see cref="ParameterReturnRequiredPropertyAttribute"/> on related properties.</para>
    /// <para>When a parameter marked with "ref / ByRef", the value of the parameter will be passed back to the caller. Due to lack of tracking information, regardless of whether this parameter contains changed properties or fields, the whole object will be transferred and replaced. If this is not the expected operation, use <see cref="ParameterReturnRequiredPropertyAttribute"/> on related properties and fields instead of marking "ref / ByRef".</para>
    /// <para><see cref="EventParameterReturnRequiredPropertyAttribute"/> can be marked on event, with higher priority than <see cref="ParameterReturnRequiredPropertyAttribute"/> with the same parameter.</para>
    /// <para>Without <see cref="EventParameterReturnRequiredPropertyAttribute"/> or <see cref="ParameterReturnRequiredPropertyAttribute"/> specified, properties will not be send back to the caller unless the parameter is marked with "ref / ByRef" or "out / Out".</para>
    /// <para>This attribute can only be marked on the parameter without "ref / ByRef" and "out / Out".</para>
    /// <para>By specifying this on properties, only set operating will be affected.</para>
    /// </remarks>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#ParameterLevel" />
    /// <conceptualLink target="168d9d48-771b-4912-9bcd-880f1d65c090" />
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
#pragma warning disable CA1813 // Avoid unsealed attributes
    public class ParameterReturnRequiredPropertyAttribute : Attribute
#pragma warning restore CA1813 // Avoid unsealed attributes
    {
        /// <summary>
        /// Gets whether properties or fields specified should be included in return entity.
        /// </summary>
        public bool IsIncludedInReturning { get; }

        /// <summary>
        /// Gets whether this is in simple mode.
        /// </summary>
        /// <remarks>When the value is <see langword="true"/>, property specified by <see cref="PropertyNameInParameter"/> need to be sent back to the caller; when the value is <see langword="false"/>, properties marked with <see cref="ReturnRequiredPropertyHelperAttribute"/> and <see cref="ReturnRequiredPropertyHelperAttribute.IsIncludedInReturning"/> set to <see langword="true"/> are used as the helper fow return required property accessing.</remarks>
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
        /// <para>The helper class should have a public constructor with one parameter in the same type of the parameter marked with this attribute. All properties in the helper class marked with <see cref="ReturnRequiredPropertyHelperAttribute"/> and <see cref="ReturnRequiredPropertyHelperAttribute.IsIncludedInReturning"/> set to <see langword="true"/> are used as the helper fow return required property accessing.</para></remarks>
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
        /// Initializes an instance of the ParameterReturnRequiredPropertyAttribute. <see cref="IsSimpleMode"/> will be set to <see langword="false"/>.
        /// </summary>
        /// <param name="helperClass">Type of the helper class.</param>
        /// <param name="isIncludedInReturning">Whether this helper class should be processed in return entity. Default value is <see langword="true" />.</param>
        /// <seealso cref="HelperClass"/>
        public ParameterReturnRequiredPropertyAttribute(Type helperClass, bool isIncludedInReturning = true)
        {
            HelperClass = helperClass;
            IsSimpleMode = false;
            IsIncludedInReturning = isIncludedInReturning;
        }

        /// <summary>
        /// Initializes an instance of the ParameterReturnRequiredPropertyAttribute. <see cref="IsSimpleMode"/> will be set to <see langword="true"/>.
        /// </summary>
        /// <param name="propertyNameInParameter">The name of property or field of the parameter entity.</param>
        /// <param name="responseEntityPropertyName">Preferred property name in response entity. When the value is <see langword="null"/> or empty string, name is chosen automatically. Default value is <see langword="null" />.</param>
        /// <param name="isIncludedWhenExceptionThrown">Whether this property should be included in return entity when exception thrown by the user code on the remote site. Default value is <see langword="false" />.</param>
        /// <param name="isIncludedInReturning">Whether this property or field should be included in return entity. Default value is <see langword="true" />.</param>
        public ParameterReturnRequiredPropertyAttribute(string propertyNameInParameter, string responseEntityPropertyName = null, bool isIncludedWhenExceptionThrown = false, bool isIncludedInReturning = true)
        {
            PropertyNameInParameter = propertyNameInParameter;
            ResponseEntityPropertyName = responseEntityPropertyName;
            IsSimpleMode = true;
            IsIncludedWhenExceptionThrown = isIncludedWhenExceptionThrown;
            IsIncludedInReturning = isIncludedInReturning;
        }
    }
}
