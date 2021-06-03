Imports SecretNest.RemoteAgency
Imports SecretNest.RemoteAgency.Attributes

Namespace Test9
    Public Interface ITest9
        <AssetIgnored>
        Event Ignored As EventHandler

        <OperatingTimeoutTime(1000)>
        Event MyEvent As MyEventCallback
        Delegate Sub MyEventCallback(parameter As EntityInTest9)

        Event MyEventWithTwoWayParameter As MyEventWithTwoWayParameterCallback
        Delegate Sub MyEventWithTwoWayParameterCallback(parameter As Integer, ByRef parameter1 As Integer, ByRef parameter2 As Integer, <ParameterIgnored> ignored As Integer)

        <LocalExceptionHandling>
        Event WithException As EventHandler

        <EventParameterReturnRequiredProperty("parameter", "TwoWayProperty", Nothing, True)>
        Event MyEventWithException As MyEventWithExceptionCallback
        <LocalExceptionHandling>
        Delegate Sub MyEventWithExceptionCallback(parameter As EntityInTest9B)
    End Interface

    Public Class EntityInTest9
        Public Property FromServerToClientProperty As String

        <ParameterReturnRequiredProperty("EntityTwoWayProperty", Nothing, True)>
        Public Property TwoWayProperty As String
    End Class

    Public Class EntityInTest9B
        Public Property FromServerToClientProperty As String

        Public Property TwoWayProperty As String
    End Class

    Public Class Server9
        Implements ITest9

        Public Custom Event Ignored As EventHandler Implements ITest9.Ignored
            AddHandler(ByVal value As EventHandler)
                Console.WriteLine("Server side: This is predicted due to ignored.")
            End AddHandler
            RemoveHandler(ByVal value As EventHandler)
                Console.WriteLine("Server side: This is predicted due to ignored.")
            End RemoveHandler
            RaiseEvent(sender As Object, e As EventArgs)
            End RaiseEvent
        End Event

        Public Event MyEvent As ITest9.MyEventCallback Implements ITest9.MyEvent
        Public Event MyEventWithTwoWayParameter As ITest9.MyEventWithTwoWayParameterCallback Implements ITest9.MyEventWithTwoWayParameter
        Public Event WithException As EventHandler Implements ITest9.WithException
        Public Event MyEventWithException As ITest9.MyEventWithExceptionCallback Implements ITest9.MyEventWithException

        Public Sub Test()
            Console.WriteLine("Server side: MyEvent")
            Dim parameter As New EntityInTest9()
            With parameter
                .FromServerToClientProperty = "SetFromServer"
                .TwoWayProperty = "SetFromServer"
            End With
            RaiseEvent MyEvent(parameter)
            Console.WriteLine($"Server side: parameter.FromServerToClientProperty (should be SetFromServer): {parameter.FromServerToClientProperty}")
            Console.WriteLine($"Server side: parameter.TwoWayProperty (should be ChangedByClient): {parameter.TwoWayProperty}")

            Console.WriteLine("Server side: MyEventWithTwoWayParameter")
            Dim p1 = 101
            Dim p2 As Integer
            RaiseEvent MyEventWithTwoWayParameter(100, p1, p2, 103)
            Console.WriteLine($"Server side: p1=(501): {p1}")
            Console.WriteLine($"Server side: p2=(502): {p2}")

            Console.WriteLine("Server side: WithException")
            Try
                RaiseEvent WithException(Me, EventArgs.Empty)
#Disable Warning CA1031 ' Do not catch general exception types
            Catch ex As Exception
                Console.WriteLine("Predicted Exception: " + ex.Message)
#Enable Warning CA1031 ' Do not catch general exception types
            End Try

            Console.WriteLine("Server side: MyEventWithException")
            Dim parameterB As New EntityInTest9B()
            With parameterB
                .FromServerToClientProperty = "SetFromServer"
                .TwoWayProperty = "SetFromServer"
            End With
            Try
                RaiseEvent MyEventWithException(parameterB)
