using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyManagingObject
    {
        protected virtual void PreProcessMessageReceivedFromOutside(IRemoteAgencyMessage message)
        {
        }

        public void ProcessMessageReceivedFromOutside(IRemoteAgencyMessage message)
        {
            switch (message.MessageType)
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
                //case MessageType.SpecialCommand:
                //    ProcessSpecialCommandMessageReceived(message);
                //    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(IRemoteAgencyMessage.MessageType));
            }
        }

        public void SetRespondDirectly(IRemoteAgencyMessage message)
            => OnResponseReceived(message);

        protected abstract void ProcessMethodMessageReceived(IRemoteAgencyMessage message);

        protected abstract void ProcessEventAddMessageReceived(IRemoteAgencyMessage message);

        protected abstract void ProcessEventRemoveMessageReceived(IRemoteAgencyMessage message);

        protected abstract void ProcessEventMessageReceived(IRemoteAgencyMessage message);

        protected abstract void ProcessPropertyGetMessageReceived(IRemoteAgencyMessage message);

        protected abstract void ProcessPropertySetMessageReceived(IRemoteAgencyMessage message);

        //protected abstract void ProcessSpecialCommandMessageReceived(IRemoteAgencyMessage message);

        protected void ProcessRequestAndSendResponseIfRequired(IRemoteAgencyMessage message, AccessWithReturn withReturnCallback, AccessWithoutReturn withoutReturnCallback)
        {
            Exception exception;
            LocalExceptionHandlingMode localExceptionHandlingMode;

            if (message.IsOneWay)
            {
                ProcessThreadLockWithoutReturn(withoutReturnCallback, message, out exception, out localExceptionHandlingMode);
            }
            else
            {
                ProcessThreadLockWithReturn(withReturnCallback, message, out var response, out exception, out localExceptionHandlingMode);

                response.Exception = exception;
                ProcessResponseMessageReceivedFromInside(response, message);
            }

            ThrowExceptionWhenNecessary(message.AssetName, exception, localExceptionHandlingMode);
        }

        protected void ThrowExceptionWhenNecessary(string assetName, Exception exception, LocalExceptionHandlingMode localExceptionHandlingMode)
        {
            if (exception != null)
            {
                if (localExceptionHandlingMode == LocalExceptionHandlingMode.Throw)
                {
                    throw exception;
                }
                else if (localExceptionHandlingMode == LocalExceptionHandlingMode.Redirect)
                {
                    _sendExceptionToManagerCallback(exception);
                }
            }
        }
    }

    partial class RemoteAgencyManagingObjectProxy<TEntityBase>
    {
        protected override void PreProcessMessageReceivedFromOutside(IRemoteAgencyMessage message)
        {
            SetStickyTargetId(message);
        }

        protected override void ProcessMethodMessageReceived(IRemoteAgencyMessage message)
        {
            //response
            OnResponseReceived(message);
        }

        protected override void ProcessEventAddMessageReceived(IRemoteAgencyMessage message)
        {
            //response
            OnResponseReceived(message);
        }

        protected override void ProcessEventMessageReceived(IRemoteAgencyMessage message)
        {
            //request (notify)
            ProcessRequestAndSendResponseIfRequired(message, _proxyObject.ProcessEventRaisingMessage,
                _proxyObject.ProcessOneWayEventRaisingMessage);
        }

        protected override void ProcessEventRemoveMessageReceived(IRemoteAgencyMessage message)
        {
            //response
            OnResponseReceived(message);
        }

        protected override void ProcessPropertyGetMessageReceived(IRemoteAgencyMessage message)
        {
            //response
            OnResponseReceived(message);
        }

        protected override void ProcessPropertySetMessageReceived(IRemoteAgencyMessage message)
        {
            //response
            OnResponseReceived(message);
        }

        //protected override void ProcessSpecialCommandMessageReceived(IRemoteAgencyMessage message)
        //{
        //    //nothing to do.
        //}

        void SetStickyTargetId(IRemoteAgencyMessage message)
        {
            if (_isStickyModeEnabled && !_stickyTargetSiteId.HasValue)
            {
                _stickyTargetSiteId = message.SenderSiteId;
            }
        }
    }

    partial class RemoteAgencyManagingObjectServiceWrapper<TEntityBase>
    {
        void ProcessRequestAndSendResponseIfRequired(IRemoteAgencyMessage message, AccessWithoutReturn withoutReturnCallback) //when add and remove event handler
        {
            ProcessThreadLockWithoutReturn(withoutReturnCallback, message, out var exception, out var localExceptionHandlingMode);

            if (!message.IsOneWay)
            {
                var response = CreateEmptyMessage();
                response.Exception = exception;
                ProcessResponseMessageReceivedFromInside(response, message);
            }

            ThrowExceptionWhenNecessary(message.AssetName, exception, localExceptionHandlingMode);
        }

        protected override void ProcessMethodMessageReceived(IRemoteAgencyMessage message)
        {
            //request
            ProcessRequestAndSendResponseIfRequired(message, _serviceWrapperObject.ProcessMethodMessage,
                _serviceWrapperObject.ProcessOneWayMethodMessage);
        }

        protected override void ProcessEventAddMessageReceived(IRemoteAgencyMessage message)
        {
            //request
            ProcessRequestAndSendResponseIfRequired(message, _serviceWrapperObject.ProcessEventAddingMessage);
        }
        
        protected override void ProcessEventMessageReceived(IRemoteAgencyMessage message)
        {
            //response
            OnResponseReceived(message);
        }

        protected override void ProcessEventRemoveMessageReceived(IRemoteAgencyMessage message)
        {
            //request
            ProcessRequestAndSendResponseIfRequired(message, _serviceWrapperObject.ProcessEventRemovingMessage);
        }

        protected override void ProcessPropertyGetMessageReceived(IRemoteAgencyMessage message)
        {
            //request
            ProcessRequestAndSendResponseIfRequired(message, _serviceWrapperObject.ProcessPropertyGettingMessage, _serviceWrapperObject.ProcessOneWayPropertyGettingMessage);
        }

        protected override void ProcessPropertySetMessageReceived(IRemoteAgencyMessage message)
        {
            //request
            ProcessRequestAndSendResponseIfRequired(message, _serviceWrapperObject.ProcessPropertySettingMessage,
                _serviceWrapperObject.ProcessOneWayPropertySettingMessage);
        }

        //protected override void ProcessSpecialCommandMessageReceived(IRemoteAgencyMessage message)
        //{
        //    //nothing to do.
        //}
    }
}
