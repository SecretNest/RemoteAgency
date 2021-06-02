using System;
using SecretNest.RemoteAgency;
using SecretNest.RemoteAgency.Attributes;

namespace Test.CSharp.Test8
{
    public interface ITest8
    {
        [EventParameterIgnored("sender")]
        event EventHandler MyEventWithHandler;

        void Test();
    }

    public class Server8 : ITest8
    {
        public event EventHandler MyEventWithHandler;

        public void Test()
        {
            Console.WriteLine("Server side: Method called.");
            MyEventWithHandler?.Invoke(this, EventArgs.Empty);
        }
    }

    public static class TestCode
    {
        public static void MyTest()
        {
            //test router
            var router = new RemoteAgencyRouter<byte[], object>();

            //Server
            var originalService = new Server8();
            using var serverRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(true);
            router.AddRemoteAgencyInstance(serverRemoteAgencyInstance);
            var serverSiteId = serverRemoteAgencyInstance.SiteId;
            var serviceWrapperInstanceId = serverRemoteAgencyInstance.CreateServiceWrapper(originalService);

            //Client 1
            using var clientRemoteAgencyInstance1 = RemoteAgencyBase.CreateWithBinarySerializer(true);
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance1);
            var clientProxy1 = clientRemoteAgencyInstance1.CreateProxy<ITest8>(serverSiteId,
                serviceWrapperInstanceId,
                //ReSharper disable once UnusedVariable
                out var clientProxyInstance1Id);

            //Client 2
            using var clientRemoteAgencyInstance2 = RemoteAgencyBase.CreateWithBinarySerializer(true);
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance2);
            var clientProxy2 = clientRemoteAgencyInstance2.CreateProxy<ITest8>(serverSiteId,
                serviceWrapperInstanceId,
                //ReSharper disable once UnusedVariable
                out var clientProxyInstance2Id);

            //Run test
            Console.WriteLine("Run(WithoutHandler)");
            clientProxy1.Test();

            Console.WriteLine("Run(HandlerFromClient1)");
            clientProxy1.MyEventWithHandler += ClientProxy1_MyEventWithHandler;
            clientProxy1.Test();

            Console.WriteLine("Run(HandlerFromClient1+2)");
            clientProxy2.MyEventWithHandler += ClientProxy2_MyEventWithHandler;
            clientProxy1.Test();

            Console.WriteLine("Run(HandlerFromClient1+2+2)");
            clientProxy2.MyEventWithHandler += ClientProxy2_MyEventWithHandler;
            clientProxy1.Test();

            Console.WriteLine("Run(HandlerFromClient2+2)");
            clientProxy1.MyEventWithHandler -= ClientProxy2_MyEventWithHandler;
            clientProxy1.Test();

            Console.WriteLine("Run(WithoutHandler2)");
            clientProxy2.MyEventWithHandler -= ClientProxy2_MyEventWithHandler;
            clientProxy1.Test();

            Console.WriteLine("Run(WithoutHandler2)");
            clientProxy1.MyEventWithHandler -= ClientProxy2_MyEventWithHandler; //useless coz no active handler registered.
            clientProxy1.Test();

            Console.WriteLine("Run(WithoutHandler)");
            clientProxy2.MyEventWithHandler -= ClientProxy2_MyEventWithHandler;
            clientProxy1.Test();

            Console.Write("Press any key to quit...");
            Console.ReadKey(true);
            Console.WriteLine();
        }

        private static void ClientProxy1_MyEventWithHandler(object sender, EventArgs e)
        {
            Console.WriteLine("Client1 Event Handler Called.");
        }

        private static void ClientProxy2_MyEventWithHandler(object sender, EventArgs e)
        {
            Console.WriteLine("Client2 Event Handler Called.");
        }
    }
}
