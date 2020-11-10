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

        public RemoteAgencyTester()
        {

        }

        //public TServiceContractInterface CreateProxy(Guid targetSiteId, Guid targetInstanceId, ref Guid instanceId)
        //{
        //    return _remoteAgency.CreateProxy<TServiceContractInterface>(targetSiteId, targetInstanceId, ref instanceId);
        //}

        //public Guid CreateServiceWrapper(TServiceContractInterface serviceObject, ref Guid instanceId)
        //{
        //    return _remoteAgency.CreateServiceWrapper(serviceObject, ref instanceId);
        //}
    }
}
