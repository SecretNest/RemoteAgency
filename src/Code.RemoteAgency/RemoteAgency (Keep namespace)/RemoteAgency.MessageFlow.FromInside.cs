﻿namespace SecretNest.RemoteAgency
{
    partial class RemoteAgency<TSerialized, TEntityBase>
    {
        void ProcessMessageReceivedFromInside(IRemoteAgencyMessage message)
        {
            message.SenderSiteId = SiteId;

            var entityMessage = (TEntityBase) message;

            //internal routing
            if (LoopbackAddressDetection && message.TargetSiteId == SiteId)
            {
                ProcessMessageReceivedAfterFiltering(entityMessage);
            }
            else
            {
                BeforeMessageSendingProcess(ref entityMessage, out bool shouldTerminate);

                if (shouldTerminate)
                    return;

                ProcessMessageReceivedFromInsideAfterInternalRoutingAndFiltering(entityMessage);
            }
        }

        void ProcessMessageReceivedFromInsideBypassFiltering(TEntityBase message)
        {
            //internal routing
            if (LoopbackAddressDetection && ((IRemoteAgencyMessage) message).TargetSiteId == SiteId)
            {
                ProcessMessageReceivedAfterFiltering(message);
            }
            else
            {
                ProcessMessageReceivedFromInsideAfterInternalRoutingAndFiltering(message);
            }
        }

        void ProcessMessageReceivedFromInsideAfterInternalRoutingAndFiltering(TEntityBase message)
        {
            SendMessageFinal(message);
        }
    }
}
