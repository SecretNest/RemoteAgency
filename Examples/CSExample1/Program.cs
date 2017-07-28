using SecretNest.RemoteAgency;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSExample1
{
    class Program
    {
        static Dictionary<Guid, RemoteAgencyManager<string, string, object>> sites = new Dictionary<Guid, RemoteAgencyManager<string, string, object>>();
        static void Main(string[] args)
        {
            DataContractSerializerEntityCodeBuilder entityCodeBuilder = new DataContractSerializerEntityCodeBuilder();
            DataContractToJsonPackingHelper packingHelper = new DataContractToJsonPackingHelper();
            DataContractSerializerSerializingHelper serializingHelper = new DataContractSerializerSerializingHelper();

            ServiceWrapperCreator<string, object> serviceWrapperCreator = new ServiceWrapperCreator<string, object>(entityCodeBuilder, typeof(DataContractSerializerSerializingHelper));
            ProxyCreator<string, object> proxyCreator = new ProxyCreator<string, object>(entityCodeBuilder, typeof(DataContractSerializerSerializingHelper));

            RemoteAgencyManager<string, string, object> clientSite = new RemoteAgencyManager<string, string, object>(packingHelper, serializingHelper);
            RemoteAgencyManager<string, string, object> serverSite = new RemoteAgencyManager<string, string, object>(packingHelper, serializingHelper);

            clientSite.MessageForSendingPrepared += OnMessageForSendingPrepared;
            serverSite.MessageForSendingPrepared += OnMessageForSendingPrepared;

            clientSite.DefaultTargetSiteId = serverSite.SiteId;

            sites.Add(clientSite.SiteId, clientSite);
            sites.Add(serverSite.SiteId, serverSite);

            clientSite.Connect();
            serverSite.Connect();

            HelloWorld serviceObject = new HelloWorld();
            var serviceWrapper = serverSite.AddServiceWrapper<IHello>(serviceWrapperCreator, serviceObject, out var serviceWrapperInstanceId);

            var proxy = clientSite.AddProxy<IHello>(proxyCreator, serviceWrapperInstanceId, out var proxyInstanceId);

            proxy.HelloWorld();

            Console.ReadKey(); //Pause before quit.
        }

        private static void OnMessageForSendingPrepared(object sender, RemoteAgencyManagerMessageForSendingEventArgs<string> e)
        {
            //Async mode
            Task.Run(() =>
              sites[e.TargetSiteId].ProcessPackagedMessage(e.Message));
        }
    }
}
