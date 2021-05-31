using SecretNest.RemoteAgency.Attributes;

namespace Test.CSharp.Test11
{
    [ThreadLock(ThreadLockMode.TaskSchedulerSpecified)]
    public interface IContractTest11
    {
        void Hello();
    }
}
