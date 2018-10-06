using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// The exception that is thrown when the object specified by instance id cannot be found in target <see cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}"/> instance.
    /// </summary>
    [DataContract(Namespace = "")]
    public sealed class ObjectNotFoundException : NullReferenceException
    {
        /// <summary>
        /// Id of the instance that cannot be found.
        /// </summary>
        [DataMember]
        public Guid InstanceId { get; set; }

        /// <summary>
        /// Initializes an instance of ObjectNotFoundException.
        /// </summary>
        /// <param name="instanceId">Id of the instance that cannot be found.</param>
        public ObjectNotFoundException(Guid instanceId)
        {
            InstanceId = instanceId;
        }

        /// <summary>
        /// Gets the error message of the current exception.
        /// </summary>
        public override string Message => string.Format("RemoteAgency object instance {0} not found.", InstanceId);

        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        /// <returns>A string representation of the current exception.</returns>
        public override string ToString() => Message;
    }
}
