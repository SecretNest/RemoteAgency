using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    class InterfaceLevelAttributePassThrough
    {
        public List<AttributePassThrough> Interface { get; set; }
        public Dictionary<Type, List<AttributePassThrough>> GenericTypes { get; set; }
    }
}
