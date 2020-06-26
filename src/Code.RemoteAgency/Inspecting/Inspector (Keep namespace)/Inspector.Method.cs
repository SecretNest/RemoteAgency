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

        void ProcessMethod(MethodInfo method, MemberInfo @interface, RemoteAgencyMethodInfo remoteAgencyMethodInfo)
        {
            Stack<MemberInfo> parentPath = new Stack<MemberInfo>();
            parentPath.Push(@interface);

            //method.GetGenericArguments()
        }
    }
}