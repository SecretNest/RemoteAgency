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

            if (_serializerAssetLevelAttributeBaseType != null)
            {
                property.SerializerAssetLevelAttributes =
                    propertyInfo.GetCustomAttributes(_serializerAssetLevelAttributeBaseType, true).Cast<Attribute>()
                        .ToList();
            }

            //asset level pass through attributes
            property.AssetLevelPassThroughAttributes = GetAttributePassThrough(propertyInfo,
                (m, a) => new InvalidAttributeDataException(m, a, memberPath));

            var getMethod = propertyInfo.GetGetMethod();
            var setMethod = propertyInfo.GetSetMethod();

            property.IsGettable = propertyInfo.GetGetMethod() != null;
            property.IsSettable = propertyInfo.GetSetMethod() != null;

            if (property.IsIgnored)
            {
                ProcessMethodBodyForIgnoredAsset(getMethod, property.WillThrowExceptionWhileCalling,
                    property.GettingMethodBodyInfo);
                ProcessMethodBodyForIgnoredAsset(setMethod, property.WillThrowExceptionWhileCalling,
                    property.SettingMethodBodyInfo);
            }
            else
            {
                var timeoutTime = propertyInfo.GetCustomAttribute<OperatingTimeoutTimeAttribute>();

                List<Attribute> valueParameterSerializerParameterLevelAttributesOverride = propertyInfo
                    .GetCustomAttributes(_serializerParameterLevelAttributeBaseType, true)
                    .Cast<Attribute>().ToList();

                if (property.IsGettable)
                {
                    //getting
                    if (property.IsGettingOneWay)
                    {
                        ProcessMethodBodyForOneWayAsset(getMethod, memberPath, _includesProxyOnlyInfo,
                            property.GettingMethodBodyInfo, null, null,
                            valueParameterSerializerParameterLevelAttributesOverride);
                    }
                    else
                    {
                        //normal getting
                        var gettingTimeout =
                            timeoutTime?.PropertyGettingTimeout ?? interfaceLevelPropertyGettingTimeout;

                        ProcessMethodBodyForNormalAsset(getMethod, memberPath,
                            new ICustomAttributeProvider[] {propertyInfo}, gettingTimeout,
                            property.GettingMethodBodyInfo, null, null, null,
                            valueParameterSerializerParameterLevelAttributesOverride);
                    }
                }

                if (property.IsSettable)
                {
                    //setting
                    if (property.IsSettingOneWay)
                    {
                        ProcessMethodBodyForOneWayAsset(setMethod, memberPath, _includesProxyOnlyInfo,
                            property.SettingMethodBodyInfo, null, null,
                            valueParameterSerializerParameterLevelAttributesOverride);
                    }
                    else
                    {
                        //normal setting
                        var settingTimeout =
                            timeoutTime?.PropertySettingTimeout ?? interfaceLevelPropertySettingTimeout;

                        ProcessMethodBodyForNormalAsset(setMethod, memberPath,
                            new ICustomAttributeProvider[] {propertyInfo}, settingTimeout,
                            property.SettingMethodBodyInfo, null, null, null,
                            valueParameterSerializerParameterLevelAttributesOverride);
                    }
                }
            }
        }
    }
}