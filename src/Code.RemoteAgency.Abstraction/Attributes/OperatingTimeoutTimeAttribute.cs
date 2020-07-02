using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the time out setting of this asset.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Priority:
    /// 1 Attributes specified in method, property and event.
    /// 2 Attributes specified in delegate related to event.
    /// 3 Attributes specified in interface.
    /// 4 Default value: set while building.
    /// 5 Default value: set while initializing. In this mode, only one value for all kinds of operations is set.
    /// </para>
    /// <para>Set value to 0 to use the value from lower priority position.</para>
    /// </remarks>
    /// <seealso cref="AccessingTimeOutException"/>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
    public class OperatingTimeoutTimeAttribute : Attribute
    {
        /// <summary>
        /// Gets the length of time for waiting response, in milliseconds; or -1 to indicate that the waiting does not time out.
        /// </summary>
        public int Timeout { get; }

        /// <summary>
        /// Gets the chosen mode for timeout setting.
        /// </summary>
        public OperatingTimeoutTimeMode Mode { get; }

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
            Mode = OperatingTimeoutTimeMode.Default;
            //Timeout = 0;
            //EventAddingTimeout = 0;
            //EventRemovingTimeout = 0;
            //EventRaisingTimeout = 0;
            //PropertyGettingTimeout = 0;
            //PropertySettingTimeout = 0;
        }

        /// <summary>
        /// Initializes an instance of the OperatingTimeoutTimeAttribute with simple mode.
        /// </summary>
        /// <param name="timeout">The length of time for waiting response, in milliseconds; or -1 to indicate that the waiting does not time out.</param>
        /// <remarks><para>All timeout settings will be set as the value specified by <paramref name="timeout"/>.</para>
        /// <para>To set the timeout for event adding, removing and raising separately, uses <see cref="OperatingTimeoutTimeAttribute(int, int, int)"/> instead.</para>
        /// <para>To set the timeout for property getting and setting separately, uses <see cref="OperatingTimeoutTimeAttribute(int, int)"/> instead.</para>
        /// </remarks>
        public OperatingTimeoutTimeAttribute(int timeout)
        {
            Mode = OperatingTimeoutTimeMode.Simple;
            Timeout = timeout;
            EventAddingTimeout = timeout;
            EventRemovingTimeout = timeout;
            EventRaisingTimeout = timeout;
            PropertyGettingTimeout = timeout;
            PropertySettingTimeout = timeout;
        }

        /// <summary>
        /// Initializes an instance of the OperatingTimeoutTimeAttribute with event mode.
        /// </summary>
        /// <param name="eventAddingTimeout">The length of time for waiting response for event adding, in milliseconds; or -1 to indicate that the waiting does not time out.</param>
        /// <param name="eventRemovingTimeout">The length of time for waiting response for event removing, in milliseconds; or -1 to indicate that the waiting does not time out.</param>
        /// <param name="eventRaisingTimeout">The length of time for waiting response for event raising, in milliseconds; or -1 to indicate that the waiting does not time out.</param>
        /// <remarks><para>This constructor is for setting the timeout for event adding, removing and raising separately only. To set to the same value, or set for asset other than event, uses <see cref="OperatingTimeoutTimeAttribute(int)"/> instead.</para>
        /// <para>To set the timeout for property getting and setting separately, uses <see cref="OperatingTimeoutTimeAttribute(int, int)"/> instead.</para>
        /// </remarks>
        public OperatingTimeoutTimeAttribute(int eventAddingTimeout, int eventRemovingTimeout, int eventRaisingTimeout)
        {
            Mode = OperatingTimeoutTimeMode.Event;
            //Timeout = 0;
            EventAddingTimeout = eventAddingTimeout;
            EventRemovingTimeout = eventRemovingTimeout;
            EventRaisingTimeout = eventRaisingTimeout;
            //PropertyGettingTimeout = 0;
            //PropertySettingTimeout = 0;
        }
        
        /// <summary>
        /// Initializes an instance of the OperatingTimeoutTimeAttribute with property mode.
        /// </summary>
        /// <param name="propertyGettingTimeout">The length of time for waiting response for property getting, in milliseconds; or -1 to indicate that the waiting does not time out.</param>
        /// <param name="propertySettingTimeout">The length of time for waiting response for property setting, in milliseconds; or -1 to indicate that the waiting does not time out.</param>
        /// <remarks><para>This constructor is for setting the timeout for property getting and setting separately only. To set to the same value, or set for asset other than property, uses <see cref="OperatingTimeoutTimeAttribute(int)"/> instead.</para>
        /// <para>To set the timeout for event adding, removing and raising separately, uses <see cref="OperatingTimeoutTimeAttribute(int, int, int)"/> instead.</para>
        /// </remarks>
        public OperatingTimeoutTimeAttribute(int propertyGettingTimeout, int propertySettingTimeout)
        {
            Mode = OperatingTimeoutTimeMode.Property;
            //Timeout = 0;
            //EventAddingTimeout = 0;
            //EventRemovingTimeout = 0;
            //EventRaisingTimeout = 0;
            PropertyGettingTimeout = propertyGettingTimeout;
            PropertySettingTimeout = propertySettingTimeout;
        }
    }

    /// <summary>
    /// Mode choosing for timeout setting.
    /// </summary>
    public enum OperatingTimeoutTimeMode
    {
        /// <summary>
        /// Uses default set by Remote Agency instance.
        /// </summary>
        Default,
        /// <summary>
        /// Uses the value set by <see cref="OperatingTimeoutTimeAttribute.Timeout"/>.
        /// </summary>
        Simple,
        /// <summary>
        /// Uses the value set by 
        /// </summary>
        Event,
        /// <summary>
        /// 
        /// </summary>
        Property
    }
}
