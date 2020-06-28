using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    class RemoteAgencyParameterInfo
    {
        public string PropertyName { get; set; }
        public ParameterInfo Parameter { get; set; }

        public List<RemoteAgencyAttributePassThrough> PassThroughAttributes { get; set; }
    }
}
