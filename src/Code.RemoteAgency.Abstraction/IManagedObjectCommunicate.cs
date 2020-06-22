using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Represents a created managed object that can communicate with Remote Agency.
    /// </summary>
    public interface IManagedObjectCommunicate
    {
        /// <summary>
        /// Will be called when site id is required.
        /// </summary>
        Func<Guid> GetSiteIdCallback { get; set; }

        /// <summary>
        /// Sends messages to all relevant objects and closes the functions of this object.
        /// </summary>
        /// <exception cref="AggregateException">When exceptions occurred.</exception>
        void CloseRequestedByManagingObject();
    }
}
