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
    [AttributeUsage(AttributeTargets.Event | AttributeTargets.Delegate, Inherited = true, AllowMultiple = false)]
    public class CustomizedEventEntityNameAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the entity class for processing event adding request.
        /// </summary>
        /// <remarks>When the value is <see langword="null"/> or empty string, name is chosen automatically.</remarks>
        public string AddingRequestEntityName { get; }
        
        /// <summary>
        /// Gets the name of the entity class for processing event adding response.
        /// </summary>
        /// <remarks>When the value is <see langword="null"/> or empty string, name is chosen automatically.</remarks>
        public string AddingResponseEntityName { get; }

        /// <summary>
        /// Gets the name of the entity class for processing event removing response.
        /// </summary>
        /// <remarks>When the value is <see langword="null"/> or empty string, name is chosen automatically.</remarks>
        public string RemovingRequestEntityName { get; }

        /// <summary>
        /// Gets the name of the entity class for processing event removing response.
        /// </summary>
        /// <remarks>When the value is <see langword="null"/> or empty string, name is chosen automatically.</remarks>
        public string RemovingResponseEntityName { get; }

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
        /// <param name="addingRequestEntityName">Name of the entity class for processing event adding request. When the value is <see langword="null"/> or empty string, name is chosen automatically. Default value is <see langword="null"/>.</param>
        /// <param name="addingResponseEntityName">Name of the entity class for processing event adding response. When the value is <see langword="null"/> or empty string, name is chosen automatically. Default value is <see langword="null"/>.</param>
        /// <param name="removingRequestEntityName">Name of the entity class for processing event removing response. When the value is <see langword="null"/> or empty string, name is chosen automatically. Default value is <see langword="null"/>.</param>
        /// <param name="removingResponseEntityName">Name of the entity class for processing event removing response. When the value is <see langword="null"/> or empty string, name is chosen automatically. Default value is <see langword="null"/>.</param>
        /// <param name="raisingNotificationEntityName">Name of the entity class for processing event raising notification. When the value is <see langword="null"/> or empty string, name is chosen automatically. Default value is <see langword="null"/>.</param>
        /// <param name="raisingFeedbackEntityName">Name of the entity class for processing event raising feedback. When the value is <see langword="null"/> or empty string, name is chosen automatically. Default value is <see langword="null"/>.</param>
        public CustomizedEventEntityNameAttribute(
            string addingRequestEntityName = null, string addingResponseEntityName = null,
            string removingRequestEntityName = null, string removingResponseEntityName = null,
            string raisingNotificationEntityName = null, string raisingFeedbackEntityName = null)
        {
            AddingRequestEntityName = addingRequestEntityName;
            AddingResponseEntityName = addingResponseEntityName;
            RemovingRequestEntityName = removingRequestEntityName;
            RemovingResponseEntityName = removingResponseEntityName;
            RaisingNotificationEntityName = raisingNotificationEntityName;
            RaisingFeedbackEntityName = raisingFeedbackEntityName;
        }
    }
}
