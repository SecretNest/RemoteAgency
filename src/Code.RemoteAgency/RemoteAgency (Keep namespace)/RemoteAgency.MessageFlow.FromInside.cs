using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgency<TSerialized, TEntityBase>
    {
        void ProcessMessageReceivedFromInside(IRemoteAgencyMessage message)
        {
            message.SenderSiteId = SiteId;

            var entityMessage = (TEntityBase) message;

            //internal routing
            if (message.TargetSiteId == SiteId)
            {
                ProcessMessageReceivedAfterFiltering(entityMessage);
            }

            BeforeMessageSendingProcess(ref entityMessage, out bool shouldTerminate);

            if (shouldTerminate)
                return;

            ProcessMessageReceivedFromInsideAfterInternalRoutingAndFiltering(entityMessage);
        }

        void ProcessMessageReceivedFromInsideBypassFiltering(TEntityBase message)
        {
            //internal routing
            if (((IRemoteAgencyMessage) message).TargetSiteId == SiteId)
            {
                ProcessMessageReceivedAfterFiltering(message);
            }

            ProcessMessageReceivedFromInsideAfterInternalRoutingAndFiltering(message);
        }

        void ProcessMessageReceivedFromInsideAfterInternalRoutingAndFiltering(TEntityBase message)
        {
            SendMessageFinal(message);
        }
    }
}
