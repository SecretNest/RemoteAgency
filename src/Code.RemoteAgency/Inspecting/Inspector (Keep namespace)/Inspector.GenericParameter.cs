using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency.Inspecting
{
    partial class Inspector
    {
        List<RemoteAgencyGenericParameterInfo> ProcessGenericParameter(MemberInfo memberInfo, Type[] genericArguments, Stack<MemberInfo> memberParentPath)
        {
            List<RemoteAgencyGenericParameterInfo> result = new List<RemoteAgencyGenericParameterInfo>();

            if (genericArguments.Length > 0)
            {
                memberParentPath.Push(memberInfo);
                foreach (var type in genericArguments)
                {
                    var item = new RemoteAgencyGenericParameterInfo();

                    if (_includeProxyOnlyInfo)
                    {
                        item.AttributePassThroughs = GetAttributePassThrough(type, memberParentPath);
                    }

                    item.Type = type;
                    result.Add(item);
                }

                memberParentPath.Pop();
            }

            return result;
        }
    }
}