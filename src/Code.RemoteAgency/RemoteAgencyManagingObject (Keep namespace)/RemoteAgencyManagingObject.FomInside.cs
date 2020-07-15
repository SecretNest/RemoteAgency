using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyManagingObject
    {
        protected void ProcessPreparedRequestMessageReceivedFromInside(IRemoteAgencyMessage message)
        {
            //local site id will be set by manager.
            _sendMessageToManagerCallback(message);
        }

        protected void PrepareRequestMessageReceivedFromInside(IRemoteAgencyMessage message, MessageType messageType, bool isOneWay)
        {
            message.MessageId = Guid.NewGuid();
            //message.AssetName set while generating.
            message.IsOneWay = isOneWay;
            //message.Exception = null;
            message.SenderInstanceId = InstanceId;
            //message.SenderSiteId leave to manager.
            //message.TargetInstanceId set while generating.
            //message.TargetSiteId set while generating.
            message.MessageType = messageType;
        }
    }

    partial class RemoteAgencyManagingObjectProxy<TEntityBase>
    {
        void PrepareDefaultTargetRequestMessageReceivedFromInside(IRemoteAgencyMessage message, MessageType messageType, bool isOneWay)
        {
            message.MessageId = Guid.NewGuid();
            //message.AssetName set while generating.
            message.IsOneWay = isOneWay;
            //message.Exception = null;
            message.SenderInstanceId = InstanceId;
            //message.SenderSiteId leave to manager.
            message.TargetInstanceId = DefaultTargetInstanceId;
            message.TargetSiteId = DefaultTargetSiteId;
            message.MessageType = messageType;
        }

        IRemoteAgencyMessage ProcessMethodMessageReceivedFromInside(IRemoteAgencyMessage message, int timeout)
        {
            PrepareDefaultTargetRequestMessageReceivedFromInside(message, MessageType.Method, false);
            return ProcessRequestAndWaitResponse(message, ProcessPreparedRequestMessageReceivedFromInside, timeout);
        }

        void ProcessOneWayMethodMessageReceivedFromInside(IRemoteAgencyMessage message)
        {
            PrepareDefaultTargetRequestMessageReceivedFromInside(message, MessageType.Method, true);
            ProcessPreparedRequestMessageReceivedFromInside(message);
        }

        IRemoteAgencyMessage ProcessEventAddMessageReceivedFromInside(IRemoteAgencyMessage message, int timeout)
        {
            PrepareDefaultTargetRequestMessageReceivedFromInside(message, MessageType.EventAdd, false);
            return ProcessRequestAndWaitResponse(message, ProcessPreparedRequestMessageReceivedFromInside, timeout);
        }

        IRemoteAgencyMessage ProcessEventRemoveMessageReceivedFromInside(IRemoteAgencyMessage message, int timeout)
        {
            //target is already specified inside message.
            PrepareRequestMessageReceivedFromInside(message, MessageType.EventRemove, false); 
            return ProcessRequestAndWaitResponse(message, ProcessPreparedRequestMessageReceivedFromInside, timeout);
        }

        IRemoteAgencyMessage ProcessPropertyGetMessageReceivedFromInside(IRemoteAgencyMessage message, int timeout)
        {
            PrepareDefaultTargetRequestMessageReceivedFromInside(message, MessageType.PropertyGet, false);
            return ProcessRequestAndWaitResponse(message, ProcessPreparedRequestMessageReceivedFromInside, timeout);
        }

        void ProcessOneWayPropertyGetMessageReceivedFromInside(IRemoteAgencyMessage message)
        {
            PrepareDefaultTargetRequestMessageReceivedFromInside(message, MessageType.PropertyGet, true);
            ProcessPreparedRequestMessageReceivedFromInside(message);
        }

        IRemoteAgencyMessage ProcessPropertySetMessageReceivedFromInside(IRemoteAgencyMessage message, int timeout)
        {
            PrepareDefaultTargetRequestMessageReceivedFromInside(message, MessageType.PropertySet, false);
            return ProcessRequestAndWaitResponse(message, ProcessPreparedRequestMessageReceivedFromInside, timeout);
        }

        void ProcessOneWayPropertySetMessageReceivedFromInside(IRemoteAgencyMessage message)
        {
            PrepareDefaultTargetRequestMessageReceivedFromInside(message, MessageType.PropertySet, true);
            ProcessPreparedRequestMessageReceivedFromInside(message);
        }

        void ProcessOneWaySpecialCommandMessageReceivedFromInside(IRemoteAgencyMessage message)
        {
            //target is already specified inside message.
            PrepareRequestMessageReceivedFromInside(message, MessageType.SpecialCommand, true);
            ProcessPreparedRequestMessageReceivedFromInside(message);
        }
    }

    partial class RemoteAgencyManagingObjectServiceWrapper<TEntityBase>
    {
        IRemoteAgencyMessage ProcessEventMessageReceivedFromInside(IRemoteAgencyMessage message, int timeout)
        {
            PrepareRequestMessageReceivedFromInside(message, MessageType.Event, false);
            return ProcessRequestAndWaitResponse(message, ProcessPreparedRequestMessageReceivedFromInside, timeout);
        }

        void ProcessOneWayEventMessageReceivedFromInside(IRemoteAgencyMessage message)
        {
            PrepareRequestMessageReceivedFromInside(message, MessageType.Event, true);
            ProcessPreparedRequestMessageReceivedFromInside(message);
        }

        void ProcessOneWaySpecialCommandMessageReceivedFromInside(IRemoteAgencyMessage message)
        {
            PrepareRequestMessageReceivedFromInside(message, MessageType.SpecialCommand, true);
            ProcessPreparedRequestMessageReceivedFromInside(message);
        }
    }
}