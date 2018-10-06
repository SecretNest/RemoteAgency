using SecretNest.RemoteAgency.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SecretNest.RemoteAgency
{
    static class ExtractTwoWaySettingHelper
    {
        internal static ValueMapping ExtractTwoWaySetting(this ParameterTwoWayPropertyAttribute attribute, string rootName, Type parameterType, GetTypeFullNameParameter getTypeFullNameParameter)
        {
            if (attribute.IsSimpleMode)
            {
                var propertyName = attribute.ParameterProperty;
                var propertyInfo = parameterType.GetRuntimeProperty(propertyName);
                Type propertyType;
                if (propertyInfo != null)
                {
                    propertyType = propertyInfo.PropertyType;
                }
                else
                {
                    var fieldInfo = parameterType.GetRuntimeField(propertyName);
                    if (fieldInfo != null)
                    {
                        propertyType = fieldInfo.FieldType;
                    }
                    else
                        return null;
                }
                var entityPropertyName = attribute.EntityPropertyName ?? propertyName;
                var propertyTypeName = propertyType.GetFullName(getTypeFullNameParameter, null);
                return new ValueMapping(NamingHelper.GetRandomName(propertyName), entityPropertyName, propertyTypeName, rootName + "." + propertyName);
            }
            else
            {
                var propertyPath = attribute.ParameterProperty;
                var entityPropertyName = attribute.EntityPropertyName ?? NamingHelper.GetRandomName("P");
                var propertyTypeName = attribute.ElementType.GetFullName(getTypeFullNameParameter, null);
                return new ValueMapping(NamingHelper.GetRandomName("v"), entityPropertyName, propertyTypeName, rootName + propertyPath);
            }
        }
    }
}
