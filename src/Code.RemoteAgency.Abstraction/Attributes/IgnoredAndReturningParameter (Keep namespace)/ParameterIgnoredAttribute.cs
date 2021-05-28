using System;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the parameter should or should not be transferred to remote site. If this attribute absent, the default behavior is transferring all parameters.
    /// </summary>
    /// <remarks>
    /// <para><see cref="EventParameterIgnoredAttribute"/> can be marked on event, with higher priority than <see cref="ParameterIgnoredAttribute"/> with the same parameter.</para>
    /// <para>When <see cref="IsIgnored"/> is set to <see langword="true"/>, <see cref="ParameterReturnRequiredAttribute"/>, <see cref="ParameterReturnRequiredPropertyAttribute"/>, <see cref="EventParameterReturnRequiredAttribute"/>, <see cref="EventParameterReturnRequiredPropertyAttribute"/>, <see cref="CustomizedEventParameterEntityPropertyNameAttribute"/> and <see cref="CustomizedParameterEntityPropertyNameAttribute"/> on or in the same parameter of the asset and the delegate related to the asset if the asset is an event, will be ignored.</para>
    /// <para>Without <see cref="EventParameterIgnoredAttribute"/> or <see cref="ParameterIgnoredAttribute"/> specified, no parameter is ignored.</para>
    /// </remarks>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#ParameterLevel" />
    /// <conceptualLink target="168d9d48-771b-4912-9bcd-880f1d65c090" />
    [AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
#pragma warning disable CA1813 // Avoid unsealed attributes
    public class ParameterIgnoredAttribute : Attribute, IIsIgnored
#pragma warning restore CA1813 // Avoid unsealed attributes
    {
        /// <summary>
        /// Gets whether this parameter is excluded from parameter entity. If set to <see langword="true"/>, this parameter should not be transferred to remote site.
        /// </summary>
        public bool IsIgnored { get; }

        /// <summary>
        /// Initializes an instance of the ParameterIgnoredAttribute.
        /// </summary>
        /// <param name="isIgnored">Ignored from parameter. If set to <see langword="true"/>, this parameter should not be transferred to remote site.</param>
        public ParameterIgnoredAttribute(bool isIgnored = true)
        {
            IsIgnored = isIgnored;
        }
    }
}
