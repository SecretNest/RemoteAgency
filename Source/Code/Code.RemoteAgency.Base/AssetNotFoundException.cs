using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// The exception that is thrown when the asset specified cannot be found.
    /// </summary>
    [DataContract(Namespace = "")]
    public sealed class AssetNotFoundException : Exception
    {
        /// <summary>
        /// Gets the asset name.
        /// </summary>
        [DataMember]
        public string AssetName { get; set; }

        /// <summary>
        /// <see cref="MessageType">Gets or sets the message type.</see>
        /// </summary>
        [DataMember]
        public MessageType MessageType { get; set; }

        /// <summary>
        /// Initializes an instance of the AssetNotFoundException.
        /// </summary>
        /// <param name="messageType"><see cref="MessageType">Gets or sets the message type.</see></param>
        /// <param name="assetName">Asset name.</param>
        public AssetNotFoundException(MessageType messageType, string assetName)
        {
            MessageType = messageType;
            AssetName = assetName;
        }

        /// <summary>
        /// Gets the error message of the current exception.
        /// </summary>
        public override string Message => string.Format("RemoteAgency asset {0}({1}) not found.", AssetName, MessageType);

        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        /// <returns>A string representation of the current exception.</returns>
        public override string ToString() => Message;

    }
}
