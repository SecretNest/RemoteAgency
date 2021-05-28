using System;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyManagingObject
    {
        protected void SetMessagePropertyFromRequest(IRemoteAgencyMessage message, IRemoteAgencyMessage requestMessage)
        {
            message.MessageId = requestMessage.MessageId;
            message.AssetName = requestMessage.AssetName;
            message.IsOneWay = true;
            //message.Exception set by caller.
            message.SenderInstanceId = InstanceId;
            //message.SenderSiteId leave to manager.
            message.TargetInstanceId = requestMessage.SenderInstanceId;
            message.TargetSiteId = requestMessage.SenderSiteId;
            message.MessageType = requestMessage.MessageType;

            _sendMessageToManagerCallback(message);
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
                case MessageType.SpecialCommand:
                    ProcessSpecialCommandMessageReceived(message);
                    break;
                default:
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                    throw new ArgumentOutOfRangeException(nameof(IRemoteAgencyMessage.MessageType));
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
#pragma warning restore IDE0079 // Remove unnecessary suppression
            }
        }

        //public void SetRespondDirectly(IRemoteAgencyMessage message)
        //    => OnResponseReceived(message);

        protected abstract void ProcessMethodMessageReceived(IRemoteAgencyMessage message);

        protected abstract void ProcessEventAddMessageReceived(IRemoteAgencyMessage message);

        protected abstract void ProcessEventRemoveMessageReceived(IRemoteAgencyMessage message);

        protected abstract void ProcessEventMessageReceived(IRemoteAgencyMessage message);

        protected abstract void ProcessPropertyGetMessageReceived(IRemoteAgencyMessage message);

        protected abstract void ProcessPropertySetMessageReceived(IRemoteAgencyMessage message);

        protected void ProcessSpecialCommandMessageReceived(IRemoteAgencyMessage message)
        {
            if (message.AssetName == Const.SpecialCommandProxyPing)
            {
                if (message.IsOneWay)
                {
                    //pong
                    OnResponseReceived(message);
                }
                else
                {
                    //ping -> send pong back

                    try
                    {
                        var pong = CreateEmptyMessage();
                        SetMessagePropertyFromRequest(pong, message);
                    }
#pragma warning disable CA1031 // Do not catch general exception types
                    catch (Exception e)
                    {
                        ThrowExceptionWhenNecessary($"<{nameof(MessageType.SpecialCommand)}>{message.AssetName}", e,
                            LocalExceptionHandlingMode.Redirect);
                    }
#pragma warning restore CA1031 // Do not catch general exception types
                }
            }
        }

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
                SetMessagePropertyFromRequest(response, message);
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
                    _sendExceptionToManagerCallback(InstanceId, assetName, exception);
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
    }

    partial class RemoteAgencyManagingObjectServiceWrapper<TEntityBase>
    {
        void ProcessRequestAndSendResponse(IRemoteAgencyMessage message, AccessWithoutReturn withoutReturnCallback) //when add and remove event handler
        {
            ProcessThreadLockWithoutReturn(withoutReturnCallback, message, out var exception, out var localExceptionHandlingMode);

            //if (!message.IsOneWay) //event adding and removing are always two-way.
            //{
            var response = CreateEmptyMessage();
            response.Exception = exception;
            SetMessagePropertyFromRequest(response, message);
            //}

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
            ProcessRequestAndSendResponse(message, _serviceWrapperObject.ProcessEventAddingMessage);
        }
        
        protected override void ProcessEventMessageReceived(IRemoteAgencyMessage message)
        {
            //response
            OnResponseReceived(message);
        }

        protected override void ProcessEventRemoveMessageReceived(IRemoteAgencyMessage message)
        {
            //request
            ProcessRequestAndSendResponse(message, _serviceWrapperObject.ProcessEventRemovingMessage);
        }

        protected override void ProcessPropertyGetMessageReceived(IRemoteAgencyMessage message)
        {
            //request
            ProcessRequestAndSendResponseIfRequired(message, _serviceWrapperObject.ProcessPropertyGettingMessage,
                _serviceWrapperObject.ProcessOneWayPropertyGettingMessage);
        }

        protected override void ProcessPropertySetMessageReceived(IRemoteAgencyMessage message)
        {
            //request
            ProcessRequestAndSendResponseIfRequired(message, _serviceWrapperObject.ProcessPropertySettingMessage,
                _serviceWrapperObject.ProcessOneWayPropertySettingMessage);
        }
    }
}
