using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the time out setting of this asset.
    /// </summary>
    /// <remarks>
    /// Priority:
    /// 1 Attributes specified in method, property and event.
    /// 2 Attributes specified in delegate related to event.
    /// 3 Attributes specified in interface.
    /// 4 Default value: set while adding proxy or service wrapper to the Remote Agency instance. Default value is -1 (Infinity).
    /// </remarks>
    /// <seealso cref="AccessingTimeOutException"/>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Interface, Inherited = true, AllowMultiple = false)]
    public class CustomizedOperatingTimeoutTimeAttribute : Attribute
    {
        /// <summary>
        /// Gets the length of time for waiting response, in milliseconds, or the value -1 to indicate that the waiting does not time out.
        /// </summary>
        public int MillisecondsTimeout { get; }

        /// <summary>
        /// Initializes an instance of the CustomizedOperatingTimeoutTimeAttribute.
        /// </summary>
        /// <param name="millisecondsTimeout">The length of time for waiting response, in milliseconds, or the value -1 to indicate that the waiting does not time out.</param>
        public CustomizedOperatingTimeoutTimeAttribute(int millisecondsTimeout)
        {
            MillisecondsTimeout = millisecondsTimeout;
        }
    }
}
