using SecretNest.RemoteAgency.Attributes;

namespace Test.CSharp.Test6
{
    interface IContractTest6
    {
        int this[int index]
        {
            get;
            set;
        }

        [OperatingTimeoutTime(1000, 2000)]
        int this[string name]
        {
            get;
            set;
        }
    }
}
