Imports System
Imports SecretNest.RemoteAgency.Attributes

Module Program
    Sub Main(args As String())
        Console.WriteLine("Hello World!")
    End Sub
End Module

Public Class MyOwnAttribute
    Inherits Attribute

    Public Property MyProperty As String
    Public MyField As Integer

    Public Sub New(a As String, Optional b As Integer = 0, Optional c As Boolean = False)
    End Sub
End Class

Public Interface IMyService
    <AttributePassThrough(GetType(MyOwnAttribute), New Type() {GetType(String), GetType(Integer), GetType(Boolean)}, New String() {"ValueOfA"}, "IdOfThisInstance")>
    <AttributePassThroughIndexBasedParameter("IdOfThisInstance", 2, True)>
    <AttributePassThroughProperty("IdOfThisInstance", NameOf(MyOwnAttribute.MyProperty), "PropertyValue")>
    <AttributePassThroughField("IdOfThisInstance", NameOf(MyOwnAttribute.MyField), 123)>
    Sub MyMethod()
End Interface

Public Class MyService_Proxy
    Implements IMyService

    <MyOwn("ValueOfA", , True, MyProperty:="PropertyValue", MyField:=123)>
    Public Sub MyMethod() Implements IMyService.MyMethod
        '...
    End Sub
End Class