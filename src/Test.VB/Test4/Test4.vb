Imports SecretNest.RemoteAgency

Namespace Test4
    Public Interface ITest4
        Function ReturnItself(Of T As {Structure})(value As T)

        Function GetGenericTypeName(Of T As {Class})()

        Sub Supported(Of T1 As {T2}, T2 As {New})(obj As T1)

        Sub Supported(Of T1 As {IEnumerable(Of T3)}, T2 As {Exception, New}, T3)(obj As T1, ByRef obj2 As T2)
    End Interface

    Public Class Server4
        Implements ITest4

        Public Function ReturnItself(Of T As Structure)(value As T) As Object Implements ITest4.ReturnItself
            Return value
        End Function

        Public Function GetGenericTypeName(Of T As Class)() As Object Implements ITest4.GetGenericTypeName
            Dim name = (GetType(T)).FullName
            Return name
        End Function

        Public Sub Supported(Of T1 As T2, T2 As New)(obj As T1) Implements ITest4.Supported
            Console.WriteLine($"Server side: T1: {GetType(T1).FullName}")
            Console.WriteLine($"Server side: T2: {GetType(T2).FullName}")
        End Sub

        Public Sub Supported(Of T1 As IEnumerable(Of T3), T2 As {Exception, New}, T3)(obj As T1, ByRef obj2 As T2) Implements ITest4.Supported
            Console.WriteLine($"Server side: T1: {GetType(T1).FullName}")
            Console.WriteLine($"Server side: T2: {GetType(T2).FullName}")
            Console.WriteLine($"Server side: T3: {GetType(T3).FullName}")

            Dim myException As Object = New NotSupportedException("oops.")
            obj2 = CType(myException, T2)
        End Sub
    End Class

    Public NotInheritable Class TestCode
        Public Shared Sub MyTest()
            'test router
            Dim router = New RemoteAgencyRouter(Of Byte(), Object)

            'Server
            Dim originalService As New Server4()
            Dim serverRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(True)
            router.AddRemoteAgencyInstance(serverRemoteAgencyInstance)
            Dim serverSiteId = serverRemoteAgencyInstance.SiteId
            Dim serviceWrapperInstanceId = serverRemoteAgencyInstance.CreateServiceWrapper(originalService)

            'Client
            Dim clientRemoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(True)
            router.AddRemoteAgencyInstance(clientRemoteAgencyInstance)
            Dim clientProxy = clientRemoteAgencyInstance.CreateProxy(Of ITest4)(serverSiteId, serviceWrapperInstanceId).ProxyGeneric

            'Run test
            Console.WriteLine("ReturnItself(1024):")
            Console.WriteLine(clientProxy.ReturnItself(1024))

            Console.WriteLine("GetGenericTypeName(System.Collections.ArrayList):")
            Console.WriteLine(clientProxy.GetGenericTypeName(Of ArrayList)())

            Console.WriteLine("Supported(System.Collections.ArrayList, System.Object):")
            clientProxy.Supported(Of ArrayList, Object)(Nothing)

            Dim exception As Exception = Nothing
            Console.WriteLine("Supported(System.Collections.Generic.ICollection<System.Collections.ArrayList>), System.NotSupportedException, System.Collections.ArrayList):")
            clientProxy.Supported(Of ICollection(Of ArrayList), NotSupportedException, ArrayList)(New List(Of ArrayList)(), exception)
            Console.WriteLine($"Out parameter type: {exception.GetType().FullName}")
            Console.WriteLine($"Out parameter message: {exception.Message}")

            Console.Write("Press any key to continue...")
            Console.ReadKey(True)
            Console.WriteLine()

            serverRemoteAgencyInstance.Dispose()
            clientRemoteAgencyInstance.Dispose()
        End Sub
    End Class

End Namespace