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

            if (_serializerAssetLevelAttributeBaseType != null)
            {
                method.SerializerAssetLevelAttributes =
                    methodInfo.GetCustomAttributes(_serializerAssetLevelAttributeBaseType, true).Cast<Attribute>().ToList();
            }

            //generic parameter
            method.AssetLevelGenericArguments = ProcessGenericArgument(methodInfo.GetGenericArguments(), memberPath);

            //asset level pass through attributes
            method.AssetLevelPassThroughAttributes = GetAttributePassThrough(methodInfo,
                (m, a) => new InvalidAttributeDataException(m, a, memberPath));
            method.ReturnValuePassThroughAttributes = GetAttributePassThrough(methodInfo.ReturnTypeCustomAttributes,
                (m, a) => new InvalidReturnValueAttributeDataException(m, a, memberPath));

            if (method.IsIgnored)
            {
                method.ParameterEntityProperties = new List<RemoteAgencyParameterInfo>();
                if (_includesProxyOnlyInfo)
                {
                    var parameterInfo = methodInfo.GetParameters();
                    if (method.WillThrowExceptionWhileCalling)
                    {
                        method.ParameterPassThroughAttributes = FillAttributePassThroughOnParameters(parameterInfo,
                            (m, a, p) => new InvalidParameterAttributeDataException(m, a, p, memberPath));
                        method.ReturnValueEntityProperties = new List<RemoteAgencyReturnValueInfoBase>();
                    }
                    else
                    {
                        ProcessParameterAndReturnValueForIgnoredAsset(parameterInfo, methodInfo.ReturnType, out var returnValues);
                        method.ParameterPassThroughAttributes = FillAttributePassThroughOnParameters(parameterInfo,
                            (m, a, p) => new InvalidParameterAttributeDataException(m, a, p, memberPath));
                        method.ReturnValueEntityProperties = returnValues;
                    }
                }
                else
                {
                    method.ReturnValueEntityProperties = new List<RemoteAgencyReturnValueInfoBase>();
                }
            }
            else if (method.IsOneWay)
            {
                var parameterInfo = methodInfo.GetParameters();
                ProcessParameterAndReturnValueForOneWayAsset(parameterInfo, methodInfo.ReturnType,
                    memberPath, out var parameters, out var returnValues, _includesProxyOnlyInfo);
                method.ParameterPassThroughAttributes = FillAttributePassThroughOnParameters(parameterInfo,
                    (m, a, p) => new InvalidParameterAttributeDataException(m, a, p, memberPath));
                method.ParameterEntityProperties = parameters;
                method.ReturnValueEntityProperties = returnValues;
            }
            else
            {
                //normal
                var parameterInfo = methodInfo.GetParameters();
                ProcessParameterAndReturnValueForNormalAsset(parameterInfo, methodInfo.ReturnType, methodInfo.ReturnTypeCustomAttributes,
                    memberPath, new[] {methodInfo.ReturnTypeCustomAttributes, methodInfo}, out var parameters,
                    out var returnValues);
                method.ParameterPassThroughAttributes = FillAttributePassThroughOnParameters(parameterInfo,
                    (m, a, p) => new InvalidParameterAttributeDataException(m, a, p, memberPath));
                method.ParameterEntityProperties = parameters;
                method.ReturnValueEntityProperties = returnValues;
            }
        }
    }
}