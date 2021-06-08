Imports SecretNest.RemoteAgency
Imports SecretNest.RemoteAgency.Attributes

Namespace Test2

    Public Interface ITest2
        <ReturnIgnored>
        Function IgnoredParameter(value As Integer, <ParameterIgnored> ignored As Integer) As Integer

        Sub AddOne(ByRef value As Long)

        Sub Read(ByRef value As Long)

        Sub Process(<ParameterReturnRequiredProperty("TwoWayProperty")> <ParameterReturnRequiredProperty(GetType(EntityHelperInTest2))> entity As EntityInTest2)
    End Interface

    Public Class EntityInTest2
        Public Property FromClientToServerProperty As String

        Public Property TwoWayProperty As String

        Public Property ComplexResult As List(Of Integer)
    End Class

    Public Class EntityHelperInTest2
        <ReturnRequiredPropertyHelper>
        Public Property Helper As Boolean
            Get
                Return _value.Contains(100)
            End Get
            Set(value As Boolean)
                If value Then
                    _value.Add(100)
                End If
            End Set
        End Property

        Private ReadOnly _value As List(Of Integer)

        Public Sub New(value As EntityInTest2)
            _value = value.ComplexResult
        End Sub
    End Class

    Public Class Server2
        Implements ITest2
        Dim _data As Long

        Public Function IgnoredParameter(value As Integer, <ParameterIgnored(True)> ignored As Integer) As Integer Implements ITest2.IgnoredParameter
            Console.WriteLine($"Server side: value (should be 100): {value}")
            Console.WriteLine($"Server side: ignored (should be 0 due to ignored): {ignored}")

            Return value
        End Function

        Public Sub AddOne(ByRef value As Long) Implements ITest2.AddOne
            value += 1
            _data = value
        End Sub

        Public Sub Read(ByRef value As Long) Implements ITest2.Read
            value = _data
        End Sub

        Public Sub Process(entity As EntityInTest2) Implements ITest2.Process
            If entity.ComplexResult.Contains(0) Then
                entity.ComplexResult.Add(100) 'This will be returned due to matching the Helper code in EntityHelperInTest2.
                entity.ComplexResult.Add(200) 'This will not be returned.
            End If
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
                .ComplexResult = New List(Of Integer)
            End With
            entity.ComplexResult.Add(0)

            Console.WriteLine("Process:")
            clientProxy.Process(entity)
            Console.WriteLine($"Client side: entity.FromClientToServerProperty (should be SetFromClient): {entity.FromClientToServerProperty}")
            Console.WriteLine($"Client side: entity.TwoWayProperty (should be SetFromServer): {entity.TwoWayProperty}")
            Console.WriteLine($"Client side: entity.ComplexResult.Contains(100) (should be True): {entity.ComplexResult.Contains(100)}")
            Console.WriteLine($"Client side: entity.ComplexResult.Contains(200) (should be False): {entity.ComplexResult.Contains(200)}")

            Console.WriteLine("IgnoredParameter(0 due to ignored):")
            Console.WriteLine(clientProxy.IgnoredParameter(100, 200))

            Console.Write("Press any key to continue...")
            Console.ReadKey(True)
            Console.WriteLine()

            serverRemoteAgencyInstance.Dispose()
            clientRemoteAgencyInstance.Dispose()
        End Sub
    End Class
End Namespace
