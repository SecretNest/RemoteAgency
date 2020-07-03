using System;
using System.Collections.Generic;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgency
    {
        /// <summary>
        /// Occurs when an exception thrown from user code.
        /// </summary>
        /// <seealso cref="LocalExceptionHandlingAttribute"/>
        public event EventHandler<ExceptionRedirectedEventArgs> ExceptionRedirected;

        /// <summary>
        /// Redirects an exception.
        /// </summary>
        /// <param name="exception">Exception to be redirected.</param>
        private protected void RedirectException(Exception exception)
        {
            if (ExceptionRedirected == null)
            {
                throw exception;
            }
            else
            {
                var e = new ExceptionRedirectedEventArgs(exception);
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
    }

    partial class RemoteAgency<TSerialized, TEntityBase>
    {
        /// <summary>
        /// Occurs when a message is generated and ready to be sent.
        /// </summary>
        public event EventHandler<MessageBodyEventArgs<TSerialized, TEntityBase>> MessageForSendingPrepared;

        void SendMessageFinal(TEntityBase message)
        {
            MessageForSendingPrepared?.Invoke(this, new MessageBodyEventArgs<TSerialized, TEntityBase>(message, Serialize));
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

        /// <summary>
        /// Processes a message received.
        /// </summary>
        /// <param name="message">Received message.</param>
        /// <event cref="AfterMessageReceived">Raised after deserialized before further processing.</event>
        public void ProcessReceivedMessage(IRemoteAgencyMessage message)
        {
            ProcessMessageReceivedFromOutside((TEntityBase)message); //Casting for security only.
        }
    }
}
