using System;
using System.Threading;
using SecretNest.RemoteAgency;
using SecretNest.RemoteAgency.Attributes;

namespace Test.CSharp.Test3
{
    public interface ITest3
    {
        [AssetIgnored]
        long MethodIgnored();

        long Value { get; set; }

        [AssetOneWayOperating]
        void Add(long value);

        [LocalExceptionHandling]
        void WithException([ParameterReturnRequiredProperty("TwoWayProperty", isIncludedInReturning: true)] EntityInTest3 entity);

        [OperatingTimeoutTime(1000)]
        void TimeOutMethod();
    }

    public class EntityInTest3
    {
        public string FromClientToServerProperty { get; set; }

        public string TwoWayProperty { get; set; }
    }

    public class Server3 : ITest3
    {
        private long _data;

        public long MethodIgnored()
        {
            throw new Exception("You should never see this due to ignored.");
        }

        public long Value { get => _data; set => _data = value; }

        public void Add(long value)
        {
            _data += value;
            throw new Exception("You should never receive this from client side due to one way calling.");
        }

        public void WithException(EntityInTest3 entity)
        {
            Console.WriteLine($"Server side: entity.FromClientToServerProperty (should be SetFromClient): {entity.FromClientToServerProperty}");
            Console.WriteLine($"Server side: entity.TwoWayProperty (should be SetFromClient): {entity.TwoWayProperty}");

            entity.TwoWayProperty = "SetBeforeException";
            throw new Exception("oops.");
        }

        public void TimeOutMethod()
        {
            Thread.Sleep(2000);
        }
    }

    public static class TestCode
    {
        public static void MyTest()
        {
            //test router
            var router = new RemoteAgencyRouter<byte[], object>();
            
            //Server
            var originalService = new Server3();
            using var serverRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(true);
            router.AddRemoteAgencyInstance(serverRemoteAgencyInstance);
            serverRemoteAgencyInstance.ExceptionRedirected += ServerRemoteAgencyInstance_ExceptionRedirected;
            var serverSiteId = serverRemoteAgencyInstance.SiteId;
            var serviceWrapperInstanceId = serverRemoteAgencyInstance.CreateServiceWrapper(originalService);

            //Client
            using var clientRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(true);
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance);
            var clientProxy = clientRemoteAgencyInstance.CreateProxy<ITest3>(serverSiteId, serviceWrapperInstanceId).ProxyGeneric;

            //Run test
            Console.WriteLine("MethodIgnored(Exception):");
            try
            {
                _ = clientProxy.MethodIgnored();
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
            {
                Console.WriteLine("Predicted Exception: " + e);
            }
#pragma warning restore CA1031 // Do not catch general exception types

            Console.WriteLine("Value(Set, no return):");
            clientProxy.Value = 500;

            Console.WriteLine("Add(No return on client but exception on server):");
            clientProxy.Add(100);

            Console.WriteLine("Value(Get, 600):");
            Console.WriteLine(clientProxy.Value);

            var entity = new EntityInTest3
            {
                FromClientToServerProperty = "SetFromClient",
                TwoWayProperty = "SetFromClient"
            };
            Console.WriteLine("WithException:");
            try
            {
                clientProxy.WithException(entity);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
            {
                Console.WriteLine("Predicted Exception: " + e.Message);
            }
#pragma warning restore CA1031 // Do not catch general exception types
            Console.WriteLine($"Client side: entity.FromClientToServerProperty (should be SetFromClient): {entity.FromClientToServerProperty}");
            Console.WriteLine($"Client side: entity.TwoWayProperty (should be SetBeforeException): {entity.TwoWayProperty}");
            
            Console.WriteLine("TimeOutMethod:");
            try
            {
                clientProxy.TimeOutMethod();
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

        private static void ServerRemoteAgencyInstance_ExceptionRedirected(object sender, ExceptionRedirectedEventArgs e)
        {   
            Console.WriteLine($"Server side exception: \n  Interface: {e.ServiceContractInterface.FullName}\n  InstanceId: {e.InstanceId}\n  AssetName: {e.AssetName}\n  ExceptionType: {e.RedirectedException.GetType().FullName}\n  ExceptionMessage: {e.RedirectedException.Message}");
        }
    }
}
