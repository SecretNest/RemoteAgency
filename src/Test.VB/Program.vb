Imports System

Module Program
    ' ReSharper disable once UnusedParameter.Global
#Disable Warning IDE0060 ' Remove unused parameter
    Sub Main(args As String())
#Enable Warning IDE0060 ' Remove unused parameter
        Console.WriteLine("Hello World!")

        Console.WriteLine("Run Test 0")
        Test0.TestCode.MyTest()

        Console.WriteLine("Run Test 1")
        Test1.TestCode.MyTest()

        Console.WriteLine("Run Test 2")
        Test2.TestCode.MyTest()

        Console.WriteLine("Run Test 3")
        Test3.TestCode.MyTest()










        Console.WriteLine("Done")
    End Sub
End Module
