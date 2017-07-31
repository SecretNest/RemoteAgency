using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretNest.RemoteAgency;

namespace Test
{
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

            //More testing code should be written here.

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