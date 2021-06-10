Imports SecretNest.RemoteAgency
Imports SecretNest.RemoteAgency.Attributes

Namespace Test1
    Public Interface ITest1
        Function Add(a As Integer, b As Integer) As Integer

        'When unmark this line below, an exception about asset naming conflicts will be thrown.
        '<CustomizedAssetName("AddDouble")>
        Function Add(a As Single, b As Single) As Single

        <CustomizedAssetName("AddDouble")>
        Function Add(a As Double, b As Double) As Double

        <AsyncMethod>
        Function AsyncCall() As Task(Of Integer)
    End Interface

    Public Class Server1
        Implements ITest1

        Public Function Add(a As Integer, b As Integer) As Integer Implements ITest1.Add
            Return a + b
        End Function

        Public Function Add(a As Single, b As Single) As Single Implements ITest1.Add
            Return a + b
        End Function

        Public Function Add(a As Double, b As Double) As Double Implements ITest1.Add
            Return a + b
        End Function

        Public Function AsyncCall() As Task(Of Integer) Implements ITest1.AsyncCall
            Return Task.FromResult(100)
        End Function
    End Class

    Public NotInheritable Class TestCode
        Public Shared Sub MyTest()
            'test router
            Dim router = New RemoteAgencyRouter(Of Byte(), Object)

            'Server
            Dim originalService As New Server1()
            Dim serverRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(True)
            router.AddRemoteAgencyInstance(serverRemoteAgencyInstance)
            Dim serverSiteId = serverRemoteAgencyInstance.SiteId
            Dim serviceWrapperInstanceId = serverRemoteAgencyInstance.CreateServiceWrapper(originalService)

            'Client
            Dim clientRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(True)
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance)
            Dim clientProxy = clientRemoteAgencyInstance.CreateProxy(Of ITest1)(serverSiteId, serviceWrapperInstanceId).ProxyGeneric

            'Run test
            Console.WriteLine("Add(Integer): 1 + 2")
            Console.WriteLine(clientProxy.Add(1, 2))

            Console.WriteLine("Add(Single): 1.1 + 2.2")
            Console.WriteLine(clientProxy.Add(1.1!, 2.2!))

            Console.WriteLine("Add(Double): 1.1 + 2.2")
            Console.WriteLine(clientProxy.Add(1.1, 2.2))

            Console.WriteLine("AsyncCall(100):")
            Console.WriteLine(clientProxy.AsyncCall().Result)

            Console.Write("Press any key to continue...")
            Console.ReadKey(True)
            Console.WriteLine()

            serverRemoteAgencyInstance.Dispose()
            clientRemoteAgencyInstance.Dispose()
        End Sub
    End Class

End Namespace