using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the operating should be one way. No response is required.
    /// </summary>
    /// <remarks><para>If the operating required a result (method return, some parameter is set as ref, some parameter is set in two way, etc.), this attribute will be ignored.</para>
    /// <para>By specifying this, any exception raised from the user code on target site will not be transferred to the caller site.</para>
    /// <para>By specifying this on properties, only set operating will be affected. If one way get operating, which gets from a property but ignore the result and exception from the caller, is expected, mark the property with <see cref="CustomizedOneWayPropertyGetOperatingAttribute"/>.</para></remarks>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
    public class CustomizedOneWayOperatingAttribute : Attribute
    {
    }

    /// <summary>
    /// Specifies the property get operating should be one way. No response is required.
    /// </summary>
    /// <remarks><para>By specifying this on properties, only get operating will be affected. If one way set operating is expected, mark the property with <see cref="CustomizedOneWayOperatingAttribute"/>.</para></remarks>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class CustomizedOneWayPropertyGetOperatingAttribute : Attribute
    {
    }
}
