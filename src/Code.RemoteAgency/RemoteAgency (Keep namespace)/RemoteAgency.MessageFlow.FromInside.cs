namespace SecretNest.RemoteAgency
{
    partial class RemoteAgency<TSerialized, TEntityBase>
    {
        //called from RemoteAgencyManagingObjectProxy and RemoteAgencyManagingObjectServiceWrapper
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
                //filter message before being sent out
                BeforeMessageSendingProcess(ref entityMessage, out bool shouldTerminate);

                if (shouldTerminate)
                    return;

                ProcessMessageReceivedFromInsideAfterInternalRoutingAndFiltering(entityMessage);
            }
        }

        //called from ProcessMessageReceivedAfterFiltering
        //and when an error message of InstanceNotFoundException need to be sent back.
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

        //Final step to send message out
        void ProcessMessageReceivedFromInsideAfterInternalRoutingAndFiltering(TEntityBase message)
        {
            SendMessageFinal(message);
        }
    }
}
