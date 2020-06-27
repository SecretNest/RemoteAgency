﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency.Inspecting
{
    partial class Inspector
    {
        void ProcessMethod(RemoteAgencyMethodInfo method, TypeInfo @interface)
        {
            Stack<MemberInfo> parentPath = new Stack<MemberInfo>();
            parentPath.Push(@interface);

            MethodInfo methodInfo = (MethodInfo) method.Asset;

            //generic parameter
            method.AssetLevelGenericParameters = ProcessGenericParameter(method.Asset, methodInfo.GetGenericArguments(), parentPath);


        }
    }
}