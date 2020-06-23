using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the property is a helper for accessing two way property of a parameter. 
    /// </summary>
    /// <remarks>The property should be public readable and writable with the same type as the property pointed.</remarks>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class TwoWayHelperAttribute : Attribute
    {
        /// <summary>
        /// Gets whether this parameter should be included in return entity.
        /// </summary>
        public bool IsTwoWay { get; }

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
        /// Initializes an instance of TwoWayHelperAttribute.
        /// </summary>
        /// <param name="isTwoWay">Whether this parameter should be included in return entity. Default value is <see langword="true" />.</param>
        /// <param name="responseEntityPropertyName">Preferred property name in response entity. When the value is <see langword="null"/> or empty string, name is chosen automatically.</param>
        /// <param name="isIncludedWhenExceptionThrown">Whether this property should be included in return entity when exception thrown by the user code on the remote site. Default value is <see langword="false" />.</param>
        public TwoWayHelperAttribute(bool isTwoWay = true, string responseEntityPropertyName = null, bool isIncludedWhenExceptionThrown = false)
        {
            IsTwoWay = isTwoWay;
            ResponseEntityPropertyName = responseEntityPropertyName;
            IsIncludedWhenExceptionThrown = isIncludedWhenExceptionThrown;
        }
    }
}