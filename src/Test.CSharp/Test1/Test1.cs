using System;
using SecretNest.RemoteAgency;
using SecretNest.RemoteAgency.Attributes;

namespace Test.CSharp.Test1
{
    public interface ITest1
    {
        int Add(int a, int b);

        //When unmark this line, an exception about asset naming conflicts will be thrown.
        //[CustomizedAssetName("AddDouble")]
        float Add(float a, float b);

        [CustomizedAssetName("AddDouble")]
        double Add(double a, double b);
    }

    public class Server1 : ITest1
    {
        public int Add(int a, int b)
        {
            return a + b;
        }

        public float Add(float a, float b)
        {
            return a + b;
        }

        public double Add(double a, double b)
        {
            return a + b;
        }
    }

    public static class TestCode
    {
        public static void MyTest()
        {
            //test router
            var router = new RemoteAgencyRouter<byte[], object>();
            
            //Server
            var originalService = new Server1();
            using var serverRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(true);
            router.AddRemoteAgencyInstance(serverRemoteAgencyInstance);
            var serverSiteId = serverRemoteAgencyInstance.SiteId;
            var serviceWrapperInstanceId = serverRemoteAgencyInstance.CreateServiceWrapper(originalService);

            //Client
            using var clientRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(true);
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance);
            var clientProxy = clientRemoteAgencyInstance.CreateProxy<ITest1>(serverSiteId, serviceWrapperInstanceId).ProxyGeneric;

            //Run test
            Console.WriteLine("Add(int): 1 + 2");
            Console.WriteLine(clientProxy.Add(1, 2));

            Console.WriteLine("Add(float): 1.1 + 2.2");
            Console.WriteLine(clientProxy.Add(1.1f, 2.2f));

            Console.WriteLine("Add(double): 1.1 + 2.2");
            Console.WriteLine(clientProxy.Add(1.1, 2.2));

            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
            Console.WriteLine();
        }
    }
}
