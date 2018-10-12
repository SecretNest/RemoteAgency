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
        /// <param name="metadata">Metadata of this message instance.</param>
        /// <param name="serialized">Serialized data.</param>
        /// <param name="genericArguments">Generic arguments used in this calling.</param>
        /// <returns>The packed data.</returns>
        public override string Pack(MessageInstanceMetadata metadata, string serialized, Type[] genericArguments)
        {
            DataContractToJsonPackingEntity entity = new DataContractToJsonPackingEntity()
            {
                SenderSiteId = metadata.SenderSiteId,
                TargetSiteId = metadata.TargetSiteId,
                SenderInstanceId = metadata.SenderInstanceId,
                TargetInstanceId = metadata.TargetInstanceId,
                IsException = metadata.IsException,
                MessageType = metadata.MessageType,
                AssetName = metadata.AssetName,
                MessageId = metadata.MessageId,
                IsOneWay = metadata.IsOneWay,
                Serialized = serialized,
                GenericArguments = genericArguments
            };
            return JsonConvert.SerializeObject(entity, stringEnumConverter);
        }

        /// <summary>
        /// Unpack the data receiving form transporting.
        /// </summary>
        /// <param name="message">The packed data received from the remote site.</param>
        /// <param name="metadata">Metadata of this message instance.</param>
        /// <param name="serialized">Serialized data.</param>
        /// <param name="genericArguments">Generic arguments used in this calling.</param>
        /// <returns>Whether processing is finished without problem.</returns>
        public override bool TryUnpack(string message, out MessageInstanceMetadata metadata, out string serialized, out Type[] genericArguments)
        {
            try
            {
                var entity = JsonConvert.DeserializeObject<DataContractToJsonPackingEntity>(message, stringEnumConverter);
                metadata = new MessageInstanceMetadata(entity.SenderSiteId, entity.SenderInstanceId, entity.TargetSiteId, entity.TargetInstanceId,
                    entity.MessageType, entity.AssetName, entity.MessageId, entity.IsOneWay, entity.IsException);
                serialized = entity.Serialized;
                genericArguments = entity.GenericArguments;
                return true;
            }
            catch
            {
                metadata = null;
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
        public Guid SenderSiteId;
        /// <summary>
        /// Site id of the target Remote Agency manager
        /// </summary>
        public Guid TargetSiteId;
        /// <summary>
        /// Instance id of the source proxy or service wrapper
        /// </summary>
        public Guid SenderInstanceId;
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
