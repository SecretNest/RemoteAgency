using System;
using SecretNest.RemoteAgency;
using SecretNest.RemoteAgency.Attributes;

namespace Test.CSharp.Test2
{
    public interface ITest2
    {
        void Add(ref long value);

        void Read(out long value);

        void Process(EntityInTest2 entity);
    }

    public class EntityInTest2
    {
        public string FromClientToServerProperty { get; set; }

        [ParameterReturnRequiredProperty("EntityTwoWayProperty")]
        public string TwoWayProperty { get; set; }
    }

    public class Server2 : ITest2
    {
        private long _data;

        public void Add(ref long value)
        {
            value += 1;
            _data = value;
        }

        public void Read(out long value)
        {
            value = _data;
        }

        public void Process(EntityInTest2 entity)
        {
            Console.WriteLine($"Server side: entity.FromClientToServerProperty (should be SetFromClient): {entity.FromClientToServerProperty}");
            Console.WriteLine($"Server side: entity.TwoWayProperty (should be SetFromClient): {entity.TwoWayProperty}");

            entity.TwoWayProperty = "SetFromServer";
            entity.FromClientToServerProperty = "useless";
        }
    }

    public static class TestCode
    {
        public static void MyTest()
        {
            //test router
            var router = new RemoteAgencyRouter<byte[], object>();
            
            //Server
            var originalService = new Server2();
            using var serverRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(true);
            router.AddRemoteAgencyInstance(serverRemoteAgencyInstance);
            var serverSiteId = serverRemoteAgencyInstance.SiteId;
            var serviceWrapperInstanceId = serverRemoteAgencyInstance.CreateServiceWrapper(originalService);

            //Client
            using var clientRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(true);
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance);
            var clientProxy = clientRemoteAgencyInstance.CreateProxy<ITest2>(serverSiteId, serviceWrapperInstanceId,
                //ReSharper disable once UnusedVariable
                out var clientProxyInstanceId);

            //Run test
            long value = 100;

            Console.WriteLine("Add:");
            clientProxy.Add(ref value);
            Console.WriteLine(value);

            Console.WriteLine("Read:");
            clientProxy.Read(out var parameter);
            Console.WriteLine(parameter);

            var entity = new EntityInTest2
            {
                FromClientToServerProperty = "SetFromClient",
                TwoWayProperty = "SetFromClient"
            };

            Console.WriteLine("Process:");
            clientProxy.Process(entity);
            Console.WriteLine($"Client side: entity.FromClientToServerProperty (should be SetFromClient): {entity.FromClientToServerProperty}");
            Console.WriteLine($"Client side: entity.TwoWayProperty (should be SetFromServer): {entity.TwoWayProperty}");

            Console.Write("Press any key to quit...");
            Console.ReadKey(true);
            Console.WriteLine();
        }
    }
}
