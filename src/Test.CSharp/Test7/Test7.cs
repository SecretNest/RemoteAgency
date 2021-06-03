using System;
using System.Threading;
using SecretNest.RemoteAgency;
using SecretNest.RemoteAgency.Attributes;

namespace Test.CSharp.Test7
{
    public interface ITest7
    {
        [AssetIgnored]
        DateTime Ignored { get; set; }

        [AssetOneWayOperating]
        DateTime OneWaySet { get; set; }

        [PropertyGetOneWayOperating]
        DateTime OneWayGet { get; }

        [OperatingTimeoutTime(1000, 2000)]
        DateTime TimeoutExceptionTest { get; set; }

        [LocalExceptionHandling]
        EntityInTest7 WithException { get; set; }
    }

    public class EntityInTest7
    {
        public string FromClientToServerProperty { get; set; }

        [ParameterReturnRequiredProperty("EntityTwoWayProperty", isIncludedInReturning: true)]
        public string TwoWayProperty { get; set; }
    }

    public class Server7 : ITest7
    {
        public DateTime Ignored {
            get => throw new Exception("You should never see this due to ignored.");
            set => throw new Exception("You should never see this due to ignored.");
        }

        public DateTime OneWaySet
        {
            get => DateTime.Now;
            set
            {
                Console.WriteLine("Server side: Operation received. Value={0}", value.ToLongDateString());
                throw new Exception("You should never receive this from client coz this is a one way (set) property.");
            }
        }

        public DateTime OneWayGet
        {
            get
            {
                Console.WriteLine("Server side: Operation received.");
                throw new Exception("You should never receive this from client coz this is a one way (set) property.");
            }
        }

        public DateTime TimeoutExceptionTest
        {
            get
            {
                Thread.Sleep(2000);
                return DateTime.Now;
            }
            set
            {
                Thread.Sleep(3000);
                _ = value;
            }
        }

        public EntityInTest7 WithException
        {
            get => null;
            set
            {
                value.FromClientToServerProperty = "ChangedByServer"; //should never returned
                value.TwoWayProperty = "SetBeforeException";
                throw new Exception("oops.");
            }
        }
    }

    public static class TestCode
    {
        public static void MyTest()
        {
            //test router
            var router = new RemoteAgencyRouter<byte[], object>();
            
            //Server
            var originalService = new Server7();
            using var serverRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(true);
            router.AddRemoteAgencyInstance(serverRemoteAgencyInstance);
            var serverSiteId = serverRemoteAgencyInstance.SiteId;
            var serviceWrapperInstanceId = serverRemoteAgencyInstance.CreateServiceWrapper(originalService);

            //Client
            using var clientRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(true);
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance);
            var clientProxy = clientRemoteAgencyInstance.CreateProxy<ITest7>(serverSiteId, serviceWrapperInstanceId).ProxyGeneric;
            clientRemoteAgencyInstance.ExceptionRedirected += ClientRemoteAgencyInstance_ExceptionRedirected;

            //Run test
            Console.WriteLine("Ignored(Exception):");
            try
            {
                _ = clientProxy.Ignored;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
            {
                Console.WriteLine("Predicted Exception: " + e);
            }
#pragma warning restore CA1031 // Do not catch general exception types

            Console.WriteLine("OneWaySet(NoClientException):");
            clientProxy.OneWaySet = DateTime.Now;

            Console.WriteLine("OneWayGet(default):");
            Console.WriteLine(clientProxy.OneWayGet);

            Console.WriteLine("TimeoutExceptionTest:");
            try
            {
                Console.WriteLine(clientProxy.TimeoutExceptionTest);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
            {
                Console.WriteLine("Predicted Exception: " + e);
            }
#pragma warning restore CA1031 // Do not catch general exception types

            var entity = new EntityInTest7
            {
                FromClientToServerProperty = "SetFromClient",
                TwoWayProperty = "SetFromClient"
            };

            Console.WriteLine("WithException(Exception):");
            try
            {
                clientProxy.WithException = entity;
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
            {
                Console.WriteLine("Predicted Exception: " + e.Message);
            }
#pragma warning restore CA1031 // Do not catch general exception types
            Console.WriteLine($"Client side: entity.FromClientToServerProperty (should be SetFromClient): {entity.FromClientToServerProperty}");
            Console.WriteLine($"Client side: entity.TwoWayProperty (should be SetBeforeException): {entity.TwoWayProperty}");

            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
            Console.WriteLine();
        }

        private static void ClientRemoteAgencyInstance_ExceptionRedirected(object sender, ExceptionRedirectedEventArgs e)
        {   
            Console.WriteLine($"Client side received exception: \n  Interface:{e.ServiceContractInterface.FullName}\n  InstanceId: {e.InstanceId}\n  AssetName: {e.AssetName}\n  ExceptionType: {e.RedirectedException.GetType().FullName}\n  ExceptionMessage: {e.RedirectedException.Message}");
        }
    }
}
