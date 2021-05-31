using SecretNest.RemoteAgency.Attributes;

namespace Test.CSharp.Test3
{
    interface IContractTest3
    {
        [AssetOneWayOperating]
        void Add(long value);

        [PropertyGetOneWayOperating]
        long ValueOneWayGet { get; }

        [AssetIgnored()]
        long ValueIgnored { get; }

        long Value { get; set; }

        [LocalExceptionHandling]
        void WithException(EntityInContractTest3 parameter);

        [OperatingTimeoutTime(1000)]
        void TimeOutMethod();
    }

    public class EntityInContractTest3
    {
        public string FromClientToServerProperty { get; set; }

        [ParameterReturnRequiredProperty("EntityTwoWayProperty", isIncludedInReturning: true)]
        public string TwoWayProperty { get; set; }
    }
}
