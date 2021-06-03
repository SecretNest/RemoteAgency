using SecretNest.RemoteAgency;
using System;
using System.Collections.Generic;

namespace Test.CSharp.Test4
{
    public interface ITest4
    {
        T ReturnItself<T>(T value) where T : struct;

        #nullable enable
        string GetGenericTypeName<T>() where T : class?;
        #nullable disable

        string GetGenericTypeName2<T>() where T : notnull;

        void Supported<T1, T2>(T1 obj) where T1 : T2, new();

        void Supported<T1, T2, T3>(T1 obj, out T2 obj2) where T1 : IEnumerable<T3> where T2 : Exception, new();
    }

    public class Server4 : ITest4
    {
        public T ReturnItself<T>(T value) where T : struct
        {
            return value;
        }

#nullable enable
        public string GetGenericTypeName<T>() where T : class?
        {
#nullable disable
            var name = typeof(T).FullName;
            return name ?? "<null>";
        }

        public string GetGenericTypeName2<T>() where T : notnull
        {
            return typeof(T).FullName;
        }

        public void Supported<T1, T2>(T1 obj) where T1 : T2, new()
        {
            Console.WriteLine($"Server side: T1: {typeof(T1).FullName}");
            Console.WriteLine($"Server side: T2: {typeof(T2).FullName}");
        }

        public void Supported<T1, T2, T3>(T1 obj, out T2 obj2) where T1 : IEnumerable<T3> where T2 : Exception, new()
        {
            Console.WriteLine($"Server side: T1: {typeof(T1).FullName}");
            Console.WriteLine($"Server side: T2: {typeof(T2).FullName}");
            Console.WriteLine($"Server side: T3: {typeof(T3).FullName}");

            var myException = new NotSupportedException("oops.");
            obj2 = __refvalue(__makeref(myException), T2);
        }
    }

    public sealed class TestCode
    {
        public static void MyTest()
        {
            //test router
            var router = new RemoteAgencyRouter<byte[], object>();

            //Server
            var originalService = new Server4();
            using var serverRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(true);
            router.AddRemoteAgencyInstance(serverRemoteAgencyInstance);
            var serverSiteId = serverRemoteAgencyInstance.SiteId;
            var serviceWrapperInstanceId = serverRemoteAgencyInstance.CreateServiceWrapper(originalService);

            //Client
            using var clientRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(true);
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance);
            var clientProxy = clientRemoteAgencyInstance.CreateProxy<ITest4>(serverSiteId, serviceWrapperInstanceId).ProxyGeneric;

            //Run test
            Console.WriteLine("ReturnItself(1024):");
            Console.WriteLine(clientProxy.ReturnItself(1024));

            Console.WriteLine("GetGenericTypeName(Test.CSharp.Test4.TestCode):");
            Console.WriteLine(clientProxy.GetGenericTypeName<TestCode>());

            Console.WriteLine("GetGenericTypeName2(System.String):");
            Console.WriteLine(clientProxy.GetGenericTypeName2<string>());

            Console.WriteLine("Supported(Test.CSharp.Test4.TestCode, System.Object):");
            clientProxy.Supported<TestCode, object>(null);

            Console.WriteLine("Supported(System.Collections.Generic.ICollection<Test.CSharp.Test4.TestCode>), System.NotSupportedException, Test.CSharp.Test4.TestCode):");
            clientProxy.Supported<ICollection<TestCode>, NotSupportedException, TestCode>(new List<TestCode>(), out var exception);
            Console.WriteLine($"Out parameter type: {exception.GetType().FullName}");
            Console.WriteLine($"Out parameter message: {exception.Message}");

            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
            Console.WriteLine();
        }
    }
}

