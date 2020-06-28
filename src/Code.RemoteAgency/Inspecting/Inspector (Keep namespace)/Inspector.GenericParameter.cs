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
        List<RemoteAgencyGenericArgumentInfo> ProcessGenericArgument(MemberInfo memberInfo, Type[] genericArguments, Stack<MemberInfo> memberParentPath)
        {
            List<RemoteAgencyGenericArgumentInfo> result = new List<RemoteAgencyGenericArgumentInfo>();

            if (genericArguments.Length > 0)
            {
                memberParentPath.Push(memberInfo);
                foreach (var genericArgument in genericArguments)
                {
                    var item = new RemoteAgencyGenericArgumentInfo
                    {
                        PassThroughAttributes = GetAttributePassThrough(genericArgument, (m, a) => new InvalidAttributeDataException(m, a, genericArgument, memberParentPath)),
                        GenericArgument = genericArgument
                    };

                    result.Add(item);
                }

                memberParentPath.Pop();
            }

            return result;
        }
    }
}