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
        protected void RedirectException(Exception exception)
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
        public void ProcessReceivedSerializedMessage(TSerialized serializedMessage)
        {
            var message = Deserialize(serializedMessage);
            ProcessReceivedMessage(message);
        }

        /// <summary>
        /// Processes a message received.
        /// </summary>
        /// <param name="message">Received message.</param>
        public void ProcessReceivedMessage(TEntityBase message)
        {
            ProcessReceivedMessage((IRemoteAgencyMessage) message);
        }

        /// <summary>
        /// Processes a message received.
        /// </summary>
        /// <param name="message">Received message.</param>
        public void ProcessReceivedMessage(IRemoteAgencyMessage message)
        {
            ProcessMessageReceivedFromOutside((TEntityBase)message); //Casting for security only.
        }
    }
}
