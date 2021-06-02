using System;
using System.Threading;
using SecretNest.RemoteAgency;
using SecretNest.RemoteAgency.Attributes;

namespace Test.CSharp.Test3
{
    public interface ITest3
    {
        [AssetOneWayOperating]
        void Add(long value);

        [PropertyGetOneWayOperating]
        long ValueOneWayGet { get; }

        [AssetIgnored()]
        long ValueIgnored { get; }

        long Value { get; set; }

        [LocalExceptionHandling]
        void WithException(EntityInTest3 entity);

        [OperatingTimeoutTime(1000)]
        void TimeOutMethod();
    }

    public class EntityInTest3
    {
        public string FromClientToServerProperty { get; set; }

        [ParameterReturnRequiredProperty("EntityTwoWayProperty", isIncludedInReturning: true)]
        public string TwoWayProperty { get; set; }
    }

    public class Server3 : ITest3
    {
        private long _data;

        public void Add(long value)
        {
            _data += value;
        }

        public long ValueOneWayGet 
        {
            get
            {
                Console.WriteLine("Server side: ValueOneWayGet called.");
                return _data;
            }
        }
        public long ValueIgnored => 0;

        public long Value { get => _data; set => _data = value; }

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
            var serverSiteId = serverRemoteAgencyInstance.SiteId;
            var serviceWrapperInstanceId = serverRemoteAgencyInstance.CreateServiceWrapper(originalService);

            //Client
            using var clientRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(true);
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance);
            var clientProxy = clientRemoteAgencyInstance.CreateProxy<ITest3>(serverSiteId, serviceWrapperInstanceId,
                //ReSharper disable once UnusedVariable
                out var clientProxyInstanceId);
            clientRemoteAgencyInstance.ExceptionRedirected += ClientRemoteAgencyInstance_ExceptionRedirected;

            //Run test
            Console.WriteLine("Add(No return):");
            clientProxy.Add(100);

            Console.WriteLine("ValueOneWayGet(Default value returning after server processed):");
            Console.WriteLine(clientProxy.ValueOneWayGet);

            Console.WriteLine("ValueIgnored(Exception):");
            try
            {
                _ = clientProxy.ValueIgnored;
            }
            catch (Exception e)
            {
                Console.WriteLine("Predicted Exception: " + e);
            }

            Console.WriteLine("Value(Get, 100):");
            Console.WriteLine(clientProxy.Value);

            Console.WriteLine("Value(Set, no return):");
            clientProxy.Value = 500;

            Console.WriteLine("Add(No return):");
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
            catch (Exception e)
            {
                Console.WriteLine("Predicted Exception: " + e.Message);
            }
            Console.WriteLine($"Client side: entity.FromClientToServerProperty (should be SetFromClient): {entity.FromClientToServerProperty}");
            Console.WriteLine($"Client side: entity.TwoWayProperty (should be SetBeforeException): {entity.TwoWayProperty}");
            
            Console.WriteLine("TimeOutMethod:");
            try
            {
                clientProxy.TimeOutMethod();
            }
            catch (Exception e)
            {
                Console.WriteLine("Predicted Exception: " + e);
            }

            Console.Write("Press any key to quit...");
            Console.ReadKey(true);
            Console.WriteLine();
        }

        private static void ClientRemoteAgencyInstance_ExceptionRedirected(object sender, ExceptionRedirectedEventArgs e)
        {   
            Console.WriteLine($"Client side received exception: \n  Interface:{e.ServiceContractInterface.FullName}\n  InstanceId: {e.InstanceId}\n  AssetName: {e.AssetName}\n  ExceptionType: {e.RedirectedException.GetType().FullName}\n  ExceptionMessage: {e.RedirectedException.Message}");
        }
    }
}
