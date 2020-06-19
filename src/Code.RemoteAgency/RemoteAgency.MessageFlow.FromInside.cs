using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    public partial class RemoteAgency<TSerialized, TEntityBase>
    {
        void ProcessMessageReceivedFromInside(TEntityBase message)
        {
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
