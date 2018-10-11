using SecretNest.RemoteAgency;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Test
{
    public interface IMetadataTest
    {
        void PrintMetadata();
    }

    class MetadataTest : IMetadataTest
    {
        public void PrintMetadata()
        {
            Console.WriteLine("\nMetadata printing from server:\n" + MessageInstanceMetadataService.CurrentMessageInstanceMetadata);
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

            clientSite.AfterMessageReceived += ClientSite_AfterMessageReceived;
            clientSite.BeforeMessageSending += ClientSite_BeforeMessageSending;
            serverSite.AfterMessageReceived += ServerSite_AfterMessageReceived;
            serverSite.BeforeMessageSending += ServerSite_BeforeMessageSending;

            clientSite.DefaultTargetSiteId = serverSite.SiteId;

            sites.Add(clientSite.SiteId, clientSite);
            sites.Add(serverSite.SiteId, serverSite);

            clientSite.Connect();
            serverSite.Connect();

            MetadataTest serviceObject = new MetadataTest();
            serverSite.AddServiceWrapper<IMetadataTest>(serviceObject, out Guid serviceWrapperInstanceId);

            IMetadataTest proxy = clientSite.AddProxy<IMetadataTest>(serviceWrapperInstanceId, out _);

            proxy.PrintMetadata();

            Console.WriteLine("Finished.");
            Console.ReadKey(); //Pause before quit.
        }

        private static void ClientSite_AfterMessageReceived(object sender, BeforeMessageProcessingEventArgsBase<string> e)
        {
            WriteEventArgs("ClientSite_AfterMessageReceived", e);
        }

        private static void ClientSite_BeforeMessageSending(object sender, BeforeMessageProcessingEventArgsBase<string> e)
        {
            WriteEventArgs("ClientSite_BeforeMessageSending", e);
        }

        private static void ServerSite_AfterMessageReceived(object sender, BeforeMessageProcessingEventArgsBase<string> e)
        {
            //e.FurtherProcessing = MessageFurtherProcessing.TerminateWithExceptionReturned;
            WriteEventArgs("ServerSite_AfterMessageReceived", e);
        }

        private static void ServerSite_BeforeMessageSending(object sender, BeforeMessageProcessingEventArgsBase<string> e)
        {
            //e.FurtherProcessing = MessageFurtherProcessing.ReplacedWithException;
            WriteEventArgs("ServerSite_BeforeMessageSending", e);
        }

        private static void WriteEventArgs(string eventName, BeforeMessageProcessingEventArgsBase<string> e)
        {
            Console.WriteLine(string.Format("\n{0}:\n{1}\nFurther Processing: {2}\nMessage: {3}", eventName, e.MessageInstanceMetadata, e.FurtherProcessing, e.SerializedMessage));
            if (e.IsException)
            {
                Console.WriteLine("Exception Type: " + (e as BeforeExceptionMessageProcessingEventArgs<string>).ExceptionType.Name);
            }
            else
            {
                var arg = e as BeforeMessageProcessingEventArgs<string>;
                if (arg.GenericArguments != null && arg.GenericArguments.Length > 0)
                {
                    Console.WriteLine("Generic Arguments: " + string.Join(", ", Array.ConvertAll(arg.GenericArguments, i => i.Name)));
                }
            }
        }

        private static void OnMessageForSendingPrepared(object sender, RemoteAgencyManagerMessageForSendingEventArgs<string> e)
        {
            //Async mode
            Task.Run(() =>
                sites[e.TargetSiteId].ProcessPackagedMessage(e.Message));
        }
    }
}