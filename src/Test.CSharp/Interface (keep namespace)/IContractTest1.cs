using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecretNest.RemoteAgency.Attributes;

namespace Test.CSharp
{
    public interface IContractTest1
    {
        int Add(int a, int b);

        //[CustomizedAssetName("AddDouble")]
        float Add(float a, float b);

        [CustomizedAssetName("AddDouble")]
        double Add(double a, double b);
    }
}
