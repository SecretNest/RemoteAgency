using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgency
    {
        private protected abstract bool FindManagingObjectAndSendMessage(IRemoteAgencyMessage message);
    }

    partial class RemoteAgency<TSerialized, TEntityBase>
    {
        void ProcessMessageReceivedFromOutside(TEntityBase message)
        {
            AfterMessageReceivedProcess(ref message, out bool shouldTerminate);

            if (shouldTerminate)
                return;

            ProcessMessageReceivedAfterFiltering(message);
        }

        void ProcessMessageReceivedAfterFiltering(TEntityBase message)
            => ProcessMessageReceivedAfterFiltering((IRemoteAgencyMessage) message);

        void ProcessMessageReceivedAfterFiltering(IRemoteAgencyMessage message)
        {
            if (message.MessageType == MessageType.SpecialCommand)
            {
                try
                {
                    if (message.AssetName == Const.SpecialCommandServiceWrapperDisposed)
                    {
                        OnRemoteServiceWrapperClosing(message.SenderSiteId, message.SenderInstanceId);
                        return;
                    }
                    else if (message.AssetName == Const.SpecialCommandProxyDisposed)
                    {
                        OnRemoteProxyClosing(message.SenderSiteId, message.SenderInstanceId);
                        return;
                    }
                }
                catch (Exception e)
                {
                    RedirectException(e);
                }
            }

            if (FindManagingObjectAndSendMessage(message))
            {
            }
            else if (message.IsOneWay)
            {
                //Send InstanceNotFoundException back to sender.
                var exception = new InstanceNotFoundException(message, SiteId);
                var emptyMessage = GenerateEmptyMessage(message.SenderSiteId,
                    message.SenderInstanceId, message.MessageType,
                    message.AssetName, message.MessageId, exception);
                ProcessMessageReceivedFromInsideBypassFiltering((TEntityBase) emptyMessage);
            }
        }

        void ProcessMessageReceivedOnManagingObject(RemoteAgencyManagingObject<TEntityBase> managingObject,
            IRemoteAgencyMessage message)
        {
            managingObject.ProcessMessageReceivedFromOutside(message);
        }

        private protected override bool FindManagingObjectAndSendMessage(IRemoteAgencyMessage message)
        {
            if (_managingObjects.TryGetValue(message.TargetInstanceId, out var managingObject))
            {
                ProcessMessageReceivedOnManagingObject(managingObject, message);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
