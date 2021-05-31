using System;
using SecretNest.RemoteAgency.Attributes;

namespace Test.CSharp.Test9
{
    interface IContractTest9
    {
        [AssetIgnored]
        event EventHandler Ignored;

        [OperatingTimeoutTime(1000)]
        event MyEventCallback MyEvent;
        delegate int MyEventCallback(int parameter);

        event MyEventWithTwoWayParameterCallback MyEventWithTwoWayParameter;
        delegate int MyEventWithTwoWayParameterCallback(int parameter, ref int parameter1, out int parameter2, [ParameterIgnored]int ignored);

        [LocalExceptionHandling]
        event EventHandler WithException;

        event MyEventWithExceptionCallback MyEventWithException;
        [LocalExceptionHandling]
        delegate void MyEventWithExceptionCallback();
    }
}
