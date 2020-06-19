using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    public partial class RemoteAgency<TSerialized, TEntityBase>
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

        /// <summary>
        /// Unlinks specified remote proxy from the event registered in service wrapper objects.
        /// </summary>
        /// <param name="siteId">The site id of the instance of the Remote Agency which managing the disposing proxy.</param>
        /// <param name="proxyInstanceId">The instance id of the disposing proxy.</param>
        /// <remarks><p>Should be called when a remote proxy disposing happened without message routed to <see cref="ProcessReceivedMessage(TEntityBase)"/> or <see cref="ProcessReceivedMessage(IRemoteAgencyMessage)"/>.</p>
        /// <p>Service wrapper object manages links of all proxies which need to handle events. When remote proxy is disposed, a message is sent to the service wrapper object to remove the link. But when something wrong happened, network disconnected or proxy object crashed for example, the crucial message may not be able to transferred correctly. In this case, this method need to be called, or the obsolete link will stay in service wrapper object which may cause lags or exceptions while processing events.</p></remarks>
        public void OnRemoteProxyDisposed(Guid siteId, Guid proxyInstanceId)
        {
            foreach (var remoteAgencyManagingObject in _managingObjects.Values)
            {
                remoteAgencyManagingObject.OnProxyDisposed(siteId, proxyInstanceId);
            }
        }

        /// <summary>
        /// Unlinks all remote proxies managed by the Remote Agency instance specified by <paramref name="siteId"/> from the event registered in service wrapper objects.
        /// </summary>
        /// <param name="siteId">The site id of the instance of the Remote Agency which managing the disposing proxy.</param>
        /// <remarks><p>Should be called when disposing of all proxies of a site happened without message routed to <see cref="ProcessReceivedMessage(TEntityBase)"/> or <see cref="ProcessReceivedMessage(IRemoteAgencyMessage)"/>.</p>
        /// <p>Service wrapper object manages links of all proxies which need to handle events. When remote proxy is disposed, a message is sent to the service wrapper object to remove the link. But when something wrong happened, network disconnected or proxy object crashed for example, the crucial message may not be able to transferred correctly. In this case, this method need to be called, or the obsolete link will stay in service wrapper object which may cause lags or exceptions while processing events.</p></remarks>
        public void OnRemoteProxiesDisposed(Guid siteId)
        {
            foreach (var remoteAgencyManagingObject in _managingObjects.Values)
            {
                remoteAgencyManagingObject.OnProxiesDisposed(siteId);
            }
        }

        /// <summary>
        /// Sends a message through <see cref="MessageForSendingPrepared"/> about the disposing of the local proxy.
        /// </summary>
        /// <param name="proxyInstanceId">Instance id of the disposing proxy.</param>
        /// <param name="targetSiteId">Site id of the target Remote Agency instance.</param>
        /// <param name="targetServiceWrapperInstanceId">Instance id of the target service wrapper object related.</param>
        /// <remarks>This method is called automatically when the proxy with active event handling disposed. A manually call can be made when the original message is not sent successfully.</remarks>
        public void SendProxyDisposeMessage(Guid proxyInstanceId, Guid targetSiteId, Guid targetServiceWrapperInstanceId)
        {
            var emptyMessage = GenerateEmptyMessage(
                proxyInstanceId,
                targetSiteId, targetServiceWrapperInstanceId,
                MessageType.SpecialCommand, SpecialCommands.Dispose,
                Guid.NewGuid(), null, true);
            ProcessMessageReceivedFromInsideAfterInternalRoutingAndFiltering(emptyMessage);
        }
    }
}
