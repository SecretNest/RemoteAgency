using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    class RemoteAgencyGenericParameterInfo
    {
        public Type GenericParameter { get; set; }

        public List<RemoteAgencyAttributePassThrough> PassThroughAttributes { get; set; }
    }
}
