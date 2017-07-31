using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretNest.RemoteAgency;
using System.Runtime.Serialization;
using SecretNest.RemoteAgency.Attributes;

namespace Test
{
    [DataContract]
    public class Parameter<TParameter>
    {
        [DataMember] public TParameter MyValue { get; set; }
    }

    public interface IHello<TInterface>
    {
        TInterface NormalMethod(TInterface value1);

        TMethod GenericMethod<TMethod, TParameter>(TInterface value1, TMethod value2, Parameter<TParameter> value3);
    }

    class Hello : IHello<int>
    {
        public int NormalMethod(int value1)
        {
            return value1++;
        }

        public TMethod GenericMethod<TMethod, TParameter>(int value1, TMethod value2, Parameter<TParameter> value3)
        {
            Console.WriteLine("TMethod: " + typeof(TMethod).Name);
            Console.WriteLine("TParameter: " + typeof(TParameter).Name);
            Console.WriteLine("value1: " + value1.ToString());
            Console.WriteLine("value2: " + value2.ToString());
            Console.WriteLine("value3.MyValue: " + value3.MyValue.ToString());
            return value2;
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
            serverSite.AddServiceWrapper<IHello<int>>(serviceObject, out Guid serviceWrapperInstanceId);

            IHello<int> proxy = clientSite.AddProxy<IHello<int>>(serviceWrapperInstanceId, out _);

            int value1 = 100;
            int result1 = proxy.NormalMethod(value1);
            if (result1 == 101)
                Console.WriteLine("NormalMethod: Pass.");

            string value2 = "TestString";
            Parameter<long> parameter = new Parameter<long>() { MyValue = 1000 };
            string result2 = proxy.GenericMethod(value1, value2, parameter);
            if (result2 == "TestString")
                Console.WriteLine("GenericMethod: Pass.");

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