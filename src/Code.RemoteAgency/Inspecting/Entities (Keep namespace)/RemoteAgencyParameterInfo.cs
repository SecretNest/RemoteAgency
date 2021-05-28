using System;
using System.Collections.Generic;
using System.Reflection;

namespace SecretNest.RemoteAgency.Inspecting
{
    class RemoteAgencyParameterInfo
    {
        public string PropertyName { get; set; }
        public ParameterInfo Parameter { get; set; }

        public List<Attribute> SerializerParameterLevelAttributes { get; set; }

        public Type DataType => Parameter.ParameterType;
        public string ParameterName => Parameter.Name;
        public bool IsOut => Parameter.IsOut;
        public bool IsRef => !Parameter.IsOut && Parameter.ParameterType.IsByRef;
        public bool NeedTempVariable => Parameter.ParameterType.IsByRef; //including out, byRef
    }
}
