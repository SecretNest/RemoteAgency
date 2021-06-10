using System;

namespace SecretNest.RemoteAgency.JsonSerializer
{
    /// <summary>
    /// Defines an empty message with Json serialization support.
    /// </summary>
    /// <remarks><para>This class is not present in Neat release.</para></remarks>
    public class RemoteAgencyJsonEmptyMessage : IRemoteAgencyMessage
    {
        Guid IRemoteAgencyMessage.SenderSiteId { get; set; }
        Guid IRemoteAgencyMessage.TargetSiteId { get; set; }
        Guid IRemoteAgencyMessage.SenderInstanceId { get; set; }
        Guid IRemoteAgencyMessage.TargetInstanceId { get; set; }
        MessageType IRemoteAgencyMessage.MessageType { get; set; }
        string IRemoteAgencyMessage.AssetName { get; set; }
        Guid IRemoteAgencyMessage.MessageId { get; set; }
        Exception IRemoteAgencyMessage.Exception { get; set; }
        bool IRemoteAgencyMessage.IsOneWay { get; set; }
        [Newtonsoft.Json.JsonIgnore] bool IRemoteAgencyMessage.IsEmptyMessage { get; set; }

        /// <summary>
        /// Initializes an instance of RemoteAgencyJsonEmptyMessage.
        /// </summary>
        /// <remarks><para>This constructor and this class are not present in Neat release.</para></remarks>
        public RemoteAgencyJsonEmptyMessage()
        {
            ((IRemoteAgencyMessage) this).IsEmptyMessage = true;
        }
    }
}
