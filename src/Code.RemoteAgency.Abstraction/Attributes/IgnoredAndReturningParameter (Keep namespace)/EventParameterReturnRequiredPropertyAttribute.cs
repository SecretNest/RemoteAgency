using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies a property or field which value should be send back to the caller is contained in a parameter of the event.
    /// </summary>
    /// <remarks>
    /// <para>When a parameter contains properties or fields which may be changed on the target site and need to be sent back to the caller, use <see cref="EventParameterReturnRequiredPropertyAttribute"/> or <see cref="ParameterReturnRequiredPropertyAttribute"/> on related properties.</para>
    /// <para>When a parameter marked with "ref / ByRef", the value of the parameter will be passed back to the caller. Due to lack of tracking information, regardless of whether this parameter contains changed properties or fields, the whole object will be transferred and replaced. If this is not the expected operation, use <see cref="EventParameterReturnRequiredPropertyAttribute"/> or <see cref="ParameterReturnRequiredPropertyAttribute"/> on related properties and fields instead of marking "ref / ByRef".</para>
    /// <para><see cref="ParameterReturnRequiredPropertyAttribute"/> can be marked on parameters of the delegate related to this event, with lower priority than <see cref="EventParameterReturnRequiredPropertyAttribute"/>.</para>
    /// <para>Without <see cref="EventParameterReturnRequiredPropertyAttribute"/> or <see cref="ParameterReturnRequiredPropertyAttribute"/> specified, properties will not be send back to the caller unless the parameter is marked with "ref / ByRef" or "out / Out".</para>
    /// <para>This attribute can only be marked for the parameter without "ref / ByRef" and "out / Out".</para>
    /// </remarks>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#ParameterLevelEventOnly" />
    /// <conceptualLink target="168d9d48-771b-4912-9bcd-880f1d65c090" />
    [AttributeUsage(AttributeTargets.Event, Inherited = true, AllowMultiple = true)]
    public class EventParameterReturnRequiredPropertyAttribute : ParameterReturnRequiredPropertyAttribute
    {
        /// <summary>
        /// Gets the parameter name of the event.
        /// </summary>
        public string ParameterName { get; }

        /// <summary>
        /// Initializes an instance of the EventParameterReturnRequiredPropertyAttribute. <see cref="ParameterReturnRequiredPropertyAttribute.IsSimpleMode"/> will be set as <see langword="false"/>.
        /// </summary>
        /// <param name="parameterName">Parameter name of the event.</param>
        /// <param name="helperClass">Type of the helper class.</param>
        /// <param name="isIncludedInReturning">Whether this helper class should be processed in return entity. Default value is <see langword="true" />.</param>
        /// <seealso cref="ParameterReturnRequiredPropertyAttribute.HelperClass"/>
        public EventParameterReturnRequiredPropertyAttribute(string parameterName, Type helperClass, bool isIncludedInReturning = true) : base(helperClass, isIncludedInReturning)
        {
            ParameterName = parameterName;
        }

        /// <summary>
        /// Initializes an instance of the EventParameterReturnRequiredPropertyAttribute. <see cref="ParameterReturnRequiredPropertyAttribute.IsSimpleMode"/> will be set as <see langword="true"/>.
        /// </summary>
        /// <param name="parameterName">Parameter name of the event.</param>
        /// <param name="propertyNameInParameter">The name of property or field of the parameter entity.</param>
        /// <param name="responseEntityPropertyName">Preferred property name in response entity. When the value is <see langword="null"/> or empty string, name is chosen automatically. Default value is <see langword="null" />.</param>
        /// <param name="isIncludedWhenExceptionThrown">Whether this property should be included in return entity when exception thrown by the user code on the remote site. Default value is <see langword="false" />.</param>
        /// <param name="isIncludedInReturning">Whether this property or field should be included in return entity. Default value is <see langword="true" />.</param>
        public EventParameterReturnRequiredPropertyAttribute(string parameterName, string propertyNameInParameter,
            string responseEntityPropertyName = null, bool isIncludedWhenExceptionThrown = false, bool isIncludedInReturning = true) : base(
            propertyNameInParameter, responseEntityPropertyName, isIncludedWhenExceptionThrown, isIncludedInReturning)
        {
            ParameterName = parameterName;
        }
    }
}
