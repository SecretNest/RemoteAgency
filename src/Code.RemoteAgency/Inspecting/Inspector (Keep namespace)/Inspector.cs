using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency.Inspecting
{
    partial class Inspector
    {
        private Type _sourceInterface;
        private TypeInfo _sourceInterfaceTypeInfo;
        private InterfaceTypeInfo _result;

        public Inspector(Type sourceInterface)
        {
            _sourceInterface = sourceInterface;
            _sourceInterfaceTypeInfo = sourceInterface.GetTypeInfo();
            _result = new InterfaceTypeInfo();
            SetInterfaceTypeBasicInfo(_result, _sourceInterface, _sourceInterfaceTypeInfo);
        }

        public InterfaceTypeInfo InterfaceTypeInfo => _result;

        public void Process()
        {

        }


    }
}