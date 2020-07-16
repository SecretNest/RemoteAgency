using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Defines a class contains message body derived from <see cref="MessageBodyEventArgsBase"/>.
    /// </summary>
    public class MessageBodyEventArgs : MessageBodyEventArgsBase
    {
        /// <summary>
        /// Initializes an instance of MessageBodyEventArgs.
        /// </summary>
        /// <param name="messageBodyGeneric">Message body</param>
        public MessageBodyEventArgs(IRemoteAgencyMessage messageBodyGeneric)
        {
            MessageBodyGeneric = messageBodyGeneric;
        }

        /// <inheritdoc />
        public override IRemoteAgencyMessage MessageBodyGeneric { get; }
    }

    /// <summary>
    /// Defines a class contains message body derived from <see cref="MessageBodyEventArgsBase"/> and implemented from <see cref="IMessageBodyGenericEventArgs{TSerialized, TEntityBase}"/>.
    /// </summary>
    /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
    /// <typeparam name="TEntityBase">Type of the parent class of all entities.</typeparam>
    public class MessageBodyEventArgs<TSerialized, TEntityBase> : MessageBodyEventArgs, IMessageBodyGenericEventArgs<TSerialized, TEntityBase>
    {
        /// <summary>
        /// Gets the message body.
        /// </summary>
        public TEntityBase MessageBody => (TEntityBase) MessageBodyGeneric;

        readonly Lazy<TSerialized> _serialized;

        /// <inheritdoc />
        public TSerialized Serialize()
        {
            return _serialized.Value;
        }

        /// <summary>
        /// Initializes an instance of MessageBodyEventArgs.
        /// </summary>
        /// <param name="messageBody">Message.</param>
        /// <param name="serializerCallback">Callback for serializing message body.</param>
        public MessageBodyEventArgs(TEntityBase messageBody, Func<TEntityBase, TSerialized> serializerCallback) : base((IRemoteAgencyMessage) messageBody)
        {
            _serialized = new Lazy<TSerialized>(() => serializerCallback(MessageBody));
        }
    }
}
