Imports SecretNest.RemoteAgency

Namespace Test5
    Public Interface ITest5(Of Out T As {Structure})
        ReadOnly Property Current As T

        Property Value As Integer
    End Interface

    Public Class Server5
        Implements ITest5(Of DateTime)

        Public ReadOnly Property Current As Date Implements ITest5(Of Date).Current
            Get
                Return DateTime.Now
            End Get
        End Property

        Public Property Value As Integer Implements ITest5(Of Date).Value
    End Class

    Public NotInheritable Class TestCode
        Public Shared Sub MyTest()
            'test router
            Dim router = New RemoteAgencyRouter(Of Byte(), Object)

            'Server
            Dim originalService As New Server5()
            Dim serverRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(True)
            router.AddRemoteAgencyInstance(serverRemoteAgencyInstance)
            Dim serverSiteId = serverRemoteAgencyInstance.SiteId
            Dim serviceWrapperInstanceId = serverRemoteAgencyInstance.CreateServiceWrapper(originalService)

            'Client
            Dim clientRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(True)
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance)
            Dim clientProxy = clientRemoteAgencyInstance.CreateProxy(Of ITest5(Of DateTime))(serverSiteId, serviceWrapperInstanceId).ProxyGeneric

            'Run test
            Console.WriteLine("Current(Current date):")
            Console.WriteLine(clientProxy.Current.ToLongDateString())

            Console.WriteLine("Value(Set, No return):")
            clientProxy.Value = 100

            Console.WriteLine("Value(Get, 100:")
            Console.WriteLine(clientProxy.Value)

            Console.Write("Press any key to continue...")
            Console.ReadKey(True)
            Console.WriteLine()

            serverRemoteAgencyInstance.Dispose()
            clientRemoteAgencyInstance.Dispose()
        End Sub
    End Class
End Namespace