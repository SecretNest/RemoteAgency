using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyManagingObject
    {
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
                case MessageType.SpecialCommand:
                    ProcessSpecialCommandMessageReceived(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(IRemoteAgencyMessage.MessageType));
            }
        }

        protected abstract void ProcessMethodMessageReceived(IRemoteAgencyMessage message);

        protected abstract void ProcessEventAddMessageReceived(IRemoteAgencyMessage message);

        protected abstract void ProcessEventRemoveMessageReceived(IRemoteAgencyMessage message);

        protected abstract void ProcessEventMessageReceived(IRemoteAgencyMessage message);

        protected abstract void ProcessPropertyGetMessageReceived(IRemoteAgencyMessage message);

        protected abstract void ProcessPropertySetMessageReceived(IRemoteAgencyMessage message);

        protected abstract void ProcessSpecialCommandMessageReceived(IRemoteAgencyMessage message);

        protected void ProcessRequestAndSendResponseIfRequired(IRemoteAgencyMessage message, AccessWithReturn withReturnCallback, AccessWithoutReturn withoutReturnCallback)
        {
            Exception exception;

            if (message.IsOneWay)
            {
                ProcessThreadLockWithoutReturn(withoutReturnCallback, message, out exception);
            }
            else
            {
                ProcessThreadLockWithReturn(withReturnCallback, message, out var response, out exception);

                response.Exception = exception;
                ProcessResponseMessageReceivedFromInside(response, message);
            }

            if (exception != null)
            {
                if (_localExceptionHandlingAssets.TryGetValue(message.AssetName, out var localExceptionHandlingMode)) //if not exist, suppressed (default).
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

        protected void ProcessRequestAndSendResponseIfRequired(IRemoteAgencyMessage message, AccessWithReturn withReturnCallback) //when drop response while property getting
        {
            ProcessThreadLockWithReturn(withReturnCallback, message, out var response, out var exception);

            if (!message.IsOneWay)
            {
                response.Exception = exception;
                ProcessResponseMessageReceivedFromInside(response, message);
            }

            if (exception != null)
            {
                if (_localExceptionHandlingAssets.TryGetValue(message.AssetName, out var localExceptionHandlingMode)) //if not exist, suppressed (default).
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
    }

    partial class RemoteAgencyManagingObjectProxy<TEntityBase>
    {
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
            //request
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

        protected override void ProcessSpecialCommandMessageReceived(IRemoteAgencyMessage message)
        {
            //nothing to do.
        }
    }

    partial class RemoteAgencyManagingObjectServiceWrapper<TEntityBase>
    {


        protected override void ProcessMethodMessageReceived(IRemoteAgencyMessage message)
        {
            //request
            ProcessRequestAndSendResponseIfRequired(message, _serviceWrapperObject.ProcessMethodMessage,
                _serviceWrapperObject.ProcessOneWayMethodMessage);
        }

        protected override void ProcessEventAddMessageReceived(IRemoteAgencyMessage message)
        {
            //request

            //special 


            throw new NotImplementedException();
        }
        
        protected override void ProcessEventMessageReceived(IRemoteAgencyMessage message)
        {
            //response

            //special 


            OnResponseReceived(message);
        }

        protected override void ProcessEventRemoveMessageReceived(IRemoteAgencyMessage message)
        {
            //request

            //special 


            throw new NotImplementedException();
        }

        protected override void ProcessPropertyGetMessageReceived(IRemoteAgencyMessage message)
        {
            //request
            ProcessRequestAndSendResponseIfRequired(message, _serviceWrapperObject.ProcessPropertyGettingMessage);
        }

        protected override void ProcessPropertySetMessageReceived(IRemoteAgencyMessage message)
        {
            //request
            ProcessRequestAndSendResponseIfRequired(message, _serviceWrapperObject.ProcessPropertySettingMessage,
                _serviceWrapperObject.ProcessOneWayPropertySettingMessage);
        }

        protected override void ProcessSpecialCommandMessageReceived(IRemoteAgencyMessage message)
        {
            //request (notification): dispose
            if (message.AssetName == SpecialCommands.Dispose)
            {
                OnProxyDisposed(message.SenderSiteId,
                    message.SenderInstanceId);
            }
        }
    }
}
