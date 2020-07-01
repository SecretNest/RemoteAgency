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
        List<RemoteAgencyGenericParameterInfo> ProcessGenericArgument(Type[] genericArguments, Stack<MemberInfo> memberPath)
        {
            List<RemoteAgencyGenericParameterInfo> result = new List<RemoteAgencyGenericParameterInfo>();

            if (genericArguments.Length > 0)
            {
                foreach (var genericArgument in genericArguments)
                {
                    var item = new RemoteAgencyGenericParameterInfo
                    {
                        PassThroughAttributes = GetAttributePassThrough(genericArgument, (m, a) => new InvalidAttributeDataException(m, a, genericArgument, memberPath)),
                        GenericParameter = genericArgument
                    };

                    result.Add(item);
                }
            }

            return result;
        }
    }
}