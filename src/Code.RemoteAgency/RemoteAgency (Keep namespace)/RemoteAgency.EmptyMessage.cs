using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgency<TSerialized, TEntityBase>
    {
        TEntityBase GenerateEmptyMessage(Guid targetSiteId, Guid targetInstanceId, MessageType messageType,
            string assetName, Guid messageId, Exception exception)
            => GenerateEmptyMessage(Guid.Empty, targetSiteId, targetInstanceId, messageType, assetName, messageId,
                exception, true);

        TEntityBase GenerateEmptyMessage(Guid senderInstanceId, Guid targetSiteId, Guid targetInstanceId, MessageType messageType,
            string assetName, Guid messageId, Exception exception, bool isOneWay)
        {
            Guid senderSiteId = SiteId;

            var emptyMessage = _entityCodeBuilder.CreateEmptyMessage();
            emptyMessage.SenderSiteId = senderSiteId;
            emptyMessage.TargetSiteId = targetSiteId;
            emptyMessage.SenderInstanceId = senderInstanceId;
            emptyMessage.TargetInstanceId = targetInstanceId;
            emptyMessage.MessageType = messageType;
            emptyMessage.AssetName = assetName;
            emptyMessage.MessageId = messageId;
            emptyMessage.Exception = exception;
            emptyMessage.IsOneWay = isOneWay;

            return (TEntityBase) emptyMessage;
        }
    }
}
