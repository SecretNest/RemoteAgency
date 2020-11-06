using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the property is a helper for accessing property of a parameter.
    /// </summary>
    /// <remarks>The property should be public readable and writable with the same type as the property pointed.</remarks>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#ParameterLevel" />
    /// <conceptualLink target="168d9d48-771b-4912-9bcd-880f1d65c090" />
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class ReturnRequiredPropertyHelperAttribute : Attribute
    {
        /// <summary>
        /// Gets whether this parameter should be included in return entity.
        /// </summary>
        public bool IsIncludedInReturning { get; }

        /// <summary>
        /// Gets the preferred property name in response entity.
        /// </summary>
        /// <remarks>When the value is <see langword="null"/> or empty string, name is chosen automatically.</remarks>
        public string ResponseEntityPropertyName { get; }

        /// <summary>
        /// Gets whether this property should be included in return entity when exception thrown by the user code on the remote site.
        /// </summary>
        public bool IsIncludedWhenExceptionThrown { get; }

        /// <summary>
        /// Initializes an instance of ReturnRequiredPropertyHelperAttribute.
        /// </summary>
        /// <param name="isIncludedInReturning">Whether this parameter should be included in return entity. Default value is <see langword="true" />.</param>
        /// <param name="responseEntityPropertyName">Preferred property name in response entity. When the value is <see langword="null"/> or empty string, name is chosen automatically.</param>
        /// <param name="isIncludedWhenExceptionThrown">Whether this property should be included in return entity when exception thrown by the user code on the remote site. Default value is <see langword="false" />.</param>
        public ReturnRequiredPropertyHelperAttribute(bool isIncludedInReturning = true, string responseEntityPropertyName = null, bool isIncludedWhenExceptionThrown = false)
        {
            IsIncludedInReturning = isIncludedInReturning;
            ResponseEntityPropertyName = responseEntityPropertyName;
            IsIncludedWhenExceptionThrown = isIncludedWhenExceptionThrown;
        }
    }
}