using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretNest.RemoteAgency;

namespace Test
{
    public interface IHello
    {
        void HelloWorld();
    }

    public interface IGoodbye
    {
        void Goodbye();
    }

    class Service : IHello, IGoodbye
    {
        public void HelloWorld()
        {
            Console.WriteLine("Hello World.");
        }

        public void Goodbye()
        {
            Console.WriteLine("Goodbye.");
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

            Service serviceObject = new Service();
            serverSite.AddServiceWrapper(serviceObject, new List<Type>() { typeof(IHello), typeof(IGoodbye) }, out Guid serviceWrapperInstanceId);

            IHello proxyHello = clientSite.AddProxy<IHello>(serviceWrapperInstanceId, out _);
            IGoodbye proxyGoodbye = clientSite.AddProxy<IGoodbye>(serviceWrapperInstanceId, out _);

            proxyHello.HelloWorld();
            proxyGoodbye.Goodbye();

            Console.WriteLine("Finished.");
            Console.ReadKey(); //Pause before quit.
        }

        private static void OnMessageForSendingPrepared(object sender, RemoteAgencyManagerMessageForSendingEventArgs<string> e)
        {
            //Async mode
            Task.Run(() =>
                sites[e.TargetSiteId].ProcessPackagedMessage(e.Message));
        }
    }
}