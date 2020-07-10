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

            if (_serializerAssetLevelAttributeBaseType != null)
            {
                method.SerializerAssetLevelAttributes =
                    methodInfo.GetCustomAttributes(_serializerAssetLevelAttributeBaseType, true).Cast<Attribute>().ToList();
            }

            //generic parameter
            method.AssetLevelGenericParameters = methodInfo.GetGenericArguments();
            method.AssetLevelGenericParameterPassThroughAttributes = FillAttributePassThroughOnGenericParameters(method.AssetLevelGenericParameters,
                (m, a, t) => new InvalidAttributeDataException(m, a, t, memberPath));

            //pass through attributes
            if (_includesProxyOnlyInfo)
            {
                method.AssetLevelPassThroughAttributes = GetAttributePassThrough(methodInfo,
                    (m, a) => new InvalidAttributeDataException(m, a, memberPath));
                method.ReturnValuePassThroughAttributes = GetAttributePassThrough(methodInfo.ReturnTypeCustomAttributes,
                    (m, a) => new InvalidReturnValueAttributeDataException(m, a, memberPath));

                var parameterInfo = methodInfo.GetParameters();
                method.ParameterPassThroughAttributes = FillAttributePassThroughOnParameters(parameterInfo,
                    (m, a, p) => new InvalidParameterAttributeDataException(m, a, p, memberPath));
            }

            if (method.IsIgnored)
            {
                ProcessMethodBodyForIgnoredAsset(methodInfo, method.WillThrowExceptionWhileCalling,
                    method.MethodBodyInfo);
            }
            else if (method.IsOneWay)
            {
                ProcessMethodBodyForOneWayAsset(methodInfo, memberPath, _includesProxyOnlyInfo, method.MethodBodyInfo);
            }
            else
            {
                //normal
                var isReturnValueIgnored =
                    GetValueFromAttribute<ReturnIgnoredAttribute, bool>(methodInfo.ReturnTypeCustomAttributes, i => i.IsIgnored,
                        out var returnIgnoredAttribute);
                if (returnIgnoredAttribute == null)
                {
                    isReturnValueIgnored =
                        GetValueFromAttribute(methodInfo, i => i.IsIgnored, out returnIgnoredAttribute);
                }

                string returnValuePropertyNameSpecifiedByAttribute =
                    GetValueFromAttribute<CustomizedReturnValueEntityPropertyNameAttribute, string>(
                        methodInfo.ReturnTypeCustomAttributes, i => i.EntityPropertyName,
                        out var customizedReturnValueEntityPropertyNameAttribute);
                if (customizedReturnValueEntityPropertyNameAttribute == null)
                {
                    returnValuePropertyNameSpecifiedByAttribute = GetValueFromAttribute(methodInfo,
                        i => i.EntityPropertyName, out customizedReturnValueEntityPropertyNameAttribute);
                }

                ProcessMethodBodyForNormalAsset(methodInfo, memberPath,
                    GetValueFromAttribute<OperatingTimeoutTimeAttribute, int>(methodInfo, i => i.Timeout, out _, interfaceLevelMethodCallingTimeout),
                    isReturnValueIgnored, returnValuePropertyNameSpecifiedByAttribute, customizedReturnValueEntityPropertyNameAttribute, method.MethodBodyInfo);
            }
        }
    }
}