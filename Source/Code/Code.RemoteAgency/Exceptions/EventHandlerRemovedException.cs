using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// The exception that is thrown when all event handlers linked to this event are removed.
    /// </summary>
    /// <remarks>When all event handlers are removed, the handler managed in <see cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}"/> will be removed from the event of the service object also in a short while. 
    /// Due to concurrent processing, there still be a small chance that the event is raised at the exactly moment when the last handler is removing.
    /// The code for raising events in service object, should catch this kind of exception as well.</remarks>
    [Serializable]
    public class EventHandlerRemovedException : Exception
    {
        /// <summary>
        /// Gets the asset (event) name.
        /// </summary>
        public string AssetName { get; }

        /// <summary>
        /// Initializes an instance of the EventHandlerRemovedException.
        /// </summary>
        /// <param name="assetName">Asset (event) name.</param>
        internal EventHandlerRemovedException(string assetName)
        {
            AssetName = assetName;
        }

        /// <summary>
        /// Initializes a new instance of the EventHandlerRemovedException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        public EventHandlerRemovedException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }

        /// <summary>
        /// Gets the error message of the current exception.
        /// </summary>
        public override string Message => string.Format("All handlers linked with asset {0} are removed.", AssetName);
        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        /// <returns>A string representation of the current exception.</returns>
        public override string ToString()
        {
            return Message;
        }
    }
}
