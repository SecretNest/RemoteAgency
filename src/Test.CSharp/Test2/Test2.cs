using System;
using System.Collections.Generic;
using SecretNest.RemoteAgency;
using SecretNest.RemoteAgency.Attributes;

namespace Test.CSharp.Test2
{
    public interface ITest2
    {
        [ReturnIgnored]
        int IgnoredParameter(int value, [ParameterIgnored] int ignored);

        void AddOne(ref long value);

        void Read(out long value);

        void Process([ParameterReturnRequiredProperty("TwoWayProperty")][ParameterReturnRequiredProperty(typeof(EntityHelperInTest2))] EntityInTest2 entity);
    }

    public class EntityInTest2
    {
        public string FromClientToServerProperty { get; set; }

        public string TwoWayProperty { get; set; }

        public List<int> ComplexResult { get; set; }
    }

    public class EntityHelperInTest2
    {
        [ReturnRequiredPropertyHelper]
        public bool Helper
        {
            get => _value.Contains(100);
            set
            {
                if (value)
                {
                    _value.Add(100);
                }
            }
        }

        private readonly List<int> _value;

        public EntityHelperInTest2(EntityInTest2 value)
        {
            _value = value.ComplexResult;
        }
    }

    public class Server2 : ITest2
    {
        private long _data;

        public int IgnoredParameter(int value, int ignored)
        {
            Console.WriteLine($"Server side: value (should be 100): {value}");
            Console.WriteLine($"Server side: ignored (should be 0 due to ignored): {ignored}");

            return value;
        }

        public void AddOne(ref long value)
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
            if (entity.ComplexResult.Contains(0))
            {
                entity.ComplexResult.Add(100); //This will be returned due to matching the Helper code in EntityHelperInTest2.
                entity.ComplexResult.Add(200); //This will not be returned.
            }
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
            var clientProxy = clientRemoteAgencyInstance.CreateProxy<ITest2>(serverSiteId, serviceWrapperInstanceId).ProxyGeneric;

            //Run test
            long value = 100;

            Console.WriteLine("AddOne:");
            clientProxy.AddOne(ref value);
            Console.WriteLine(value);

            Console.WriteLine("Read:");
            clientProxy.Read(out var parameter);
            Console.WriteLine(parameter);

            var entity = new EntityInTest2
            {
                FromClientToServerProperty = "SetFromClient",
                TwoWayProperty = "SetFromClient",
                ComplexResult = new List<int> {0}
            };

            Console.WriteLine("Process:");
            clientProxy.Process(entity);
            Console.WriteLine($"Client side: entity.FromClientToServerProperty (should be SetFromClient): {entity.FromClientToServerProperty}");
            Console.WriteLine($"Client side: entity.TwoWayProperty (should be SetFromServer): {entity.TwoWayProperty}");
            Console.WriteLine($"Client side: entity.ComplexResult.Contains(100) (should be true): {entity.ComplexResult.Contains(100)}");
            Console.WriteLine($"Client side: entity.ComplexResult.Contains(200) (should be false): {entity.ComplexResult.Contains(200)}");

            Console.WriteLine("IgnoredParameter(0 due to ignored):");
            Console.WriteLine(clientProxy.IgnoredParameter(100, 200));

            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
            Console.WriteLine();
        }
    }
}
