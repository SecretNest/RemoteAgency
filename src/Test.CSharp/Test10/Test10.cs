using System;
using System.Reflection;
using SecretNest.RemoteAgency;
using SecretNest.RemoteAgency.Attributes;

namespace Test.CSharp.Test10
{
    public interface ITest10
    {
        [AttributePassThrough(typeof(MyOwnAttribute),
            new[] {typeof(string), typeof(int), typeof(bool)},
            new object[] {"ValueOfA"},
            "ThisIsMyMethod")]
        [AttributePassThroughIndexBasedParameter("ThisIsMyMethod", 2, true)]
        [AttributePassThroughProperty("ThisIsMyMethod", nameof(MyOwnAttribute.MyProperty), "PropertyValue")]
        [AttributePassThroughField("ThisIsMyMethod", nameof(MyOwnAttribute.MyField), 123)]
        void MyMethod();
    }

    public class MyOwnAttribute : Attribute
    {
        public string MyProperty { get; set; }
        public int MyField;
        public string AFromCtor { get; }
        public int BFromCtor { get; } 
        public bool CFromCtor { get; }

        public MyOwnAttribute(string a, int b = 0, bool c = false)
        {
            AFromCtor = a;
            BFromCtor = b;
            CFromCtor = c;
        }
    }

    public static class TestCode
    {
        public static void MyTest()
        {
            //Create a remote agency instance without target for creating proxy class only.
            using var remoteAgencyInstance = RemoteAgencyBase.CreateWithBinarySerializer(true);
            var clientProxy = remoteAgencyInstance.CreateProxy<ITest10>(Guid.Empty, Guid.Empty).ProxyGeneric;

            Console.WriteLine("Getting attribute...");
            var proxyClassType = clientProxy.GetType();
            var myMethod = proxyClassType.GetMethod(nameof(ITest10.MyMethod));
            var myAttribute = myMethod!.GetCustomAttribute<MyOwnAttribute>();
            if (myAttribute == null)
            {
                Console.WriteLine("Houston, we have a problem.");
            }
            else
            {
                Console.WriteLine("Here are attribute values:");
                Console.WriteLine("  MyProperty: (PropertyValue): {0}", myAttribute.MyProperty);
                Console.WriteLine("  MyField: (123): {0}", myAttribute.MyField);
                Console.WriteLine("  AFromCtor: (ValueOfA): {0}", myAttribute.AFromCtor);
                Console.WriteLine("  BFromCtor: (0): {0}", myAttribute.BFromCtor);
                Console.WriteLine("  CFromCtor: (true): {0}", myAttribute.CFromCtor);
            }

            Console.Write("Press any key to quit...");
            Console.ReadKey(true);
            Console.WriteLine();
        }
    }
}
