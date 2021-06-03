using System;
using SecretNest.RemoteAgency;

namespace Test.CSharp.Test5
{
    public interface ITest5<out T> where T: struct
    {
        T Current { get; }

        int Value { get; set; }
    }

    public class Server5 : ITest5<DateTime>
    {
        public DateTime Current => DateTime.Now;
        public int Value { get; set; }
    }

    public static class TestCode
    {
        public static void MyTest()
        {
            //test router
            var router = new RemoteAgencyRouter<byte[], object>();

            //Server
            var originalService = new Server5();
            using var serverRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(true);
            router.AddRemoteAgencyInstance(serverRemoteAgencyInstance);
            var serverSiteId = serverRemoteAgencyInstance.SiteId;
            var serviceWrapperInstanceId = serverRemoteAgencyInstance.CreateServiceWrapper(originalService);

            //Client
            using var clientRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(true);
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance);
            var clientProxy = clientRemoteAgencyInstance.CreateProxy<ITest5<DateTime>>(serverSiteId, serviceWrapperInstanceId).ProxyGeneric;

            //Run test
            Console.WriteLine("Current(Current date):");
            Console.WriteLine(clientProxy.Current.ToLongDateString());

            Console.WriteLine("Value(Set, No return):");
            clientProxy.Value = 100;

            Console.WriteLine("Value(Get, 100:");
            Console.WriteLine(clientProxy.Value);

            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
            Console.WriteLine();
        }
    }
}
