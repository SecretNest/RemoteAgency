using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyManagingObjectProxy<TEntityBase>
    {
        public bool TryPing(TimeSpan maxWaitingTime, out TimeSpan delay, out Guid remoteSiteId,
            out Guid remoteInstanceId, out Exception exception)
        {
            var pingMessage = CreateEmptyMessage();
            pingMessage.AssetName = Const.SpecialCommandProxyPing;
            PrepareDefaultTargetRequestMessageReceivedFromInside(pingMessage, MessageType.SpecialCommand, false);
            DateTime start = DateTime.Now;

            if (TryProcessRequestAndWaitResponseWithoutException(pingMessage,
                ProcessPreparedRequestMessageReceivedFromInside, (int) maxWaitingTime.TotalMilliseconds,
                out var response))
            {
                delay = DateTime.Now - start;
                remoteSiteId = response.SenderSiteId;
                remoteInstanceId = response.SenderInstanceId;
                exception = response.Exception;
                return response.Exception == null;
            }
            else
            {
                exception = new AccessingTimeOutException(pingMessage);
                remoteSiteId = DefaultTargetSiteId;
                remoteInstanceId = DefaultTargetInstanceId;
                delay = Timeout.InfiniteTimeSpan;
                return false;
            }
        }
    }
}
