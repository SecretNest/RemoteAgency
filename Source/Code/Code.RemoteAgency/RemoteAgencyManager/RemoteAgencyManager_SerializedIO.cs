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
            if (!PackingHelper.TryUnpack(message, out Guid senderSiteId, out Guid targetSiteId, out Guid senderInstanceId, out Guid targetInstanceId, out bool isException, out MessageType messageType, out string assetName, out Guid messageId, out bool isOneWay, out TSerialized serialized, out Type[] genericArguments))
            {
                throw new PackagedMessageNotRecognizedException<TNetworkMessage>(message);
            }

            if (isException)
            {
                ProcessReceivedException(senderSiteId, senderInstanceId, targetSiteId, targetInstanceId, messageType, assetName, messageId, serialized, genericArguments[0]);
            }
            else
            {
                ProcessReceivedMessage(senderSiteId, senderInstanceId, targetSiteId, targetInstanceId, messageType, assetName, messageId, isOneWay, serialized, genericArguments);
            }

            //if (!managingObjects.TryGetValue(targetInstanceId, out var managingObject))
            //{
            //    if (!isOneWay)
            //    {
            //        var exception = new WrappedException<ObjectNotFoundException>(new ObjectNotFoundException(targetInstanceId));
            //        var serializedException = SerializingHelper.SerializeException(exception);
            //        var packaged = PackingHelper.Pack(targetSiteId, senderSiteId, targetInstanceId, senderInstanceId, true, messageType, assetName, messageId, true, serializedException, new Type[] { typeof(ObjectNotFoundException) });
            //        SendPackagedMessage(senderSiteId, packaged);
            //    }
            //    return;
            //}

            //if (isException)
            //{
            //    ProcessReceivedExceptionBypassCheckingEvent(managingObject, messageType, assetName, messageId, serialized, genericArguments[0]);
            //}
            //else
            //{
            //    if (messageType == MessageType.SpecialCommand)
            //    {
            //        if (assetName == "Dispose")
            //        {
            //            managingObject.TargetProxyDisposed(senderInstanceId);
            //            OnDisposingMessageRequiredProxyRemoved(senderSiteId, senderInstanceId);
            //            return;
            //        }
            //    }
            //    ProcessReceivedMessageBypassCheckingEvent(managingObject, senderSiteId, senderInstanceId, messageType, assetName, messageId, isOneWay, serialized, genericArguments);
            //}
        }

        /// <summary>
        /// Occurs when a message need to be checked for sending.
        /// </summary>
        /// <seealso cref="MessageProcessTerminatedException" />
        public event EventHandler<BeforeMessageProcessingEventArgsBase<TSerialized>> AfterMessageReceived;

        void ProcessReceivedMessage(Guid senderSiteId, Guid senderInstanceId, Guid targetSiteId, Guid targetInstanceId, MessageType messageType, string assetName, Guid messageId, bool isOneWay, TSerialized serialized, Type[] genericArguments)
        {
            if (AfterMessageReceived != null && messageType != MessageType.SpecialCommand)
            {
                BeforeMessageProcessingEventArgs<TSerialized> e = new BeforeMessageProcessingEventArgs<TSerialized>
                    (senderSiteId, senderInstanceId, targetSiteId, targetInstanceId, messageType, assetName, messageId, isOneWay, serialized, genericArguments);
                AfterMessageReceived(this, e);

                if (e.FurtherProcessing == MessageFurtherProcessing.Continue)
                { }
                else if (e.FurtherProcessing == MessageFurtherProcessing.TerminateSilently)
                {
                    return;
                }
                else
                {
                    //if (managingObjects.TryGetValue(senderInstanceId, out var managingObject))
                    //{
                    //    ProcessReceivedExceptionBypassCheckingEvent(managingObject, messageType, assetName, messageId, sendingProcessTerminatedException.Value, typeof(MessageProcessTerminatedException));
                    //}
                    return;
                }
            }




        }

        void ProcessReceivedException(Guid senderSiteId, Guid senderInstanceId, Guid targetSiteId, Guid targetInstanceId, MessageType messageType, string assetName, Guid messageId, TSerialized serializedException, Type exceptionType)
        {
            if (AfterMessageReceived != null)
            {
                BeforeExceptionMessageProcessingEventArgs<TSerialized> e = new BeforeExceptionMessageProcessingEventArgs<TSerialized>
                    (senderSiteId, senderInstanceId, targetSiteId, targetInstanceId, messageType, assetName, messageId, serializedException, exceptionType);
                AfterMessageReceived(this, e);

                if (e.FurtherProcessing == MessageFurtherProcessing.Continue)
                { }
                else
                {
                    //Exception will always has one way sending setting.
                    return;
                }
            }



        }

        void ProcessReceivedMessageBypassCheckingEvent(RemoteAgencyManagingObject<TSerialized> managingObject, Guid senderSiteId, Guid senderInstanceId, MessageType messageType, string assetName, Guid messageId, bool isOneWay, TSerialized serialized, Type[] genericArguments)
        {
            managingObject.ProcessMessage(senderSiteId, senderInstanceId, messageType, assetName, messageId, isOneWay, serialized, genericArguments);
        }

        void ProcessReceivedExceptionBypassCheckingEvent(RemoteAgencyManagingObject<TSerialized> managingObject, MessageType messageType, string assetName, Guid messageId, TSerialized serializedException, Type exceptionType)
        {
            managingObject.ProcessException(messageType, assetName, messageId, serializedException, exceptionType);
        }

        /// <summary>
        /// Occurs when a message need to be checked for sending.
        /// </summary>
        /// <seealso cref="MessageProcessTerminatedException" />
        public event EventHandler<BeforeMessageProcessingEventArgsBase<TSerialized>> BeforeMessageSending;

        Lazy<TSerialized> sendingProcessTerminatedException;

        void SendMessage(Guid targetSiteId, Guid senderInstanceId, Guid targetInstanceId, MessageType messageType, string assetName, Guid messageId, bool isOneWay, TSerialized serialized, Type[] genericArguments)
        {
            if (BeforeMessageSending != null && messageType != MessageType.SpecialCommand)
            {
                BeforeMessageProcessingEventArgs<TSerialized> e = new BeforeMessageProcessingEventArgs<TSerialized>
                    (SiteId, senderInstanceId, targetSiteId, targetInstanceId, messageType, assetName, messageId, isOneWay, serialized, genericArguments);
                BeforeMessageSending(this, e);

                if (e.FurtherProcessing == MessageFurtherProcessing.Continue)
                { }
                else if (e.FurtherProcessing == MessageFurtherProcessing.TerminateSilently)
                {
                    return;
                }
                else
                {
                    if (managingObjects.TryGetValue(senderInstanceId, out var managingObject))
                    {
                        ProcessReceivedExceptionBypassCheckingEvent(managingObject, messageType, assetName, messageId, sendingProcessTerminatedException.Value, typeof(MessageProcessTerminatedException));
                    }
                    return;
                }
            }

            if (targetSiteId == SiteId)
            {
                ProcessReceivedMessage(SiteId, senderInstanceId, targetSiteId, targetInstanceId, messageType, assetName, messageId, isOneWay, serialized, genericArguments);
            }
            else
            {
                ThrowIfNotConnected();
                var packaged = PackingHelper.Pack(SiteId, targetSiteId, senderInstanceId, targetInstanceId, false, messageType, assetName, messageId, isOneWay, serialized, genericArguments);
                SendPackagedMessage(targetSiteId, packaged);
            }
        }

        void SendException(Guid targetSiteId, Guid senderInstanceId, Guid targetInstanceId, MessageType messageType, string assetName, Guid messageId, TSerialized serializedException, Type exceptionType)
        {
            if (BeforeMessageSending != null)
            {
                BeforeExceptionMessageProcessingEventArgs<TSerialized> e = new BeforeExceptionMessageProcessingEventArgs<TSerialized>
                    (SiteId, senderInstanceId, targetSiteId, targetInstanceId, messageType, assetName, messageId, serializedException, exceptionType);
                BeforeMessageSending(this, e);

                if (e.FurtherProcessing == MessageFurtherProcessing.Continue)
                { }
                else
                { 
                    //Exception will always has one way sending setting.
                    return;
                }
            }

            if (targetSiteId == SiteId)
            {
                ProcessReceivedException(SiteId, senderInstanceId, targetSiteId, targetInstanceId, messageType, assetName, messageId, serializedException, exceptionType);
            }
            else
            {
                ThrowIfNotConnected();
                var packaged = PackingHelper.Pack(SiteId, targetSiteId, senderInstanceId, targetInstanceId, true, messageType, assetName, messageId, true, serializedException, new Type[] { exceptionType });
                SendPackagedMessage(targetSiteId, packaged);
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
