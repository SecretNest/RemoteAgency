using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    class RemoteAgencyAttributePassThrough
    {
        public Type Attribute { get; set; }
        public Type[] AttributeConstructorParameterTypes { get; set; }
        public List<KeyValuePair<int, object>> AttributeConstructorParameters { get; set; }
        public List<KeyValuePair<string, object>> AttributeProperties { get; set; }
    }
}
