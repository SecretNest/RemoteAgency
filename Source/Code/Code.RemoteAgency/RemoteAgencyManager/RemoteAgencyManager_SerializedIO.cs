using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    public partial class RemoteAgencyManager<TNetworkMessage, TSerialized, TEntityBase> where TEntityBase : class
    {
        object ioStateLock = new object();
        /// <summary>
        /// Gets whether IO is connected.
        /// </summary>
        public bool IOConnected { get; private set; }

        void ThrowIfNotConnected()
        {
            if (!IOConnected)
            {
                throw new IONotConnectedException();
            }
        }

        /// <summary>
        /// Occurs when a message is generated and ready to be sent.
        /// </summary>
        public event EventHandler<RemoteAgencyManagerMessageForSendingEventArgs<TNetworkMessage>> MessageForSendingPrepared;
        void SendPackagedMessage(Guid targetSiteId, TNetworkMessage message)
        {
            RemoteAgencyManagerMessageForSendingEventArgs<TNetworkMessage> e = new RemoteAgencyManagerMessageForSendingEventArgs<TNetworkMessage>(targetSiteId, message);
            MessageForSendingPrepared(this, e);
        }

        /// <summary>
        /// Processes a message received.
        /// </summary>
        /// <param name="message">Message received.</param>
        /// <exception cref="PackagedMessageNotRecognizedException{TNetworkMessage}">Thrown when the message received cannot be recognized or decoded.</exception>
        public void ProcessPackagedMessage(TNetworkMessage message)
        {
            ThrowIfNotConnected();
            if (!PackingHelper.TryUnpack(message, out MessageInstanceMetadata metadata, out TSerialized serialized, out Type[] genericArguments))
            {
                throw new PackagedMessageNotRecognizedException<TNetworkMessage>(message);
            }

            if (metadata.IsException)
            {
                ProcessReceivedException(metadata, serialized, genericArguments[0]);
            }
            else
            {
                ProcessReceivedMessage(metadata, serialized, genericArguments);
            }
        }

        /// <summary>
        /// Occurs when a message need to be checked for sending.
        /// </summary>
        /// <seealso cref="MessageProcessTerminatedException" />
        public event EventHandler<BeforeMessageProcessingEventArgsBase<TSerialized>> AfterMessageReceived;

        void ProcessReceivedMessage(MessageInstanceMetadata metadata, TSerialized serialized, Type[] genericArguments)
        {
            RemoteAgencyManagingObject<TSerialized> managingObject;
            if (AfterMessageReceived != null && metadata.MessageType != MessageType.SpecialCommand)
            {
                BeforeMessageProcessingEventArgs<TSerialized> e = new BeforeMessageProcessingEventArgs<TSerialized>
                    (metadata, serialized, genericArguments);
                AfterMessageReceived(this, e);

                switch (e.FurtherProcessing)
                {
                    //case MessageFurtherProcessing.Continue:
                    case MessageFurtherProcessing.TerminateSilently:
                        return;
                    case MessageFurtherProcessing.TerminateWithExceptionReturned:
                        metadata = new MessageInstanceMetadata(SiteId, metadata.TargetInstanceId, metadata.SenderSiteId, metadata.SenderInstanceId, metadata.MessageType, metadata.AssetName, metadata.MessageId, true, true);
                        SendExceptionBypassCheckingEvent(metadata, messageProcessTerminatedException.Value, typeof(MessageProcessTerminatedException));
                        return;
                    case MessageFurtherProcessing.ReplacedWithException:
                        if (!managingObjects.TryGetValue(metadata.TargetInstanceId, out managingObject))
                        {
                            if (!metadata.IsOneWay)
                            {
                                var exception = new WrappedException<ObjectNotFoundException>(new ObjectNotFoundException(metadata.TargetInstanceId));
                                var serializedException = SerializingHelper.SerializeException(exception);
                                metadata = new MessageInstanceMetadata(SiteId, metadata.TargetInstanceId, metadata.SenderSiteId, metadata.SenderInstanceId, metadata.MessageType, metadata.AssetName, metadata.MessageId, true, true);
                                var packaged = PackingHelper.Pack(metadata, serializedException, new Type[] { typeof(ObjectNotFoundException) });
                                SendPackagedMessage(metadata.SenderSiteId, packaged);
                            }
                            return;
                        }
                        metadata = new MessageInstanceMetadata(metadata.SenderSiteId, metadata.SenderInstanceId, metadata.TargetSiteId, metadata.TargetInstanceId, metadata.MessageType, metadata.AssetName, metadata.MessageId, metadata.IsOneWay, true);
                        ProcessReceivedExceptionBypassCheckingEvent(managingObject, metadata, messageProcessTerminatedException.Value, typeof(MessageProcessTerminatedException));
                        return;
                }
            }

            if (!managingObjects.TryGetValue(metadata.TargetInstanceId, out managingObject))
            {
                if (!metadata.IsOneWay)
                {
                    var exception = new WrappedException<ObjectNotFoundException>(new ObjectNotFoundException(metadata.TargetInstanceId));
                    var serializedException = SerializingHelper.SerializeException(exception);
                    metadata = new MessageInstanceMetadata(SiteId, metadata.TargetInstanceId, metadata.SenderSiteId, metadata.SenderInstanceId, metadata.MessageType, metadata.AssetName, metadata.MessageId, true, true);
                    var packaged = PackingHelper.Pack(metadata, serializedException, new Type[] { typeof(ObjectNotFoundException) });
                    SendPackagedMessage(metadata.SenderSiteId, packaged);
                }
                return;
            }

            ProcessReceivedMessageBypassCheckingEvent(managingObject, metadata, serialized, genericArguments);
        }

        void ProcessReceivedException(MessageInstanceMetadata metadata, TSerialized serializedException, Type exceptionType)
        {
            RemoteAgencyManagingObject<TSerialized> managingObject;
            if (AfterMessageReceived != null)
            {
                BeforeExceptionMessageProcessingEventArgs<TSerialized> e = new BeforeExceptionMessageProcessingEventArgs<TSerialized>
                    (metadata, serializedException, exceptionType);
                AfterMessageReceived(this, e);

                switch (e.FurtherProcessing)
                {
                    //case MessageFurtherProcessing.Continue:
                    case MessageFurtherProcessing.TerminateSilently:
                        return;
                    //case MessageFurtherProcessing.TerminateWithExceptionReturned:
                    case MessageFurtherProcessing.ReplacedWithException:
                        serializedException = messageProcessTerminatedException.Value;
                        exceptionType = typeof(MessageProcessTerminatedException);
                        break;
                }
            }

            if (!managingObjects.TryGetValue(metadata.TargetInstanceId, out managingObject))
            {
                return;
            }

            ProcessReceivedExceptionBypassCheckingEvent(managingObject, metadata, serializedException, exceptionType);
        }

        void ProcessReceivedMessageBypassCheckingEvent(RemoteAgencyManagingObject<TSerialized> managingObject, MessageInstanceMetadata metadata, TSerialized serialized, Type[] genericArguments)
        {
            if (metadata.MessageType == MessageType.SpecialCommand)
            {
                if (metadata.AssetName == "Dispose")
                {
                    managingObject.TargetProxyDisposed(metadata.SenderInstanceId);
                    OnDisposingMessageRequiredProxyRemoved(metadata.SenderSiteId, metadata.SenderInstanceId);
                    return;
                }
            }

            try
            {
                MessageInstanceMetadataService.messageInstanceMetadata = metadata;
                managingObject.ProcessMessage(metadata.SenderSiteId, metadata.SenderInstanceId, metadata.MessageType, metadata.AssetName, metadata.MessageId, metadata.IsOneWay, serialized, genericArguments);
            }
            finally
            {
                MessageInstanceMetadataService.messageInstanceMetadata = null;
            }
        }

        void ProcessReceivedExceptionBypassCheckingEvent(RemoteAgencyManagingObject<TSerialized> managingObject, MessageInstanceMetadata metadata, TSerialized serializedException, Type exceptionType)
        {
            try
            {
                MessageInstanceMetadataService.messageInstanceMetadata = metadata;
                managingObject.ProcessException(metadata.MessageType, metadata.AssetName, metadata.MessageId, serializedException, exceptionType);
            }
            finally
            {
                MessageInstanceMetadataService.messageInstanceMetadata = null;
            }
        }

        /// <summary>
        /// Occurs when a message need to be checked for sending.
        /// </summary>
        /// <seealso cref="MessageProcessTerminatedException" />
        public event EventHandler<BeforeMessageProcessingEventArgsBase<TSerialized>> BeforeMessageSending;

        Lazy<TSerialized> messageProcessTerminatedException;

        void SendMessage(Guid targetSiteId, Guid senderInstanceId, Guid targetInstanceId, MessageType messageType, string assetName, Guid messageId, bool isOneWay, TSerialized serialized, Type[] genericArguments)
        {
            MessageInstanceMetadata metadata = new MessageInstanceMetadata(SiteId, senderInstanceId, targetSiteId, targetInstanceId, messageType, assetName, messageId, isOneWay, false);

            if (BeforeMessageSending != null && messageType != MessageType.SpecialCommand)
            {
                BeforeMessageProcessingEventArgs<TSerialized> e = new BeforeMessageProcessingEventArgs<TSerialized>
                    (metadata, serialized, genericArguments);
                BeforeMessageSending(this, e);

                switch(e.FurtherProcessing)
                {
                    //case MessageFurtherProcessing.Continue:
                    case MessageFurtherProcessing.TerminateSilently:
                        return;
                    case MessageFurtherProcessing.TerminateWithExceptionReturned:
                        if (managingObjects.TryGetValue(senderInstanceId, out var managingObject))
                        {
                            //directly send it to the original sender (within the same manager)
                            metadata = new MessageInstanceMetadata(targetSiteId, targetInstanceId, SiteId, senderInstanceId, messageType, assetName, messageId, true, true);
                            ProcessReceivedExceptionBypassCheckingEvent(managingObject, metadata, messageProcessTerminatedException.Value, typeof(MessageProcessTerminatedException));
                        }
                        return;
                    case MessageFurtherProcessing.ReplacedWithException:
                        metadata = new MessageInstanceMetadata(SiteId, senderInstanceId, targetSiteId, targetInstanceId, messageType, assetName, messageId, true, true);
                        SendExceptionBypassCheckingEvent(metadata, messageProcessTerminatedException.Value, typeof(MessageProcessTerminatedException));
                        return;
                }
            }

            SendMessageBypassCheckingEvent(metadata, serialized, genericArguments);
        }

        void SendException(Guid targetSiteId, Guid senderInstanceId, Guid targetInstanceId, MessageType messageType, string assetName, Guid messageId, TSerialized serializedException, Type exceptionType)
        {
            MessageInstanceMetadata metadata = new MessageInstanceMetadata(SiteId, senderInstanceId, targetSiteId, targetInstanceId, messageType, assetName, messageId, true, true);

            if (BeforeMessageSending != null)
            {
                BeforeExceptionMessageProcessingEventArgs<TSerialized> e = new BeforeExceptionMessageProcessingEventArgs<TSerialized>
                    (metadata, serializedException, exceptionType);
                BeforeMessageSending(this, e);

                switch (e.FurtherProcessing)
                {
                    //case MessageFurtherProcessing.Continue:
                    case MessageFurtherProcessing.TerminateSilently:
                        return;
                    //case MessageFurtherProcessing.TerminateWithExceptionReturned:
                    case MessageFurtherProcessing.ReplacedWithException:
                        serializedException = messageProcessTerminatedException.Value;
                        exceptionType = typeof(MessageProcessTerminatedException);
                        break;
                }
            }

            SendExceptionBypassCheckingEvent(metadata, serializedException, exceptionType);
        }

        void SendMessageBypassCheckingEvent(MessageInstanceMetadata metadata, TSerialized serialized, Type[] genericArguments)
        {
            if (metadata.TargetSiteId == SiteId)
            {
                ProcessReceivedMessage(metadata, serialized, genericArguments);
            }
            else
            {
                ThrowIfNotConnected();
                var packaged = PackingHelper.Pack(metadata, serialized, genericArguments);
                SendPackagedMessage(metadata.TargetSiteId, packaged);
            }
        }

        void SendExceptionBypassCheckingEvent(MessageInstanceMetadata metadata, TSerialized serializedException, Type exceptionType)
        {
            if (metadata.TargetSiteId == SiteId)
            {
                ProcessReceivedException(metadata, serializedException, exceptionType);
            }
            else
            {
                ThrowIfNotConnected();
                var packaged = PackingHelper.Pack(metadata, serializedException, new Type[] { exceptionType });
                SendPackagedMessage(metadata.TargetSiteId, packaged);
            }
        }

        /// <summary>
        /// Sets the status to connected.
        /// </summary>
        /// <param name="contextId">Context id to be set to <see cref="ContextId"/>. A randomized value will be used if this parameter absents.</param>
        /// <exception cref="InvalidOperationException">Thrown if the <see cref="MessageForSendingPrepared"/> is not handled.</exception>
        public void Connect(Guid? contextId = null)
        {
            lock(ioStateLock)
            {
                if (IOConnected) return;
                if (MessageForSendingPrepared == null)
                    throw new InvalidOperationException("Event MessageForSendingPrepared not handled.");
                if (contextId.HasValue)
                    ResetContextId(contextId.Value);
                else
                    ResetContextId();
                IOConnected = true;
            }
        }

        /// <summary>
        /// Sets the status to disconnected.
        /// </summary>
        /// <param name="force">True: break all waiting messages (throwing timeout exceptions to all messages); False: wait for processing of all messages finished.</param>
        public void Disconnect(bool force)
        {
            lock(ioStateLock)
            {
                if (!IOConnected) return;
                ResetContextId(Guid.Empty);
                IOConnected = false;
                if (force) BreakConnections(); else WaitConnectionsDone();
            }
        }

        void BreakConnections()
        {
            foreach(var key in managingObjects.Keys)
            {
                if (managingObjects.TryRemove(key, out var item))
                {
                    item.CancelWaiting();
                    item.Dispose();
                }
            }
        }

        void WaitConnectionsDone()
        {
            foreach (var key in managingObjects.Keys)
            {
                if (managingObjects.TryRemove(key, out var item))
                {
                    item.WaitAll();
                    item.Dispose();
                }
            }
        }
    }


    /// <summary>
    /// Represents a message to be sent.
    /// </summary>
    /// <typeparam name="TNetworkMessage">Type of the message for transporting.</typeparam>
    /// <seealso cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}.MessageForSendingPrepared"/>
    public class RemoteAgencyManagerMessageForSendingEventArgs<TNetworkMessage> : EventArgs
    {
        /// <summary>
        /// Gets the target site id.
        /// </summary>
        public Guid TargetSiteId { get; }

        /// <summary>
        /// Gets the message to be sent.
        /// </summary>
        public TNetworkMessage Message { get; }

        /// <summary>
        /// Initializes an instance of the RemoteAgencyManagerMessageForSendingEventArgs.
        /// </summary>
        /// <param name="targetSiteId">Target site id.</param>
        /// <param name="message">Message to be sent.</param>
        public RemoteAgencyManagerMessageForSendingEventArgs(Guid targetSiteId, TNetworkMessage message)
        {
            TargetSiteId = targetSiteId;
            Message = message;
        }
    }
}
