using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies a parameter of the event contains a property or field which value should be send back to the caller.
    /// </summary>
    /// <remarks>
    /// <para>When a parameter contains properties or fields which may be changed on the target site and need to be sent back to the caller, use <see cref="EventParameterTwoWayPropertyAttribute"/> or <see cref="ParameterTwoWayPropertyAttribute"/> on related properties.</para>
    /// <para>When a parameter marked with "ref / ByRef", the value of the parameter will be passed back to the caller. Due to lack of tracking information, regardless of whether this parameter contains changed properties or fields, the whole object will be transferred and replaced. If this is not the expected operation, use <see cref="EventParameterTwoWayPropertyAttribute"/> or <see cref="ParameterTwoWayPropertyAttribute"/> on related properties and fields instead of marking "ref / ByRef".</para>
    /// <para><see cref="ParameterTwoWayPropertyAttribute"/> can be marked on parameters of the delegate related to this event, with lower priority than <see cref="EventParameterTwoWayPropertyAttribute"/>.</para>
    /// <para>Without <see cref="EventParameterTwoWayPropertyAttribute"/> or <see cref="ParameterTwoWayPropertyAttribute"/> specified, properties will not be send back to the caller unless the parameter is marked with "ref / ByRef".</para></remarks>
    [AttributeUsage(AttributeTargets.Event, Inherited = true, AllowMultiple = true)]
    public class EventParameterTwoWayPropertyAttribute : ParameterTwoWayPropertyAttribute
    {
        /// <summary>
        /// Gets the parameter name of the event.
        /// </summary>
        public string ParameterName { get; }

        /// <summary>
        /// Initializes an instance of the EventParameterTwoWayPropertyAttribute. <see cref="ParameterTwoWayPropertyAttribute.IsSimpleMode"/> will be set to <see langword="false"/>.
        /// </summary>
        /// <param name="parameterName">Parameter name of the event.</param>
        /// <param name="helperClass">Type of the helper class.</param>
        /// <param name="disable">Disable the function specified with this helper class.</param>
        /// <seealso cref="ParameterTwoWayPropertyAttribute.HelperClass"/>
        public EventParameterTwoWayPropertyAttribute(string parameterName, Type helperClass, bool disable = false) : base(helperClass, disable)
        {
            ParameterName = parameterName;
        }

        /// <summary>
        /// Initializes an instance of the EventParameterTwoWayPropertyAttribute. <see cref="ParameterTwoWayPropertyAttribute.IsSimpleMode"/> will be set to <see langword="true"/>.
        /// </summary>
        /// <param name="parameterName">Parameter name of the event.</param>
        /// <param name="propertyNameInParameter">The name of property or field of the parameter entity.</param>
        /// <param name="responseEntityPropertyName">Preferred property name in response entity. When the value is <see langword="null"/> or empty string, name is chosen automatically. Default value is <see langword="null" />.</param>
        /// <param name="isIncludedWhenExceptionThrown">Whether this property should be included in return entity when exception thrown by the user code on the remote site. Default value is <see langword="false" />.</param>
        /// <param name="disable">Disable the function specified with this property or field.</param>
        public EventParameterTwoWayPropertyAttribute(string parameterName, string propertyNameInParameter,
            string responseEntityPropertyName = null, bool isIncludedWhenExceptionThrown = false, bool disable = false) : base(
            propertyNameInParameter, responseEntityPropertyName, isIncludedWhenExceptionThrown, disable)
        {
            ParameterName = parameterName;
        }
    }
}
