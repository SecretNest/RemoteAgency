using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies a parameter of the event should be send back to the caller.
    /// </summary>
    /// <remark>
    /// <para>This attribute is only available for parameters marked with "ref / ByRef" and "out / Out". For sending a property within a parameter, use <see cref="EventParameterReturnRequiredPropertyAttribute"/> or <see cref="ParameterReturnRequiredPropertyAttribute"/> instead.</para>
    /// <para><see cref="ParameterReturnRequiredAttribute"/> can be marked on parameters of the delegate related to this event, with lower priority than <see cref="EventParameterReturnRequiredAttribute"/>.</para>
    /// <para>When this attribute is absent, the value of the parameter will be send back to the caller only when no exception thrown.</para>
    /// <para>When <see cref="ParameterReturnRequiredAttribute.IsIncludedInReturning"/> is set as <see langword="false" />, or <see cref="ParameterReturnRequiredAttribute.IsIncludedWhenExceptionThrown"/> is set as <see langword="false" /> when exception thrown, the parameter will be set as default value when the parameter is marked with "out / Out", or stay untouched when parameter is marked with "ref / ByRef".</para>
    /// </remark>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#ParameterLevelEventOnly" />
    /// <conceptualLink target="168d9d48-771b-4912-9bcd-880f1d65c090" />
    [AttributeUsage(AttributeTargets.Event, Inherited = true, AllowMultiple = true)]
    public sealed class EventParameterReturnRequiredAttribute : ParameterReturnRequiredAttribute
    {
        /// <summary>
        /// Gets the parameter name of the event.
        /// </summary>
        public string ParameterName { get; }

        /// <summary>
        /// Initializes an instance of EventParameterReturnRequiredAttribute.
        /// </summary>
        /// <param name="parameterName">Parameter name of the event.</param>
        /// <param name="isIncludedInReturning">Whether this parameter should be included in return entity. Default value is <see langword="true" />.</param>
        /// <param name="isIncludedWhenExceptionThrown">Whether this parameter should be included in return entity when exception thrown by the user code on the remote site. Default value is <see langword="true" />.</param>
        /// <param name="responseEntityPropertyName">Preferred property name in response entity. When the value is <see langword="null"/> or empty string, name is chosen automatically. Default value is <see langword="null" />.</param>
        public EventParameterReturnRequiredAttribute(string parameterName, bool isIncludedInReturning = true,
            bool isIncludedWhenExceptionThrown = true, string responseEntityPropertyName = null) : base(isIncludedInReturning, isIncludedWhenExceptionThrown, responseEntityPropertyName)
        {
            ParameterName = parameterName;
        }
    }
}
