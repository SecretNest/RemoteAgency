using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the timeout settings of this asset.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Priority:
    /// 1 Attributes specified on method, property and event.
    /// 2 Attributes specified on delegate related to event.
    /// 3 Attributes specified on interface.
    /// 4 Default value: set while building.
    /// 5 Default value: set while initializing. In this mode, only one value for all kinds of operations is set.
    /// </para>
    /// <para>Set value to 0 to use the value from lower priority position.</para>
    /// </remarks>
    /// <seealso cref="AccessingTimeOutException"/>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#InterfaceLevel" />
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
    public class OperatingTimeoutTimeAttribute : Attribute
    {
        /// <summary>
        /// Gets the length of time for waiting response, in milliseconds; or -1 to indicate that the waiting does not time out.
        /// </summary>
        public int Timeout { get; }

        /// <summary>
        /// Gets the length of time for waiting response for event adding, in milliseconds; or -1 to indicate that the waiting does not time out.
        /// </summary>
        public int EventAddingTimeout { get; }

        /// <summary>
        /// Gets the length of time for waiting response for event removing, in milliseconds; or -1 to indicate that the waiting does not time out.
        /// </summary>
        public int EventRemovingTimeout { get; }

        /// <summary>
        /// Gets the length of time for waiting response for event raising, in milliseconds; or -1 to indicate that the waiting does not time out.
        /// </summary>
        public int EventRaisingTimeout { get; }

        /// <summary>
        /// Gets the length of time for waiting response for property getting, in milliseconds; or -1 to indicate that the waiting does not time out.
        /// </summary>
        public int PropertyGettingTimeout { get; }

        /// <summary>
        /// Gets the length of time for waiting response for property setting, in milliseconds; or -1 to indicate that the waiting does not time out.
        /// </summary>
        public int PropertySettingTimeout { get; }

        /// <summary>
        /// Initializes an instance of the OperatingTimeoutTimeAttribute to use the default value.
        /// </summary>
        /// <remarks>To set specific value or values, uses other constructors.</remarks>
        public OperatingTimeoutTimeAttribute()
        {
            //Timeout = 0;
            //EventAddingTimeout = 0;
            //EventRemovingTimeout = 0;
            //EventRaisingTimeout = 0;
            //PropertyGettingTimeout = 0;
            //PropertySettingTimeout = 0;
        }

        /// <summary>
        /// Initializes an instance of the OperatingTimeoutTimeAttribute with only one value for all waiting time.
        /// </summary>
        /// <param name="timeout">The length of time for waiting response, in milliseconds; or -1 to indicate that the waiting does not time out.</param>
        /// <remarks><para>All timeout settings will be set as the value specified by <paramref name="timeout"/>.</para>
        /// </remarks>
        public OperatingTimeoutTimeAttribute(int timeout)
        {
            Timeout = timeout;
            EventAddingTimeout = timeout;
            EventRemovingTimeout = timeout;
            EventRaisingTimeout = timeout;
            PropertyGettingTimeout = timeout;
            PropertySettingTimeout = timeout;
        }

        /// <summary>
        /// Initializes an instance of the OperatingTimeoutTimeAttribute with settings dedicated for event.
        /// </summary>
        /// <param name="eventAddingTimeout">The length of time for waiting response for event adding, in milliseconds; or -1 to indicate that the waiting does not time out.</param>
        /// <param name="eventRemovingTimeout">The length of time for waiting response for event removing, in milliseconds; or -1 to indicate that the waiting does not time out.</param>
        /// <param name="eventRaisingTimeout">The length of time for waiting response for event raising, in milliseconds; or -1 to indicate that the waiting does not time out.</param>
        /// <remarks><para>This constructor is for setting the timeout for event adding, removing and raising separately only. To set as the same value, or set for asset other than event, uses <see cref="OperatingTimeoutTimeAttribute(int)"/> instead.</para>
        /// </remarks>
        public OperatingTimeoutTimeAttribute(int eventAddingTimeout, int eventRemovingTimeout, int eventRaisingTimeout)
        {
            //Timeout = 0;
            EventAddingTimeout = eventAddingTimeout;
            EventRemovingTimeout = eventRemovingTimeout;
            EventRaisingTimeout = eventRaisingTimeout;
            //PropertyGettingTimeout = 0;
            //PropertySettingTimeout = 0;
        }
        
        /// <summary>
        /// Initializes an instance of the OperatingTimeoutTimeAttribute with settings dedicated for property.
        /// </summary>
        /// <param name="propertyGettingTimeout">The length of time for waiting response for property getting, in milliseconds; or -1 to indicate that the waiting does not time out.</param>
        /// <param name="propertySettingTimeout">The length of time for waiting response for property setting, in milliseconds; or -1 to indicate that the waiting does not time out.</param>
        /// <remarks><para>This constructor is for setting the timeout for property getting and setting separately only. To set as the same value, or set for asset other than property, uses <see cref="OperatingTimeoutTimeAttribute(int)"/> instead.</para>
        /// </remarks>
        public OperatingTimeoutTimeAttribute(int propertyGettingTimeout, int propertySettingTimeout)
        {
            //Timeout = 0;
            //EventAddingTimeout = 0;
            //EventRemovingTimeout = 0;
            //EventRaisingTimeout = 0;
            PropertyGettingTimeout = propertyGettingTimeout;
            PropertySettingTimeout = propertySettingTimeout;
        }
        /// <summary>
        /// Initializes an instance of the OperatingTimeoutTimeAttribute with all settings specified.
        /// </summary>
        /// <param name="methodCallingTimeout">The length of time for waiting response for method calling, in milliseconds; or -1 to indicate that the waiting does not time out.</param>
        /// <param name="eventAddingTimeout">The length of time for waiting response for event adding, in milliseconds; or -1 to indicate that the waiting does not time out.</param>
        /// <param name="eventRemovingTimeout">The length of time for waiting response for event removing, in milliseconds; or -1 to indicate that the waiting does not time out.</param>
        /// <param name="eventRaisingTimeout">The length of time for waiting response for event raising, in milliseconds; or -1 to indicate that the waiting does not time out.</param>
        /// <param name="propertyGettingTimeout">The length of time for waiting response for property getting, in milliseconds; or -1 to indicate that the waiting does not time out.</param>
        /// <param name="propertySettingTimeout">The length of time for waiting response for property setting, in milliseconds; or -1 to indicate that the waiting does not time out.</param>
        /// <remarks>This constructor is designed for interface level which need to specify timeout for all kinds of assets.</remarks>
        public OperatingTimeoutTimeAttribute(int methodCallingTimeout, int eventAddingTimeout, int eventRemovingTimeout, int eventRaisingTimeout, int propertyGettingTimeout, int propertySettingTimeout)
        {
            Timeout = methodCallingTimeout;
            EventAddingTimeout = eventAddingTimeout;
            EventRemovingTimeout = eventRemovingTimeout;
            EventRaisingTimeout = eventRaisingTimeout;
            PropertyGettingTimeout = propertyGettingTimeout;
            PropertySettingTimeout = propertySettingTimeout;
        }
    }
}
