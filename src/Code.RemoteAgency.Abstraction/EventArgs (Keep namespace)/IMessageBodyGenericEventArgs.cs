using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Defines property and method to hold the message body and provide an access point for serialization.
    /// </summary>
    /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
    /// <typeparam name="TEntityBase">Type of the parent class of all entities.</typeparam>
    public interface IMessageBodyGenericEventArgs<out TSerialized, out TEntityBase>
    {
        /// <summary>
        /// Gets the message body.
        /// </summary>
        TEntityBase MessageBody { get; }

        /// <summary>
        /// Serializes this message using the serializer from the Remote Agency instance.
        /// </summary>
        /// <returns>Serialized data.</returns>
        TSerialized Serialize();
    }
}
