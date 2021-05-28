using System;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Represents a created managed object that can communicate with Remote Agency.
    /// </summary>
    public interface IManagedObjectCommunicate
    {
        ///// <summary>
        ///// Gets or sets the callback for a delegate which will be called when site id is required.
        ///// </summary>
        //Func<Guid> GetSiteIdCallback { get; set; }

        /// <summary>
        /// Sends messages to all relevant objects and closes the functions of this object.
        /// </summary>
        /// <param name="sendSpecialCommand">Whether need to send special command.</param>
        /// <exception cref="AggregateException">When exceptions occurred.</exception>
        void CloseRequestedByManagingObject(bool sendSpecialCommand);

        /// <summary>
        /// Gets or sets the callback for a delegate which will be called while a special command message need to be sent to a remote site without getting response.
        /// </summary>
        SendOneWayMessageCallback SendOneWaySpecialCommandMessageCallback { get; set; }

        /// <summary>
        /// Gets or sets the callback for a delegate which will be called while an empty message need to be created.
        /// </summary>
        CreateEmptyMessageCallback CreateEmptyMessageCallback { get; set; }
    }
}
