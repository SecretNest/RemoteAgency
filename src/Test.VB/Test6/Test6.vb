Imports System.Threading
Imports SecretNest.RemoteAgency
Imports SecretNest.RemoteAgency.Attributes

Namespace Test6
    Public Interface ITest6
        Default Property Item(index As Integer) As Integer

        <OperatingTimeoutTime(1000, 2000)>
        Default Property Item(name As String) As Integer

    End Interface

    Public Class Server6
        Implements ITest6

        Private ReadOnly _data As Integer() = {101, 102, 103, 104, 105}

        Default Public Property Item(index As Integer) As Integer Implements ITest6.Item
            Get
                Return _data(index)
            End Get
            Set(value As Integer)
                _data(index) = value
            End Set
        End Property

        Default Public Property Item(name As String) As Integer Implements ITest6.Item
            Get
                Dim index As Integer
                If Integer.TryParse(name, index) Then
                    Return _data(index)
                Else
                    Thread.Sleep(5000)
                    Return -1
                End If
            End Get
            Set(value As Integer)
                Dim index As Integer
                If Integer.TryParse(name, index) Then
                    _data(index) = value
                Else
                    Thread.Sleep(5000)
                End If
            End Set
        End Property
    End Class

    Public NotInheritable Class TestCode
        Public Shared Sub MyTest()
            'test router
            Dim router = New RemoteAgencyRouter(Of Byte(), Object)

            'Server
            Dim originalService As New Server6()
            Dim serverRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(True)
            router.AddRemoteAgencyInstance(serverRemoteAgencyInstance)
            Dim serverSiteId = serverRemoteAgencyInstance.SiteId
            Dim serviceWrapperInstanceId = serverRemoteAgencyInstance.CreateServiceWrapper(originalService)

            'Client
            Dim clientRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(True)
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance)
            Dim clientProxy = clientRemoteAgencyInstance.CreateProxy(Of ITest6)(serverSiteId, serviceWrapperInstanceId).ProxyGeneric

            'Run test
            Console.WriteLine("Item(2)(Get, 102):")
            Console.WriteLine(clientProxy(2))

            Console.WriteLine("Item(4)(Set+Get, No return):")
            clientProxy(4) += 100

            Console.WriteLine("Item(""4"")(Get, 204):")
            Console.WriteLine(clientProxy("4"))

            Console.WriteLine("Item(""Timeout"")(Get, Timeout):")
            Try
                ' ReSharper disable once UnusedVariable
                Console.WriteLine(clientProxy("Timeout"))
#Disable Warning CA1031 ' Do not catch general exception types
            Catch ex As Exception
                Console.WriteLine("Predicted Exception: " + ex.ToString())
            End Try
#Enable Warning CA1031 ' Do not catch general exception types

            Console.WriteLine("Item(""Timeout"")(Set, Timeout):")
            Try
                ' ReSharper disable once UnusedVariable
                clientProxy("Timeout") = 0
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
    End Class
End Namespace
