using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the operating should be one way. No response is required.
    /// </summary>
    /// <remarks>
    /// <para>When <see cref="IsOneWay"/> is set to <see langword="true"/>, any output parameters and return value will always be set to default value due to lack of response, any exception raised from the user code on target site will not be transferred to the caller site.</para>
    /// <para>When <see cref="IsOneWay"/> is set to <see langword="true"/>, <see cref="ParameterTwoWayAttribute"/>, <see cref="ParameterTwoWayPropertyAttribute"/>, <see cref="EventParameterTwoWayAttribute"/>, <see cref="EventParameterTwoWayPropertyAttribute"/>, <see cref="ReturnIgnoredAttribute"/>, <see cref="CustomizedPropertyGetResponseEntityNameAttribute"/>, <see cref="CustomizedPropertyGetResponsePropertyNameAttribute"/>, <see cref="CustomizedPropertySetResponseEntityNameAttribute"/>, <see cref="CustomizedReturnEntityNameAttribute"/>, <see cref="CustomizedEventEntityNameAttribute.RaisingFeedbackEntityName"/>, <see cref="CustomizedReturnEntityPropertyNameAttribute"/> and <see cref="OperatingTimeoutTimeAttribute"/> on or in the same asset, and the delegate related to the asset if the asset is an event, will be ignored.</para>
    /// <para>By specifying this on event, only event raising operating is affected. Event adding and removing are always two way.</para>
    /// <para>By specifying this on properties, only setting operating is affected. If one way getting operating, which gets from a property but ignore the result and exception from the caller, is expected, mark the property with <see cref="PropertyGetOneWayOperatingAttribute"/>.</para>
    /// <para>The one marked on the event has higher priority than the one marked on the delegate of the same event.</para>
    /// <para>When this attribute is absent, the one way mode of this asset other than property get operating is disabled.</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
    public class AssetOneWayOperatingAttribute : Attribute
    {
        /// <summary>
        /// Gets whether the operating is one way.
        /// </summary>
        public bool IsOneWay { get; }

        /// <summary>
        /// Initializes an instance of AssetOneWayOperatingAttribute.
        /// </summary>
        /// <param name="isOneWay">Whether the operating is one way. Default value is true.</param>
        public AssetOneWayOperatingAttribute(bool isOneWay = true)
        {
            IsOneWay = isOneWay;
        }
    }
}
