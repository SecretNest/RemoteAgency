using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretNest.RemoteAgency;
using System.Runtime.Serialization;

namespace Test
{
    [DataContract]
    public class Parameter
    {
        [DataMember] public string MyValue { get; set; }
    }

    public interface IHello
    {
        string NormalMethod(Parameter parameter, ref string refParameter, out int outParameter);
    }

    class Hello : IHello
    {
        public string NormalMethod(Parameter parameter, ref string refParameter, out int outParameter)
        {
            refParameter += "Changed";
            outParameter = 100;
            return parameter.MyValue;
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

            Parameter parameter = new Parameter() { MyValue = "ParameterValue" };
            string refParameter = "RefValue";
            var result = proxy.NormalMethod(parameter, ref refParameter, out int outParameter);
            if (result == "ParameterValue" && refParameter == "RefValueChanged" && outParameter == 100)
                Console.WriteLine("Pass.");

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