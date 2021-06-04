Imports SecretNest.RemoteAgency
Imports SecretNest.RemoteAgency.Attributes

Namespace Test2

    Public Interface ITest2
        Sub AddOne(ByRef value As Long)

        Sub Read(ByRef value As Long)

        Sub Process(entity As EntityInTest2)
    End Interface

    Public Class EntityInTest2
        Public Property FromClientToServerProperty As String

        <ParameterReturnRequiredProperty("EntityTwoWayProperty")>
        Public Property TwoWayProperty As String
    End Class

    Public Class Server2
        Implements ITest2
        Dim _data As Long

        Public Sub AddOne(ByRef value As Long) Implements ITest2.AddOne
            value += 1
            _data = value
        End Sub

        Public Sub Read(ByRef value As Long) Implements ITest2.Read
            value = _data
        End Sub

        Public Sub Process(entity As EntityInTest2) Implements ITest2.Process
            Console.WriteLine($"Server side: entity.FromClientToServerProperty (should be SetFromClient): {entity.FromClientToServerProperty}")
            Console.WriteLine($"Server side: entity.TwoWayProperty (should be SetFromClient): {entity.TwoWayProperty}")

            entity.TwoWayProperty = "SetFromServer"
            entity.FromClientToServerProperty = "useless"
        End Sub
    End Class

    Public NotInheritable Class TestCode
        Public Shared Sub MyTest()
            'test router
            Dim router = New RemoteAgencyRouter(Of Byte(), Object)

            'Server
            Dim originalService As New Server2()
            Dim serverRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(True)
            router.AddRemoteAgencyInstance(serverRemoteAgencyInstance)
            Dim serverSiteId = serverRemoteAgencyInstance.SiteId
            Dim serviceWrapperInstanceId = serverRemoteAgencyInstance.CreateServiceWrapper(originalService)

            'Client
            Dim clientRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(True)
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance)
            Dim clientProxy = clientRemoteAgencyInstance.CreateProxy(Of ITest2)(serverSiteId, serviceWrapperInstanceId).ProxyGeneric

            'Run test
            Dim value As Long = 100

            Console.WriteLine("AddOne:")
            clientProxy.AddOne(value)
            Console.WriteLine(value)

            Dim parameter As Long
            Console.WriteLine("Read:")
            clientProxy.Read(parameter)
            Console.WriteLine(parameter)

            Dim entity As New EntityInTest2()
            With entity
                .FromClientToServerProperty = "SetFromClient"
                .TwoWayProperty = "SetFromClient"
            End With

            Console.WriteLine("Process:")
            clientProxy.Process(entity)
            Console.WriteLine($"Client side: entity.FromClientToServerProperty (should be SetFromClient): {entity.FromClientToServerProperty}")
            Console.WriteLine($"Client side: entity.TwoWayProperty (should be SetFromServer): {entity.TwoWayProperty}")

            Console.Write("Press any key to continue...")
            Console.ReadKey(True)
            Console.WriteLine()

            serverRemoteAgencyInstance.Dispose()
            clientRemoteAgencyInstance.Dispose()
        End Sub
    End Class
End Namespace
