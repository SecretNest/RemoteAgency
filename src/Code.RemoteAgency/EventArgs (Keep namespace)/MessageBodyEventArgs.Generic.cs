using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Defines a class contains message body derived from <see cref="EventArgs"/> and implemented from <see cref="IMessageBodyGenericEventArgs{TSerialized, TEntityBase}"/>.
    /// </summary>
    /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
    /// <typeparam name="TEntityBase">Type of the parent class of all entities.</typeparam>
    public class MessageBodyEventArgs<TSerialized, TEntityBase> : MessageBodyEventArgsBase, IMessageBodyGenericEventArgs<TSerialized, TEntityBase>
    {
        /// <summary>
        /// Gets the message body.
        /// </summary>
        public TEntityBase MessageBody { get; }

        readonly Lazy<TSerialized> _serialized;

        /// <inheritdoc />
        public TSerialized Serialize()
        {
            return _serialized.Value;
        }

        /// <inheritdoc />
        public override IRemoteAgencyMessage MessageBodyGeneric => (IRemoteAgencyMessage) MessageBody;

        /// <summary>
        /// Initializes an instance of MessageBodyEventArgs.
        /// </summary>
        /// <param name="messageBody">Message.</param>
        /// <param name="serializerCallback">Callback for serializing message body.</param>
        public MessageBodyEventArgs(TEntityBase messageBody, Func<TEntityBase, TSerialized> serializerCallback)
        {
            MessageBody = messageBody;
            _serialized = new Lazy<TSerialized>(() => serializerCallback(MessageBody));
        }
    }
}
