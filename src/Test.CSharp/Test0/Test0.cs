using System;
using SecretNest.RemoteAgency;

namespace Test.CSharp.Test0
{
    public interface ITest0
    {
        void Hello();
    }

    public class Server0 : ITest0
    {
        public void Hello()
        {
            Console.WriteLine("Hello world.");
        }
    }

    public static class TestCode
    {
        public static void MyTest()
        {
            //test router
            var router = new RemoteAgencyRouter<byte[], object>();
            
            //Server
            var originalService = new Server0();
            using var serverRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(true);
            router.AddRemoteAgencyInstance(serverRemoteAgencyInstance);
            var serverSiteId = serverRemoteAgencyInstance.SiteId;
            var serviceWrapperInstanceId = serverRemoteAgencyInstance.CreateServiceWrapper(originalService);

            //Client
            using var clientRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(true);
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance);
            var clientProxy = clientRemoteAgencyInstance.CreateProxy<ITest0>(serverSiteId, serviceWrapperInstanceId).ProxyGeneric;

            //Run test
            clientProxy.Hello();

            Console.Write("Press any key to quit...");
            Console.ReadKey(true);
            Console.WriteLine();
        }
    }
}
