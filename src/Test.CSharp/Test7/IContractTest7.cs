using System;
using SecretNest.RemoteAgency.Attributes;

namespace Test.CSharp.Test7
{
    interface IContractTest7
    {
        [AssetIgnored]
        DateTime Ignored { get; set; }

        [AssetOneWayOperating]
        DateTime OneWaySet { get; set; }

        [PropertyGetOneWayOperating]
        DateTime OneWayGet { get; }

        [OperatingTimeoutTime(1000, 2000)]
        DateTime TimeoutExceptionTest { get; set; }

        [LocalExceptionHandling]
        int WithException { get; set; }
    }
}
