using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Defines the common properties contained in messages.
    /// </summary>
    public interface IRemoteAgencyMessage
    {
        /// <summary>
        /// Gets or sets the site id of the source Remote Agency instance.
        /// </summary>
        Guid SenderSiteId { get; set; }
        /// <summary>
        /// Gets or sets the site id of the target Remote Agency instance.
        /// </summary>
        Guid TargetSiteId { get; set; }
        /// <summary>
        /// Gets or sets the instance id of the source proxy or service wrapper.
        /// </summary>
        Guid SenderInstanceId { get; set; }
        /// <summary>
        /// Gets or sets the instance id of the target proxy or service wrapper.
        /// </summary>
        Guid TargetInstanceId { get; set; }

        /// <summary>
        /// Gets or sets the message type.
        /// </summary>
        MessageType MessageType { get; set; }

        /// <summary>
        /// Gets or sets the asset name.
        /// </summary>
        string AssetName { get; set; }

        /// <summary>
        /// Gets or sets the message id.
        /// </summary>
        Guid MessageId { get; set; }

        /// <summary>
        /// Gets or sets the exception object.
        /// </summary>
        Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets whether this message is one-way (do not need any response).
        /// </summary>
        bool IsOneWay { get; set; }

        /// <summary>
        /// Gets or sets whether this message is empty, not containing parameters required by asset.
        /// </summary>
        bool IsEmptyMessage { get; set; }
    }
    
    /// <summary>
    /// Contains a list of message type
    /// </summary>
    [Serializable]
    [DataContract(Namespace = "")]
    public enum MessageType
    {
        /// <summary>
        /// Declares this message is related to a method calling or the returning of it.
        /// </summary>
        [EnumMember]
        Method,
        /// <summary>
        /// Declares this message is related to adding event handler or the result of it.
        /// </summary>
        [EnumMember]
        EventAdd,
        /// <summary>
        /// Declares this message is related to removing event handler or the result of it.
        /// </summary>
        [EnumMember]
        EventRemove,
        /// <summary>
        /// Declares this message is related to an event raised or the returning of it.
        /// </summary>
        [EnumMember]
        Event,
        /// <summary>
        /// Declares this message is related to getting value of a property or the returning of it.
        /// </summary>
        [EnumMember]
        PropertyGet,
        /// <summary>
        /// Declares this message is related to setting value of a property or the result of it.
        /// </summary>
        [EnumMember]
        PropertySet,
        /// <summary>
        /// Declares this message is a system reserved message.
        /// </summary>
        [EnumMember]
        SpecialCommand
    }
}
