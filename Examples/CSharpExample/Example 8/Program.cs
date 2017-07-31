using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretNest.RemoteAgency;
using SecretNest.RemoteAgency.Attributes;

namespace Test
{
    public delegate string MyTestHandler(string value);

    public interface IHello
    {
        [EventParameterIgnored("sender")]
        event EventHandler WorldOpened;

        event MyTestHandler MyTestEvent;
        void HelloWorld();
    }

    class Hello : IHello
    {
        public event EventHandler WorldOpened;
        public event MyTestHandler MyTestEvent;

        public void HelloWorld()
        {
            WorldOpened?.Invoke(this, EventArgs.Empty);

            string value = MyTestEvent?.Invoke("Request");
            if (value == "Response")
                Console.WriteLine("Pass.");
        }
    }

    class Program
    {
        static Dictionary<Guid, RemoteAgencyManagerEncapsulated> sites = new Dictionary<Guid, RemoteAgencyManagerEncapsulated>();

        static void Main(string[] args)
        {
            RemoteAgencyManagerEncapsulated clientSite = new RemoteAgencyManagerEncapsulated(true, false);
            RemoteAgencyManagerEncapsulated serverSite = new RemoteAgencyManagerEncapsulated(false, true);

            clientSite.MessageForSendingPrepared += OnMessageForSendingPrepared;
            serverSite.MessageForSendingPrepared += OnMessageForSendingPrepared;

            clientSite.DefaultTargetSiteId = serverSite.SiteId;

            sites.Add(clientSite.SiteId, clientSite);
            sites.Add(serverSite.SiteId, serverSite);

            clientSite.Connect();
            serverSite.Connect();

            Hello serviceObject = new Hello();
            serverSite.AddServiceWrapper<IHello>(serviceObject, out Guid serviceWrapperInstanceId);

            IHello proxy = clientSite.AddProxy<IHello>(serviceWrapperInstanceId, out _);
            proxy.WorldOpened += Proxy_WorldOpened;
            proxy.MyTestEvent += Proxy_MyTestEvent;

            proxy.HelloWorld();

            Console.WriteLine("Finished.");
            Console.ReadKey(); //Pause before quit.
            ((IDisposable)proxy).Dispose();
        }

        private static void Proxy_WorldOpened(object sender, EventArgs e)
        {
            Console.WriteLine("Hello World!");
        }

        private static string Proxy_MyTestEvent(string value)
        {
            if (value == "Request")
                return "Response";
            else
                return "Unknown";
        }

        private static void OnMessageForSendingPrepared(object sender, RemoteAgencyManagerMessageForSendingEventArgs<string> e)
        {
            //Async mode
            Task.Run(() =>
                sites[e.TargetSiteId].ProcessPackagedMessage(e.Message));
        }
    }
}