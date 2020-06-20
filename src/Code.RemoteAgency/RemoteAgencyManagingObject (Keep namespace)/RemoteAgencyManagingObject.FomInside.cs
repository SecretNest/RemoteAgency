using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyManagingObject<TEntityBase>
    {
        private Action<TEntityBase> _sendToManagerCallback;

        protected void ProcessMessageReceivedFromInside(TEntityBase message)
        {
            //add remote site id, remote instance id, local instance id





            //local site id will be set by manager.
            _sendToManagerCallback(message);


            //switch (((IRemoteAgencyMessage) message).MessageType)
            //{
            //    case MessageType.Method:
            //        break;
            //    case MessageType.EventAdd:
            //        break;
            //    case MessageType.EventRemove:
            //        break;
            //    case MessageType.Event:
            //        break;
            //    case MessageType.PropertyGet:
            //        break;
            //    case MessageType.PropertySet:
            //        break;
            //    case MessageType.SpecialCommand:
            //        break;
            //    default:
            //        throw new ArgumentOutOfRangeException(nameof(IRemoteAgencyMessage.MessageType));
            //}
        }


    }

}
