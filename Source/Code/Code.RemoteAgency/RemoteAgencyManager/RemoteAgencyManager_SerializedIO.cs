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
        /// Occurs when a message need to be sent.
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

            if (!managingObjects.TryGetValue(targetInstanceId, out var managingObject))
            {
                if (!isOneWay)
                {
                    var exception = new WrappedException<ObjectNotFoundException>(new ObjectNotFoundException(targetInstanceId));
                    var serializedException = SerializingHelper.SerializeException(exception);
                    var packaged = PackingHelper.Pack(targetSiteId, senderSiteId, targetInstanceId, senderInstanceId, true, messageType, assetName, messageId, true, serializedException, new Type[] { typeof(ObjectNotFoundException) });
                    SendPackagedMessage(senderSiteId, packaged);
                }
                return;
            }

            if (isException)
            {
                managingObject.ProcessException(messageType, assetName, messageId, serialized, genericArguments[0]);
            }
            else
            {
                if (messageType == MessageType.SpecialCommand)
                {
                    if (assetName == "Dispose")
                    {
                        managingObject.TargetProxyDisposed(senderInstanceId);
                        OnDisposingMessageRequiredProxyRemoved(senderSiteId, senderInstanceId);
                        return;
                    }
                }
                managingObject.ProcessMessage(senderSiteId, senderInstanceId, messageType, assetName, messageId, isOneWay, serialized, genericArguments);
            }
        }

        void SendMessage(Guid targetSiteId, Guid senderInstanceId, Guid targetInstanceId, MessageType messageType, string assetName, Guid messageId, bool isOneWay, TSerialized serialized, Type[] genericArguments)
        {
            ThrowIfNotConnected();
            var packaged = PackingHelper.Pack(SiteId, targetSiteId, senderInstanceId, targetInstanceId, false, messageType, assetName, messageId, isOneWay, serialized, genericArguments);
            SendPackagedMessage(targetSiteId, packaged);
        }

        void SendException(Guid targetSiteId, Guid senderInstanceId, Guid targetInstanceId, MessageType messageType, string assetName, Guid messageId, TSerialized serializedException, Type exceptionType)
        {
            ThrowIfNotConnected();
            var packaged = PackingHelper.Pack(SiteId, targetSiteId, senderInstanceId, targetInstanceId, true, messageType, assetName, messageId, true, serializedException, new Type[] { exceptionType });
            SendPackagedMessage(targetSiteId, packaged);
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
