using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SecretNest.RemoteAgency.Attributes;
using SecretNest.RemoteAgency.Helper;

namespace SecretNest.RemoteAgency.Inspecting
{
    partial class Inspector
    {
        void ProcessProperty(RemoteAgencyPropertyInfo property, TypeInfo @interface,
            LocalExceptionHandlingMode interfaceLevelLocalExceptionHandlingMode,
            int interfaceLevelPropertyGettingTimeout, int interfaceLevelPropertySettingTimeout)
        {
            Stack<MemberInfo> memberPath = new Stack<MemberInfo>();
            memberPath.Push(@interface);
            memberPath.Push(property.Asset);

            PropertyInfo propertyInfo = (PropertyInfo) property.Asset;
            property.LocalExceptionHandlingMode =
                GetValueFromAttribute<LocalExceptionHandlingAttribute, LocalExceptionHandlingMode>(propertyInfo,
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
                    getMethod.GetCustomAttributes(_serializerAssetLevelAttributeBaseType, true).Cast<Attribute>()
                        .ToList();

                property.SettingMethodSerializerAssetLevelAttributes =
                    setMethod.GetCustomAttributes(_serializerAssetLevelAttributeBaseType, true).Cast<Attribute>()
                        .ToList();
            }

            //pass through attributes
            property.AssetLevelPassThroughAttributes = GetAttributePassThrough(propertyInfo,
                (m, a) => new InvalidAttributeDataException(m, a, memberPath));
            if (property.IsGettable)
            {
                property.GettingMethodPassThroughAttributes = GetAttributePassThrough(getMethod, 
                    (m, a) => new InvalidReturnValueAttributeDataException(m, a, getMethod, memberPath));

                property.GettingMethodReturnValuePassThroughAttributes = GetAttributePassThrough(getMethod.ReturnTypeCustomAttributes,
                    (m, a) => new InvalidReturnValueAttributeDataException(m, a, getMethod, memberPath));
            }
            if (property.IsSettable)
            {
                property.SettingMethodPassThroughAttributes = GetAttributePassThrough(setMethod, 
                    (m, a) => new InvalidReturnValueAttributeDataException(m, a, setMethod, memberPath));
            }

            if (property.IsIgnored)
            {
                ProcessMethodBodyForIgnoredAsset(getMethod, getMethod.ReturnType, property.WillThrowExceptionWhileCalling,
                    property.GettingMethodBodyInfo, AsyncMethodOriginalReturnValueDataTypeClass.NotAsyncMethod);
                ProcessMethodBodyForIgnoredAsset(setMethod, setMethod.ReturnType, property.WillThrowExceptionWhileCalling,
                    property.SettingMethodBodyInfo, AsyncMethodOriginalReturnValueDataTypeClass.NotAsyncMethod);
            }
            else
            {
                var parameterInfo = getMethod.GetParameters(); //Parameters are shared for get and set. Except the value in set, which is not able to set an attribute. 
                property.MethodParameterPassThroughAttributes = FillAttributePassThroughOnParameters(parameterInfo,
                    (m, a, p) => new InvalidParameterAttributeDataException(m, a, p, memberPath));

                var timeoutTime = propertyInfo.GetCustomAttribute<OperatingTimeoutTimeAttribute>();

                List<Attribute> valueParameterSerializerParameterLevelAttributesOverrideForProperty = propertyInfo
                    .GetCustomAttributes(_serializerParameterLevelAttributeBaseType, true)
                    .Cast<Attribute>().ToList();

                if (property.IsGettable)
                {
                    //getting
                    if (property.IsGettingOneWay)
                    {
                        ProcessMethodBodyForOneWayAsset(getMethod, getMethod.ReturnType, memberPath, _includesProxyOnlyInfo,
                            property.GettingMethodBodyInfo, AsyncMethodOriginalReturnValueDataTypeClass.NotAsyncMethod,
                            null, null);
                    }
                    else
                    {
                        //normal getting
                        var gettingTimeout =
                            timeoutTime?.PropertyGettingTimeout ?? interfaceLevelPropertyGettingTimeout;

                        var isReturnValueIgnored =
                            GetValueFromAttribute<ReturnIgnoredAttribute, bool>(propertyInfo, i => i.IsIgnored,
                                out _);

                        string returnValuePropertyNameSpecifiedByAttribute =
                            GetValueFromAttribute<CustomizedPropertyGetResponsePropertyNameAttribute, string>(
                                propertyInfo, i => i.EntityPropertyName,
                                out var customizedPropertyGetResponsePropertyNameAttribute);

                        ProcessMethodBodyForNormalAsset(getMethod, getMethod.ReturnType, memberPath,
                            gettingTimeout, isReturnValueIgnored, returnValuePropertyNameSpecifiedByAttribute,
                            customizedPropertyGetResponsePropertyNameAttribute, property.GettingMethodBodyInfo,
                            AsyncMethodOriginalReturnValueDataTypeClass.NotAsyncMethod, null,
                            null, null, null,
                            valueParameterSerializerParameterLevelAttributesOverrideForProperty, null);
                    }
                }

                if (property.IsSettable)
                {
                    var propertyValuePropertyName =
                        GetValueFromAttribute<CustomizedPropertySetRequestPropertyNameAttribute, string>(propertyInfo,
                            i => i.EntityPropertyName, out var customizedPropertySetRequestPropertyNameAttribute);
                    var parameterReturnRequiredProperty = propertyInfo
                        .GetCustomAttributes<ParameterReturnRequiredPropertyAttribute>().ToList();

                    //setting
                    if (property.IsSettingOneWay)
                    {
                        ProcessMethodBodyForOneWayAsset(setMethod, typeof(void) /* Set will never have return */, memberPath, _includesProxyOnlyInfo,
                            property.SettingMethodBodyInfo,
                            AsyncMethodOriginalReturnValueDataTypeClass.NotAsyncMethod, null, null,
                            valueParameterSerializerParameterLevelAttributesOverrideForProperty,
                            propertyValuePropertyName, customizedPropertySetRequestPropertyNameAttribute);
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
                            propertyValuePropertyName, customizedPropertySetRequestPropertyNameAttribute,
                            parameterReturnRequiredProperty);
                    }
                }
            }
        }
    }
}