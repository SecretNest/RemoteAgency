Imports System.Threading
Imports SecretNest.RemoteAgency
Imports SecretNest.RemoteAgency.Attributes
Imports SecretNest.TaskSchedulers

Namespace Test11
    <ThreadLock("MyTaskScheduler")>
    Public Interface ITest11
        Sub Hello()
    End Interface

    Public Class Server11
        Implements ITest11

        Public Sub Hello() Implements ITest11.Hello
            Console.WriteLine("Server side: Thread: {0}", Thread.CurrentThread.ManagedThreadId)
        End Sub
    End Class

    Public NotInheritable Class TestCode
        Public Shared Sub MyTest()
            'test router
            Dim router = New RemoteAgencyRouter(Of Byte(), Object)

            Dim originalService As New Server11()

            'Server 1
            Dim taskScheduler1 As SequentialScheduler = Nothing
            Dim serverRemoteAgencyInstance1 = RemoteAgencyBase.CreateWithBinarySerializer(True)
            serverRemoteAgencyInstance1.TryCreateAndAddSequentialScheduler("MyTaskScheduler", taskScheduler1)
            router.AddRemoteAgencyInstance(serverRemoteAgencyInstance1)
            Dim serverSite1Id = serverRemoteAgencyInstance1.SiteId
            Dim serviceWrapperInstance1Id = serverRemoteAgencyInstance1.CreateServiceWrapper(originalService)

            'Client 1
            Dim clientRemoteAgencyInstance1 = RemoteAgencyBase.CreateWithBinarySerializer(True)
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance1)
            Dim clientCreatedProxy1 = clientRemoteAgencyInstance1.CreateProxy(Of ITest11)(serverSite1Id, serviceWrapperInstance1Id)
            Dim clientProxy1 = clientCreatedProxy1.ProxyGeneric
            Dim clientProxyInstance1Id = clientCreatedProxy1.InstanceId

            'Run test in client 1
            Console.WriteLine("Run in client 1: all server side thread should be same, but may not be same as client side thread.")
            Console.WriteLine("Client side: Thread: {0}", Thread.CurrentThread.ManagedThreadId)
            Dim jobs(4) As Thread
            For i As Integer = 0 To 4
                jobs(i) = New Thread(AddressOf clientProxy1.Hello)
            Next
            For i As Integer = 0 To 4
                jobs(i).Start()
            Next
            For i As Integer = 0 To 4
                jobs(i).Join()
            Next

            router.RemoveRemoteAgencyInstance(serverSite1Id)
            router.RemoveRemoteAgencyInstance(clientProxyInstance1Id)

            'Server 2
            Dim taskScheduler2 As SequentialScheduler = Nothing
            Dim serverRemoteAgencyInstance2 = RemoteAgencyBase.CreateWithBinarySerializer(True)
            serverRemoteAgencyInstance2.TryCreateAndAddSequentialScheduler("MyTaskScheduler", taskScheduler2, True)
            router.AddRemoteAgencyInstance(serverRemoteAgencyInstance2)
            Dim serverSite2Id = serverRemoteAgencyInstance2.SiteId
            Dim serviceWrapperInstance2Id = serverRemoteAgencyInstance2.CreateServiceWrapper(originalService)

            'Client 2
            Dim clientRemoteAgencyInstance2 = RemoteAgencyBase.CreateWithBinarySerializer(True)
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance2)
            Dim clientProxy2 = clientRemoteAgencyInstance2.CreateProxy(Of ITest11)(serverSite2Id, serviceWrapperInstance2Id).ProxyGeneric

            'Run test in client 2
            Console.WriteLine("Run in client 2: all server side thread should be same as the thread specified.")
            Dim theThreadToRun As New Thread(Sub()
                                                 Console.WriteLine("Runner Thread: {0}", Thread.CurrentThread.ManagedThreadId)
                                                 ' ReSharper disable once AccessToDisposedClosure
                                                 taskScheduler2.Run()
                                             End Sub)
            theThreadToRun.Start()
            For i As Integer = 0 To 4
                jobs(i) = New Thread(AddressOf clientProxy2.Hello)
            Next
            For i As Integer = 0 To 4
                jobs(i).Start()
            Next
            For i As Integer = 0 To 4
                jobs(i).Join()
            Next

            Console.Write("Disposing task schedulers...")
            taskScheduler1.Dispose()
            taskScheduler2.Dispose()

            theThreadToRun.Join()

            Console.Write("Press any key to continue...")
            Console.ReadKey(True)
            Console.WriteLine()

            serverRemoteAgencyInstance1.Dispose()
            clientRemoteAgencyInstance1.Dispose()
            serverRemoteAgencyInstance2.Dispose()
            clientRemoteAgencyInstance2.Dispose()
        End Sub
    End Class
End Namespace
