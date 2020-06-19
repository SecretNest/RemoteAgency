using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the event subscription mode.
    /// </summary>
    /// <remarks>The default setting is <see cref="EventSubscriptionMode"/>.Dynamic if this attribute absents.</remarks>
    /// <seealso cref="EventSubscriptionMode"/>
    [AttributeUsage(AttributeTargets.Event, Inherited = true, AllowMultiple = false)]
    public class EventSubscriptionAttribute : Attribute
    {
        /// <summary>
        /// Gets the event subscription mode.
        /// </summary>
        public EventSubscriptionMode EventSubscriptionMode { get; }

        /// <summary>
        /// Initializes an instance of the EventSubscriptionAttribute.
        /// </summary>
        /// <param name="eventSubscriptionMode">Event subscription mode.</param>
        public EventSubscriptionAttribute(EventSubscriptionMode eventSubscriptionMode = EventSubscriptionMode.Dynamic)
        {
            EventSubscriptionMode = eventSubscriptionMode;
        }
    }

    /// <summary>
    /// Contains a list of event subscription mode.
    /// </summary>
    /// <seealso cref="EventSubscriptionAttribute"/>
    [DataContract(Namespace = "")]
    public enum EventSubscriptionMode
    {
        /// <summary>
        /// Subscribes the event on proxy initializing. Unsubscribes on disposing.
        /// </summary>
        [EnumMember]
        SubscribeOnStartAndKeep,
        /// <summary>
        /// Subscribes the event when the first handler is linked. Unsubscribes on disposing when subscribed before.
        /// </summary>
        [EnumMember]
        SubscribeOnFirstUseAndKeep,
        /// <summary>
        /// Subscribes the event when the first handler is linked. Unsubscribes on the last one is removed. This could happened repeatedly. Unsubscribing process is also called in disposing when the event is subscribed before.
        /// </summary>
        [EnumMember]
        Dynamic
    }
}
