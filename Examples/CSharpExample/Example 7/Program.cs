using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretNest.RemoteAgency;

namespace Test
{
    public interface IHello
    {
        event EventHandler WorldOpened;
        void HelloWorld();
    }

    class Hello : IHello
    {
        public event EventHandler WorldOpened;

        public void HelloWorld()
        {
            //sender cannot be this due to serializing support missing.
            WorldOpened?.Invoke(null, EventArgs.Empty);
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

            IHello proxy = clientSite.AddProxy<IHello>(serviceWrapperInstanceId, out var proxyInstanceId);
            proxy.WorldOpened += Proxy_WorldOpened;

            proxy.HelloWorld();

            Console.WriteLine("Finished.");
            Console.ReadKey(); //Pause before quit.
            clientSite.RemoveManagingObject(proxyInstanceId, true);
            serverSite.RemoveManagingObject(serviceWrapperInstanceId, true);
        }

        private static void Proxy_WorldOpened(object sender, EventArgs e)
        {
            Console.WriteLine("Hello World!");
        }

        private static void OnMessageForSendingPrepared(object sender, RemoteAgencyManagerMessageForSendingEventArgs<string> e)
        {
            //Async mode
            Task.Run(() =>
                sites[e.TargetSiteId].ProcessPackagedMessage(e.Message));
        }
    }
}