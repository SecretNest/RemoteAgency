using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyManagingObject<TEntityBase>
    {


        public void ProcessMessageReceivedFromOutside(TEntityBase message)
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
                {
                    if (((IRemoteAgencyMessage) message).AssetName == SpecialCommands.Dispose)
                    {
                        OnProxyDisposed(((IRemoteAgencyMessage) message).SenderSiteId,
                            ((IRemoteAgencyMessage) message).SenderInstanceId);
                    }
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(IRemoteAgencyMessage.MessageType));
            }
        }
    }
}
