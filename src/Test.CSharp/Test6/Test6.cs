using System;
using System.Threading;
using SecretNest.RemoteAgency;
using SecretNest.RemoteAgency.Attributes;

namespace Test.CSharp.Test6
{
    interface ITest6
    {
        int this[int index]
        {
            get;
            set;
        }

        [OperatingTimeoutTime(1000, 2000)]
        int this[string name]
        {
            get;
            set;
        }
    }

    public class Server6 : ITest6
    {
        private readonly int[] _data = {101, 102, 103, 104, 105};

        public int this[int index]
        {
            get => _data[index];
            set => _data[index] = value;
        }

        public int this[string name]
        {
            get
            {
                if (int.TryParse(name, out var index))
                {
                    return _data[index];
                }
                else
                {
                    Thread.Sleep(5000);
                    return -1;
                }
            }
            set
            {
                if (int.TryParse(name, out var index))
                {
                    _data[index] = value;
                }
                else
                {
                    Thread.Sleep(5000);
                }
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
            var originalService = new Server6();
            using var serverRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(true);
            router.AddRemoteAgencyInstance(serverRemoteAgencyInstance);
            var serverSiteId = serverRemoteAgencyInstance.SiteId;
            var serviceWrapperInstanceId = serverRemoteAgencyInstance.CreateServiceWrapper(originalService);

            //Client
            using var clientRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(true);
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance);
            var clientProxy = clientRemoteAgencyInstance.CreateProxy<ITest6>(serverSiteId,
                serviceWrapperInstanceId,
                //ReSharper disable once UnusedVariable
                out var clientProxyInstanceId);

            Console.WriteLine("this[2](Get, 102):");
            Console.WriteLine(clientProxy[2]);

            Console.WriteLine("this[4](Set+Get, No return):");
            clientProxy[4] += 100;

            Console.WriteLine("this[\"4\"](Get, 204):");
            Console.WriteLine(clientProxy["4"]);

            Console.WriteLine("this[\"Timeout\"](Get, Timeout):");
            try
            {
                Console.WriteLine(clientProxy["Timeout"]);
            }
            catch (Exception e)
            {
                Console.WriteLine("Predicted Exception: " + e);
            }

            Console.WriteLine("this[\"Timeout\"](Set, Timeout):");
            try
            {
                clientProxy["Timeout"] = 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("Predicted Exception: " + e);
            }

            Console.Write("Press any key to quit...");
            Console.ReadKey(true);
            Console.WriteLine();
        }
    }
}
