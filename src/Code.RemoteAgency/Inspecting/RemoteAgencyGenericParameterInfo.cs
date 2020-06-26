using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    class RemoteAgencyGenericParameterInfo
    {
        public Type Type { get; set; }

        public List<RemoteAgencyAttributePassThrough> AttributePassThroughs { get; set; }
    }
}
