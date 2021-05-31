using System;
using SecretNest.RemoteAgency.Attributes;

namespace Test.CSharp.Test8
{
    interface IContractTest8
    {
        [EventParameterIgnored("sender")]
        event EventHandler MyEventWithHandler;

        void Test();
    }
}
