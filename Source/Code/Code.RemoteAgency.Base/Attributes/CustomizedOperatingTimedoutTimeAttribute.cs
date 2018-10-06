using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the time out setting of this asset.
    /// </summary>
    /// <remarks>
    /// Priority list:
    /// 1 Attributes specified in method, property and event.
    /// 2 Attributes specified in delegate related to event.
    /// 3 Attributes specified in interface.
    /// 4 Default value: -1 (Infinity)
    /// </remarks>
    /// <seealso cref="TimeoutException"/>
    /// <seealso cref="MessageWaitingTimedOutCallback"/>
    /// <seealso cref="ICommunicate{TSerialized}.MessageWaitingTimedOutCallback"/>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Interface, Inherited = true, AllowMultiple = false)]
    public class CustomizedOperatingTimedoutTimeAttribute : Attribute
    {
        /// <summary>
        /// The length of time for waiting response, in milliseconds, or the value -1 to indicate that the waiting does not time out.
        /// </summary>
        public int MillisecondsTimeout { get; }

        /// <summary>
        /// Initializes an instance of the CustomizedOperatingTimedoutTimeAttribute.
        /// </summary>
        /// <param name="millisecondsTimeout">The length of time for waiting response, in milliseconds, or the value -1 to indicate that the waiting does not time out.</param>
        public CustomizedOperatingTimedoutTimeAttribute(int millisecondsTimeout)
        {
            MillisecondsTimeout = millisecondsTimeout;
        }
    }
}
