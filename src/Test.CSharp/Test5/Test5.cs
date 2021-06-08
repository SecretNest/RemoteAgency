using System;
using SecretNest.RemoteAgency;
using SecretNest.RemoteAgency.Attributes;

namespace Test.CSharp.Test5
{
    public interface ITest5<out T> where T: struct
    {
        T Current { get; }

        int Value { get; set; }

        [ReturnIgnored] int ReturnIgnored { get; }

        [ReturnIgnored] int ReturnIgnoredButException { get; }
    }

    public class Server5 : ITest5<DateTime>
    {
        public DateTime Current => DateTime.Now;
        public int Value { get; set; }
        public int ReturnIgnored => 100;
        public int ReturnIgnoredButException => throw new Exception("oops.");
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

            Console.WriteLine("Value(Get, 100):");
            Console.WriteLine(clientProxy.Value);

            Console.WriteLine("ReturnIgnored(Get, 0 due to return ignored):");
            Console.WriteLine(clientProxy.ReturnIgnored);

            Console.WriteLine("ReturnIgnored(Get, Exception):");
            try
            {
                _ = clientProxy.ReturnIgnoredButException;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
            {
                Console.WriteLine("Predicted Exception: " + e);
            }
#pragma warning restore CA1031 // Do not catch general exception types

            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
            Console.WriteLine();
        }
    }
}
