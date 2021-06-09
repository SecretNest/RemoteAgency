using System;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyBase
    {
        private protected abstract bool FindManagingObjectAndSendMessage(IRemoteAgencyMessage message);
    }

    partial class RemoteAgency<TSerialized, TEntityBase>
    {
        //First step to process received message.
        void ProcessMessageReceivedFromOutside(TEntityBase message)
        {
            //filter message after received
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
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
                {
                    RedirectException(null, Guid.Empty, 
                        $"<{nameof(MessageType.SpecialCommand)}>{message.AssetName}", e);
                }
#pragma warning restore CA1031 // Do not catch general exception types
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
                //Note: this will not make a deadlock coz it only send an error message, which the IsOneWay is always true.
            }
        }

        //Final step of received message, sent to managed object
        private protected override bool FindManagingObjectAndSendMessage(IRemoteAgencyMessage message)
        {
            if (_managingObjects.TryGetValue(message.TargetInstanceId, out var managingObject))
            {
                managingObject.ProcessMessageReceivedFromOutside(message);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
