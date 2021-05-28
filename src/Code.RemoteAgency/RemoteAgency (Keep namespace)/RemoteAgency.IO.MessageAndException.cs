using System;
using System.Collections.Generic;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyBase
    {
        /// <summary>
        /// Occurs when an exception thrown from user code.
        /// </summary>
        /// <seealso cref="LocalExceptionHandlingAttribute"/>
        public event EventHandler<ExceptionRedirectedEventArgs> ExceptionRedirected;

        /// <summary>
        /// Redirects an exception.
        /// </summary>
        /// <param name="serviceContractInterface">The type of the service contract interface.</param>
        /// <param name="instanceId">Instance id of the instance which throws exception.</param>
        /// <param name="assetName">The name of the asset which throws exception.</param>
        /// <param name="exception">Exception to be redirected.</param>
        private protected void RedirectException(Type serviceContractInterface, Guid instanceId, string assetName, Exception exception)
        {
            if (ExceptionRedirected == null)
            {
                throw exception;
            }
            else
            {
                var e = new ExceptionRedirectedEventArgs(serviceContractInterface, instanceId, assetName, exception);
                ExceptionRedirected(this, e);
            }
        }

        /// <summary>
        /// Sets an exception as the result of a message waiting for response and break the waiting.
        /// </summary>
        /// <param name="instanceId">Id of proxy or service wrapper instance.</param>
        /// <param name="messageId">Message id.</param>
        /// <param name="exception">The exception object to be passed.</param>
        /// <seealso cref="OperatingTimeoutTimeAttribute"/>
        public void SetExceptionAsResponse(Guid instanceId, Guid messageId, Exception exception)
        {
            var message = GenerateEmptyMessage(Guid.Empty, Guid.Empty, instanceId, MessageType.SpecialCommand,
                $"<Inserted by {nameof(SetExceptionAsResponse)}.>", messageId, exception, true);
            FindManagingObjectAndSendMessage(message);
        }

        /// <summary>
        /// Occurs when a message is generated and ready to be sent.
        /// </summary>
        /// <remarks>This will be raised only when <see cref="RemoteAgency{TSerialized, TEntityBase}.MessageForSendingPrepared"/> is not handled.</remarks>
        public event EventHandler<MessageBodyEventArgs> MessageForSendingPreparedGeneric;

        private protected void RaiseMessageForSendingPreparedGeneric(MessageBodyEventArgs eventArgs)
        {
            MessageForSendingPreparedGeneric?.Invoke(this, eventArgs);
        }

        /// <summary>
        /// Processes a message received.
        /// </summary>
        /// <param name="message">Received message.</param>
        /// <event cref="RemoteAgency{TSerialized, TEntityBase}.AfterMessageReceived">Raised after deserialized before further processing.</event>
        public abstract void ProcessReceivedMessage(IRemoteAgencyMessage message);

        /// <summary>
        /// Tries to gets id of all waiting messages.
        /// </summary>
        /// <param name="instanceId">Id of proxy or service wrapper instance.</param>
        /// <param name="messageIds">Id of all waiting messages.</param>
        /// <returns>Whether the instance is found.</returns>
        public abstract bool TryGetWaitingMessageIds(Guid instanceId, out List<Guid> messageIds);
        
        /// <summary>
        /// Tries to get information of a waiting message.
        /// </summary>
        /// <param name="instanceId">Id of proxy or service wrapper instance.</param>
        /// <param name="messageId">Message id.</param>
        /// <param name="sentMessage">The request message of this waiting one, which is sent.</param>
        /// <param name="startWaiting">The time of waiting started. It can be default value of DateTime when waiting is not started.</param>
        /// <returns>Whether the waiting message is found.</returns>
        public abstract bool TryGetWaitingMessage(Guid instanceId, Guid messageId, out IRemoteAgencyMessage sentMessage,
            out DateTime startWaiting);
    }

    partial class RemoteAgency<TSerialized, TEntityBase>
    {
        /// <summary>
        /// Occurs when a message is generated and ready to be sent.
        /// </summary>
        /// <remarks>If this event is not handled, <see cref="RemoteAgencyBase.MessageForSendingPreparedGeneric"/> will be raised.</remarks>
        public event EventHandler<MessageBodyEventArgs<TSerialized, TEntityBase>> MessageForSendingPrepared;

        void SendMessageFinal(TEntityBase message)
        {
            var e = new MessageBodyEventArgs<TSerialized, TEntityBase>(message, Serialize);

            if (MessageForSendingPrepared != null)
                MessageForSendingPrepared.Invoke(this, e);
            else
                RaiseMessageForSendingPreparedGeneric(e);
        }

        /// <summary>
        /// Processes a serialized message received.
        /// </summary>
        /// <param name="serializedMessage">Received serialized message.</param>
        /// <event cref="AfterMessageReceived">Raised after deserialized before further processing.</event>
        public void ProcessReceivedSerializedMessage(TSerialized serializedMessage)
        {
            var message = Deserialize(serializedMessage);
            ProcessReceivedMessage(message);
        }

        /// <summary>
        /// Processes a message received.
        /// </summary>
        /// <param name="message">Received message.</param>
        /// <event cref="AfterMessageReceived">Raised after deserialized before further processing.</event>
        public void ProcessReceivedMessage(TEntityBase message)
        {
            ProcessReceivedMessage((IRemoteAgencyMessage) message);
        }

        /// <inheritdoc />
        public override void ProcessReceivedMessage(IRemoteAgencyMessage message)
        {
            ProcessMessageReceivedFromOutside((TEntityBase)message); //Casting for security only.
        }

        /// <inheritdoc />
        public override bool TryGetWaitingMessageIds(Guid instanceId, out List<Guid> messageIds)
        {
            if (_managingObjects.TryGetValue(instanceId, out var managingObject))
            {
                messageIds = managingObject.GetWaitingMessageIds();
                return true;
            }
            else
            {
                messageIds = default;
                return false;
            }
        }

        /// <inheritdoc />
        public override bool TryGetWaitingMessage(Guid instanceId, Guid messageId, out IRemoteAgencyMessage sentMessage,
            out DateTime startWaiting)
        {
            if (_managingObjects.TryGetValue(instanceId, out var managingObject))
            {
                return managingObject.TryGetWaitingMessage(messageId, out sentMessage, out startWaiting);
            }
            else
            {
                sentMessage = default;
                startWaiting = default;
                return false;
            }
        }

        /// <summary>
        /// Tries to get information of a waiting message.
        /// </summary>
        /// <param name="instanceId">Id of proxy or service wrapper instance.</param>
        /// <param name="messageId">Message id.</param>
        /// <param name="sentMessage">The request message of this waiting one, which is sent.</param>
        /// <param name="startWaiting">The time of waiting started. It can be default value of DateTime when waiting is not started.</param>
        /// <returns>Whether the waiting message is found.</returns>
        public bool TryGetWaitingMessage(Guid instanceId, Guid messageId, out TEntityBase sentMessage,
            out DateTime startWaiting)
        {
            var result = TryGetWaitingMessage(instanceId, messageId, out IRemoteAgencyMessage sentMessageGeneric,
                out startWaiting);
            sentMessage = (TEntityBase) sentMessageGeneric;
            return result;
        }
    }
}
