using SecretNest.RemoteAgency.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace SecretNest.RemoteAgency.Inspecting
{
    partial class Inspector
    {
        void ProcessMethod(RemoteAgencyMethodInfo method, TypeInfo @interface,
            LocalExceptionHandlingMode interfaceLevelLocalExceptionHandlingMode,
            int interfaceLevelMethodCallingTimeout)
        {
            var memberPath = new Stack<MemberInfo>();
            memberPath.Push(@interface);
            memberPath.Push(method.Asset);

            var methodInfo = (MethodInfo)method.Asset;
            method.LocalExceptionHandlingMode =
                methodInfo.GetValueFromAttribute<LocalExceptionHandlingAttribute, LocalExceptionHandlingMode>(
                    i => i.LocalExceptionHandlingMode, out _, interfaceLevelLocalExceptionHandlingMode);

            if (_serializerAssetLevelAttributeBaseType != null)
            {
                method.SerializerAssetLevelAttributes =
                    methodInfo.GetCustomAttributes(_serializerAssetLevelAttributeBaseType, true).Cast<Attribute>().ToList();
            }

            //generic parameter
            method.AssetLevelGenericParameters = methodInfo.GetGenericArguments();

            //pass through attributes
            if (_includesProxyOnlyInfo)
            {
                method.AssetLevelGenericParameterPassThroughAttributes =
                    method.AssetLevelGenericParameters.FillAttributePassThroughOnGenericParameters((m, a, t) =>
                        new InvalidAttributeDataException(m, a, t, memberPath));

                method.AssetLevelPassThroughAttributes = methodInfo.GetAttributePassThrough(
                    (m, a) => new InvalidAttributeDataException(m, a, memberPath));
                method.ReturnValuePassThroughAttributes = methodInfo.ReturnTypeCustomAttributes.GetAttributePassThrough(
                    (m, a) => new InvalidReturnValueAttributeDataException(m, a, memberPath));

                var parameterInfo = methodInfo.GetParameters();
                method.ParameterPassThroughAttributes = parameterInfo.FillAttributePassThroughOnParameters(
                    (m, a, p) => new InvalidParameterAttributeDataException(m, a, p, memberPath));
            }
            else
            {
                method.AssetLevelGenericParameterPassThroughAttributes =
                    new Dictionary<string, List<CustomAttributeBuilder>>();
                method.AssetLevelPassThroughAttributes = new List<CustomAttributeBuilder>();
                method.ReturnValuePassThroughAttributes = new List<CustomAttributeBuilder>();
                method.ParameterPassThroughAttributes = new Dictionary<string, List<CustomAttributeBuilder>>();
            }

            if (method.IsIgnored)
            {
                ProcessMethodBodyForIgnoredAsset(methodInfo, method.AsyncMethodInnerOrNonAsyncMethodReturnValueDataType,
                    method.WillThrowExceptionWhileCalling, method.MethodBodyInfo, 
                    method.AsyncMethodOriginalReturnValueDataTypeClass);
            }
            else if (method.IsOneWay)
            {
                ProcessMethodBodyForOneWayAsset(methodInfo, method.AsyncMethodInnerOrNonAsyncMethodReturnValueDataType,
                    memberPath, _includesProxyOnlyInfo, method.MethodBodyInfo, method.AsyncMethodOriginalReturnValueDataTypeClass);
            }
            else
            {
                //normal
                var isReturnValueIgnored =
                    methodInfo.ReturnTypeCustomAttributes.GetValueFromAttribute<ReturnIgnoredAttribute, bool>(i => i.IsIgnored,
                        out var returnIgnoredAttribute);
                if (returnIgnoredAttribute == null)
                {
                    isReturnValueIgnored =
                        methodInfo.GetValueFromAttribute(i => i.IsIgnored, out returnIgnoredAttribute);
                }

                var returnValuePropertyNameSpecifiedByAttribute =
                    methodInfo.ReturnTypeCustomAttributes.GetValueFromAttribute<CustomizedReturnValueEntityPropertyNameAttribute, string>(
                        i => i.EntityPropertyName,
                        out var customizedReturnValueEntityPropertyNameAttribute);
                if (customizedReturnValueEntityPropertyNameAttribute == null)
                {
                    returnValuePropertyNameSpecifiedByAttribute = methodInfo.GetValueFromAttribute(
                        i => i.EntityPropertyName, out customizedReturnValueEntityPropertyNameAttribute);
                }

                ProcessMethodBodyForNormalAsset(methodInfo, method.AsyncMethodInnerOrNonAsyncMethodReturnValueDataType, memberPath,
                    methodInfo.GetValueFromAttribute<OperatingTimeoutTimeAttribute, int>(i => i.Timeout, out _, interfaceLevelMethodCallingTimeout),
                    isReturnValueIgnored, returnValuePropertyNameSpecifiedByAttribute, customizedReturnValueEntityPropertyNameAttribute,
                    method.MethodBodyInfo, method.AsyncMethodOriginalReturnValueDataTypeClass);
            }
        }
    }
}