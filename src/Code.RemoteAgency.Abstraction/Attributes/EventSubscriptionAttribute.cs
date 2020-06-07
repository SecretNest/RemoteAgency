using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the event subscription mode.
    /// </summary>
    /// <remarks>The default setting is <see cref="EventSubscritionMode.Dynamic"/> if this attribute absents.</remarks>
    /// <seealso cref="EventSubscritionMode"/>
    [AttributeUsage(AttributeTargets.Event, Inherited = true, AllowMultiple = false)]
    public class EventSubscriptionAttribute : Attribute
    {
        /// <summary>
        /// Event subscription mode.
        /// </summary>
        public EventSubscritionMode EventSubscritionMode { get; }

        /// <summary>
        /// Initializes an instance of the EventSubscriptionAttribute.
        /// </summary>
        /// <param name="eventSubscritionMode">Event subscription mode.</param>
        public EventSubscriptionAttribute(EventSubscritionMode eventSubscritionMode = EventSubscritionMode.Dynamic)
        {
            EventSubscritionMode = eventSubscritionMode;
        }
    }

    /// <summary>
    /// Contains a list of event subscription mode.
    /// </summary>
    /// <seealso cref="EventSubscriptionAttribute"/>
    [DataContract(Namespace = "")]
    public enum EventSubscritionMode
    {
        /// <summary>
        /// Subscribe the event on proxy initializing. Unsubscribe on disposing.
        /// </summary>
        [EnumMember]
        SubscribeOnStartAndKeep,
        /// <summary>
        /// Subscribe the event when the first handler is linked. Unsubscribe on disposing when subscribed before.
        /// </summary>
        [EnumMember]
        SubscribeOnFirstUseAndKeep,
        /// <summary>
        /// Subscribe the event when the first handler is linked. Unsubscribe on the last one is removed. This could happened repeatedly. Unsubscribing process is also called in disposing when the event is subscribed before.
        /// </summary>
        [EnumMember]
        Dynamic
    }
}
