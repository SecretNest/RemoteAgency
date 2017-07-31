using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecretNest.RemoteAgency;
using SecretNest.RemoteAgency.Attributes;
using System.Collections.Concurrent;
using System.Reflection;
using System.IO;

namespace Test
{
    public delegate string MyTestHandler(string value);

    [ProxyCacheable]
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
        static Dictionary<Guid, RemoteAgencyManager<string, string, object>> sites = new Dictionary<Guid, RemoteAgencyManager<string, string, object>>();
        static ConcurrentDictionary<Type, Tuple<Assembly, bool>> assemblyCache = new ConcurrentDictionary<Type, Tuple<Assembly, bool>>();

        static void Main(string[] args)
        {
            DataContractSerializerEntityCodeBuilder entityCodeBuilder = new DataContractSerializerEntityCodeBuilder();
            DataContractSerializerSerializingHelper serializingHelper = new DataContractSerializerSerializingHelper();
            DataContractToJsonPackingHelper packingHelper = new DataContractToJsonPackingHelper();
            ProxyCreator<string, object> proxyCreator = new ProxyCreator<string, object>(entityCodeBuilder, typeof(DataContractSerializerSerializingHelper));
            ServiceWrapperCreator<string, object> serviceWrapperCreator = new ServiceWrapperCreator<string, object>(entityCodeBuilder, typeof(DataContractSerializerSerializingHelper));

            RemoteAgencyManager<string, string, object> clientSite1 = new RemoteAgencyManager<string, string, object>(packingHelper, serializingHelper);
            RemoteAgencyManager<string, string, object> clientSite2 = new RemoteAgencyManager<string, string, object>(packingHelper, serializingHelper);
            RemoteAgencyManager<string, string, object> serverSite = new RemoteAgencyManager<string, string, object>(packingHelper, serializingHelper);

            clientSite1.MessageForSendingPrepared += OnMessageForSendingPrepared;
            clientSite2.MessageForSendingPrepared += OnMessageForSendingPrepared;
            serverSite.MessageForSendingPrepared += OnMessageForSendingPrepared;

            proxyCreator.LoadCachedAssemblyCallback = LoadCachedAssembly;
            //proxyCreator.SaveCachedAssemblyCallback = SaveCachedAssembly;
            proxyCreator.SaveCachedAssemblyImageCallback = SaveCachedAssemblyImage;

            clientSite1.DefaultTargetSiteId = serverSite.SiteId;
            clientSite2.DefaultTargetSiteId = serverSite.SiteId;

            sites.Add(clientSite1.SiteId, clientSite1);
            sites.Add(clientSite2.SiteId, clientSite2);
            sites.Add(serverSite.SiteId, serverSite);

            clientSite1.Connect();
            clientSite2.Connect();
            serverSite.Connect();

            Hello serviceObject = new Hello();
            serverSite.AddServiceWrapper<IHello>(serviceWrapperCreator, serviceObject, out Guid serviceWrapperInstanceId);

            IHello proxy1 = clientSite1.AddProxy<IHello>(proxyCreator, serviceWrapperInstanceId, out var proxyInstance1Id);
            proxy1.WorldOpened += Proxy_WorldOpened1;
            proxy1.MyTestEvent += Proxy_MyTestEvent;

            IHello proxy2 = clientSite2.AddProxy<IHello>(proxyCreator, serviceWrapperInstanceId, out var proxyInstance2Id);
            proxy2.WorldOpened += Proxy_WorldOpened2;
            proxy2.MyTestEvent += Proxy_MyTestEvent;

            proxy1.HelloWorld();

            Console.WriteLine("Finished.");
            Console.ReadKey(); //Pause before quit.
            clientSite1.RemoveManagingObject(proxyInstance1Id, true);
            clientSite2.RemoveManagingObject(proxyInstance2Id, true);
        }

        private static void Proxy_WorldOpened1(object sender, EventArgs e)
        {
            Console.WriteLine("Proxy1: Hello World!");
        }

        private static void Proxy_WorldOpened2(object sender, EventArgs e)
        {
            Console.WriteLine("Proxy2: Hello World!");
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

        static Assembly LoadCachedAssembly(Type type, out bool disposeRequired)
        {
            if (assemblyCache.TryGetValue(type, out var item))
            {
                disposeRequired = item.Item2;
                return item.Item1;
            }
            else
            {
                disposeRequired = false;
                return null;
            }
        }

        //static void SaveCachedAssembly(Type type, bool disposeRequired, Assembly assembly)
        //{
        //    Tuple<Assembly, bool> item = new Tuple<Assembly, bool>(assembly, disposeRequired);
        //    assemblyCache.AddOrUpdate(type, item, (i, j) => item);
        //}

        static void SaveCachedAssemblyImage(Type type, bool disposeRequired, byte[] image)
        {
            Assembly assembly;
            using (MemoryStream ms = new MemoryStream(image))
            {
                assembly = System.Runtime.Loader.AssemblyLoadContext.Default.LoadFromStream(ms);
            }
            Tuple<Assembly, bool> item = new Tuple<Assembly, bool>(assembly, disposeRequired);
            assemblyCache.AddOrUpdate(type, item, (i, j) => item);
        }
    }
}