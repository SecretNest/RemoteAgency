using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    public partial class RemoteAgencyManager<TNetworkMessage, TSerialized, TEntityBase> : IDisposable where TEntityBase : class
    {
        /// <summary>
        /// Occurs when an exception is thrown and redirected.
        /// </summary>
        /// <remarks>If this event is not handled, all redirected exceptions will be suppressed (ignored).</remarks>
        public event EventHandler<RedirectedExceptionEventArgs> RedirectedExceptionRaised;

        void RaiseRedirectedException(MessageType messageType, string assetName, Guid messageId, Type interfaceType, Exception exception)
        {
            if (RedirectedExceptionRaised != null)
            {
                RedirectedExceptionEventArgs e = new RedirectedExceptionEventArgs(SiteId, ContextId, messageType, assetName, messageId, interfaceType, exception);
                RedirectedExceptionRaised(this, e);
            }
        }
    }

    /// <summary>
    /// Represents a redirected exception.
    /// </summary>
    /// <seealso cref="RedirectedExceptionRaisedCallback"/>
    /// <seealso cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}.RedirectedExceptionRaised"/>
    public class RedirectedExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the redirected exception.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Gets the site id.
        /// </summary>
        /// <seealso cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}.SiteId"/>
        public Guid SiteId { get; }

        /// <summary>
        /// Gets the context id.
        /// </summary>
        /// <seealso cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}.ContextId"/>
        public Guid ContextId { get; }

        /// <summary>
        /// Gets the message type.
        /// </summary>
        /// <seealso cref="MessageType"/>
        public MessageType MessageType { get; }

        /// <summary>
        /// Gets the type of service contract interface.
        /// </summary>
        public Type InterfaceType { get; }

        /// <summary>
        /// Gets the asset name.
        /// </summary>
        public string AssetName { get; }

        /// <summary>
        /// Gets the message id.
        /// </summary>
        public Guid MessageId { get; }

        /// <summary>
        /// Initializes an instance of the RedirectedExceptionEventArgs.
        /// </summary>
        /// <param name="siteId">Site id.</param>
        /// <param name="contextId">Context id.</param>
        /// <param name="messageType">Message type.</param>
        /// <param name="assetName">Asset name.</param>
        /// <param name="messageId">Message id.</param>
        /// <param name="interfaceType">Type of service contract interface.</param>
        /// <param name="exception">Redirected exception.</param>
        /// <seealso cref="MessageType"/>
        public RedirectedExceptionEventArgs(Guid siteId, Guid contextId, MessageType messageType, string assetName, Guid messageId, Type interfaceType, Exception exception)
        {
            SiteId = siteId;
            ContextId = contextId;
            MessageType = messageType;
            AssetName = assetName;
            MessageId = messageId;
            Exception = exception;
            InterfaceType = interfaceType;
        }

        /// <summary>
        /// Creates and returns a string representation of this event argument.
        /// </summary>
        /// <returns>A string representation of this event argument.</returns>
        public override string ToString()
        {
            return string.Format("SiteId: {0}\nContextId: {1}\nMessageType: {2}\nAssetName: {3}\nMessageId: {4}\nInterfaceType: {5}\nException: {6}",
                SiteId, ContextId, MessageType, AssetName, MessageId, InterfaceType.FullName, Exception);
        }

    }
}
