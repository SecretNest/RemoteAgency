Imports System.Threading
Imports SecretNest.RemoteAgency
Imports SecretNest.RemoteAgency.Attributes

Namespace Test3
    Public Interface ITest3
        <AssetOneWayOperating>
        Sub Add(v As Long)

        <PropertyGetOneWayOperating>
        ReadOnly Property ValueOneWayGet As Long

        <AssetIgnored>
        ReadOnly Property ValueIgnored As Long

        Property Value As Long

        <LocalExceptionHandling>
        Sub WithException(entity As EntityInTest3)

        <OperatingTimeoutTime(1000)>
        Sub TimeOutMethod()
    End Interface

    Public Class EntityInTest3
        Public Property FromClientToServerProperty As String

        <ParameterReturnRequiredProperty("EntityTwoWayProperty")>
        Public Property TwoWayProperty As String
    End Class

    Public Class Server3
        Implements ITest3

        Public Sub Add(v As Long) Implements ITest3.Add
            Value += v
        End Sub

        Public ReadOnly Property ValueOneWayGet As Long Implements ITest3.ValueOneWayGet
            Get
                Console.WriteLine("Server side: ValueOneWayGet called.")
                Return Value
            End Get
        End Property

        Public ReadOnly Property ValueIgnored As Long Implements ITest3.ValueIgnored
            Get
                Return 0
            End Get
        End Property

        Public Property Value As Long Implements ITest3.Value

        Public Sub WithException(entity As EntityInTest3) Implements ITest3.WithException
            Console.WriteLine($"Server side: entity.FromClientToServerProperty (should be SetFromClient): {entity.FromClientToServerProperty}")
            Console.WriteLine($"Server side: entity.TwoWayProperty (should be SetFromClient): {entity.TwoWayProperty}")

            entity.TwoWayProperty = "SetBeforeException"
            Throw New Exception("oops.")
        End Sub

        Public Sub TimeOutMethod() Implements ITest3.TimeOutMethod
            Thread.Sleep(2000)
        End Sub
    End Class

    Public NotInheritable Class TestCode
        Public Shared Sub MyTest()
            'test router
            Dim router = New RemoteAgencyRouter(Of Byte(), Object)

            'Server
            Dim originalService As New Server3()
            Dim serverRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(True)
            router.AddRemoteAgencyInstance(serverRemoteAgencyInstance)
            AddHandler serverRemoteAgencyInstance.ExceptionRedirected, AddressOf ServerRemoteAgencyInstance_ExceptionRedirected
            Dim serverSiteId = serverRemoteAgencyInstance.SiteId
            Dim serviceWrapperInstanceId = serverRemoteAgencyInstance.CreateServiceWrapper(originalService)

            'Client
            Dim clientRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(True)
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance)
            Dim clientProxy = clientRemoteAgencyInstance.CreateProxy(Of ITest3)(serverSiteId, serviceWrapperInstanceId).ProxyGeneric

            'Run test
            Console.WriteLine("Add(No return):")
            clientProxy.Add(100)

            Console.WriteLine("ValueOneWayGet(Default value returning after server processed):")
            Console.WriteLine(clientProxy.ValueOneWayGet)

            Console.WriteLine("ValueIgnored(Exception):")

            Try
                ' ReSharper disable once UnusedVariable
                Dim useless = clientProxy.ValueIgnored
#Disable Warning CA1031 ' Do not catch general exception types
            Catch ex As Exception
                Console.WriteLine("Predicted Exception: " + ex.ToString())
            End Try
#Enable Warning CA1031 ' Do not catch general exception types

            Console.WriteLine("Value(Get, 100):")
            Console.WriteLine(clientProxy.Value)

            Console.WriteLine("Value(Set, no return):")
            clientProxy.Value = 500

            Console.WriteLine("Add(No return):")
            clientProxy.Add(100)

            Console.WriteLine("Value(Get, 600):")
            Console.WriteLine(clientProxy.Value)

            Dim entity As New EntityInTest3()
            With entity
                .FromClientToServerProperty = "SetFromClient"
                .TwoWayProperty = "SetFromClient"
            End With

            Console.WriteLine("WithException:")
            Try
                clientProxy.WithException(entity)
#Disable Warning CA1031 ' Do not catch general exception types
            Catch ex As Exception
                Console.WriteLine("Predicted Exception: " + ex.ToString())
            End Try
#Enable Warning CA1031 ' Do not catch general exception types
            Console.WriteLine($"Client side: entity.FromClientToServerProperty (should be SetFromClient): {entity.FromClientToServerProperty}")
            Console.WriteLine($"Client side: entity.TwoWayProperty (should be SetBeforeException): {entity.TwoWayProperty}")

            Console.WriteLine("TimeOutMethod:")
            Try
                clientProxy.TimeOutMethod()
#Disable Warning CA1031 ' Do not catch general exception types
            Catch ex As Exception
                Console.WriteLine("Predicted Exception: " + ex.ToString())
            End Try
#Enable Warning CA1031 ' Do not catch general exception types

            Console.Write("Press any key to continue...")
            Console.ReadKey(True)
            Console.WriteLine()

            serverRemoteAgencyInstance.Dispose()
            clientRemoteAgencyInstance.Dispose()
        End Sub

        Private Shared Sub ServerRemoteAgencyInstance_ExceptionRedirected(sender As Object, e As ExceptionRedirectedEventArgs)
            Console.WriteLine(
                "Server side exception: " + vbCrLf +
                $"  Interface: {e.ServiceContractInterface.FullName}" + vbCrLf +
                $"  InstanceId: {e.InstanceId}" + vbCrLf +
                $"  AssetName: {e.AssetName}" + vbCrLf +
                $"  ExceptionType: {e.RedirectedException.GetType().FullName}" + vbCrLf +
                $"  ExceptionMessage: {e.RedirectedException.Message}")
        End Sub
    End Class
End Namespace