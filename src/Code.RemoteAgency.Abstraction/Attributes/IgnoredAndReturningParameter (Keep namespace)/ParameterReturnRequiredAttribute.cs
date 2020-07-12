using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the parameter should be send back to the caller.
    /// </summary>
    /// <remark>
    /// <para>This attribute is only available for parameters marked with "ref / ByRef" and "out / Out". For sending a property within a parameter, use <see cref="EventParameterReturnRequiredPropertyAttribute"/> or <see cref="ParameterReturnRequiredPropertyAttribute"/> instead.</para>
    /// <para><see cref="EventParameterReturnRequiredAttribute"/> can be marked on event, with higher priority than <see cref="ParameterReturnRequiredAttribute"/> with the same parameter.</para>
    /// <para>When this attribute is absent, the value of the parameter will be send back to the caller only when no exception thrown.</para>
    /// <para>When <see cref="IsIncludedInReturning"/> is set as <see langword="false" />, or <see cref="IsIncludedWhenExceptionThrown"/> is set as <see langword="false" /> when exception thrown, the parameter will be set as default value when the parameter is marked with "out / Out", or stay untouched when parameter is marked with "ref / ByRef".</para>
    /// <para>By specifying this on properties, only set operating will be affected.</para>
    /// </remark>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#ParameterLevel" />
    [AttributeUsage(AttributeTargets.Parameter, Inherited = true, AllowMultiple = false)]
    public class ParameterReturnRequiredAttribute : Attribute
    {
        /// <summary>
        /// Gets whether this parameter should be included in return entity.
        /// </summary>
        public bool IsIncludedInReturning { get; }

        /// <summary>
        /// Gets whether this parameter should be included in return entity when exception thrown by the user code on the remote site. Only valid when <see cref="IsIncludedInReturning"/> is set as <see langword="true"/>.
        /// </summary>
        public bool IsIncludedWhenExceptionThrown { get; }

        /// <summary>
        /// Gets the preferred property name in response entity.
        /// </summary>
        /// <remark>When the value is <see langword="null"/> or empty string, name is chosen automatically.</remark>
        public string ResponseEntityPropertyName { get; }

        /// <summary>
        /// Initializes an instance of ParameterReturnRequiredAttribute.
        /// </summary>
        /// <param name="isIncludedInReturning">Whether this parameter should be included in return entity. Default value is <see langword="true" />.</param>
        /// <param name="isIncludedWhenExceptionThrown">Whether this parameter should be included in return entity when exception thrown by the user code on the remote site. Default value is <see langword="true" />.</param>
        /// <param name="responseEntityPropertyName">Preferred property name in response entity. When the value is <see langword="null"/> or empty string, name is chosen automatically. Default value is <see langword="null" />.</param>
        public ParameterReturnRequiredAttribute(bool isIncludedInReturning = true, bool isIncludedWhenExceptionThrown = true, string responseEntityPropertyName = null)
        {
            IsIncludedInReturning = isIncludedInReturning;
            IsIncludedWhenExceptionThrown = isIncludedWhenExceptionThrown;
            ResponseEntityPropertyName = responseEntityPropertyName;
        }
    }
}
