using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    class RemoteAgencyEventInfo : RemoteAgencyAssetInfoBase
    {
        public Type Delegate { get; set; }

        public string AddingRequestEntityName { get; set; }
        public string AddingResponseEntityName { get; set; }

        public string RemovingRequestEntityName { get; set; }
        public string RemovingResponseEntityName { get; set; }

        public string RaisingNotificationEntityName { get; set; }
        public List<RemoteAgencyParameterInfo> RaisingNotificationEntityProperties { get; set; }
        public string RaisingFeedbackEntityName { get; set; }
        public List<RemoteAgencyReturnValueInfo> RaisingFeedbackEntityProperties { get; set; }

        public int EventAddingTimeout { get; set; }
        public int EventRemovingTimeout { get; set; }
        public int EventRaisingTimeout { get; set; }

    }
}
