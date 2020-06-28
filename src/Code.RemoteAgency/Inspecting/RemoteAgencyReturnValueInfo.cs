using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SecretNest.RemoteAgency.Inspecting
{
    class RemoteAgencyReturnValueInfo
    {
        public string PropertyName { get; set; }
        public Type DataType { get; set; }

        public bool ShouldSetToDefault { get; set; }
        public bool IsReturnValue { get; set; }

        public string ParameterName { get; set; }
        public ParameterInfo DataSourceParameter { get; set; }
        public bool IsIncludedWhenExceptionThrown { get; set; }
        public bool IsProperty { get; set; }
        public bool IsField { get; set; }
        public bool IsHelperClass { get; set; }
        public PropertyInfo PropertyInHelperClass { get; set; }
        public Type HelperClass { get; set; }
    }
}