#Disable Warning CA1031 ' Do not catch general exception types
            Catch ex As Exception
                Console.WriteLine($"Server side: parameterB.FromServerToClientProperty (should be SetFromServer): {parameterB.FromServerToClientProperty}")
                Console.WriteLine($"Server side: parameterB.TwoWayProperty (should be SetBeforeException): {parameterB.TwoWayProperty}")
                Console.WriteLine("Predicted Exception: " + ex.Message)
#Enable Warning CA1031 ' Do not catch general exception types
            End Try
        End Sub
    End Class

    Public NotInheritable Class TestCode
        Public Shared Sub MyTest()
            'test router
            Dim router = New RemoteAgencyRouter(Of Byte(), Object)

            'Server
            Dim originalService As New Server9()
            Dim serverRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(True)
            router.AddRemoteAgencyInstance(serverRemoteAgencyInstance)
            Dim serverSiteId = serverRemoteAgencyInstance.SiteId
            Dim serviceWrapperInstanceId = serverRemoteAgencyInstance.CreateServiceWrapper(originalService)

            'Client
            Dim clientRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(True)
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance)
            Dim clientProxy = clientRemoteAgencyInstance.CreateProxy(Of ITest9)(serverSiteId, serviceWrapperInstanceId).ProxyGeneric
            AddHandler clientRemoteAgencyInstance.ExceptionRedirected, AddressOf ClientRemoteAgencyInstance_ExceptionRedirected

            'Run test
            Console.WriteLine("Ignored Add(Exception):")
            Try
                AddHandler clientProxy.Ignored, Sub(sender, args)
                                                End Sub
#Disable Warning CA1031 ' Do not catch general exception types
            Catch ex As Exception
                Console.WriteLine("Predicted Exception: " + ex.ToString())
            End Try
#Enable Warning CA1031 ' Do not catch general exception types

            AddHandler clientProxy.MyEvent, Sub(parameter)
                                                parameter.FromServerToClientProperty = "ChangedByClient"
                                                parameter.TwoWayProperty = "ChangedByClient"
                                            End Sub

            AddHandler clientProxy.MyEventWithTwoWayParameter, Sub(parameter, ByRef parameter1, ByRef parameter2, ignored)
                                                                   Console.WriteLine($"parameter: (100): {parameter}")
                                                                   Console.WriteLine($"parameter1: (101): {parameter1}")
                                                                   Console.WriteLine($"ignored: (0): {ignored}")
                                                                   parameter1 = 501
                                                                   parameter2 = 502
                                                               End Sub

            AddHandler clientProxy.WithException, Sub(sender, args) Throw New Exception("oops.")

            AddHandler clientProxy.MyEventWithException, Sub(parameter)
                                                             parameter.FromServerToClientProperty = "ChangedByClient"
                                                             parameter.TwoWayProperty = "SetBeforeException"
                                                             Throw New Exception("oops.")
                                                         End Sub

            Console.WriteLine("Run:")
            originalService.Test()

            Console.Write("Press any key to continue...")
            Console.ReadKey(True)
            Console.WriteLine()

            serverRemoteAgencyInstance.Dispose()
            clientRemoteAgencyInstance.Dispose()
        End Sub

        Private Shared Sub ClientRemoteAgencyInstance_ExceptionRedirected(sender As Object, e As ExceptionRedirectedEventArgs)
            Console.WriteLine(
                "Client side exception: " + vbCrLf +
                $"  Interface: {e.ServiceContractInterface.FullName}" + vbCrLf +
                $"  InstanceId: {e.InstanceId}" + vbCrLf +
                $"  AssetName: {e.AssetName}" + vbCrLf +
                $"  ExceptionType: {e.RedirectedException.GetType().FullName}" + vbCrLf +
                $"  ExceptionMessage: {e.RedirectedException.Message}")
        End Sub
    End Class
End Namespace