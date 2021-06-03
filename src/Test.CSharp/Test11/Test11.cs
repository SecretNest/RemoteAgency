using System;
using System.Threading;
using SecretNest.RemoteAgency;
using SecretNest.RemoteAgency.Attributes;

namespace Test.CSharp.Test11
{
    [ThreadLock("MyTaskScheduler")]
    public interface ITest11
    {
        void Hello();
    }

    public class Test11 : ITest11
    {
        public void Hello()
        {
            Console.WriteLine("Server side: Thread: {0}", Thread.CurrentThread.ManagedThreadId);
        }
    }

    public static class TestCode
    {
        public static void MyTest()
        {
            //test router
            var router = new RemoteAgencyRouter<byte[], object>();

            var originalService = new Test11();

            //Server 1
            using var serverRemoteAgencyInstance1 = RemoteAgencyBase.CreateWithBinarySerializer(true);
            serverRemoteAgencyInstance1.TryCreateAddSequentialScheduler("MyTaskScheduler", out var taskScheduler1);
            router.AddRemoteAgencyInstance(serverRemoteAgencyInstance1);
            var serverSite1Id = serverRemoteAgencyInstance1.SiteId;
            var serviceWrapperInstance1Id = serverRemoteAgencyInstance1.CreateServiceWrapper(originalService);

            //Client 1
            using var clientRemoteAgencyInstance1 = RemoteAgencyBase.CreateWithBinarySerializer(true);
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance1);
            var clientCreatedProxy1 = clientRemoteAgencyInstance1.CreateProxy<Test11>(serverSite1Id, serviceWrapperInstance1Id);
            var clientProxy1 = clientCreatedProxy1.ProxyGeneric;
            var clientProxyInstance1Id = clientCreatedProxy1.InstanceId;

            //Run test in client 1
            Console.WriteLine("Run in client 1: all server side thread should be same, but may not be same as client side thread.");
            Console.WriteLine("Client side: Thread: {0}", Thread.CurrentThread.ManagedThreadId);
            var jobs = new Thread[5];
            for (var i = 0; i < 5; i++)
            {
                jobs[i] = new Thread(clientProxy1.Hello);
            }
            for (var i = 0; i < 5; i++)
            {
                jobs[i].Start();
            }
            for (var i = 0; i < 5; i++)
            {
                jobs[i].Join();
            }

            router.RemoveRemoteAgencyInstance(serverSite1Id);
            router.RemoveRemoteAgencyInstance(clientProxyInstance1Id);

            //Server 2
            using var serverRemoteAgencyInstance2 = RemoteAgencyBase.CreateWithBinarySerializer(true);
            serverRemoteAgencyInstance2.TryCreateAddSequentialScheduler("MyTaskScheduler", out var taskScheduler2, true);
            router.AddRemoteAgencyInstance(serverRemoteAgencyInstance2);
            var serverSite2Id = serverRemoteAgencyInstance2.SiteId;
            var serviceWrapperInstance2Id = serverRemoteAgencyInstance2.CreateServiceWrapper(originalService);

            //Client 2
            using var clientRemoteAgencyInstance2 = RemoteAgencyBase.CreateWithBinarySerializer(true);
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance2);
            var clientProxy2 = clientRemoteAgencyInstance2.CreateProxy<Test11>(serverSite2Id, serviceWrapperInstance2Id).ProxyGeneric;

            //Run test in client 2
            Console.WriteLine("Run in client 2: all server side thread should be same as the thread specified.");
            var theThreadToRun = new Thread(() =>
            {
                Console.WriteLine("Runner Thread: {0}", Thread.CurrentThread.ManagedThreadId);
                // ReSharper disable once AccessToDisposedClosure
                taskScheduler2.Run();
            });
            theThreadToRun.Start();
            for (var i = 0; i < 5; i++)
            {
                jobs[i] = new Thread(clientProxy2.Hello);
            }
            for (var i = 0; i < 5; i++)
            {
                jobs[i].Start();
            }
            for (var i = 0; i < 5; i++)
            {
                jobs[i].Join();
            }

            Console.Write("Disposing task schedulers...");
            taskScheduler1.Dispose();
            taskScheduler2.Dispose();
            
            theThreadToRun.Join();

            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
            Console.WriteLine();
        }
    }
}
