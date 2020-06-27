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
            Stack<MemberInfo> parentPath = new Stack<MemberInfo>();
            parentPath.Push(@interface);

            PropertyInfo propertyInfo = (PropertyInfo) property.Asset;
            property.LocalExceptionHandlingMode =
                GetValueFromAttribute<LocalExceptionHandlingAttribute, LocalExceptionHandlingMode>(propertyInfo,
                    i => i.LocalExceptionHandlingMode, out _, interfaceLevelLocalExceptionHandlingMode);
            var timeoutTime = propertyInfo.GetCustomAttribute<OperatingTimeoutTimeAttribute>();
            property.PropertyGettingTimeout = timeoutTime?.PropertyGettingTimeout ?? interfaceLevelPropertyGettingTimeout;
            property.PropertySettingTimeout = timeoutTime?.PropertySettingTimeout ?? interfaceLevelPropertySettingTimeout;

        }
    }
}