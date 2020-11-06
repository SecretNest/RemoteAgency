using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the operating should be one-way. No response is required.
    /// </summary>
    /// <remarks>
    /// <para>When <see cref="IsOneWay"/> is set as <see langword="true"/>, any output parameters and return value will always be set as default value due to lack of response, any exception raised from the user code on target site will not be transferred to the caller site.</para>
    /// <para>When <see cref="IsOneWay"/> is set as <see langword="true"/>, <see cref="ParameterReturnRequiredAttribute"/>, <see cref="ParameterReturnRequiredPropertyAttribute"/>, <see cref="EventParameterReturnRequiredAttribute"/>, <see cref="EventParameterReturnRequiredPropertyAttribute"/>, <see cref="ReturnIgnoredAttribute"/>, <see cref="CustomizedPropertyGetEntityNameAttribute.ResponseEntityName"/>, <see cref="CustomizedPropertyGetResponsePropertyNameAttribute"/>, <see cref="CustomizedPropertySetEntityNameAttribute.ResponseEntityName"/>, <see cref="CustomizedMethodEntityNameAttribute.ReturnValueEntityName"/>, <see cref="CustomizedEventEntityNameAttribute.RaisingFeedbackEntityName"/>, <see cref="CustomizedReturnValueEntityPropertyNameAttribute"/> and <see cref="OperatingTimeoutTimeAttribute"/> on or in the same asset, and the delegate related to the asset if the asset is an event, will be ignored.</para>
    /// <para>By specifying this on event, only event raising operating is affected. Event adding and removing are always return required.</para>
    /// <para>By specifying this on properties, only setting operating is affected. If one-way getting operating, which gets from a property but ignore the result and exception from the caller, is expected, mark the property with <see cref="PropertyGetOneWayOperatingAttribute"/>.</para>
    /// <para>The one marked on the event has higher priority than the one marked on the delegate of the same event.</para>
    /// <para>When this attribute is absent, the one-way mode of this asset other than property get operating is disabled.</para>
    /// </remarks>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#AssetLevel" />
    /// <conceptualLink target="e7b65736-b2df-4aa9-897a-3a3050d3cceb" />
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
    public sealed class AssetOneWayOperatingAttribute : Attribute
    {
        /// <summary>
        /// Gets whether the operating is one-way.
        /// </summary>
        public bool IsOneWay { get; }

        /// <summary>
        /// Initializes an instance of AssetOneWayOperatingAttribute.
        /// </summary>
        /// <param name="isOneWay">Whether the operating is one-way. Default value is <see langword="true"/>.</param>
        public AssetOneWayOperatingAttribute(bool isOneWay = true)
        {
            IsOneWay = isOneWay;
        }
    }
}
