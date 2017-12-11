using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretNest.RemoteAgency;
using System.Runtime.Serialization;

namespace Test
{
    public interface IHello
    {
        string MyProperty { get; set; }
    }

    class Hello : IHello
    {
        string value = null;
        public string MyProperty
        {
            get
            {
                Console.WriteLine("Debug: MyProperty.Get");
                return value;
            }
            set
            {
                Console.WriteLine("Debug: MyProperty.Set");
                this.value = value;
            }
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

            Console.WriteLine("Client: " + clientSite.SiteId.ToString());
            Console.WriteLine("Server: " + serverSite.SiteId.ToString());

            clientSite.Connect();
            serverSite.Connect();

            Hello serviceObject = new Hello();
            serverSite.AddServiceWrapper<IHello>(serviceObject, out Guid serviceWrapperInstanceId);

            IHello proxy = clientSite.AddProxy<IHello>(serviceWrapperInstanceId, out _);

            string value = "value";
            proxy.MyProperty = value;
            var result = proxy.MyProperty;

            if (result == "value")
                Console.WriteLine("Pass.");

            Console.WriteLine("Finished.");
            Console.ReadKey(); //Pause before quit.
        }

        private static void OnMessageForSendingPrepared(object sender, RemoteAgencyManagerMessageForSendingEventArgs<string> e)
        {
            try
            {
                sites[e.TargetSiteId].ProcessPackagedMessage(e.Message);
            }
            catch
            {
                Console.WriteLine("Error TargetSiteId: " + e.TargetSiteId.ToString());
                throw;
            }
            //Async mode
            //Task.Run(() =>
            //    sites[e.TargetSiteId].ProcessPackagedMessage(e.Message));
        }
    }
}