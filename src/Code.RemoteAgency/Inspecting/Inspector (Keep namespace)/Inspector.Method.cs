using SecretNest.RemoteAgency.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SecretNest.RemoteAgency.Inspecting
{
    partial class Inspector
    {
        void ProcessMethod(RemoteAgencyMethodInfo method, TypeInfo @interface,
            LocalExceptionHandlingMode interfaceLevelLocalExceptionHandlingMode,
            int interfaceLevelMethodCallingTimeout)
        {
            Stack<MemberInfo> memberPath = new Stack<MemberInfo>();
            memberPath.Push(@interface);
            memberPath.Push(method.Asset);

            MethodInfo methodInfo = (MethodInfo)method.Asset;
            method.LocalExceptionHandlingMode =
                GetValueFromAttribute<LocalExceptionHandlingAttribute, LocalExceptionHandlingMode>(methodInfo,
                    i => i.LocalExceptionHandlingMode, out _, interfaceLevelLocalExceptionHandlingMode);

            method.MethodCallingTimeout =
                GetValueFromAttribute<OperatingTimeoutTimeAttribute, int>(methodInfo,
                    i => i.Timeout, out _, interfaceLevelMethodCallingTimeout);

            //generic parameter
            method.AssetLevelGenericArguments = ProcessGenericArgument(methodInfo.GetGenericArguments(), memberPath);

            //asset level pass through attributes
            method.AssetLevelPassThroughAttributes = GetAttributePassThrough(methodInfo,
                (m, a) => new InvalidAttributeDataException(m, a, memberPath));
            method.ReturnValuePassThroughAttributes = GetAttributePassThrough(methodInfo.ReturnTypeCustomAttributes,
                (m, a) => new InvalidReturnValueAttributeDataException(m, a, memberPath));

            if (method.IsIgnored)
            {
                if (_includesProxyOnlyInfo)
                {
                    if (method.WillThrowExceptionWhileCalling)
                    {
                        ProcessParameterForIgnoredAndThrowExceptionAsset(methodInfo.GetParameters(),
                            out var parameters);
                        FillAttributePassThroughOnParameters(parameters,
                            (m, a, p) => new InvalidParameterAttributeDataException(m, a, p, memberPath));
                        method.ParameterEntityProperties = parameters;
                        method.ReturnValueEntityProperties = new List<RemoteAgencyReturnValueInfoBase>();
                    }
                    else
                    {
                        ProcessParameterAndReturnValueForIgnoredAsset(methodInfo.GetParameters(), methodInfo.ReturnType,
                            out var parameters, out var returnValues);
                        FillAttributePassThroughOnParameters(parameters,
                            (m, a, p) => new InvalidParameterAttributeDataException(m, a, p, memberPath));
                        method.ParameterEntityProperties = parameters;
                        method.ReturnValueEntityProperties = returnValues;
                    }
                }
                else
                {
                    method.ParameterEntityProperties = new List<RemoteAgencyParameterInfo>();
                    method.ReturnValueEntityProperties = new List<RemoteAgencyReturnValueInfoBase>();
                }
            }
            else if (method.IsOneWay)
            {
                ProcessParameterAndReturnValueForOneWayAsset(methodInfo.GetParameters(), methodInfo.ReturnType,
                    memberPath, out var parameters, out var returnValues);
                FillAttributePassThroughOnParameters(parameters,
                    (m, a, p) => new InvalidParameterAttributeDataException(m, a, p, memberPath));
                method.ParameterEntityProperties = parameters;
                method.ReturnValueEntityProperties = returnValues;
            }
            else
            {
                //normal
                ProcessParameterAndReturnValueForNormalAsset(methodInfo.GetParameters(), methodInfo.ReturnType,
                    memberPath, new[] {methodInfo.ReturnTypeCustomAttributes, methodInfo}, out var parameters,
                    out var returnValues);
                FillAttributePassThroughOnParameters(parameters,
                    (m, a, p) => new InvalidParameterAttributeDataException(m, a, p, memberPath));
                method.ParameterEntityProperties = parameters;
                method.ReturnValueEntityProperties = returnValues;
            }
        }
    }
}