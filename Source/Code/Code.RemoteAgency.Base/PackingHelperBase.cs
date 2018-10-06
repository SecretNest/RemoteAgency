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
        /// <param name="sourceSiteId">Id of the source site.</param>
        /// <param name="targetSiteId">Id of the target site.</param>
        /// <param name="sourceInstanceId">Id of the source instance.</param>
        /// <param name="targetInstanceId">Id of the target instance.</param>
        /// <param name="isException">Whether this message is an exception.</param>
        /// <param name="messageType"><see cref="MessageType" >Message type.</see></param>
        /// <param name="assetName">Asset name.</param>
        /// <param name="messageId">Id of the message.</param>
        /// <param name="isOneWay">When set to true, no response should be returned from the target site.</param>
        /// <param name="serialized">Serialized data.</param>
        /// <param name="genericArguments">Generic arguments used in this calling.</param>
        /// <returns>The packed data.</returns>
        public abstract TNetworkMessage Pack(Guid sourceSiteId, Guid targetSiteId, Guid sourceInstanceId, Guid targetInstanceId, bool isException, MessageType messageType, string assetName, Guid messageId, bool isOneWay, TSerialized serialized, Type[] genericArguments);

        /// <summary>
        /// Unpack the data receiving form transporting.
        /// </summary>
        /// <param name="message">The packed data received from the remote site.</param>
        /// <param name="sourceSiteId">Id of the source site.</param>
        /// <param name="targetSiteId">Id of the target site.</param>
        /// <param name="sourceInstanceId">Id of the source instance.</param>
        /// <param name="targetInstanceId">Id of the target instance.</param>
        /// <param name="isException">Whether this message is an exception.</param>
        /// <param name="messageType"><see cref="MessageType" >Message type.</see></param>
        /// <param name="assetName">Asset name.</param>
        /// <param name="messageId">Id of the message.</param>
        /// <param name="isOneWay">When set to true, no response should be returned from the target site.</param>
        /// <param name="serialized">Serialized data.</param>
        /// <param name="genericArguments">Generic arguments used in this calling.</param>
        /// <returns>Whether processing is finished without problem.</returns>
        public abstract bool TryUnpack(TNetworkMessage message, out Guid sourceSiteId, out Guid targetSiteId, out Guid sourceInstanceId, out Guid targetInstanceId, out bool isException, out MessageType messageType, out string assetName, out Guid messageId, out bool isOneWay, out TSerialized serialized, out Type[] genericArguments);
    }
}
