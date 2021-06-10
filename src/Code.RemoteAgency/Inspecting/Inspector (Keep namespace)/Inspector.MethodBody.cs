using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency.Inspecting
{
    partial class Inspector
    {
        static void GetAsyncReturnType(Type methodInfoReturnType, out Type returnType,
            out AsyncMethodOriginalReturnValueDataTypeClass asyncMethodOriginalReturnValueDataTypeClass,
            AsyncMethodAttribute asyncMethod, MemberInfo member, Stack<MemberInfo> parentPath)
        {
            if (asyncMethod != null)
            {
                if (methodInfoReturnType == typeof(Task))
                {
                    returnType = typeof(void);
                    asyncMethodOriginalReturnValueDataTypeClass = AsyncMethodOriginalReturnValueDataTypeClass.Task;
                }
#if !netfx
                else if (methodInfoReturnType == typeof(ValueTask))
                {
                    returnType = typeof(void);
                    asyncMethodOriginalReturnValueDataTypeClass = AsyncMethodOriginalReturnValueDataTypeClass.ValueTask;
                }
#endif
                else if (methodInfoReturnType.IsGenericType)
                {
                    if (methodInfoReturnType.GetGenericTypeDefinition() == typeof(Task<>))
                    {
                        returnType = methodInfoReturnType.GetGenericArguments()[0];
                        asyncMethodOriginalReturnValueDataTypeClass = AsyncMethodOriginalReturnValueDataTypeClass.TaskOfType;
                    }
#if !netfx
                    else if (methodInfoReturnType.GetGenericTypeDefinition() == typeof(ValueTask<>))
                    {
                        returnType = methodInfoReturnType.GetGenericArguments()[0];
                        asyncMethodOriginalReturnValueDataTypeClass = AsyncMethodOriginalReturnValueDataTypeClass.ValueTaskOfType;
                    }
#endif
                    else
                    {
                        throw new InvalidAttributeDataException(
                            "The method must has a return value as Task, ValueTask or their derived class. Method with other return type is not supported.",
                            asyncMethod, member, parentPath);
                    }
                }
                else
                {
                    throw new InvalidAttributeDataException(
                        "The method must has a return value as Task, ValueTask or their derived class. Method with other return type is not supported.",
                        asyncMethod, member, parentPath);
                }
            }
            else
            {
                returnType = methodInfoReturnType;
                asyncMethodOriginalReturnValueDataTypeClass = AsyncMethodOriginalReturnValueDataTypeClass.NotAsyncMethod;
            }
        }

        static void ProcessMethodBodyForIgnoredAsset(MethodInfo methodInfo, Type returnType, bool willThrowExceptionWhileCalling,
            RemoteAgencyMethodBodyInfo target, AsyncMethodOriginalReturnValueDataTypeClass asyncMethodOriginalReturnValueDataTypeClass)
        {
            var parameters = methodInfo.GetParameters();

            target.Parameters = parameters;
            target.ReturnType = returnType;

            target.ParameterEntityProperties = new List<RemoteAgencyParameterInfo>();
            target.ReturnValueEntityProperties = new List<RemoteAgencyReturnValueInfoBase>();

            foreach (var parameter in parameters)
            {
                if (parameter.IsOut)
                {
                    var item =
                        new RemoteAgencyReturnValueInfoFromParameterDefaultValue()
                        {
                            Parameter = parameter
                        };
                    target.ReturnValueEntityProperties.Add(item);
                }
            }

            if (returnType != typeof(void) && !willThrowExceptionWhileCalling)
            {
                var item =
                    new RemoteAgencyReturnValueInfoFromReturnValueDefaultValue()
                    {
                        ReturnValueDataType = returnType,
                        AsyncMethodOriginalReturnValueDataTypeClass = asyncMethodOriginalReturnValueDataTypeClass
                    };
                target.ReturnValueEntityProperties.Add(item);
            }
        }

        private const string PropertyValueName = "value";

        void ProcessMethodBodyForOneWayAsset(MethodInfo methodInfo, Type returnType,
            Stack<MemberInfo> memberPath, bool includeCallerOnlyInfo, RemoteAgencyMethodBodyInfo target,
            AsyncMethodOriginalReturnValueDataTypeClass asyncMethodOriginalReturnValueDataTypeClass,
            Dictionary<string, ParameterIgnoredAttribute> eventLevelParameterIgnoredAttributes = null,
            Dictionary<string, CustomizedParameterEntityPropertyNameAttribute>
                eventLevelParameterEntityPropertyNameAttributes = null,
            List<Attribute> valueParameterSerializerParameterLevelAttributesOverrideForPropertySetting = null,
            string propertyValuePropertyNameSpecifiedByAttribute = null,
            Attribute propertyValuePropertyNameSpecifyingAttribute = null)
        {
            var parameters = methodInfo.GetParameters();

            target.Parameters = parameters;
            target.ReturnType = returnType;

            target.ParameterEntityProperties = new List<RemoteAgencyParameterInfo>();
            target.ReturnValueEntityProperties = new List<RemoteAgencyReturnValueInfoBase>();

            var usedPropertyNamesInParameterEntity = new HashSet<string>(); //could be duplicated cause by the case changing :)

            foreach (var parameter in parameters)
            {
                if (parameter.GetValueFromAttribute(i => i.IsIgnored, out _, eventLevelParameterIgnoredAttributes))
                {
                    //continue;
                }
                else if (parameter.IsOut)
                {
                    if (includeCallerOnlyInfo) //one-way server won't process return value
                    {
                        var item =
                            new RemoteAgencyReturnValueInfoFromParameterDefaultValue()
                            {
                                Parameter = parameter
                            };
                        target.ReturnValueEntityProperties.Add(item);
                    }
                }
                else
                {
                    var parameterInfo = new RemoteAgencyParameterInfo
                    {
                        Parameter = parameter
                    };

                    if (_serializerParameterLevelAttributeBaseType != null)
                    {
                        if (valueParameterSerializerParameterLevelAttributesOverrideForPropertySetting != null && parameter.Name == PropertyValueName)
                        {
                            //only used in property. SerializerParameterLevelAttribute for return value is defined on the property asset.
                            parameterInfo.SerializerParameterLevelAttributes =
                                valueParameterSerializerParameterLevelAttributesOverrideForPropertySetting;
                        }
                        else
                        {
                            parameterInfo.SerializerParameterLevelAttributes =
                                parameter.GetCustomAttributes(_serializerParameterLevelAttributeBaseType, true)
                                    .Cast<Attribute>().ToList();
                        }
                    }

                    string requiredPropertyName;
                    Attribute requiredPropertyNameSpecifyingAttribute;
                    if (!string.IsNullOrEmpty(propertyValuePropertyNameSpecifiedByAttribute) &&
                        parameter.Name == PropertyValueName)
                    {
                        requiredPropertyName = propertyValuePropertyNameSpecifiedByAttribute;
                        requiredPropertyNameSpecifyingAttribute = propertyValuePropertyNameSpecifyingAttribute;
                    }
                    else
                    {
                        requiredPropertyName =
                            parameter.GetValueFromAttribute(i => i.EntityPropertyName,
                                out var customizedParameterEntityPropertyNameAttribute,
                                eventLevelParameterEntityPropertyNameAttributes);
                        requiredPropertyNameSpecifyingAttribute = customizedParameterEntityPropertyNameAttribute;
                    }

                    if (!string.IsNullOrEmpty(requiredPropertyName))
                    {
                        if (!usedPropertyNamesInParameterEntity.Add(requiredPropertyName))
                        {
                            throw new EntityPropertyNameConflictException(
                                $"The property name specified conflicts with others in parameter entity.",
                                requiredPropertyNameSpecifyingAttribute, parameter, memberPath);
                        }

                        parameterInfo.PropertyName = requiredPropertyName;
                    }
                    else
                    {
                        parameterInfo.PropertyName = AutoNamePlaceHolder;
                    }

                    target.ParameterEntityProperties.Add(parameterInfo);
                }
            }

            if (includeCallerOnlyInfo) //one-way server won't process return value
            {
                if (returnType != typeof(void))
                {
                    var item =
                        new RemoteAgencyReturnValueInfoFromReturnValueDefaultValue()
                        {
                            ReturnValueDataType = returnType,
                            AsyncMethodOriginalReturnValueDataTypeClass = asyncMethodOriginalReturnValueDataTypeClass
                        };
                    target.ReturnValueEntityProperties.Add(item);
                }
            }

            //assign auto name
            foreach (var parameter in target.ParameterEntityProperties)
            {
                if (parameter.PropertyName == AutoNamePlaceHolder)
                    parameter.PropertyName =
                        GetPropertyAutoName(parameter.Parameter.Name, usedPropertyNamesInParameterEntity);
            }
        }

        void ProcessMethodBodyForNormalAsset(MethodInfo methodInfo, Type returnType,
            Stack<MemberInfo> memberPath, int timeOut, bool isReturnValueIgnored, 
            string returnValuePropertyNameSpecifiedByAttribute, Attribute returnValuePropertyNameSpecifyingAttribute,
            RemoteAgencyMethodBodyInfo target, AsyncMethodOriginalReturnValueDataTypeClass asyncMethodOriginalReturnValueDataTypeClass, 
            Dictionary<string, ParameterIgnoredAttribute> eventLevelParameterIgnoredAttributes = null,
            Dictionary<string, CustomizedParameterEntityPropertyNameAttribute>
                eventLevelParameterEntityPropertyNameAttributes = null,
            Dictionary<string, ParameterReturnRequiredAttribute> eventLevelParameterReturnRequiredAttributes = null,
            Dictionary<string, List<ParameterReturnRequiredPropertyAttribute>> eventLevelParameterReturnRequiredPropertyAttributes = null,
            List<Attribute> valueParameterSerializerParameterLevelAttributesOverrideForPropertyGetting = null,
            List<Attribute> valueParameterSerializerParameterLevelAttributesOverrideForPropertySetting = null,
            string propertyValuePropertyNameSpecifiedByAttribute = null,
            CustomizedParameterEntityPropertyNameAttribute propertyValuePropertyNameSpecifyingAttribute = null,
            List<ParameterReturnRequiredPropertyAttribute> propertyValueParameterReturnRequiredProperty = null)
        {
            var parameters = methodInfo.GetParameters();

            target.Parameters = parameters;
            target.ReturnType = returnType;

            target.ParameterEntityProperties = new List<RemoteAgencyParameterInfo>();
            target.ReturnValueEntityProperties = new List<RemoteAgencyReturnValueInfoBase>();
            target.Timeout = timeOut;

            var usedPropertyNamesInParameterEntity = new HashSet<string>(); //could be duplicated cause by the case changing :)
            var usedPropertyNamesInReturnValueEntity = new HashSet<string>();

            foreach (var parameter in parameters)
            {
                if (parameter.IsOut || parameter.GetValueFromAttribute(i => i.IsIgnored, out _,
                    eventLevelParameterIgnoredAttributes))
                {
                    //ignored in parameter entity
                }
                else
                {
                    var parameterInfo = new RemoteAgencyParameterInfo
                    {
                        Parameter = parameter
                    };

                    if (_serializerParameterLevelAttributeBaseType != null)
                    {
                        if (valueParameterSerializerParameterLevelAttributesOverrideForPropertySetting != null && parameter.Name == PropertyValueName)
                        {
                            //only used in property. SerializerParameterLevelAttribute for return value is defined on the property asset.
                            parameterInfo.SerializerParameterLevelAttributes =
                                valueParameterSerializerParameterLevelAttributesOverrideForPropertySetting;
                        }
                        else
                        {
                            parameterInfo.SerializerParameterLevelAttributes =
                                parameter.GetCustomAttributes(_serializerParameterLevelAttributeBaseType, true)
                                    .Cast<Attribute>().ToList();
                        }
                    }

                    string requiredPropertyName;
                    CustomizedParameterEntityPropertyNameAttribute requiredPropertyNameSpecifyingAttribute;
                    if (!string.IsNullOrEmpty(propertyValuePropertyNameSpecifiedByAttribute) &&
                        parameter.Name == PropertyValueName)
                    {
                        requiredPropertyName = propertyValuePropertyNameSpecifiedByAttribute;
                        requiredPropertyNameSpecifyingAttribute = propertyValuePropertyNameSpecifyingAttribute;
                    }
                    else
                    {
                        requiredPropertyName =
                            parameter.GetValueFromAttribute(i => i.EntityPropertyName,
                                out var customizedParameterEntityPropertyNameAttribute,
                                eventLevelParameterEntityPropertyNameAttributes);
                        requiredPropertyNameSpecifyingAttribute = customizedParameterEntityPropertyNameAttribute;
                    }

                    if (!string.IsNullOrEmpty(requiredPropertyName))
                    {
                        if (!usedPropertyNamesInParameterEntity.Add(requiredPropertyName))
                        {
                            throw new EntityPropertyNameConflictException(
                                "The property name specified conflicts with others in parameter entity.",
                                requiredPropertyNameSpecifyingAttribute, parameter, memberPath);
                        }

                        parameterInfo.PropertyName = requiredPropertyName;
                    }
                    else
                    {
                        parameterInfo.PropertyName = AutoNamePlaceHolder;
                    }

                    target.ParameterEntityProperties.Add(parameterInfo);
                }

                if (parameter.IsOut || parameter.ParameterType.IsByRef)
                {
                    #region Out/Ref

                    var isIncludedInReturning = parameter.GetValueFromAttribute(i => i.IsIncludedInReturning, out var returnRequiredAttribute,
                        eventLevelParameterReturnRequiredAttributes, true); //default value is true
                    if (isIncludedInReturning)
                    {
                        string name = returnRequiredAttribute?.ResponseEntityPropertyName;
                        if (!string.IsNullOrEmpty(name))
                        {
                            if (!usedPropertyNamesInReturnValueEntity.Add(name))
                            {
                                throw new EntityPropertyNameConflictException(
                                    "The property name specified conflicts with others in return value entity.",
                                    returnRequiredAttribute, parameter, memberPath);
                            }
                        }
                        else
                        {
                            name = AutoNamePlaceHolder;
                        }

                        var returnProperty =
                            new RemoteAgencyReturnValueInfoFromParameter()
                            {
                                PropertyName = name,
                                IsIncludedWhenExceptionThrown = returnRequiredAttribute?.IsIncludedWhenExceptionThrown ?? false,
                                Parameter = parameter
                            };
                        if (_serializerParameterLevelAttributeBaseType != null)
                        {
                            returnProperty.SerializerParameterLevelAttributes =
                                parameter.GetCustomAttributes(_serializerParameterLevelAttributeBaseType, true)
                                    .Cast<Attribute>().ToList();
                        }

                        target.ReturnValueEntityProperties.Add(returnProperty);
                    }

                    var returnRequiredPropertyAttribute =
                        parameter.GetAttributes(eventLevelParameterReturnRequiredPropertyAttributes)
                            .FirstOrDefault();

                    if (returnRequiredPropertyAttribute != null)
                        throw new InvalidParameterAttributeDataException(
                            $"{returnRequiredPropertyAttribute.GetType().Name} can only be used on the parameter without \"ref / ByRef\" and \"out / Out\"",
                            returnRequiredPropertyAttribute, parameter, memberPath);

                    #endregion
                }
                else
                {
                    #region Return required property

                    var returnRequiredAttribute = parameter.GetCustomAttribute<ParameterReturnRequiredAttribute>();

                    if (returnRequiredAttribute != null)
                        throw new InvalidParameterAttributeDataException(
                            $"{nameof(ParameterReturnRequiredAttribute)} can only be used on the parameter with \"ref / ByRef\" and \"out / Out\".",
                            returnRequiredAttribute, parameter, memberPath);

                    List<ParameterReturnRequiredPropertyAttribute> returnRequiredPropertyAttributes;
                    if (propertyValueParameterReturnRequiredProperty != null && parameter.Name == PropertyValueName)
                        returnRequiredPropertyAttributes = propertyValueParameterReturnRequiredProperty;
                    else
                        returnRequiredPropertyAttributes = parameter.GetAttributes(eventLevelParameterReturnRequiredPropertyAttributes);
                    if (returnRequiredPropertyAttributes.Count > 0)
                    {
                        var processedProperties = new HashSet<string>();
                        var processedHelpers = new HashSet<Type>();

                        foreach (var returnRequiredPropertyAttribute in returnRequiredPropertyAttributes)
                        {
                            if (returnRequiredPropertyAttribute.IsSimpleMode)
                            {
                                if (!processedProperties.Add(returnRequiredPropertyAttribute.PropertyNameInParameter))
                                    continue;

                                var parameterType = parameter.ParameterType;

                                var field = parameterType.GetField(returnRequiredPropertyAttribute.PropertyNameInParameter);
                                if (field != null)
                                {
                                    #region Simple mode on field

                                    string name = returnRequiredPropertyAttribute.ResponseEntityPropertyName;
                                    if (!string.IsNullOrEmpty(name))
                                    {
                                        if (!usedPropertyNamesInReturnValueEntity.Add(name))
                                        {
                                            throw new EntityPropertyNameConflictException(
                                                "The property name specified conflicts with others in return value entity.",
                                                returnRequiredPropertyAttribute, parameter, memberPath);
                                        }
                                    }
                                    else
                                    {
                                        name = AutoNamePlaceHolder;
                                    }

                                    var returnProperty =
                                        new RemoteAgencyReturnValueInfoFromParameterField()
                                        {
                                            PropertyName = name,
                                            IsIncludedWhenExceptionThrown =
                                                returnRequiredPropertyAttribute.IsIncludedWhenExceptionThrown,
                                            ParameterField = field,
                                            Parameter = parameter
                                        };
                                    if (_serializerParameterLevelAttributeBaseType != null)
                                    {
                                        returnProperty.SerializerParameterLevelAttributes =
                                            parameter.GetCustomAttributes(_serializerParameterLevelAttributeBaseType,
                                                true).Cast<Attribute>().ToList();
                                        returnProperty.SerializerParameterLevelAttributesOnField =
                                            field.GetCustomAttributes(_serializerParameterLevelAttributeBaseType, true)
                                                .Cast<Attribute>().ToList();
                                    }

                                    target.ReturnValueEntityProperties.Add(returnProperty);

                                    #endregion
                                }
                                else
                                {
                                    var property =
                                        parameterType.GetProperty(returnRequiredPropertyAttribute.PropertyNameInParameter);
                                    if (property != null)
                                    {
                                        #region Simple mode on property

                                        if (property.GetGetMethod() == null)
                                            throw new InvalidParameterAttributeDataException(
                                                "The property name specified is not readable publicly.",
                                                returnRequiredPropertyAttribute, parameter, memberPath);
                                        if (property.GetSetMethod() == null)
                                            throw new InvalidParameterAttributeDataException(
                                                "The property name specified is not writable publicly.",
                                                returnRequiredPropertyAttribute, parameter, memberPath);

                                        string name = returnRequiredPropertyAttribute.ResponseEntityPropertyName;
                                        if (!string.IsNullOrEmpty(name))
                                        {
                                            if (!usedPropertyNamesInReturnValueEntity.Add(name))
                                            {
                                                throw new EntityPropertyNameConflictException(
                                                    "The property name specified conflicts with others in return value entity.",
                                                    returnRequiredPropertyAttribute, parameter, memberPath);
                                            }
                                        }
                                        else
                                        {
                                            name = AutoNamePlaceHolder;
                                        }

                                        var returnProperty =
                                            new RemoteAgencyReturnValueInfoFromParameterProperty()
                                            {
                                                PropertyName = name,
                                                IsIncludedWhenExceptionThrown = returnRequiredPropertyAttribute
                                                    .IsIncludedWhenExceptionThrown,
                                                ParameterProperty = property,
                                                Parameter = parameter
                                            };
                                        if (_serializerParameterLevelAttributeBaseType != null)
                                        {
                                            returnProperty.SerializerParameterLevelAttributes =
                                                parameter.GetCustomAttributes(
                                                        _serializerParameterLevelAttributeBaseType, true)
                                                    .Cast<Attribute>()
                                                    .ToList();
                                            returnProperty.SerializerParameterLevelAttributesOnProperty =
                                                property.GetCustomAttributes(_serializerParameterLevelAttributeBaseType,
                                                    true).Cast<Attribute>().ToList();
                                        }

                                        target.ReturnValueEntityProperties.Add(returnProperty);

                                        #endregion
                                    }
                                    else
                                    {
                                        throw new InvalidParameterAttributeDataException(
                                            "The property name specified is not a public property, nor a public field.",
                                            returnRequiredPropertyAttribute, parameter, memberPath);
                                    }
                                }
                            }
                            else
                            {
                                #region Helper class mode

                                if (!processedHelpers.Add(returnRequiredPropertyAttribute.HelperClass))
                                    continue;

                                var ctor = returnRequiredPropertyAttribute.HelperClass.GetConstructor(new[]
                                    {parameter.ParameterType});
                                if (ctor == null)
                                {
                                    throw new InvalidParameterAttributeDataException(
                                        $"Publicly accessible constructor with one parameter of ${parameter.ParameterType} is not found in helper class {returnRequiredPropertyAttribute.HelperClass.FullName}",
                                        returnRequiredPropertyAttribute, parameter, memberPath);
                                }

                                var propertiesInHelperClass = returnRequiredPropertyAttribute.HelperClass.GetProperties();
                                foreach (var propertyInHelperClass in propertiesInHelperClass)
                                {
                                    if (propertyInHelperClass.GetValueFromAttribute<ReturnRequiredPropertyHelperAttribute, bool>(
                                        i => i.IsIncludedInReturning, out var returnRequiredHelperAttribute))
                                    {
                                        if (propertyInHelperClass.GetGetMethod() == null)
                                            throw new InvalidParameterAttributeDataException(
                                                $"The property {propertyInHelperClass.Name} in helper class {returnRequiredPropertyAttribute.HelperClass.FullName} is not readable publicly.",
                                                returnRequiredPropertyAttribute, parameter, memberPath);
                                        if (propertyInHelperClass.GetSetMethod() == null)
                                            throw new InvalidParameterAttributeDataException(
                                                $"The property {propertyInHelperClass.Name} in helper class {returnRequiredPropertyAttribute.HelperClass.FullName} is not readable publicly.",
                                                returnRequiredPropertyAttribute, parameter, memberPath);

                                        var name = returnRequiredHelperAttribute.ResponseEntityPropertyName;
                                        if (!string.IsNullOrEmpty(name))
                                        {
                                            if (!usedPropertyNamesInReturnValueEntity.Add(name))
                                            {
                                                throw new EntityPropertyNameConflictException(
                                                    $"The property name specified for property {propertyInHelperClass.Name} in helper class {returnRequiredPropertyAttribute.HelperClass.FullName} conflicts with others in return value entity.",
                                                    returnRequiredPropertyAttribute, parameter, memberPath);
                                            }
                                        }
                                        else
                                        {
                                            name = AutoNamePlaceHolder;
                                        }

                                        var returnProperty =
                                            new RemoteAgencyReturnValueInfoFromParameterHelperProperty()
                                            {
                                                PropertyName = name,
                                                IsIncludedWhenExceptionThrown =
                                                    returnRequiredHelperAttribute.IsIncludedWhenExceptionThrown,
                                                ParameterHelperClass = returnRequiredPropertyAttribute.HelperClass,
                                                ParameterHelperProperty = propertyInHelperClass,
                                                Parameter = parameter
                                            };
                                        if (_serializerParameterLevelAttributeBaseType != null)
                                        {
                                            returnProperty.SerializerParameterLevelAttributes =
                                                parameter.GetCustomAttributes(
                                                        _serializerParameterLevelAttributeBaseType, true)
                                                    .Cast<Attribute>()
                                                    .ToList();
                                            returnProperty.SerializerParameterLevelAttributesOnHelperProperty =
                                                propertyInHelperClass
                                                    .GetCustomAttributes(_serializerParameterLevelAttributeBaseType,
                                                        true).Cast<Attribute>().ToList();
                                        }

                                        target.ReturnValueEntityProperties.Add(returnProperty);
                                    }
                                }

                                #endregion
                            }
                        }
                    }

                    #endregion
                }
            }

            //return value
            if (!isReturnValueIgnored && returnType != typeof(void))
            {
                string returnValuePropertyName;

                if (!string.IsNullOrEmpty(returnValuePropertyNameSpecifiedByAttribute))
                {
                    if (!usedPropertyNamesInReturnValueEntity.Add(returnValuePropertyNameSpecifiedByAttribute))
                    {
                        throw new EntityPropertyNameConflictException(
                            "The property name specified conflicts with others in return value entity.",
                            returnValuePropertyNameSpecifyingAttribute,
                            EntityPropertyNameConflictExceptionCausedMemberType.ReturnValue, memberPath);
                    }

                    returnValuePropertyName = returnValuePropertyNameSpecifiedByAttribute;
                }
                else
                {
                    returnValuePropertyName = AutoNamePlaceHolder;
                }

                var item = new RemoteAgencyReturnValueInfoFromReturnValue()
                {
                    PropertyName = returnValuePropertyName,
                    ReturnValueDataType = returnType,
                    AsyncMethodOriginalReturnValueDataTypeClass = asyncMethodOriginalReturnValueDataTypeClass
                };
                if (_serializerParameterLevelAttributeBaseType != null)
                {
                    if (valueParameterSerializerParameterLevelAttributesOverrideForPropertyGetting != null)
                    {
                        //only used in property. SerializerParameterLevelAttribute for return value is defined on the property asset.
                        item.SerializerParameterLevelAttributesOnReturnValue =
                            valueParameterSerializerParameterLevelAttributesOverrideForPropertyGetting;
                    }
                    else
                    {
                        item.SerializerParameterLevelAttributesOnReturnValue =
                            methodInfo.ReturnTypeCustomAttributes
                                .GetCustomAttributes(_serializerParameterLevelAttributeBaseType, true).Cast<Attribute>()
                                .ToList();
                    }
                }

                target.ReturnValueEntityProperties.Add(item);
            }

            //assign auto name
            foreach (var parameter in target.ParameterEntityProperties)
            {
                if (parameter.PropertyName == AutoNamePlaceHolder)
                    parameter.PropertyName =
                        GetPropertyAutoName(parameter.Parameter.Name, usedPropertyNamesInParameterEntity);
            }

            foreach (var returnValue in target.ReturnValueEntityProperties)
            {
                if (returnValue.IsIncludedInEntity && returnValue.PropertyName == AutoNamePlaceHolder)
                {
                    returnValue.PropertyName =
                        GetPropertyAutoName(returnValue.GetDefaultPropertyName(), usedPropertyNamesInReturnValueEntity);
                }
            }
        }
    }
}
