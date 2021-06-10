using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency.Inspecting
{
    partial class Inspector
    {
        void ProcessProperty(RemoteAgencyPropertyInfo property, TypeInfo @interface,
            LocalExceptionHandlingMode interfaceLevelLocalExceptionHandlingMode,
            int interfaceLevelPropertyGettingTimeout, int interfaceLevelPropertySettingTimeout)
        {
            var memberPath = new Stack<MemberInfo>();
            memberPath.Push(@interface);
            memberPath.Push(property.Asset);

            var propertyInfo = (PropertyInfo) property.Asset;
            property.LocalExceptionHandlingMode =
                propertyInfo.GetValueFromAttribute<LocalExceptionHandlingAttribute, LocalExceptionHandlingMode>(
                    i => i.LocalExceptionHandlingMode, out _, interfaceLevelLocalExceptionHandlingMode);

            var getMethod = propertyInfo.GetGetMethod();
            var setMethod = propertyInfo.GetSetMethod();
            property.IsGettable = propertyInfo.GetGetMethod() != null;
            property.IsSettable = propertyInfo.GetSetMethod() != null;

            if (_serializerAssetLevelAttributeBaseType != null)
            {
                property.SerializerAssetLevelAttributes =
                    propertyInfo.GetCustomAttributes(_serializerAssetLevelAttributeBaseType, true).Cast<Attribute>()
                        .ToList();

                property.GettingMethodSerializerAssetLevelAttributes =
                    getMethod!.GetCustomAttributes(_serializerAssetLevelAttributeBaseType, true).Cast<Attribute>()
                        .ToList();

                property.SettingMethodSerializerAssetLevelAttributes =
                    setMethod!.GetCustomAttributes(_serializerAssetLevelAttributeBaseType, true).Cast<Attribute>()
                        .ToList();
            }

            //pass through attributes
            property.AssetLevelPassThroughAttributes = _includesProxyOnlyInfo
                ? propertyInfo.GetAttributePassThrough((m, a) => new InvalidAttributeDataException(m, a, memberPath))
                : new List<CustomAttributeBuilder>();
            if (property.IsGettable)
            {
                property.GettingMethodPassThroughAttributes = _includesProxyOnlyInfo
                    ? getMethod.GetAttributePassThrough((m, a) => new InvalidReturnValueAttributeDataException(m, a, getMethod, memberPath))
                    : new List<CustomAttributeBuilder>();

                property.GettingMethodReturnValuePassThroughAttributes = _includesProxyOnlyInfo
                    ? getMethod!.ReturnTypeCustomAttributes.GetAttributePassThrough((m, a) => new InvalidReturnValueAttributeDataException(m, a, getMethod, memberPath))
                    : new List<CustomAttributeBuilder>();
            }
            if (property.IsSettable)
            {
                property.SettingMethodPassThroughAttributes = _includesProxyOnlyInfo
                    ? setMethod.GetAttributePassThrough((m, a) => new InvalidReturnValueAttributeDataException(m, a, setMethod, memberPath))
                    : new List<CustomAttributeBuilder>();
            }

            if (property.IsIgnored)
            {
                ProcessMethodBodyForIgnoredAsset(getMethod, getMethod!.ReturnType, property.WillThrowExceptionWhileCalling,
                    property.GettingMethodBodyInfo, AsyncMethodOriginalReturnValueDataTypeClass.NotAsyncMethod);
                ProcessMethodBodyForIgnoredAsset(setMethod, setMethod!.ReturnType, property.WillThrowExceptionWhileCalling,
                    property.SettingMethodBodyInfo, AsyncMethodOriginalReturnValueDataTypeClass.NotAsyncMethod);
            }
            else
            {
                var parameterInfo = getMethod!.GetParameters(); //Parameters are shared for get and set. Except the value in set, which is not able to set an attribute. 
                property.MethodParameterPassThroughAttributes = _includesProxyOnlyInfo
                    ? parameterInfo.FillAttributePassThroughOnParameters((m, a, p) =>
                        new InvalidParameterAttributeDataException(m, a, p, memberPath))
                    : new Dictionary<string, List<CustomAttributeBuilder>>();

                var timeoutTime = propertyInfo.GetCustomAttribute<OperatingTimeoutTimeAttribute>();

                var valueParameterSerializerParameterLevelAttributesOverrideForProperty = propertyInfo
                    .GetCustomAttributes(_serializerParameterLevelAttributeBaseType, true)
                    .Cast<Attribute>().ToList();

                if (property.IsGettable)
                {
                    //getting
                    if (property.IsGettingOneWay)
                    {
                        ProcessMethodBodyForOneWayAsset(getMethod, getMethod.ReturnType, memberPath, _includesProxyOnlyInfo,
                            property.GettingMethodBodyInfo, AsyncMethodOriginalReturnValueDataTypeClass.NotAsyncMethod);
                    }
                    else
                    {
                        //normal getting
                        var gettingTimeout =
                            timeoutTime?.PropertyGettingTimeout ?? interfaceLevelPropertyGettingTimeout;

                        var isReturnValueIgnored =
                            propertyInfo.GetValueFromAttribute<ReturnIgnoredAttribute, bool>(i => i.IsIgnored,
                                out _);

                        var returnValuePropertyNameSpecifiedByAttribute =
                            propertyInfo.GetValueFromAttribute<CustomizedPropertyGetValuePropertyNameAttribute, string>(
                                i => i.EntityPropertyName,
                                out var customizedPropertyGetValuePropertyNameAttribute);

                        ProcessMethodBodyForNormalAsset(getMethod, getMethod.ReturnType, memberPath,
                            gettingTimeout, isReturnValueIgnored, returnValuePropertyNameSpecifiedByAttribute,
                            customizedPropertyGetValuePropertyNameAttribute, property.GettingMethodBodyInfo,
                            AsyncMethodOriginalReturnValueDataTypeClass.NotAsyncMethod, null,
                            null, null, null,
                            valueParameterSerializerParameterLevelAttributesOverrideForProperty);
                    }
                }

                if (property.IsSettable)
                {
                    var propertyValuePropertyName =
                        propertyInfo.GetValueFromAttribute<CustomizedPropertySetValuePropertyNameAttribute, string>(
                            i => i.EntityPropertyName, out var customizedPropertySetValuePropertyNameAttribute);
                    var parameterReturnRequiredProperty = propertyInfo
                        .GetCustomAttributes<ParameterReturnRequiredPropertyAttribute>().ToList();

                    //setting
                    if (property.IsSettingOneWay)
                    {
                        ProcessMethodBodyForOneWayAsset(setMethod, typeof(void) /* Set will never have return */, memberPath, _includesProxyOnlyInfo,
                            property.SettingMethodBodyInfo,
                            AsyncMethodOriginalReturnValueDataTypeClass.NotAsyncMethod, null, null,
                            valueParameterSerializerParameterLevelAttributesOverrideForProperty,
                            propertyValuePropertyName, customizedPropertySetValuePropertyNameAttribute);
                    }
                    else
                    {
                        //normal setting
                        var settingTimeout =
                            timeoutTime?.PropertySettingTimeout ?? interfaceLevelPropertySettingTimeout;

                        ProcessMethodBodyForNormalAsset(setMethod, typeof(void) /* Set will never have return */, memberPath,
                            settingTimeout, true, null, null,
                            property.SettingMethodBodyInfo, AsyncMethodOriginalReturnValueDataTypeClass.NotAsyncMethod, null,
                            null, null, null,
                            null, valueParameterSerializerParameterLevelAttributesOverrideForProperty,
                            propertyValuePropertyName, customizedPropertySetValuePropertyNameAttribute,
                            parameterReturnRequiredProperty);
                    }
                }
            }
        }
    }
}