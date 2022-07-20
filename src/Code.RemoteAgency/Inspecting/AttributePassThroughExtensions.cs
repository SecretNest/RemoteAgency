using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using SecretNest.RemoteAgency.AssemblyBuilding;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency.Inspecting
{
    static class AttributePassThroughExtensions
    {
        static ILookup<string, T> BuildRelatedLookup<T>(this ICustomAttributeProvider dataSource) where T : AttributePassThroughAdditionalDataAttributeBase
        {
            return dataSource.GetCustomAttributes(typeof(T), true)
                .ToLookup(i => ((T)i).AttributeId,
                    i => (T)i);
        }

        public static List<CustomAttributeBuilder> GetAttributePassThrough(this ICustomAttributeProvider dataSource, Func<string, Attribute, InvalidAttributeDataException> creatingExceptionCallback)
        {
            var result = new List<CustomAttributeBuilder>();

            var attributePassThroughAttributes =
                dataSource.GetCustomAttributes(typeof(AttributePassThroughAttribute), true).Cast<AttributePassThroughAttribute>().ToArray();
            if (attributePassThroughAttributes.Length == 0)
                return result;

            var attributePassThroughIndexBasedParameterAttributes =
                dataSource.BuildRelatedLookup<AttributePassThroughIndexBasedParameterAttribute>();
            var attributePassThroughPropertyAttributes =
                dataSource.BuildRelatedLookup<AttributePassThroughPropertyAttribute>();
            var attributePassThroughFieldAttributes =
                dataSource.BuildRelatedLookup<AttributePassThroughFieldAttribute>();

            var processedAttributeId = new HashSet<string>();
            foreach (var attributePassThroughAttribute in attributePassThroughAttributes)
            {
                //Avoid process with same attribute id.
                if (attributePassThroughAttribute.AttributeId != null)
                {
                    if (!processedAttributeId.Add(attributePassThroughAttribute.AttributeId))
                        continue;
                }

                CustomAttributeBuilder customAttributeBuilder;

                var attribute = attributePassThroughAttribute.Attribute;

                var ctorInfo = attribute.GetConstructor(attributePassThroughAttribute.AttributeConstructorParameterTypes);
                if (ctorInfo == null)
                    throw new InvalidOperationException(
                        $"The constructor of {attribute.Name} specified with {nameof(AttributePassThroughAttribute)}.{nameof(AttributePassThroughAttribute.AttributeConstructorParameterTypes)} in attribute is not found.");

                var ctorParameters = new object[attributePassThroughAttribute.AttributeConstructorParameterTypes.Length];

                if (attributePassThroughAttribute.AttributeConstructorParameterTypes.Length != 0)
                {
                    //parameters
                    if (attributePassThroughAttribute.AttributeConstructorParameters != null)
                    {
                        if (attributePassThroughAttribute.AttributeConstructorParameters.Length >
                            ctorParameters.Length)
                        {
                            throw creatingExceptionCallback(
                                $"Length of {nameof(attributePassThroughAttribute.AttributeConstructorParameters)} cannot exceed the length of {nameof(attributePassThroughAttribute.AttributeConstructorParameterTypes)}.",
                                attributePassThroughAttribute);
                        }

                        for (int i = 0; i < attributePassThroughAttribute.AttributeConstructorParameters.Length; i++)
                        {
                            ctorParameters[i] = attributePassThroughAttribute.AttributeConstructorParameters[i];
                        }

                    }

                    //additional parameters
                    if (attributePassThroughAttribute.AttributeId != null && attributePassThroughIndexBasedParameterAttributes.Contains(attributePassThroughAttribute.AttributeId))
                    {
                        var linkedAttributePassThroughIndexBasedParameterAttributes =
                            attributePassThroughIndexBasedParameterAttributes[
                                attributePassThroughAttribute.AttributeId];

                        var setIndices = new HashSet<int>();

                        foreach (var attributePassThroughIndexBasedParameterAttribute in
                            linkedAttributePassThroughIndexBasedParameterAttributes)
                        {
                            //Avoid process with same index.
                            if (setIndices.Add(attributePassThroughIndexBasedParameterAttribute.ParameterIndex))
                            {
                                if (attributePassThroughIndexBasedParameterAttribute.ParameterIndex >=
                                    ctorParameters.Length || attributePassThroughIndexBasedParameterAttribute.ParameterIndex < 0)
                                {
                                    throw creatingExceptionCallback(
                                        $"{nameof(AttributePassThroughIndexBasedParameterAttribute.ParameterIndex)} cannot be negative, equal or larger than the length of {nameof(AttributePassThroughAttribute.AttributeConstructorParameterTypes)}.",
                                        attributePassThroughIndexBasedParameterAttribute);
                                }
                                else
                                {
                                    ctorParameters[attributePassThroughIndexBasedParameterAttribute.ParameterIndex] =
                                        attributePassThroughIndexBasedParameterAttribute.Value;
                                }
                            }
                        }
                    }
                }

                //properties and fields
                if (attributePassThroughAttribute.AttributeId != null)
                {
                    PropertyInfo[] propertyInfo;
                    FieldInfo[] fieldInfo;
                    object[] propertyValue, fieldValue;

                    bool useProperty, useField;

                    //properties
                    if (attributePassThroughPropertyAttributes.Contains(attributePassThroughAttribute.AttributeId))
                    {
                        var linkedAttributePassThroughPropertyAttributes =
                            attributePassThroughPropertyAttributes[attributePassThroughAttribute.AttributeId]
                                .OrderBy(i => i.Order).ToList();

                        useProperty = true;
                        var setProperties = new HashSet<string>();
                        var memberInfos = new List<PropertyInfo>();
                        var memberValues = new List<object>();

                        foreach (var attributePassThroughPropertyAttribute in linkedAttributePassThroughPropertyAttributes)
                        {
                            //Avoid process with same property.
                            if (!setProperties.Add(attributePassThroughPropertyAttribute.PropertyName))
                                continue;

                            var info = attribute.GetProperty(attributePassThroughPropertyAttribute.PropertyName, BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance);
                            if (info == null)
                                throw creatingExceptionCallback(
                                    $"Property {attributePassThroughPropertyAttribute.PropertyName} doesn't exist in {attribute.Name} or is not settable publicly.",
                                    attributePassThroughPropertyAttribute);
                            memberInfos.Add(info);
                            memberValues.Add(attributePassThroughPropertyAttribute.Value);
                        }

                        propertyInfo = memberInfos.ToArray();
                        propertyValue = memberValues.ToArray();
                    }
                    else
                    {
                        useProperty = false;
                        propertyInfo = null;
                        propertyValue = null;
                    }

                    //fields
                    if (attributePassThroughFieldAttributes.Contains(attributePassThroughAttribute.AttributeId))
                    {
                        var linkedAttributePassThroughFieldAttributes =
                            attributePassThroughFieldAttributes[attributePassThroughAttribute.AttributeId]
                                .OrderBy(i => i.Order).ToList();

                        useField = true;
                        var setFields = new HashSet<string>();
                        var memberInfos = new List<FieldInfo>();
                        var memberValues = new List<object>();

                        foreach (var attributePassThroughFieldAttribute in linkedAttributePassThroughFieldAttributes)
                        {
                            //Avoid process with same field.
                            if (!setFields.Add(attributePassThroughFieldAttribute.FieldName))
                                continue;

                            var info = attribute.GetField(attributePassThroughFieldAttribute.FieldName, BindingFlags.SetField | BindingFlags.Public | BindingFlags.Instance);
                            if (info == null)
                                throw creatingExceptionCallback(
                                    $"Field {attributePassThroughFieldAttribute.FieldName} doesn't exist in {attribute.Name} or is not settable publicly.",
                                    attributePassThroughFieldAttribute);
                            memberInfos.Add(info);
                            memberValues.Add(attributePassThroughFieldAttribute.Value);
                        }

                        fieldInfo = memberInfos.ToArray();
                        fieldValue = memberValues.ToArray();
                    }
                    else
                    {
                        useField = false;
                        fieldInfo = null;
                        fieldValue = null;
                    }

                    //build
                    if (useProperty)
                    {
                        if (useField)
                        {
                            customAttributeBuilder = new CustomAttributeBuilder(ctorInfo, ctorParameters, propertyInfo,
                                propertyValue, fieldInfo,
                                fieldValue);
                        }
                        else
                        {
                            customAttributeBuilder =
                                new CustomAttributeBuilder(ctorInfo, ctorParameters, propertyInfo, propertyValue);
                        }
                    }
                    else if (useField)
                    {
                        customAttributeBuilder =
                            new CustomAttributeBuilder(ctorInfo, ctorParameters, fieldInfo, fieldValue);
                    }
                    else
                    {
                        customAttributeBuilder = new CustomAttributeBuilder(ctorInfo, ctorParameters);
                    }
                }
                else
                {
                    //build without property or field
                    customAttributeBuilder = new CustomAttributeBuilder(ctorInfo, ctorParameters);
                }

                result.Add(customAttributeBuilder);
            }

            return result;
        }

        public static Dictionary<string, List<CustomAttributeBuilder>> FillAttributePassThroughOnParameters(
            this ParameterInfo[] parameters,
            Func<string, Attribute, ParameterInfo, InvalidAttributeDataException> creatingExceptionCallback)
        {
            var result =
                new Dictionary<string, List<CustomAttributeBuilder>>(parameters.Length);
            foreach (var parameterInfo in parameters)
            {
                result[parameterInfo.Name!] = GetAttributePassThrough(parameterInfo,
                    (m, a) => creatingExceptionCallback(m, a, parameterInfo));
            }

            return result;
        }

        public static Dictionary<string, List<CustomAttributeBuilder>> FillAttributePassThroughOnGenericParameters(
            this Type[] genericParameters, Func<string, Attribute, MemberInfo, InvalidAttributeDataException> creatingExceptionCallback)
        {
            var result =
                new Dictionary<string, List<CustomAttributeBuilder>>(genericParameters.Length);
            foreach (var typeInfo in genericParameters)
            {
                result[typeInfo.Name] = GetAttributePassThrough(typeInfo, (m, a) => creatingExceptionCallback(m, a, typeInfo));
            }

            return result;
        }
    }
}
