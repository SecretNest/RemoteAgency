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
        void ProcessMethodBodyForIgnoredAsset(MethodInfo methodInfo, bool willThrowExceptionWhileCalling, RemoteAgencyMethodBodyInfo target)
        {
            var parameters = methodInfo.GetParameters();
            var returnType = methodInfo.ReturnType;

            target.ParameterEntityProperties = new List<RemoteAgencyParameterInfo>();
            target.ReturnValueEntityProperties = new List<RemoteAgencyReturnValueInfoBase>();

            foreach (var parameter in parameters)
            {
                if (parameter.IsOut)
                {
                    RemoteAgencyReturnValueInfoFromParameterDefaultValue item =
                        new RemoteAgencyReturnValueInfoFromParameterDefaultValue()
                        {
                            Parameter = parameter
                        };
                    target.ReturnValueEntityProperties.Add(item);
                }
            }

            if (returnType != typeof(void) && !willThrowExceptionWhileCalling)
            {
                RemoteAgencyReturnValueInfoFromReturnValueDefaultValue item =
                    new RemoteAgencyReturnValueInfoFromReturnValueDefaultValue()
                    {
                        ReturnValueDataType = returnType
                    };
                target.ReturnValueEntityProperties.Add(item);
            }
        }

        private const string PropertyValueName = "value";

        void ProcessMethodBodyForOneWayAsset(MethodInfo methodInfo, Stack<MemberInfo> memberPath,
            bool includeCallerOnlyInfo, RemoteAgencyMethodBodyInfo target,
            Dictionary<string, ParameterIgnoredAttribute> eventLevelParameterIgnoredAttributes = null,
            Dictionary<string, CustomizedParameterEntityPropertyNameAttribute>
                eventLevelParameterEntityPropertyNameAttributes = null,
            List<Attribute> valueParameterSerializerParameterLevelAttributesOverrideForPropertySetting = null,
            string propertyValuePropertyNameSpecifiedByAttribute = null,
            Attribute propertyValuePropertyNameSpecifyingAttribute = null)
        {
            var parameters = methodInfo.GetParameters();
            var returnType = methodInfo.ReturnType;

            target.ParameterEntityProperties = new List<RemoteAgencyParameterInfo>();
            target.ReturnValueEntityProperties = new List<RemoteAgencyReturnValueInfoBase>();

            HashSet<string>
                usedPropertyNamesInParameterEntity =
                    new HashSet<string>(); //could be duplicated cause by the case changing :)

            foreach (var parameter in parameters)
            {
                if (GetValueFromAttribute(parameter, i => i.IsIgnored, out _, eventLevelParameterIgnoredAttributes))
                {
                    //continue;
                }
                else if (parameter.IsOut)
                {
                    if (includeCallerOnlyInfo) //one-way server won't process return value
                    {
                        RemoteAgencyReturnValueInfoFromParameterDefaultValue item =
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
                            GetValueFromAttribute(parameter, i => i.EntityPropertyName,
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
                    RemoteAgencyReturnValueInfoFromReturnValueDefaultValue item =
                        new RemoteAgencyReturnValueInfoFromReturnValueDefaultValue()
                        {
                            ReturnValueDataType = returnType
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

        void ProcessMethodBodyForNormalAsset(MethodInfo methodInfo, Stack<MemberInfo> memberPath,
            int timeOut, bool isReturnValueIgnored, string returnValuePropertyNameSpecifiedByAttribute, Attribute returnValuePropertyNameSpecifyingAttribute,
            RemoteAgencyMethodBodyInfo target, 
            Dictionary<string, ParameterIgnoredAttribute> eventLevelParameterIgnoredAttributes = null,
            Dictionary<string, CustomizedParameterEntityPropertyNameAttribute>
                eventLevelParameterEntityPropertyNameAttributes = null,
            Dictionary<string, ParameterReturnRequiredAttribute> eventLevelParameterReturnRequiredAttributes = null,
            List<Attribute> valueParameterSerializerParameterLevelAttributesOverrideForPropertyGetting = null,
            List<Attribute> valueParameterSerializerParameterLevelAttributesOverrideForPropertySetting = null,
            string propertyValuePropertyNameSpecifiedByAttribute = null,
            Attribute propertyValuePropertyNameSpecifyingAttribute = null)
        {
            var parameters = methodInfo.GetParameters();
            var returnType = methodInfo.ReturnType;

            target.ParameterEntityProperties = new List<RemoteAgencyParameterInfo>();
            target.ReturnValueEntityProperties = new List<RemoteAgencyReturnValueInfoBase>();
            target.Timeout = timeOut;

            HashSet<string>
                usedPropertyNamesInParameterEntity =
                    new HashSet<string>(); //could be duplicated cause by the case changing :)
            HashSet<string> usedPropertyNamesInReturnValueEntity = new HashSet<string>();

            foreach (var parameter in parameters)
            {
                if (parameter.IsOut || GetValueFromAttribute(parameter, i => i.IsIgnored, out _,
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
                            GetValueFromAttribute(parameter, i => i.EntityPropertyName,
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

                    var isIncludedInReturning = GetValueFromAttribute(parameter, i => i.IsIncludedInReturning, out var returnRequiredAttribute,
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

                        RemoteAgencyReturnValueInfoFromParameter returnProperty =
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

                    var returnRequiredPropertyAttribute = parameter.GetCustomAttributes<ParameterReturnRequiredPropertyAttribute>()
                        .FirstOrDefault();

                    if (returnRequiredPropertyAttribute != null)
                        throw new InvalidParameterAttributeDataException(
                            $"{nameof(ParameterReturnRequiredPropertyAttribute)} can only be used on the parameter without \"ref / ByRef\" and \"out / Out\"",
                            returnRequiredPropertyAttribute, parameter, memberPath);

                    #endregion
                }
                else
                {
                    #region Return required propery

                    var returnRequiredAttribute = parameter.GetCustomAttribute<ParameterReturnRequiredAttribute>();

                    if (returnRequiredAttribute != null)
                        throw new InvalidParameterAttributeDataException(
                            $"{nameof(ParameterReturnRequiredAttribute)} can only be used on the parameter with \"ref / ByRef\" and \"out / Out\".",
                            returnRequiredAttribute, parameter, memberPath);

                    var returnRequiredPropertyAttributes =
                        parameter.GetCustomAttributes<ParameterReturnRequiredPropertyAttribute>().ToArray();
                    if (returnRequiredPropertyAttributes.Length > 0)
                    {
                        HashSet<string> processedProperties = new HashSet<string>();
                        HashSet<Type> processedHelpers = new HashSet<Type>();

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

                                    RemoteAgencyReturnValueInfoFromParameterField returnProperty =
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

                                        RemoteAgencyReturnValueInfoFromParameterProperty returnProperty =
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

                                var propertiesInHelperClass = returnRequiredPropertyAttribute.HelperClass.GetProperties();
                                foreach (var propertyInHelperClass in propertiesInHelperClass)
                                {
                                    if (GetValueFromAttribute<ReturnRequiredPropertyHelperAttribute, bool>(propertyInHelperClass,
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

                                        string name = returnRequiredHelperAttribute.ResponseEntityPropertyName;
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

                                        RemoteAgencyReturnValueInfoFromParameterHelperProperty returnProperty =
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

                RemoteAgencyReturnValueInfoFromReturnValue item = new RemoteAgencyReturnValueInfoFromReturnValue()
                {
                    PropertyName = returnValuePropertyName,
                    ReturnValueDataType = returnType
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