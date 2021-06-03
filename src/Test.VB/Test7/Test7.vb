Imports System.Threading
Imports SecretNest.RemoteAgency
Imports SecretNest.RemoteAgency.Attributes

Namespace Test7
    Public Interface ITest7
        <AssetIgnored>
        Property Ignored As DateTime

        <AssetOneWayOperating>
        Property OneWaySet As DateTime

        <PropertyGetOneWayOperating>
        ReadOnly Property OneWayGet As DateTime

        <OperatingTimeoutTime(1000, 2000)>
        Property TimeoutExceptionTest As DateTime

        <LocalExceptionHandling>
        Property WithException As EntityInTest7
    End Interface

    Public Class EntityInTest7
        Public Property FromClientToServerProperty As String

        <ParameterReturnRequiredProperty("EntityTwoWayProperty", Nothing, True)>
        Public Property TwoWayProperty As String
    End Class

    Public Class Server7
        Implements ITest7

        Public Property Ignored As Date Implements ITest7.Ignored
            Get
                Throw New Exception("You should never see this due to ignored.")
            End Get
            Set(value As Date)
                Throw New Exception("You should never see this due to ignored.")
            End Set
        End Property

        Public Property OneWaySet As Date Implements ITest7.OneWaySet
            Get
                Return DateTime.Now
            End Get
            Set(value As Date)
                Console.WriteLine("Server side: Operation received. Value={0}", value.ToLongDateString())
                Throw New Exception("You should never receive this from client coz this is a one way (set) property.")
            End Set
        End Property

        Public ReadOnly Property OneWayGet As Date Implements ITest7.OneWayGet
            Get
                Console.WriteLine("Server side: Operation received.")
                Throw New Exception("You should never receive this from client coz this is a one way (get) property.")
            End Get
        End Property

        Public Property TimeoutExceptionTest As Date Implements ITest7.TimeoutExceptionTest
            Get
                Thread.Sleep(2000)
                Return DateTime.Now
            End Get
            Set(value As Date)
                Thread.Sleep(3000)
            End Set
        End Property

        Public Property WithException As EntityInTest7 Implements ITest7.WithException
            Get
                Return Nothing
            End Get
            Set(value As EntityInTest7)
                value.FromClientToServerProperty = "ChangedByServer" 'should never returned
                value.TwoWayProperty = "SetBeforeException"
                Throw New Exception("oops.")
            End Set
        End Property
    End Class

    Public NotInheritable Class TestCode
        Public Shared Sub MyTest()
            'test router
            Dim router = New RemoteAgencyRouter(Of Byte(), Object)

            'Server
            Dim originalService As New Server7()
            Dim serverRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(True)
            router.AddRemoteAgencyInstance(serverRemoteAgencyInstance)
            AddHandler serverRemoteAgencyInstance.ExceptionRedirected, AddressOf ServerRemoteAgencyInstance_ExceptionRedirected
            Dim serverSiteId = serverRemoteAgencyInstance.SiteId
            Dim serviceWrapperInstanceId = serverRemoteAgencyInstance.CreateServiceWrapper(originalService)

            'Client
            Dim clientRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(True)
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance)
            Dim clientProxy = clientRemoteAgencyInstance.CreateProxy(Of ITest7)(serverSiteId, serviceWrapperInstanceId).ProxyGeneric

            'Run test
            Console.WriteLine("Ignored(Exception):")
            Try
                ' ReSharper disable once UnusedVariable
                Dim useless = clientProxy.Ignored
#Disable Warning CA1031 ' Do not catch general exception types
            Catch ex As Exception
                Console.WriteLine("Predicted Exception: " + ex.ToString())
            End Try
#Enable Warning CA1031 ' Do not catch general exception types

            Console.WriteLine("OneWaySet(NoClientException):")
            clientProxy.OneWaySet = DateTime.Now

            Console.WriteLine("OneWayGet(default):")
            Console.WriteLine(clientProxy.OneWayGet)

            Console.WriteLine("TimeoutExceptionTest:")
            Try
                Console.WriteLine(clientProxy.TimeoutExceptionTest)
#Disable Warning CA1031 ' Do not catch general exception types
            Catch ex As Exception
                Console.WriteLine("Predicted Exception: " + ex.ToString())
            End Try
#Enable Warning CA1031 ' Do not catch general exception types

            Dim entity As New EntityInTest7()
            With entity
                .FromClientToServerProperty = "SetFromClient"
                .TwoWayProperty = "SetFromClient"
            End With

            Console.WriteLine("WithException(Exception):")
            Try
                clientProxy.WithException = entity
#Disable Warning CA1031 ' Do not catch general exception types
            Catch ex As Exception
                Console.WriteLine("Predicted Exception: " + ex.Message)
            End Try
#Enable Warning CA1031 ' Do not catch general exception types
            Console.WriteLine($"Client side: entity.FromClientToServerProperty (should be SetFromClient): {entity.FromClientToServerProperty}")
            Console.WriteLine($"Client side: entity.TwoWayProperty (should be SetBeforeException): {entity.TwoWayProperty}")

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
