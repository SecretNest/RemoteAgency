Imports SecretNest.RemoteAgency

Namespace Test12
    Public Interface ITest12
        Sub Hello()
    End Interface

    Public Class Server12
        Implements ITest12

        Public Sub Hello() Implements ITest12.Hello
            Console.WriteLine("Hello world.")
        End Sub
    End Class

    Public NotInheritable Class TestCode
        Public Shared Sub MyTest()
            'test router
            Dim router = New RemoteAgencyRouter(Of Byte(), Object)

            'Server
            Dim originalService As New Server12()
            Dim serverRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(True)
            router.AddRemoteAgencyInstance(serverRemoteAgencyInstance)
            Dim serverSiteId = serverRemoteAgencyInstance.SiteId
            Dim serviceWrapperInstanceId = serverRemoteAgencyInstance.CreateServiceWrapper(originalService)

            'Client
            Dim clientRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(True)
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance)
            Dim clientProxyInstanceId = clientRemoteAgencyInstance.CreateProxy(Of ITest12)(serverSiteId, serviceWrapperInstanceId).InstanceId

            'Run test
            Console.WriteLine("Ping (ms):")
            Dim remoteSiteId = Guid.Empty
            Dim remoteInstanceId = Guid.Empty
            Console.WriteLine(clientRemoteAgencyInstance.Ping(clientProxyInstanceId, remoteSiteId, remoteInstanceId).TotalMilliseconds)
            If serverSiteId <> remoteSiteId OrElse serviceWrapperInstanceId <> remoteInstanceId Then
                ' ReSharper disable once StringLiteralTypo
                Console.WriteLine("D'oh!")
            End If

            Console.Write("Press any key to continue...")
            Console.ReadKey(True)
            Console.WriteLine()

            serverRemoteAgencyInstance.Dispose()
            clientRemoteAgencyInstance.Dispose()
        End Sub
    End Class
End Namespace
