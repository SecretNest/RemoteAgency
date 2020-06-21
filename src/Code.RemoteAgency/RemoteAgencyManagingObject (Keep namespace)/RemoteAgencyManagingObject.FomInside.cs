using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyManagingObject
    {
        protected void ProcessResponseMessageReceivedFromInside(IRemoteAgencyMessage message, IRemoteAgencyMessage requestMessage)
        {
            message.MessageId = requestMessage.MessageId;
            message.AssetName = requestMessage.AssetName;
            message.IsOneWay = true;
            //message.Exception set by caller.
            message.SenderInstanceId = InstanceId;
            //message.SenderSiteId leave to manager.
            message.TargetInstanceId = requestMessage.SenderInstanceId;
            message.TargetSiteId = requestMessage.SenderSiteId;
            message.MessageType = requestMessage.MessageType;

            _sendMessageToManagerCallback(message);
        }

        protected void ProcessDefaultTargetRequestMessageReceivedFromInside(IRemoteAgencyMessage message)
        {
            //add message id, remote site id, remote instance id, local instance id
            message.MessageId = Guid.NewGuid();
            //message.AssetName set by caller.
            //message.IsOneWay set by caller.
            //message.Exception = null;
            message.SenderInstanceId = InstanceId;
            //message.SenderSiteId leave to manager.
            message.TargetInstanceId = DefaultTargetInstanceId;
            message.TargetSiteId = TargetSiteId;
            //message.MessageType set by caller.

            ProcessPreparedRequestMessageReceivedFromInside(message);
        }

        protected void ProcessPreparedRequestMessageReceivedFromInside(IRemoteAgencyMessage message)
        {
            //local site id will be set by manager.
            _sendMessageToManagerCallback(message);
        }
    }

    partial class RemoteAgencyManagingObjectProxy<TEntityBase>
    {

    }

    partial class RemoteAgencyManagingObjectServiceWrapper<TEntityBase>
    {

    }
}
