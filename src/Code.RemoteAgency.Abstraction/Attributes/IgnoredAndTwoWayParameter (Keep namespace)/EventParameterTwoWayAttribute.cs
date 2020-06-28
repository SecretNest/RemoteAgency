using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies a parameter of the event should be send back to the caller.
    /// </summary>
    /// <remark>
    /// <para>This attribute is only available for parameters marked with "ref / ByRef" and "out / Out". For sending a property within a parameter, use <see cref="EventParameterTwoWayPropertyAttribute"/> or <see cref="ParameterTwoWayPropertyAttribute"/> instead.</para>
    /// <para><see cref="ParameterTwoWayAttribute"/> can be marked on parameters of the delegate related to this event, with lower priority than <see cref="EventParameterTwoWayAttribute"/>.</para>
    /// <para>When this attribute is absent, the value of the parameter will be send back to the caller only when no exception thrown.</para>
    /// <para>When <see cref="ParameterTwoWayAttribute.IsTwoWay"/> is set to <see langword="false" />, or <see cref="ParameterTwoWayAttribute.IsIncludedWhenExceptionThrown"/> is set to <see langword="false" /> when exception thrown, the parameter will be set to default value when the parameter is marked with "out / Out", or stay untouched when parameter is marked with "ref / ByRef".</para>
    /// </remark>
    [AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = true)]
    public class EventParameterTwoWayAttribute : ParameterTwoWayAttribute
    {
        /// <summary>
        /// Gets the parameter name of the event.
        /// </summary>
        public string ParameterName { get; }

        /// <summary>
        /// Initializes an instance of EventParameterTwoWayAttribute.
        /// </summary>
        /// <param name="parameterName">Parameter name of the event.</param>
        /// <param name="isTwoWay">Whether this parameter should be included in return entity. Default value is <see langword="true" />.</param>
        /// <param name="isIncludedWhenExceptionThrown">Whether this parameter should be included in return entity when exception thrown by the user code on the remote site. Default value is <see langword="true" />.</param>
        /// <param name="responseEntityPropertyName">Preferred property name in response entity. When the value is <see langword="null"/> or empty string, name is chosen automatically. Default value is <see langword="null" />.</param>
        public EventParameterTwoWayAttribute(string parameterName, bool isTwoWay = true,
            bool isIncludedWhenExceptionThrown = true, string responseEntityPropertyName = null) : base(isTwoWay, isIncludedWhenExceptionThrown, responseEntityPropertyName)
        {
            ParameterName = parameterName;
        }
    }
}
