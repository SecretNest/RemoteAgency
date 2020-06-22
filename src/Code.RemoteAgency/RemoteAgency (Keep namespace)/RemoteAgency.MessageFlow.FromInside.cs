using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgency<TSerialized, TEntityBase>
    {
        //TODO: Link ProcessMessageReceivedFromInside to the managing object

        void ProcessMessageReceivedFromInside(TEntityBase message)
        {
            ((IRemoteAgencyMessage) message).SenderSiteId = SiteId;

            //internal routing
            if (((IRemoteAgencyMessage) message).TargetSiteId == SiteId)
            {
                ProcessMessageReceivedAfterFiltering(message);
            }

            BeforeMessageSendingProcess(ref message, out bool shouldTerminate);

            if (shouldTerminate)
                return;

            ProcessMessageReceivedFromInsideAfterInternalRoutingAndFiltering(message);
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
