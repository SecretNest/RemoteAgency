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
        /// Gets the message which causes this exception thrown.
        /// </summary>
        public IRemoteAgencyMessage OriginalMessage { get; }

        /// <summary>
        /// Gets the id of the instance that cannot be found.
        /// </summary>
        public Guid InstanceId => OriginalMessage.TargetInstanceId;

        /// <summary>
        /// Initializes an instance of InstanceNotFoundException.
        /// </summary>
        /// <param name="originalMessage">Message which causes this exception thrown.</param>
        public InstanceNotFoundException(IRemoteAgencyMessage originalMessage) : base(
            $"Remote Agency object instance {originalMessage.TargetInstanceId} is not found.")
        {
            OriginalMessage = originalMessage;
        }

        /// <summary>
        /// Initializes a new instance of the InstanceNotFoundException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        private InstanceNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            var originalMessageType = (Type)info.GetValue("OriginalMessageType", typeof(Type));
            OriginalMessage = (IRemoteAgencyMessage)info.GetValue("OriginalMessage", originalMessageType);
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("OriginalMessageType", OriginalMessage.GetType());
            info.AddValue("OriginalMessage", OriginalMessage);
        }

        /// <inheritdoc />
        public override string ToString() => Message;
    }
}
