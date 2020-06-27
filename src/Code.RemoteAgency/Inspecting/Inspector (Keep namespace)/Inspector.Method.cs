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
        void ProcessMethod(RemoteAgencyMethodInfo method, TypeInfo @interface,
            LocalExceptionHandlingMode interfaceLevelLocalExceptionHandlingMode,
            int interfaceLevelMethodCallingTimeout)
        {
            Stack<MemberInfo> parentPath = new Stack<MemberInfo>();
            parentPath.Push(@interface);

            MethodInfo methodInfo = (MethodInfo) method.Asset;
            method.LocalExceptionHandlingMode =
                GetValueFromAttribute<LocalExceptionHandlingAttribute, LocalExceptionHandlingMode>(methodInfo,
                    i => i.LocalExceptionHandlingMode, out _, interfaceLevelLocalExceptionHandlingMode);
            method.MethodCallingTimeout =
                GetValueFromAttribute<OperatingTimeoutTimeAttribute, int>(methodInfo,
                    i => i.Timeout, out _, interfaceLevelMethodCallingTimeout);

            //generic parameter
            method.AssetLevelGenericParameters = ProcessGenericParameter(method.Asset, methodInfo.GetGenericArguments(), parentPath);


        }
    }
}