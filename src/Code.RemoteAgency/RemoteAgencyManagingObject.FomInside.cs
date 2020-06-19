using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyManagingObject<TEntityBase>
    {


        public void ProcessMessageReceivedFromInside(TEntityBase message)
        {
            switch (((IRemoteAgencyMessage) message).MessageType)
            {
                case MessageType.Method:
                    break;
                case MessageType.EventAdd:
                    break;
                case MessageType.EventRemove:
                    break;
                case MessageType.Event:
                    break;
                case MessageType.PropertyGet:
                    break;
                case MessageType.PropertySet:
                    break;
                case MessageType.SpecialCommand:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(IRemoteAgencyMessage.MessageType));
            }
        }
    }
}
