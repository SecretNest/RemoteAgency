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
        {
            if (_managingObjects.TryGetValue(((IRemoteAgencyMessage) message).TargetInstanceId, out var managingObject))
            {
                ProcessMessageReceivedOnManagingObject(managingObject, message);
            }
            else if (((IRemoteAgencyMessage) message).IsOneWay)
            {
                //Send InstanceNotFoundException back to sender.
                var exception = new InstanceNotFoundException(((IRemoteAgencyMessage) message).TargetInstanceId);
                var emptyMessage = GenerateEmptyMessage(((IRemoteAgencyMessage) message).SenderSiteId,
                    ((IRemoteAgencyMessage) message).SenderInstanceId, ((IRemoteAgencyMessage) message).MessageType,
                    ((IRemoteAgencyMessage) message).AssetName, ((IRemoteAgencyMessage) message).MessageId, exception);
                ProcessMessageReceivedFromInsideBypassFiltering(emptyMessage);
            }
        }

        void ProcessMessageReceivedOnManagingObject(RemoteAgencyManagingObject<TEntityBase> managingObject,
            TEntityBase message)
        {
            managingObject.ProcessMessageReceivedFromOutside(message);
        }
    }
}
