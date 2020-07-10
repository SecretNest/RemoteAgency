using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies the names of entity classes generated for event adding, removing and raising.
    /// </summary>
    /// <remarks><para>When this attribute is not present, or name is set to <see langword="null"/> or empty string, the entity name is chosen automatically.</para>
    /// <para>The one marked on the event has higher priority than the one marked on the delegate of the same event.</para></remarks>
    /// <conceptualLink target="14c3caef-7392-4f68-b7eb-d0bb014a2e4c#CustomizedName" />
    [AttributeUsage(AttributeTargets.Event | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
    public class CustomizedEventEntityNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the entity class for processing event raising notification.
        /// </summary>
        /// <remarks>When the value is <see langword="null"/> or empty string, name is chosen automatically.</remarks>
        public string RaisingNotificationEntityName { get; }

        /// <summary>
        /// Gets the name of the entity class for processing event raising feedback.
        /// </summary>
        /// <remarks>When the value is <see langword="null"/> or empty string, name is chosen automatically.</remarks>
        public string RaisingFeedbackEntityName { get; }

        /// <summary>
        /// Initializes an instance of CustomizedEventEntityNameAttribute.
        /// </summary>
        /// <param name="raisingNotificationEntityName">Name of the entity class for processing event raising notification. When the value is <see langword="null"/> or empty string, name is chosen automatically. Default value is <see langword="null"/>.</param>
        /// <param name="raisingFeedbackEntityName">Name of the entity class for processing event raising feedback. When the value is <see langword="null"/> or empty string, name is chosen automatically. Default value is <see langword="null"/>.</param>
        public CustomizedEventEntityNameAttribute(
            string raisingNotificationEntityName = null, string raisingFeedbackEntityName = null)
        {
            RaisingNotificationEntityName = raisingNotificationEntityName;
            RaisingFeedbackEntityName = raisingFeedbackEntityName;
        }
    }
}
