using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the parameter should be send back to the caller.
    /// </summary>
    /// <remark>
    /// <para>This attribute is only available for parameters marked with "ref / ByRef" and "out / Out". For sending a property within a parameter, use <see cref="EventParameterTwoWayPropertyAttribute"/> or <see cref="ParameterTwoWayPropertyAttribute"/> instead.</para>
    /// <para><see cref="EventParameterTwoWayAttribute"/> can be marked on event, with higher priority than <see cref="ParameterTwoWayAttribute"/> with the same parameter.</para>
    /// <para>When this attribute is absent, the value of the parameter will be send back to the caller only when no exception thrown.</para>
    /// <para>When <see cref="IsTwoWay"/> is set to <see langword="false" />, or <see cref="IsIncludedWhenExceptionThrown"/> is set to <see langword="false" /> when exception thrown, the parameter will be set to default value when the parameter is marked with "out / Out", or stay untouched when parameter is marked with "ref / ByRef".</para>
    /// <para>By specifying this on properties, only set operating will be affected.</para>
    /// </remark>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class ParameterTwoWayAttribute : Attribute
    {
        /// <summary>
        /// Gets whether this parameter should be included in return entity.
        /// </summary>
        public bool IsTwoWay { get; }

        /// <summary>
        /// Gets whether this parameter should be included in return entity when exception thrown by the user code on the remote site. Only valid when <see cref="IsTwoWay"/> is set to <see langword="true"/>.
        /// </summary>
        public bool IsIncludedWhenExceptionThrown { get; }

        /// <summary>
        /// Initializes an instance of ParameterTwoWayAttribute.
        /// </summary>
        /// <param name="isTwoWay">Whether this parameter should be included in return entity. Default value is <see langword="true" />.</param>
        /// <param name="isIncludedWhenExceptionThrown">Whether this parameter should be included in return entity when exception thrown by the user code on the remote site. Default value is <see langword="true" />.</param>
        public ParameterTwoWayAttribute(bool isTwoWay = true, bool isIncludedWhenExceptionThrown = true)
        {
            IsTwoWay = isTwoWay;
            IsIncludedWhenExceptionThrown = isIncludedWhenExceptionThrown;
        }
    }
}
