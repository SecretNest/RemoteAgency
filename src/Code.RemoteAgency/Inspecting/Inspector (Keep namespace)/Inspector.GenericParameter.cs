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
        List<RemoteAgencyGenericParameterInfo> ProcessGenericParameter(Type member, Stack<MemberInfo> memberParentPath, bool includePassThroughAttribute)
        {
            List<RemoteAgencyGenericParameterInfo> result = new List<RemoteAgencyGenericParameterInfo>();

            var types = member.GetGenericArguments();
            if (types.Length > 0)
            {
                memberParentPath.Push(member);
                foreach (var type in member.GetGenericArguments())
                {
                    var item = new RemoteAgencyGenericParameterInfo();

                    if (includePassThroughAttribute)
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