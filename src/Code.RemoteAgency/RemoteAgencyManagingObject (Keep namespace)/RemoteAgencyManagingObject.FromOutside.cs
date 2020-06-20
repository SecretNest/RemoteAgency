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
                    ProcessMethodMessageReceived(message);
                    break;
                case MessageType.EventAdd:
                    ProcessEventAddMessageReceived(message);
                    break;
                case MessageType.EventRemove:
                    ProcessEventRemoveMessageReceived(message);
                    break;
                case MessageType.Event:
                    ProcessEventMessageReceived(message);
                    break;
                case MessageType.PropertyGet:
                    ProcessPropertyGetMessageReceived(message);
                    break;
                case MessageType.PropertySet:
                    ProcessPropertySetMessageReceived(message);
                    break;
                case MessageType.SpecialCommand:
                    ProcessSpecialCommandMessageReceived(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(IRemoteAgencyMessage.MessageType));
            }
        }

        protected abstract void ProcessMethodMessageReceived(TEntityBase message);

        protected abstract void ProcessEventAddMessageReceived(TEntityBase message);

        protected abstract void ProcessEventRemoveMessageReceived(TEntityBase message);

        protected abstract void ProcessEventMessageReceived(TEntityBase message);

        protected abstract void ProcessPropertyGetMessageReceived(TEntityBase message);

        protected abstract void ProcessPropertySetMessageReceived(TEntityBase message);

        protected abstract void ProcessSpecialCommandMessageReceived(TEntityBase message);
    }

    partial class RemoteAgencyManagingObjectProxy<TEntityBase>
    {
        protected override void ProcessEventAddMessageReceived(TEntityBase message)
        {
            throw new NotImplementedException();
        }

        protected override void ProcessEventMessageReceived(TEntityBase message)
        {
            throw new NotImplementedException();
        }

        protected override void ProcessEventRemoveMessageReceived(TEntityBase message)
        {
            throw new NotImplementedException();
        }

        protected override void ProcessMethodMessageReceived(TEntityBase message)
        {
            throw new NotImplementedException();
        }

        protected override void ProcessPropertyGetMessageReceived(TEntityBase message)
        {
            throw new NotImplementedException();
        }

        protected override void ProcessPropertySetMessageReceived(TEntityBase message)
        {
            throw new NotImplementedException();
        }

        protected override void ProcessSpecialCommandMessageReceived(TEntityBase message)
        {
            //nothing to do.
        }
    }

    partial class RemoteAgencyManagingObjectServiceWrapper<TEntityBase>
    {
        protected override void ProcessMethodMessageReceived(TEntityBase message)
        {
            throw new NotImplementedException();
        }

        protected override void ProcessEventAddMessageReceived(TEntityBase message)
        {
            throw new NotImplementedException();
        }

        protected override void ProcessEventRemoveMessageReceived(TEntityBase message)
        {
            throw new NotImplementedException();
        }

        protected override void ProcessEventMessageReceived(TEntityBase message)
        {
            throw new NotImplementedException();
        }

        protected override void ProcessPropertyGetMessageReceived(TEntityBase message)
        {
            throw new NotImplementedException();
        }

        protected override void ProcessPropertySetMessageReceived(TEntityBase message)
        {
            throw new NotImplementedException();
        }

        protected override void ProcessSpecialCommandMessageReceived(TEntityBase message)
        {
            if (((IRemoteAgencyMessage) message).AssetName == SpecialCommands.Dispose)
            {
                OnProxyDisposed(((IRemoteAgencyMessage) message).SenderSiteId,
                    ((IRemoteAgencyMessage) message).SenderInstanceId);
            }
        }
    }
}
