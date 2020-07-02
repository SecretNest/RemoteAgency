using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgency
    {
        /// <summary>
        /// Creates an empty message with sender instance id set to <see cref="Guid.Empty"/> and one way is <see langword="true"/>.
        /// </summary>
        /// <param name="targetSiteId">Target site id.</param>
        /// <param name="targetInstanceId">Target instance id.</param>
        /// <param name="messageType">Message type.</param>
        /// <param name="assetName">Asset name.</param>
        /// <param name="messageId">Message id.</param>
        /// <param name="exception">Exception.</param>
        /// <returns>Empty message.</returns>
        protected IRemoteAgencyMessage GenerateEmptyMessage(Guid targetSiteId, Guid targetInstanceId, MessageType messageType,
            string assetName, Guid messageId, Exception exception)
            => GenerateEmptyMessage(Guid.Empty, targetSiteId, targetInstanceId, messageType, assetName, messageId,
                exception, true);

        /// <summary>
        /// Creates an empty message.
        /// </summary>
        /// <param name="senderInstanceId">Sender instance id.</param>
        /// <param name="targetSiteId">Target site id.</param>
        /// <param name="targetInstanceId">Target instance id.</param>
        /// <param name="messageType">Message type.</param>
        /// <param name="assetName">Asset name.</param>
        /// <param name="messageId">Message id.</param>
        /// <param name="exception">Exception.</param>
        /// <param name="isOneWay">Whether the message is one way.</param>
        /// <returns>Empty message.</returns>
        protected IRemoteAgencyMessage GenerateEmptyMessage(Guid senderInstanceId, Guid targetSiteId, Guid targetInstanceId, MessageType messageType,
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

            return emptyMessage;
        }
    }
}
