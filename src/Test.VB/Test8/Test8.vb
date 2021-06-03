Imports SecretNest.RemoteAgency
Imports SecretNest.RemoteAgency.Attributes

Namespace Test8
    Public Interface ITest8
        <EventParameterIgnored("sender")>
        Event MyEventWithHandler As EventHandler

        Sub Test()
    End Interface

    Public Class Server8
        Implements ITest8

        Public Event MyEventWithHandler As EventHandler Implements ITest8.MyEventWithHandler

        Public Sub Test() Implements ITest8.Test
            Console.WriteLine("Server side: Method called.")
            RaiseEvent MyEventWithHandler(Me, EventArgs.Empty)
        End Sub
    End Class

    Public NotInheritable Class TestCode
        Public Shared Sub MyTest()
            'test router
            Dim router = New RemoteAgencyRouter(Of Byte(), Object)

            'Server
            Dim originalService As New Server8()
            Dim serverRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(True)
            router.AddRemoteAgencyInstance(serverRemoteAgencyInstance)
            Dim serverSiteId = serverRemoteAgencyInstance.SiteId
            Dim serviceWrapperInstanceId = serverRemoteAgencyInstance.CreateServiceWrapper(originalService)

            'Client 1
            Dim clientRemoteAgencyInstance1 = RemoteAgencyBase.CreateWithBinarySerializer(True)
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance1)
            Dim clientProxy1 = clientRemoteAgencyInstance1.CreateProxy(Of ITest8)(serverSiteId, serviceWrapperInstanceId).ProxyGeneric

            'Client 2
            Dim clientRemoteAgencyInstance2 = RemoteAgencyBase.CreateWithBinarySerializer(True)
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance2)
            Dim clientProxy2 = clientRemoteAgencyInstance2.CreateProxy(Of ITest8)(serverSiteId, serviceWrapperInstanceId).ProxyGeneric

            'Run test
            Console.WriteLine("Run(WithoutHandler)")
            clientProxy1.Test()

            Console.WriteLine("Run(HandlerFromClient1)")
            AddHandler clientProxy1.MyEventWithHandler, AddressOf ClientProxy1_MyEventWithHandler
            clientProxy1.Test()

            Console.WriteLine("Run(HandlerFromClient1+2)")
            AddHandler clientProxy2.MyEventWithHandler, AddressOf ClientProxy2_MyEventWithHandler
            clientProxy1.Test()

            Console.WriteLine("Run(HandlerFromClient1+2+2)")
            AddHandler clientProxy2.MyEventWithHandler, AddressOf ClientProxy2_MyEventWithHandler
            clientProxy1.Test()

            Console.WriteLine("Run(HandlerFromClient2+2)")
            RemoveHandler clientProxy1.MyEventWithHandler, AddressOf ClientProxy1_MyEventWithHandler
            clientProxy1.Test()

            Console.WriteLine("Run(WithoutHandler2)")
            RemoveHandler clientProxy2.MyEventWithHandler, AddressOf ClientProxy2_MyEventWithHandler
            clientProxy1.Test()

            Console.WriteLine("Run(WithoutHandler2)")
            RemoveHandler clientProxy1.MyEventWithHandler, AddressOf ClientProxy1_MyEventWithHandler 'useless coz no active handler registered.
            clientProxy1.Test()

            Console.WriteLine("Run(WithoutHandler)")
            RemoveHandler clientProxy2.MyEventWithHandler, AddressOf ClientProxy2_MyEventWithHandler
            clientProxy1.Test()

            Console.Write("Press any key to continue...")
            Console.ReadKey(True)
            Console.WriteLine()

            serverRemoteAgencyInstance.Dispose()
            clientRemoteAgencyInstance1.Dispose()
            clientRemoteAgencyInstance2.Dispose()
        End Sub

        Private Shared Sub ClientProxy1_MyEventWithHandler(sender As Object, e As EventArgs)
            Console.WriteLine("Client1 Event Handler Called.")
        End Sub

        Private Shared Sub ClientProxy2_MyEventWithHandler(sender As Object, e As EventArgs)
            Console.WriteLine("Client2 Event Handler Called.")
        End Sub
    End Class

End Namespace