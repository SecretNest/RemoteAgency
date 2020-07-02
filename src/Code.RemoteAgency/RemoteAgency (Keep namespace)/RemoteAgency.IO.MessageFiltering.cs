using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SecretNest.RemoteAgency.MessageFiltering;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgency<TSerialized, TEntityBase>
    {
        /// <summary>
        /// Gets or sets whether system messages should be bypassed from filtering. Default value is <see langword="true" />.
        /// </summary>
        public bool BypassSystemMessagesFromFiltering { get; set; } = true;

        /// <summary>
        /// Occurs when a message need to be checked for sending.
        /// </summary>
        /// <seealso cref="MessageProcessTerminatedException" />
        /// <remarks>Internal routing message, which is sending to an object managed by the same instance of Remote Agency, never raise this event.</remarks>
        public event EventHandler<BeforeMessageProcessingEventArgs<TSerialized, TEntityBase>> BeforeMessageSending;

        /// <summary>
        /// Occurs when a message need to be checked for sending.
        /// </summary>
        /// <seealso cref="MessageProcessTerminatedException" />
        /// <remarks>Internal routing message, which is sent from object managed by the same instance of Remote Agency, never raise this event.</remarks>
        public event EventHandler<BeforeMessageProcessingEventArgs<TSerialized, TEntityBase>> AfterMessageReceived;

        void BeforeMessageProcess(ref TEntityBase message,
            EventHandler<BeforeMessageProcessingEventArgs<TSerialized, TEntityBase>> eventHandler, MessageDirection messageDirection,
            Func<TEntityBase, string, bool, TEntityBase> generateExceptionForContinueSendingCallback,
            Func<TEntityBase, string, bool, TEntityBase> generateExceptionForSendingBackCallback,
            Action<TEntityBase> sendMessageBackCallback,
            out bool shouldTerminate)
        {
            if (eventHandler == null)
            {
                shouldTerminate = false;
                return;
            }

            if (BypassSystemMessagesFromFiltering &&
                (((IRemoteAgencyMessage) message).MessageType == MessageType.SpecialCommand ||
                 ((IRemoteAgencyMessage) message).MessageType == MessageType.EventAdd ||
                 ((IRemoteAgencyMessage) message).MessageType == MessageType.EventRemove
                ))
            {
                shouldTerminate = false;
                return;
            }

            var e = new BeforeMessageProcessingEventArgs<TSerialized, TEntityBase>(messageDirection, message, Serialize);
            eventHandler(this, e);

            switch (e.FurtherProcessing)
            {
                case MessageFurtherProcessing.Continue:
                {
                    shouldTerminate = false;
                    return;
                }
                case MessageFurtherProcessing.TerminateAndReturnException:
                {
                    shouldTerminate = true;
                    var newMessage = generateExceptionForSendingBackCallback(message,
                        e.MessageOfMessageProcessTerminatedException,
                        e.IncludeTerminatedMessageInException);
                    Task.Run(() => sendMessageBackCallback(newMessage));

                    return;
                }
                case MessageFurtherProcessing.ReplaceWithException:
                {
                    shouldTerminate = false;
                    message = generateExceptionForContinueSendingCallback(message,
                        e.MessageOfMessageProcessTerminatedException,
                        e.IncludeTerminatedMessageInException);
                    return;
                }
                case MessageFurtherProcessing.ReplaceWithExceptionAndReturn:
                {
                    shouldTerminate = false;
                    var newMessage = generateExceptionForSendingBackCallback(message,
                        e.MessageOfMessageProcessTerminatedException,
                        e.IncludeTerminatedMessageInException);
                    Task.Run(() => sendMessageBackCallback(newMessage));

                    message = generateExceptionForContinueSendingCallback(message,
                        e.MessageOfMessageProcessTerminatedException,
                        e.IncludeTerminatedMessageInException);
                    return;

                }
                case MessageFurtherProcessing.TerminateSilently:
                {
                    shouldTerminate = true;
                    return;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(BeforeMessageProcessingEventArgs<TSerialized, TEntityBase>.FurtherProcessing));
            }
        }

        void BeforeMessageSendingProcess(ref TEntityBase message, out bool shouldTerminate)
        {
            BeforeMessageProcess(
                ref message,
                BeforeMessageSending,
                MessageDirection.Sending,
                BeforeMessageSendingProcess_GenerateExceptionForContinueSending,
                BeforeMessageSendingProcess_GenerateExceptionForSendingBack,
                ProcessMessageReceivedAfterFiltering,
                out shouldTerminate);
        }

        TEntityBase BeforeMessageSendingProcess_GenerateExceptionForContinueSending(
            TEntityBase originalMessage, string exceptionMessage, bool includeTerminatedMessage)
        {
            //Sending message cannot set to ReplaceWithException.
            throw new NotSupportedException(); 
        }

        TEntityBase BeforeMessageSendingProcess_GenerateExceptionForSendingBack(
            TEntityBase originalMessage, string exceptionMessage, bool includeTerminatedMessage)
        {
            var emptyMessage = GenerateEmptyMessage(
                SiteId,
                ((IRemoteAgencyMessage) originalMessage).SenderInstanceId,
                ((IRemoteAgencyMessage) originalMessage).MessageType,
                ((IRemoteAgencyMessage) originalMessage).AssetName,
                ((IRemoteAgencyMessage) originalMessage).MessageId,
                new MessageProcessTerminatedException(exceptionMessage, MessageProcessTerminatedPosition.BeforeSending,
                    (IRemoteAgencyMessage) (includeTerminatedMessage ? originalMessage : default(TEntityBase))));
            return (TEntityBase) emptyMessage;
        }

        void AfterMessageReceivedProcess(ref TEntityBase message, out bool shouldTerminate)
        {
            BeforeMessageProcess(
                ref message,
                AfterMessageReceived,
                MessageDirection.Receiving,
                AfterMessageReceivedProcess_GenerateExceptionForContinueSending,
                AfterMessageReceivedProcess_GenerateExceptionForSendingBack,
                ProcessMessageReceivedFromInsideAfterInternalRoutingAndFiltering,
                out shouldTerminate);
        }

        TEntityBase AfterMessageReceivedProcess_GenerateExceptionForContinueSending(
            TEntityBase originalMessage, string exceptionMessage, bool includeTerminatedMessage)
        {
            var emptyMessage = GenerateEmptyMessage(
                ((IRemoteAgencyMessage) originalMessage).TargetSiteId,
                ((IRemoteAgencyMessage) originalMessage).TargetInstanceId,
                ((IRemoteAgencyMessage) originalMessage).MessageType,
                ((IRemoteAgencyMessage) originalMessage).AssetName,
                ((IRemoteAgencyMessage) originalMessage).MessageId,
                new MessageProcessTerminatedException(exceptionMessage, MessageProcessTerminatedPosition.AfterReceived,
                    (IRemoteAgencyMessage) (includeTerminatedMessage ? originalMessage : default(TEntityBase))));
            return (TEntityBase) emptyMessage;
        }

        TEntityBase AfterMessageReceivedProcess_GenerateExceptionForSendingBack(
            TEntityBase originalMessage, string exceptionMessage, bool includeTerminatedMessage)
        {
            var emptyMessage = GenerateEmptyMessage(
                ((IRemoteAgencyMessage) originalMessage).SenderSiteId,
                ((IRemoteAgencyMessage) originalMessage).SenderInstanceId,
                ((IRemoteAgencyMessage) originalMessage).MessageType,
                ((IRemoteAgencyMessage) originalMessage).AssetName,
                ((IRemoteAgencyMessage) originalMessage).MessageId,
                new MessageProcessTerminatedException(exceptionMessage, MessageProcessTerminatedPosition.AfterReceived,
                    (IRemoteAgencyMessage) (includeTerminatedMessage ? originalMessage : default(TEntityBase))));
            return (TEntityBase) emptyMessage;
        }
    }
}
