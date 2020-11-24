using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecretNest.RemoteAgency;

namespace Test.CSharp
{
    class RemoteAgencyRouter<TSerialized, TEntityBase>
    {
        private readonly Dictionary<Guid, RemoteAgency<TSerialized, TEntityBase>> _instances = new Dictionary<Guid, RemoteAgency<TSerialized, TEntityBase>>();

        public void AddRemoteAgencyInstance(RemoteAgency<TSerialized, TEntityBase> instance)
        {
            var id = instance.SiteId;
            _instances.Add(id, instance);
            instance.MessageForSendingPrepared += Instance_MessageForSendingPrepared;
        }

        private void Instance_MessageForSendingPrepared(object sender, MessageBodyEventArgs<TSerialized, TEntityBase> e)
        {
            //serialize
            var serialized = e.Serialize();

            if (!_instances.TryGetValue(e.TargetSiteId, out var targetInstance))
            {
                throw new Exception("Target instance doesn't exist.");
            }

            //Process without serialization
            //targetInstance.ProcessReceivedMessage(e.MessageBody);

            //Process with serialized data
            targetInstance.ProcessReceivedSerializedMessage(serialized);
        }

    }
}
