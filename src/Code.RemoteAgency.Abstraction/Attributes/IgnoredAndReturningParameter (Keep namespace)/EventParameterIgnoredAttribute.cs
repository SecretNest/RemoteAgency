using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies a parameter of the event should or should not be transferred to remote site.
    /// </summary>
    /// <remarks>
    /// <para><see cref="ParameterIgnoredAttribute"/> can be marked on parameters of the delegate related to this event, with lower priority than <see cref="EventParameterIgnoredAttribute"/>.</para>
    /// <para>When <see cref="ParameterIgnoredAttribute.IsIgnored"/> is set to <see langword="true"/>, <see cref="ParameterReturnRequiredAttribute"/>, <see cref="ParameterReturnRequiredPropertyAttribute"/>, <see cref="EventParameterReturnRequiredAttribute"/>, <see cref="EventParameterReturnRequiredPropertyAttribute"/>, <see cref="CustomizedEventParameterEntityPropertyNameAttribute"/> and <see cref="CustomizedParameterEntityPropertyNameAttribute"/> on or in the same parameter of the asset and the delegate related to the asset if the asset is an event, will be ignored.</para>
    /// <para>Without <see cref="EventParameterIgnoredAttribute"/> or <see cref="ParameterIgnoredAttribute"/> specified, no parameter is ignored.</para>
    /// </remarks>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#ParameterLevelEventOnly" />
    /// <conceptualLink target="168d9d48-771b-4912-9bcd-880f1d65c090" />
    [AttributeUsage(AttributeTargets.Event, Inherited = true, AllowMultiple = true)]
    public sealed class EventParameterIgnoredAttribute : ParameterIgnoredAttribute, IIsIgnored
    {
        /// <summary>
        /// Gets the parameter name of the event.
        /// </summary>
        public string ParameterName { get; }

        /// <summary>
        /// Initializes an instance of the EventParameterIgnoredAttribute.
        /// </summary>
        /// <param name="parameterName">Parameter name of the event.</param>
        /// <param name="isIgnored">Ignored from parameter. If set to <see langword="true"/>, this parameter should not be transferred to remote site.</param>
        public EventParameterIgnoredAttribute(string parameterName, bool isIgnored = true) : base(isIgnored)
        {
            ParameterName = parameterName;
        }
    }
}
