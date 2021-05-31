using System;
using SecretNest.RemoteAgency.Attributes;

namespace Test.CSharp.Test10
{
    interface IContractTest10
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

    public class MyOwnAttribute : Attribute
    {
        public string MyProperty { get; set; }
        public int MyField;

        public MyOwnAttribute(string a, int b = 0, bool c = false)
        {
        }
    }

    //public class MyService_Proxy : IContractTest10
    //{
    //    [MyOwn("ValueOfA", c: true, MyProperty = "PropertyValue", MyField = 123)]
    //    public void MyMethod()
    //    {
    //        //...
    //    }
    //}
}
