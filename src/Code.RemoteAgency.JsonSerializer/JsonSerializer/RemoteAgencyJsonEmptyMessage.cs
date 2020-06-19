using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.JsonSerializer
{    /// <summary>
    /// Defines an empty message with Json serialization support.
    /// </summary>
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
        [Newtonsoft.Json.JsonIgnore] bool IRemoteAgencyMessage.IsEmptyMessage => true;
    }
}
