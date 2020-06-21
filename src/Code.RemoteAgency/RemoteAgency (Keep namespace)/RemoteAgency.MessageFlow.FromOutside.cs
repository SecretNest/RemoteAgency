using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
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
            if (_managingObjects.TryGetValue(message.TargetInstanceId, out var managingObject))
            {
                ProcessMessageReceivedOnManagingObject(managingObject, message);
            }
            else if (((IRemoteAgencyMessage) message).IsOneWay)
            {
                //Send InstanceNotFoundException back to sender.
                var exception = new InstanceNotFoundException(message);
                var emptyMessage = GenerateEmptyMessage(message.SenderSiteId,
                    message.SenderInstanceId, message.MessageType,
                    message.AssetName, message.MessageId, exception);
                ProcessMessageReceivedFromInsideBypassFiltering(emptyMessage);
            }
        }

        void ProcessMessageReceivedOnManagingObject(RemoteAgencyManagingObject<TEntityBase> managingObject,
            IRemoteAgencyMessage message)
        {
            managingObject.ProcessMessageReceivedFromOutside(message);
        }
    }
}
