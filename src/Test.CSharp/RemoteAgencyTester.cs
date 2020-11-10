using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecretNest.RemoteAgency;

namespace Test.CSharp
{
    class RemoteAgencyTester<TServiceContractInterface>
    {
        private RemoteAgency<string> _remoteAgency = RemoteAgencyBase.CreateWithJsonSerializer();

        public Guid RemoteAgencyInstanceId => _remoteAgency.SiteId;

        public event EventHandler<PreparedMessage> MessagePrepared;

        public RemoteAgencyTester()
        {
            _remoteAgency.MessageForSendingPrepared += _remoteAgency_MessageForSendingPrepared;
        }

        public TServiceContractInterface CreateProxy(Guid targetSiteId, Guid targetInstanceId, out Guid instanceId)
        {
            return _remoteAgency.CreateProxy<TServiceContractInterface>(targetSiteId, targetInstanceId, out instanceId);
        }

        public Guid CreateServiceWrapper(TServiceContractInterface serviceObject)
        {
            return _remoteAgency.CreateServiceWrapper(serviceObject);
        }

        private void _remoteAgency_MessageForSendingPrepared(object sender, MessageBodyEventArgs<string, object> e)
        {
            PreparedMessage message = new PreparedMessage()
            {
                Message = e.Serialize(),
                TargetSiteId = e.TargetSiteId
            };
            MessagePrepared?.Invoke(this, message);
        }

        public void ProcessMessage(string message)
        {
            _remoteAgency.ProcessReceivedSerializedMessage(message);
        }
    }

    public class PreparedMessage : EventArgs
    {
        public string Message { get; set; }
        public Guid TargetSiteId { get; set; }
    }
}
