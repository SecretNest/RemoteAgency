Imports System.Reflection
Imports SecretNest.RemoteAgency
Imports SecretNest.RemoteAgency.Attributes

Namespace Test10
    Public Interface ITest10
        <AttributePassThrough(GetType(MyOwnAttribute), New Type() {GetType(String), GetType(Integer), GetType(Boolean)}, New Object() {"ValueOfA"}, "ThisIsMyMethod")>
        <AttributePassThroughIndexBasedParameter("ThisIsMyMethod", 2, True)>
        <AttributePassThroughProperty("ThisIsMyMethod", NameOf(MyOwnAttribute.MyProperty), "PropertyValue")>
        <AttributePassThroughField("ThisIsMyMethod", NameOf(MyOwnAttribute.MyField), 123)>
        Sub MyMethod()
    End Interface

    <AttributeUsage(AttributeTargets.Method)>
    Public NotInheritable Class MyOwnAttribute
        Inherits Attribute
        Public Property MyProperty As String
        Public MyField As Integer
        Public ReadOnly Property AFromCtor As String
        Public ReadOnly Property BFromCtor As Integer
        Public ReadOnly Property CFromCtor As Boolean

        Sub New(a As String, Optional b As Integer = 0, Optional c As Boolean = False)
            AFromCtor = a
            BFromCtor = b
            CFromCtor = c
        End Sub
    End Class

    Public NotInheritable Class TestCode
        Public Shared Sub MyTest()
            'Create a remote agency instance without target for creating proxy class only.
            Dim remoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(True)
            Dim clientProxy = remoteAgencyInstance.CreateProxy(Of ITest10)(Guid.Empty, Guid.Empty).ProxyGeneric

            Console.WriteLine("Getting attribute...")
            Dim proxyClassType = clientProxy.GetType()
            Dim myMethod = proxyClassType.GetMethod(NameOf(ITest10.MyMethod))
            Dim myAttribute = myMethod.GetCustomAttribute(Of MyOwnAttribute)()
            If (myAttribute Is Nothing) Then
                Console.WriteLine("Houston, we have a problem.")
            Else
                Console.WriteLine("Here are attribute values:")
                Console.WriteLine("  MyProperty: (PropertyValue): {0}", myAttribute.MyProperty)
                Console.WriteLine("  MyField: (123): {0}", myAttribute.MyField)
                Console.WriteLine("  AFromCtor: (ValueOfA): {0}", myAttribute.AFromCtor)
                Console.WriteLine("  BFromCtor: (0): {0}", myAttribute.BFromCtor)
                Console.WriteLine("  CFromCtor: (true): {0}", myAttribute.CFromCtor)
            End If

            Console.Write("Press any key to continue...")
            Console.ReadKey(True)
            Console.WriteLine()

            remoteAgencyInstance.Dispose()
        End Sub
    End Class
End Namespace
