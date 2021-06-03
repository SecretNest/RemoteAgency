using System;
using SecretNest.RemoteAgency;

namespace Test.CSharp.Test12
{
    public interface ITest12
    {
        void Hello();
    }

    public class Server12 : ITest12
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
            var originalService = new Server12();
            using var serverRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(true);
            router.AddRemoteAgencyInstance(serverRemoteAgencyInstance);
            var serverSiteId = serverRemoteAgencyInstance.SiteId;
            var serviceWrapperInstanceId = serverRemoteAgencyInstance.CreateServiceWrapper(originalService);

            //Client
            using var clientRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(true);
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance);
            var clientProxyInstanceId = clientRemoteAgencyInstance.CreateProxy<ITest12>(serverSiteId, serviceWrapperInstanceId).InstanceId;

            //Run test
            Console.WriteLine("Ping (ms):");
            Console.WriteLine(clientRemoteAgencyInstance.Ping(clientProxyInstanceId, out var remoteSiteId, out var remoteInstanceId).TotalMilliseconds);
            if (serverSiteId != remoteSiteId ||
                serviceWrapperInstanceId != remoteInstanceId)
            {
                // ReSharper disable once StringLiteralTypo
                Console.WriteLine("D'oh!");
            }

            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
            Console.WriteLine();
        }
    }
}
