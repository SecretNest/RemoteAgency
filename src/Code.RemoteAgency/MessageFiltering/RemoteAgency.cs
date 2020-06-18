using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using SecretNest.RemoteAgency.MessageFiltering;

namespace SecretNest.RemoteAgency
{
    public partial class RemoteAgency<TSerialized, TEntityBase>
    {
        /// <summary>
        /// Occurs when a message need to be checked for sending.
        /// </summary>
        /// <seealso cref="MessageProcessTerminatedException" />
        public event EventHandler<BeforeMessageProcessingEventArgs<TEntityBase>> BeforeMessageSending;

        /// <summary>
        /// Occurs when a message need to be checked for sending.
        /// </summary>
        /// <seealso cref="MessageProcessTerminatedException" />
        public event EventHandler<BeforeMessageProcessingEventArgs<TEntityBase>> AfterMessageReceived;

        void BeforeMessageProcess(ref TEntityBase message,
            EventHandler<BeforeMessageProcessingEventArgs<TEntityBase>> eventHandler, MessageDirection messageDirection,
            Func<IRemoteAgencyMessage, string, IRemoteAgencyMessage> generateExceptionForContinueSendingCallback,
            Func<IRemoteAgencyMessage, string, IRemoteAgencyMessage> generateExceptionForSendingBackCallback,
            out bool shouldTerminate)
        {
            if (eventHandler == null)
            {
                shouldTerminate = false;
                return;
            }

            var e = new BeforeMessageProcessingEventArgs<TEntityBase>(messageDirection, message);
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
                    var newMessage =
                        (TEntityBase) generateExceptionForSendingBackCallback((IRemoteAgencyMessage) message,
                            e.MessageOfMessageProcessTerminatedException);
                    Task.Run(() => ProcessMessage(newMessage));

                    return;
                }
                case MessageFurtherProcessing.ReplaceWithException:
                {
                    shouldTerminate = false;
                    message = (TEntityBase) generateExceptionForContinueSendingCallback((IRemoteAgencyMessage) message,
                        e.MessageOfMessageProcessTerminatedException);
                    return;
                }
                case MessageFurtherProcessing.ReplaceWithExceptionAndReturn:
                {
                    shouldTerminate = false;
                    var newMessage =
                        (TEntityBase) generateExceptionForSendingBackCallback((IRemoteAgencyMessage) message,
                            e.MessageOfMessageProcessTerminatedException);
                    Task.Run(() => ProcessMessage(newMessage));

                    message = (TEntityBase) generateExceptionForContinueSendingCallback((IRemoteAgencyMessage) message,
                        e.MessageOfMessageProcessTerminatedException);
                    return;

                }
                case MessageFurtherProcessing.TerminateSilently:
                {
                    shouldTerminate = true;
                    return;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(BeforeMessageProcessingEventArgs<TEntityBase>.FurtherProcessing));
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
                out shouldTerminate);
        }

        IRemoteAgencyMessage BeforeMessageSendingProcess_GenerateExceptionForContinueSending(
            IRemoteAgencyMessage originalMessage, string exceptionMessage)
        {
            //Sending message cannot set to ReplaceWithException.
            throw new NotSupportedException(); 
        }

        IRemoteAgencyMessage BeforeMessageSendingProcess_GenerateExceptionForSendingBack(
            IRemoteAgencyMessage originalMessage, string exceptionMessage)
        {
            var emptyMessage = entityCodeBuilder.CreateEmptyMessage();
            emptyMessage.SenderSiteId = SiteId; //Set to self
            emptyMessage.TargetSiteId = SiteId; //Set to self
            emptyMessage.SenderInstanceId = Guid.Empty; //Set to empty.
            emptyMessage.TargetInstanceId = originalMessage.SenderInstanceId; //Set to self
            emptyMessage.MessageType = originalMessage.MessageType;
            emptyMessage.AssetName = originalMessage.AssetName;
            emptyMessage.MessageId = originalMessage.MessageId;
            emptyMessage.Exception = new MessageProcessTerminatedException(exceptionMessage);
            emptyMessage.IsOneWay = true;
            return emptyMessage;
        }

        void AfterMessageReceivedProcess(ref TEntityBase message, out bool shouldTerminate)
        {
            BeforeMessageProcess(
                ref message,
                AfterMessageReceived,
                MessageDirection.Receiving,
                AfterMessageReceivedProcess_GenerateExceptionForContinueSending,
                AfterMessageReceivedProcess_GenerateExceptionForSendingBack,
                out shouldTerminate);
        }

        IRemoteAgencyMessage AfterMessageReceivedProcess_GenerateExceptionForContinueSending(
            IRemoteAgencyMessage originalMessage, string exceptionMessage)
        {
            var emptyMessage = entityCodeBuilder.CreateEmptyMessage();
            emptyMessage.SenderSiteId = originalMessage.SenderSiteId;
            emptyMessage.TargetSiteId = originalMessage.TargetSiteId;
            emptyMessage.SenderInstanceId = originalMessage.SenderInstanceId;
            emptyMessage.TargetInstanceId = originalMessage.TargetInstanceId;
            emptyMessage.MessageType = originalMessage.MessageType;
            emptyMessage.AssetName = originalMessage.AssetName;
            emptyMessage.MessageId = originalMessage.MessageId;
            emptyMessage.Exception = new MessageProcessTerminatedException(exceptionMessage);
            emptyMessage.IsOneWay = originalMessage.IsOneWay;
            return emptyMessage;
        }

        IRemoteAgencyMessage AfterMessageReceivedProcess_GenerateExceptionForSendingBack(
            IRemoteAgencyMessage originalMessage, string exceptionMessage)
        {
            var emptyMessage = entityCodeBuilder.CreateEmptyMessage();
            emptyMessage.SenderSiteId = SiteId;
            emptyMessage.TargetSiteId = originalMessage.SenderSiteId;
            emptyMessage.SenderInstanceId = originalMessage.TargetInstanceId;
            emptyMessage.TargetInstanceId = originalMessage.SenderInstanceId;
            emptyMessage.MessageType = originalMessage.MessageType;
            emptyMessage.AssetName = originalMessage.AssetName;
            emptyMessage.MessageId = originalMessage.MessageId;
            emptyMessage.Exception = new MessageProcessTerminatedException(exceptionMessage);
            emptyMessage.IsOneWay = true;
            return emptyMessage;
        }
    }
}
