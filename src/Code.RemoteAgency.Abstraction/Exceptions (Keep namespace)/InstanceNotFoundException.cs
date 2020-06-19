using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// The exception that is thrown when the object specified by instance id cannot be found in target instance of Remote Agency.
    /// </summary>
    [Serializable]
    public sealed class InstanceNotFoundException : NullReferenceException
    {
        /// <summary>
        /// Gets the id of the instance that cannot be found.
        /// </summary>
        public Guid InstanceId { get; set; }

        /// <summary>
        /// Initializes an instance of InstanceNotFoundException.
        /// </summary>
        /// <param name="instanceId">Id of the instance that cannot be found.</param>
        public InstanceNotFoundException(Guid instanceId) : base(
            $"Remote Agency object instance {instanceId} is not found.")
        {
            InstanceId = instanceId;
        }

        /// <summary>
        /// Initializes a new instance of the InstanceNotFoundException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        private InstanceNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            InstanceId = (Guid)info.GetValue("InstanceId", typeof(Guid));
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("InstanceId", InstanceId);
        }

        /// <inheritdoc />
        public override string ToString() => Message;
    }
}
