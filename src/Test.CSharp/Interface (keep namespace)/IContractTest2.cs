using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecretNest.RemoteAgency.Attributes;

namespace Test.CSharp
{
    interface IContractTest2
    {
        void Add(ref long value);

        void Read(out long value);

        void Process(EntityInContractTest2 entity);
    }

    public class EntityInContractTest2
    {
        public string FromClientToServerProperty { get; set; }

        [ParameterReturnRequiredProperty("EntityTwoWayProperty")]
        public string TwoWayProperty { get; set; }
    }
}
