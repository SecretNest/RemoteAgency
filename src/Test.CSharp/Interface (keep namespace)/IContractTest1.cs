using SecretNest.RemoteAgency.Attributes;

namespace Test.CSharp
{
    public interface IContractTest1
    {
        int Add(int a, int b);

        //When unmark this line, an exception about asset naming conflicts will be thrown.
        //[CustomizedAssetName("AddDouble")]
        float Add(float a, float b);

        [CustomizedAssetName("AddDouble")]
        double Add(double a, double b);
    }
}
