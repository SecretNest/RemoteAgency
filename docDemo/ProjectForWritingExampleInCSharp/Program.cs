using System;
using SecretNest.RemoteAgency.Attributes;

namespace ProjectForWritingExampleInCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public class MyOwnAttribute : Attribute
    {
        public string MyProperty { get; set; }
        public int MyField;

        public MyOwnAttribute(string a, int b = 0, bool c = false)
        {
        }
    }

    public interface IMyService
    {
        [AttributePassThrough(typeof(MyOwnAttribute),
            new[] {typeof(string), typeof(int), typeof(bool)},
            new object[] {"ValueOfA"},
            "IdOfThisInstance")]
        [AttributePassThroughIndexBasedParameter("IdOfThisInstance", 2, true)]
        [AttributePassThroughProperty("IdOfThisInstance", nameof(MyOwnAttribute.MyProperty), "PropertyValue")]
        [AttributePassThroughField("IdOfThisInstance", nameof(MyOwnAttribute.MyField), 123)]
        void MyMethod();
    }

    public class MyService_Proxy : IMyService
    {
        [MyOwn("ValueOfA", c: true, MyProperty = "PropertyValue", MyField = 123)]
        public void MyMethod()
        {
            //...
        }
    }
}