using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Provides packing and unpacking methods for message for transporting using JsonConvert.
    /// </summary>
    public class DataContractToJsonPackingHelper : PackingHelperBase<string, string>
    {
        JsonConverter stringEnumConverter = new Newtonsoft.Json.Converters.StringEnumConverter();

        /// <summary>
        /// Packs the data for transporting.
        /// </summary>
        /// <param name="sourceSiteId">Id of the source site.</param>
        /// <param name="targetSiteId">Id of the target site.</param>
        /// <param name="sourceInstanceId">Id of the source instance.</param>
        /// <param name="targetInstanceId">Id of the target instance.</param>
        /// <param name="isException">Whether this message is an exception.</param>
        /// <param name="messageType">Message type.</param>
        /// <param name="assetName">Asset name.</param>
        /// <param name="messageId">Id of the message.</param>
        /// <param name="isOneWay">When set to true, no response should be returned from the target site.</param>
        /// <param name="serialized">Serialized data.</param>
        /// <param name="genericArguments">Generic arguments used in this calling.</param>
        /// <returns>The packed data.</returns>
        public override string Pack(Guid sourceSiteId, Guid targetSiteId, Guid sourceInstanceId, Guid targetInstanceId, bool isException, MessageType messageType, string assetName, Guid messageId, bool isOneWay, string serialized, Type[] genericArguments)
        {
            DataContractToJsonPackingEntity entity = new DataContractToJsonPackingEntity()
            {
                SourceSiteId = sourceSiteId,
                TargetSiteId = targetSiteId,
                SourceInstanceId = sourceInstanceId,
                TargetInstanceId = targetInstanceId,
                IsException = isException,
                MessageType = messageType,
                AssetName = assetName,
                MessageId = messageId,
                IsOneWay = isOneWay,
                Serialized = serialized,
                GenericArguments = genericArguments
            };
            return JsonConvert.SerializeObject(entity, stringEnumConverter);
        }

        /// <summary>
        /// Unpack the data receiving form transporting.
        /// </summary>
        /// <param name="message">The packed data received from the remote site.</param>
        /// <param name="sourceSiteId">Id of the source site.</param>
        /// <param name="targetSiteId">Id of the target site.</param>
        /// <param name="sourceInstanceId">Id of the source instance.</param>
        /// <param name="targetInstanceId">Id of the target instance.</param>
        /// <param name="isException">Whether this message is an exception.</param>
        /// <param name="messageType">Message type.</param>
        /// <param name="assetName">Asset name.</param>
        /// <param name="messageId">Id of the message.</param>
        /// <param name="isOneWay">When set to true, no response should be returned from the target site.</param>
        /// <param name="serialized">Serialized data.</param>
        /// <param name="genericArguments">Generic arguments used in this calling.</param>
        /// <returns>Whether processing is finished without problem.</returns>
        public override bool TryUnpack(string message, out Guid sourceSiteId, out Guid targetSiteId, out Guid sourceInstanceId, out Guid targetInstanceId, out bool isException, out MessageType messageType, out string assetName, out Guid messageId, out bool isOneWay, out string serialized, out Type[] genericArguments)
        {
            try
            {
                var entity = JsonConvert.DeserializeObject<DataContractToJsonPackingEntity>(message, stringEnumConverter);
                sourceSiteId = entity.SourceSiteId;
                targetSiteId = entity.TargetSiteId;
                sourceInstanceId = entity.SourceInstanceId;
                targetInstanceId = entity.TargetInstanceId;
                isException = entity.IsException;
                messageType = entity.MessageType;
                assetName = entity.AssetName;
                messageId = entity.MessageId;
                isOneWay = entity.IsOneWay;
                serialized = entity.Serialized;
                genericArguments = entity.GenericArguments;
                return true;
            }
            catch
            {
                sourceSiteId = Guid.Empty;
                targetSiteId = Guid.Empty;
                sourceInstanceId = Guid.Empty;
                targetInstanceId = Guid.Empty;
                isException = false;
                messageType = MessageType.SpecialCommand;
                assetName = null;
                messageId = Guid.Empty;
                isOneWay = true;
                serialized = null;
                genericArguments = null;
                return false;
            }
        }
    }

    /// <summary>
    /// Entity used in json packing
    /// </summary>
    public class DataContractToJsonPackingEntity
    {
        /// <summary>
        /// Site id of the source Remote Agency manager
        /// </summary>
        public Guid SourceSiteId;
        /// <summary>
        /// Site id of the target Remote Agency manager
        /// </summary>
        public Guid TargetSiteId;
        /// <summary>
        /// Instance id of the source proxy or service wrapper
        /// </summary>
        public Guid SourceInstanceId;
        /// <summary>
        /// Instance id of the target proxy or service wrapper
        /// </summary>
        public Guid TargetInstanceId;
        /// <summary>
        /// Whether this message represents an exception
        /// </summary>
        public bool IsException;
        /// <summary>
        /// Message type
        /// </summary>
        public MessageType MessageType;
        /// <summary>
        /// Asset name
        /// </summary>
        public string AssetName;
        /// <summary>
        /// Message id
        /// </summary>
        public Guid MessageId;
        /// <summary>
        /// Whether this message is one way (do not need any response)
        /// </summary>
        public bool IsOneWay;
        /// <summary>
        /// Serialized data
        /// </summary>
        public string Serialized;
        /// <summary>
        /// Type arguments for generic method and exception
        /// </summary>
        public Type[] GenericArguments;
    }
}
