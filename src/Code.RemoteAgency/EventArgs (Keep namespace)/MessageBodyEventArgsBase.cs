using System;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Defines a class contains message body derived from <see cref="EventArgs"/>. This is an abstract class.
    /// </summary>
    /// <seealso cref="MessageBodyEventArgs{TSerialized, TEntityBase}"/>
    public abstract class MessageBodyEventArgsBase : EventArgs
    {
        /// <summary>
        /// Gets the site id of the source Remote Agency instance.
        /// </summary>
        public Guid SenderSiteId => MessageBodyGeneric.SenderSiteId;

        /// <summary>
        /// Gets the site id of the target Remote Agency instance.
        /// </summary>
        public Guid TargetSiteId => MessageBodyGeneric.TargetSiteId;

        /// <summary>
        /// Gets the instance id of the source proxy or service wrapper.
        /// </summary>
        public Guid SenderInstanceId => MessageBodyGeneric.SenderInstanceId;

        /// <summary>
        /// Gets the instance id of the target proxy or service wrapper.
        /// </summary>
        public Guid TargetInstanceId => MessageBodyGeneric.TargetInstanceId;

        /// <summary>
        /// Gets the message type.
        /// </summary>
        public MessageType MessageType => MessageBodyGeneric.MessageType;

        /// <summary>
        /// Gets the asset name.
        /// </summary>
        public string AssetName => MessageBodyGeneric.AssetName;

        /// <summary>
        /// Gets the message id.
        /// </summary>
        public Guid MessageId => MessageBodyGeneric.MessageId;

        /// <summary>
        /// Gets the exception object.
        /// </summary>
        public Exception Exception => MessageBodyGeneric.Exception;

        /// <summary>
        /// Gets whether this message is one-way (do not need any response).
        /// </summary>
        public bool IsOneWay => MessageBodyGeneric.IsOneWay;

        /// <summary>
        /// Gets whether this message is empty, not containing parameters required by asset.
        /// </summary>
        public bool IsEmptyMessage => MessageBodyGeneric.IsEmptyMessage;

        /// <summary>
        /// Gets the message body.
        /// </summary>
        public abstract IRemoteAgencyMessage MessageBodyGeneric { get; }

    }
}
