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
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
    public class CustomizedOneWayOperatingAttribute : Attribute
    {
    }
}
