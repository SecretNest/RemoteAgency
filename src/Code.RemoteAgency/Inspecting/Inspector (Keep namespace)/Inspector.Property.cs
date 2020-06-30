using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

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
            var timeoutTime = propertyInfo.GetCustomAttribute<OperatingTimeoutTimeAttribute>();
            property.PropertyGettingTimeout = timeoutTime?.PropertyGettingTimeout ?? interfaceLevelPropertyGettingTimeout;
            property.PropertySettingTimeout = timeoutTime?.PropertySettingTimeout ?? interfaceLevelPropertySettingTimeout;

            if (_serializerAssetLevelAttributeBaseType != null)
            {
                property.SerializerAssetLevelAttributes =
                    propertyInfo.GetCustomAttributes(_serializerAssetLevelAttributeBaseType, true).Cast<Attribute>().ToList();
            }

            //asset level pass through attributes
            property.AssetLevelPassThroughAttributes = GetAttributePassThrough(propertyInfo,
                (m, a) => new InvalidAttributeDataException(m, a, memberPath));

            if (property.IsIgnored)
            {
                if (property.WillThrowExceptionWhileCalling)
                {
                    //property.GettingResponseEntityProperty = null;
                    //property.SettingRequestEntityValuePropertyName = null;
                    property.SettingResponseEntityProperties = new List<RemoteAgencyReturnValueInfoBase>();
                }
                else
                {
                    ProcessSettingResponseForIgnoredProperty(property.DataType, out var response);
                    property.SettingResponseEntityProperties = response;
                }
            }
            else
            {
                property.SerializerParameterLevelAttributes = propertyInfo
                    .GetCustomAttributes(_serializerParameterLevelAttributeBaseType, true).Cast<Attribute>()
                    .ToList();

                //getting
                if (property.IsGettingOneWay)
                {
                    ProcessGettingResponseForOneWayProperty(property.DataType, out var response);
                    property.GettingResponseEntityProperty = response;
                }
                else
                {
                    //normal getting
                    ProcessGettingResponseForNormalProperty(propertyInfo, property.SerializerParameterLevelAttributes,
                        out var response);
                    property.GettingResponseEntityProperty = response;
                }

                //setting
                property.SettingRequestEntityValuePropertyName =
                    GetValueFromAttribute<CustomizedPropertySetRequestPropertyNameAttribute, string>(propertyInfo,
                        i => i.EntityPropertyName, out _, "Value");

                if (property.IsSettingOneWay)
                {
                    property.SettingResponseEntityProperties = new List<RemoteAgencyReturnValueInfoBase>();
                }
                else
                {
                    //normal setting
                    ProcessSettingResponseForNormalProperty(propertyInfo, property.SerializerParameterLevelAttributes,
                        memberPath, out var response);
                    property.SettingResponseEntityProperties = response;
                }

            }
        }
    }
}