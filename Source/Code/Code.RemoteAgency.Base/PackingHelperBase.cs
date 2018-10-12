using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Provides packing and unpacking methods for message for transporting. This is an abstract class.
    /// </summary>
    /// <typeparam name="TNetworkMessage">Type of the message for transporting.</typeparam>
    /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
    public abstract class PackingHelperBase<TNetworkMessage, TSerialized>
    {
        /// <summary>
        /// Packs the data for transporting.
        /// </summary>
        /// <param name="metadata">Metadata of this message instance.</param>
        /// <param name="serialized">Serialized data.</param>
        /// <param name="genericArguments">Generic arguments used in this calling.</param>
        /// <returns>The packed data.</returns>
        public abstract TNetworkMessage Pack(MessageInstanceMetadata metadata, TSerialized serialized, Type[] genericArguments);

        /// <summary>
        /// Unpack the data receiving form transporting.
        /// </summary>
        /// <param name="message">The packed data received from the remote site.</param>
        /// <param name="metadata">Metadata of this message instance.</param>
        /// <param name="serialized">Serialized data.</param>
        /// <param name="genericArguments">Generic arguments used in this calling.</param>
        /// <returns>Whether processing is finished without problem.</returns>
        public abstract bool TryUnpack(TNetworkMessage message, out MessageInstanceMetadata metadata, out TSerialized serialized, out Type[] genericArguments);
    }
}
