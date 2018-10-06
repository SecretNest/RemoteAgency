using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Represents a created object that can communicate with Remote Agency Manager.
    /// </summary>
    /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
    public interface ICommunicate<TSerialized>
    {
        /// <summary>
        /// Will be called after this object is linked to a Remote Agency Manager.
        /// </summary>
        void AfterInitialized();

        /// <summary>
        /// Processes a message.
        /// </summary>
        /// <param name="messageType"><see cref="MessageType">Message type.</see></param>
        /// <param name="assetName">Asset name.</param>
        /// <param name="messageId">Id of the message.</param>
        /// <param name="isOneWay">When set to true, no related response should be generated from this object.</param>
        /// <param name="data">Serialized data.</param>
        /// <param name="genericArguments">Generic arguments used in this calling.</param>
        void ProcessMessage(MessageType messageType, string assetName, System.Guid messageId, bool isOneWay, TSerialized data, Type[] genericArguments);

        /// <summary>
        /// Processes an exception.
        /// </summary>
        /// <param name="messageType"><see cref="MessageType">Message type.</see></param>
        /// <param name="assetName">Asset name.</param>
        /// <param name="messageId">Id of the message.</param>
        /// <param name="serializedException">Serialized data which represents an exception.</param>
        /// <param name="exceptionType">The type of the exception.</param>
        void ProcessException(MessageType messageType, string assetName, System.Guid messageId, TSerialized serializedException, Type exceptionType);

        /// <summary>
        /// Should be called while an exception is raised in user code.
        /// </summary>
        /// <seealso cref="Attributes.LocalExceptionHandlingAttribute"/>
        /// <remarks>This will only be called when <see cref="Attributes.LocalExceptionHandlingAttribute"/> exists and the <see cref="Attributes.LocalExceptionHandlingMode"/> is set to Redirect. When it's not set, the exception will be suppressed.</remarks>
        RedirectedExceptionRaisedCallback RedirectedExceptionRaisedCallback { get; set; }
        /// <summary>
        /// Should be called while waiting for response is timed out.
        /// </summary>
        /// <seealso cref="Attributes.CustomizedOperatingTimedoutTimeAttribute"/>
        MessageWaitingTimedOutCallback MessageWaitingTimedOutCallback { get; set; }
        /// <summary>
        /// Should be called while a message should be sent to a remote site.
        /// </summary>
        SendMessageCallback<TSerialized> SendMessageCallback { get; set; }
        /// <summary>
        /// Should be called while an exception should be sent to a remote site.
        /// </summary>
        SendExceptionCallback<TSerialized> SendExceptionCallback { get; set; }
    }

    /// <summary>
    /// Sends a message out.
    /// </summary>
    /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
    /// <param name="messageType"><see cref="MessageType">Message type.</see></param>
    /// <param name="assetName">Asset name.</param>
    /// <param name="messageId">Id of the message.</param>
    /// <param name="isOneWay">When set to true, no related response should be generated from this object.</param>
    /// <param name="data">Serialized data.</param>
    /// <param name="genericArguments">Generic arguments used in this calling.</param>
    /// <seealso cref="ICommunicate{TSerialized}.SendMessageCallback"/>
    public delegate void SendMessageCallback<TSerialized>(MessageType messageType, string assetName, System.Guid messageId, bool isOneWay, TSerialized data, Type[] genericArguments);

    /// <summary>
    /// Sends an exception out.
    /// </summary>
    /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
    /// <param name="messageType"><see cref="MessageType">Message type.</see></param>
    /// <param name="assetName">Asset name.</param>
    /// <param name="messageId">Id of the message.</param>
    /// <param name="serializedException">Serialized data which represents an exception.</param>
    /// <param name="exceptionType">The type of the exception.</param>
    /// <seealso cref="ICommunicate{TSerialized}.SendExceptionCallback"/>
    public delegate void SendExceptionCallback<TSerialized>(MessageType messageType, string assetName, System.Guid messageId, TSerialized serializedException, Type exceptionType);

    /// <summary>
    /// Notifies a message waiting is timed out.
    /// </summary>
    /// <param name="messageId">Id of the message.</param>
    /// <seealso cref="ICommunicate{TSerialized}.MessageWaitingTimedOutCallback"/>
    /// <seealso cref="Attributes.CustomizedOperatingTimedoutTimeAttribute"/>
    public delegate void MessageWaitingTimedOutCallback(System.Guid messageId);

    /// <summary>
    /// Redirects the exception raised in user code.
    /// </summary>
    /// <param name="messageType"><see cref="MessageType">Message type.</see></param>
    /// <param name="assetName">Asset name.</param>
    /// <param name="messageId">Id of the message.</param>
    /// <param name="interfaceType">The type of the interface related to this asset.</param>
    /// <param name="exception">Raised exception object.</param>
    /// <seealso cref="Attributes.LocalExceptionHandlingAttribute"/>
    /// <seealso cref="ICommunicate{TSerialized}.RedirectedExceptionRaisedCallback"/>
    public delegate void RedirectedExceptionRaisedCallback(MessageType messageType, string assetName, Guid messageId, Type interfaceType, Exception exception);

    /// <summary>
    /// Contains a list of message type
    /// </summary>
    [DataContract(Namespace = "")]
    public enum MessageType
    {
        /// <summary>
        /// Declares this message is related to a method calling or the returning of it.
        /// </summary>
        [EnumMember]
        Method,
        /// <summary>
        /// Declares this message is related to adding event handler or the result of it.
        /// </summary>
        [EnumMember]
        EventAdd,
        /// <summary>
        /// Declares this message is related to removing event handler or the result of it.
        /// </summary>
        [EnumMember]
        EventRemove,
        /// <summary>
        /// Declares this message is related to an event raised or the returning of it.
        /// </summary>
        [EnumMember]
        Event,
        /// <summary>
        /// Declares this message is related to getting value of a property or the returning of it.
        /// </summary>
        [EnumMember]
        PropertyGet,
        /// <summary>
        /// Declares this message is related to setting value of a property or the result of it.
        /// </summary>
        [EnumMember]
        PropertySet,
        /// <summary>
        /// Declares this message is a system reserved message.
        /// </summary>
        [EnumMember]
        SpecialCommand
    }
}
