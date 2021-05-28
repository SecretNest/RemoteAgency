﻿using System;
using System.Runtime.Serialization;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// The exception that is thrown when the asset specified cannot be found.
    /// </summary>
    /// <remarks>When one-way is not set on the caller side (proxy for methods and properties, service wrapper for events) but set on the other side, AssetNotFoundException will be sent back to the caller. On the contrary, the message will be dropped without processing on the other side.</remarks>
    [Serializable]
#pragma warning disable CA1032 // Implement standard exception constructors
    public sealed class AssetNotFoundException : Exception
#pragma warning restore CA1032 // Implement standard exception constructors
    {
        /// <summary>
        /// Gets the message which causes this exception thrown.
        /// </summary>
        public IRemoteAgencyMessage OriginalMessage { get; }

        /// <summary>
        /// Gets the asset name.
        /// </summary>
        public string AssetName => OriginalMessage.AssetName;

        /// <summary>
        /// Gets the message type.
        /// </summary>
        public MessageType MessageType => OriginalMessage.MessageType;

        /// <summary>
        /// Initializes an instance of the AssetNotFoundException.
        /// </summary>
        /// <param name="originalMessage">Message which causes this exception thrown.</param>
        public AssetNotFoundException(IRemoteAgencyMessage originalMessage)
        {
            OriginalMessage = originalMessage;
        }

        /// <summary>
        /// Initializes a new instance of the AssetNotFoundException class with serialized data.
        /// </summary>
        /// <param name="info">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The StreamingContext that contains contextual information about the source or destination.</param>
        private AssetNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            var originalMessageType = (Type)info.GetValue("OriginalMessageType", typeof(Type));
            OriginalMessage = (IRemoteAgencyMessage)info.GetValue("OriginalMessage", originalMessageType!);
        }

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("OriginalMessageType", OriginalMessage.GetType());
            info.AddValue("OriginalMessage", OriginalMessage);
        }

        /// <summary>
        /// Gets the error message of the current exception.
        /// </summary>
        public override string Message => $"RemoteAgency asset {AssetName}({MessageType}) not found.";

        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        /// <returns>A string representation of the current exception.</returns>
        public override string ToString() => Message;
    }
}
