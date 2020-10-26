using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency.Inspecting
{
    class RemoteAgencyAttributePassThrough
    {
        public Type Attribute { get; set; }
        public Type[] AttributeConstructorParameterTypes { get; set; }
        public List<KeyValuePair<int, object>> AttributeConstructorParameters { get; set; }
        public List<KeyValuePair<string, object>> AttributeProperties { get; set; }
        public List<KeyValuePair<string, object>> AttributeFields { get; set; }

        public CustomAttributeBuilder GetAttributeBuilder()
        {
            var ctorInfo = Attribute.GetConstructor(AttributeConstructorParameterTypes);
            if (ctorInfo == null)
                throw new InvalidOperationException(
                    $"The constructor of {Attribute.Name} specified with {nameof(AttributePassThroughAttribute)}.{nameof(AttributePassThroughAttribute.AttributeConstructorParameterTypes)} in attribute is not found.");

            object[] parameters = new object[AttributeConstructorParameterTypes.Length];
            foreach (var parameter in AttributeConstructorParameters)
                parameters[parameter.Key] = parameter.Value;

            PropertyInfo[] propertyInfo;
            object[] propertyValue;

            FieldInfo[] fieldInfo;
            object[] fieldValue;

            bool useProperty, useField;

            if (AttributeProperties.Count > 0)
            {
                useProperty = true;
                propertyInfo = new PropertyInfo[AttributeProperties.Count];
                propertyValue = new object[propertyInfo.Length];
                for (int i = 0; i < propertyInfo.Length; i++)
                {
                    var setting = AttributeProperties[i];
                    var info = Attribute.GetProperty(setting.Key);
                    propertyInfo[i] = info ?? throw new InvalidOperationException(
                            $"The property {setting.Key} is not found in {Attribute.Name}.");
                    propertyValue[i] = setting.Value;
                }
            }
            else
            {
                useProperty = false;
                propertyInfo = null;
                propertyValue = null;
            }

            if (AttributeFields.Count > 0)
            {
                useField = true;
                fieldInfo = new FieldInfo[AttributeFields.Count];
                fieldValue = new object[fieldInfo.Length];
                for (int i = 0; i < fieldInfo.Length; i++)
                {
                    var setting = AttributeFields[i];
                    var info = Attribute.GetField(setting.Key);
                    fieldInfo[i] = info ?? throw new InvalidOperationException(
                            $"The field {setting.Key} is not found in {Attribute.Name}.");
                    fieldValue[i] = setting.Value;
                }
            }
            else
            {
                useField = false;
                fieldInfo = null;
                fieldValue = null;
            }

            if (useField)
            {
                return useProperty
                    ? new CustomAttributeBuilder(ctorInfo, parameters, propertyInfo, propertyValue, fieldInfo,
                        fieldValue)
                    : new CustomAttributeBuilder(ctorInfo, parameters, fieldInfo, fieldValue);
            }
            else
            {
                return useProperty
                    ? new CustomAttributeBuilder(ctorInfo, parameters, propertyInfo, propertyValue)
                    : new CustomAttributeBuilder(ctorInfo, parameters);
            }
        }
    }
}
