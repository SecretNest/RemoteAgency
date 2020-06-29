using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    class RemoteAgencyParameterInfo
    {
        public string PropertyName { get; set; } //Null: no need to put in entity
        public ParameterInfo Parameter { get; set; }

        public bool IsIncludedInEntity => !string.IsNullOrEmpty(PropertyName);

        public Type DataType => Parameter.ParameterType;
        public string ParameterName => Parameter.Name;
        public bool IsOut => Parameter.IsOut;
        public bool IsRef => !Parameter.IsOut && Parameter.ParameterType.IsByRef;
        public bool NeedTempVariable => Parameter.ParameterType.IsByRef; //including out, byRef

        public List<RemoteAgencyAttributePassThrough> PassThroughAttributes { get; set; }
    }
}
