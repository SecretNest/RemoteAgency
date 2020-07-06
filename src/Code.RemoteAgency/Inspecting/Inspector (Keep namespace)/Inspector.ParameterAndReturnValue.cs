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
        //method, event
        void ProcessParameterAndReturnValueForIgnoredAsset(ParameterInfo[] parameters, Type returnType,
            out List<RemoteAgencyReturnValueInfoBase> returnValueEntityProperties)
        {
            returnValueEntityProperties = new List<RemoteAgencyReturnValueInfoBase>();

            foreach (var parameter in parameters)
            {
                if (parameter.IsOut)
                {
                    RemoteAgencyReturnValueInfoFromParameterDefaultValue item =
                        new RemoteAgencyReturnValueInfoFromParameterDefaultValue()
                        {
                            Parameter = parameter
                        };
                    returnValueEntityProperties.Add(item);
                }
            }

            if (returnType != typeof(void))
            {
                RemoteAgencyReturnValueInfoFromReturnValueDefaultValue item =
                    new RemoteAgencyReturnValueInfoFromReturnValueDefaultValue()
                    {
                        ReturnValueDataType = returnType
                    };
                returnValueEntityProperties.Add(item);
            }
        }

        //method, event
        void ProcessParameterAndReturnValueForOneWayAsset(ParameterInfo[] parameters, Type returnType,
            Stack<MemberInfo> memberPath, out List<RemoteAgencyParameterInfo> parameterEntityProperties,
            out List<RemoteAgencyReturnValueInfoBase> returnValueEntityProperties, bool includeCallerOnlyInfo,
            Dictionary<string, ParameterIgnoredAttribute> eventLevelParameterIgnoredAttributes = null,
            Dictionary<string, CustomizedParameterEntityPropertyNameAttribute>
                eventLevelParameterEntityPropertyNameAttributes = null)
        {
            parameterEntityProperties = new List<RemoteAgencyParameterInfo>();
            returnValueEntityProperties = new List<RemoteAgencyReturnValueInfoBase>();

            HashSet<string>
                usedPropertyNamesInParameterEntity =
                    new HashSet<string>(); //could be duplicated cause by the case changing :)

            foreach (var parameter in parameters)
            {
                if (GetValueFromAttribute(parameter, i => i.IsIgnored, out _, eventLevelParameterIgnoredAttributes))
                {
                    continue;
                }
                else if (parameter.IsOut)
                {
                    if (includeCallerOnlyInfo) //one way server won't process return value
                    {
                        RemoteAgencyReturnValueInfoFromParameterDefaultValue item =
                            new RemoteAgencyReturnValueInfoFromParameterDefaultValue()
                            {
                                Parameter = parameter
                            };
                        returnValueEntityProperties.Add(item);
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
                        parameterInfo.SerializerParameterLevelAttributes =
                            parameter.GetCustomAttributes(_serializerParameterLevelAttributeBaseType, true)
                                .Cast<Attribute>().ToList();
                    }

                    var requiredPropertyName =
                        GetValueFromAttribute(parameter, i => i.EntityPropertyName,
                            out var customizedParameterEntityPropertyNameAttribute,
                            eventLevelParameterEntityPropertyNameAttributes);
                    if (!string.IsNullOrEmpty(requiredPropertyName))
                    {
                        if (!usedPropertyNamesInParameterEntity.Add(requiredPropertyName))
                        {
                            throw new EntityPropertyNameConflictException(
                                $"The property name specified conflicts with others in parameter entity.",
                                customizedParameterEntityPropertyNameAttribute, parameter, memberPath);
                        }

                        parameterInfo.PropertyName = requiredPropertyName;
                    }
                    else
                    {
                        parameterInfo.PropertyName = AutoNamePlaceHolder;
                    }

                    parameterEntityProperties.Add(parameterInfo);
                }

            }

            if (includeCallerOnlyInfo) //one way server won't process return value
            {
                if (returnType != typeof(void))
                {
                    RemoteAgencyReturnValueInfoFromReturnValueDefaultValue item =
                        new RemoteAgencyReturnValueInfoFromReturnValueDefaultValue()
                        {
                            ReturnValueDataType = returnType
                        };
                    returnValueEntityProperties.Add(item);
                }
            }

            //assign auto name
            foreach (var parameter in parameterEntityProperties)
            {
                if (parameter.PropertyName == AutoNamePlaceHolder)
                    parameter.PropertyName =
                        GetPropertyAutoName(parameter.Parameter.Name, usedPropertyNamesInParameterEntity);
            }
        }

        //method, event
        void ProcessParameterAndReturnValueForNormalAsset(ParameterInfo[] parameters, Type returnType,
            ICustomAttributeProvider returnTypeCustomAttributes,
            Stack<MemberInfo> memberPath, ICustomAttributeProvider[] returnValueAttributeProviders,
            out List<RemoteAgencyParameterInfo> parameterEntityProperties,
            out List<RemoteAgencyReturnValueInfoBase> returnValueEntityProperties,
            Dictionary<string, ParameterIgnoredAttribute> eventLevelParameterIgnoredAttributes = null,
            Dictionary<string, CustomizedParameterEntityPropertyNameAttribute>
                eventLevelParameterEntityPropertyNameAttributes = null,
            Dictionary<string, ParameterReturnRequiredAttribute> eventLevelParameterReturnRequiredAttributes = null)
        {
            parameterEntityProperties = new List<RemoteAgencyParameterInfo>();
            returnValueEntityProperties = new List<RemoteAgencyReturnValueInfoBase>();

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
                        parameterInfo.SerializerParameterLevelAttributes =
                            parameter.GetCustomAttributes(_serializerParameterLevelAttributeBaseType, true)
                                .Cast<Attribute>().ToList();
                    }

                    var requiredPropertyName =
                        GetValueFromAttribute(parameter, i => i.EntityPropertyName,
                            out var customizedParameterEntityPropertyNameAttribute,
                            eventLevelParameterEntityPropertyNameAttributes);
                    if (!string.IsNullOrEmpty(requiredPropertyName))
                    {
                        if (!usedPropertyNamesInParameterEntity.Add(requiredPropertyName))
                        {
                            throw new EntityPropertyNameConflictException(
                                "The property name specified conflicts with others in parameter entity.",
                                customizedParameterEntityPropertyNameAttribute, parameter, memberPath);
                        }

                        parameterInfo.PropertyName = requiredPropertyName;
                    }
                    else
                    {
                        parameterInfo.PropertyName = AutoNamePlaceHolder;
                    }

                    parameterEntityProperties.Add(parameterInfo);
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

                        returnValueEntityProperties.Add(returnProperty);
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

                                    returnValueEntityProperties.Add(returnProperty);

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

                                        returnValueEntityProperties.Add(returnProperty);

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

                                        returnValueEntityProperties.Add(returnProperty);
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
            bool isReturnValueIgnored = false;
            foreach (var returnValueAttributeProvider in returnValueAttributeProviders)
            {
                isReturnValueIgnored =
                    GetValueFromAttribute<ReturnIgnoredAttribute, bool>(returnValueAttributeProvider, i => i.IsIgnored,
                        out _);
                if (isReturnValueIgnored)
                    break;
            }

            if (!isReturnValueIgnored)
            {
                CustomizedReturnValueEntityPropertyNameAttribute
                    customizedReturnValueEntityPropertyNameAttribute = null;
                foreach (var returnValueAttributeProvider in returnValueAttributeProviders)
                {
                    customizedReturnValueEntityPropertyNameAttribute =
                        GetValueFromAttribute<CustomizedReturnValueEntityPropertyNameAttribute,
                            CustomizedReturnValueEntityPropertyNameAttribute>(
                            returnValueAttributeProvider, i => i, out _);
                    if (customizedReturnValueEntityPropertyNameAttribute != null)
                        break;
                }

                var returnValuePropertyName = customizedReturnValueEntityPropertyNameAttribute?.EntityPropertyName;

                if (!string.IsNullOrEmpty(returnValuePropertyName))
                {
                    if (!usedPropertyNamesInReturnValueEntity.Add(returnValuePropertyName))
                    {
                        throw new EntityPropertyNameConflictException(
                            "The property name specified conflicts with others in return value entity.",
                            customizedReturnValueEntityPropertyNameAttribute,
                            EntityPropertyNameConflictExceptionCausedMemberType.ReturnValue, memberPath);
                    }
                }
                else
                {
                    returnValuePropertyName = AutoNamePlaceHolder;
                }

                if (returnType != typeof(void))
                {
                    RemoteAgencyReturnValueInfoFromReturnValue item = new RemoteAgencyReturnValueInfoFromReturnValue()
                    {
                        PropertyName = returnValuePropertyName,
                        ReturnValueDataType = returnType
                    };
                    if (_serializerParameterLevelAttributeBaseType != null)
                    {
                        item.SerializerParameterLevelAttributesOnReturnValue =
                            returnTypeCustomAttributes
                                .GetCustomAttributes(_serializerParameterLevelAttributeBaseType, true).Cast<Attribute>()
                                .ToList();
                    }

                    returnValueEntityProperties.Add(item);
                }
                else
                {
                    throw new InvalidReturnValueAttributeDataException(
                        $"{nameof(CustomizedReturnValueEntityPropertyNameAttribute)} can only be used on the asset with return value.",
                        customizedReturnValueEntityPropertyNameAttribute, memberPath);
                }
            }

            //assign auto name
            foreach (var parameter in parameterEntityProperties)
            {
                if (parameter.PropertyName == AutoNamePlaceHolder)
                    parameter.PropertyName =
                        GetPropertyAutoName(parameter.Parameter.Name, usedPropertyNamesInParameterEntity);
            }

            foreach (var returnValue in returnValueEntityProperties)
            {
                if (returnValue.IsIncludedInEntity && returnValue.PropertyName == AutoNamePlaceHolder)
                {
                    returnValue.PropertyName =
                        GetPropertyAutoName(returnValue.GetDefaultPropertyName(), usedPropertyNamesInReturnValueEntity);
                }
            }
        }

        //property
        void ProcessSettingResponseForIgnoredProperty(Type dataType,
            out List<RemoteAgencyReturnValueInfoBase> responseEntityProperties)
        {
            responseEntityProperties = new List<RemoteAgencyReturnValueInfoBase>()
            {
                new RemoteAgencyReturnValueInfoFromReturnValueDefaultValue()
                {
                    ReturnValueDataType = dataType
                }
            };
        }

        //property
        void ProcessGettingResponseForOneWayProperty(Type dataType,
            out RemoteAgencyReturnValueInfoBase responseEntityProperty)
        {
            if (_includesProxyOnlyInfo)
            {
                RemoteAgencyReturnValueInfoFromAssetPropertyReturnValueDefaultValue item =
                    new RemoteAgencyReturnValueInfoFromAssetPropertyReturnValueDefaultValue()
                    {
                        ReturnValueDataType = dataType
                    };

                responseEntityProperty = item;
            }
            else
            {
                responseEntityProperty = null;
            }
        }

        //property
        void ProcessGettingResponseForNormalProperty(PropertyInfo property,
            List<Attribute> serializerParameterLevelAttributesOnAsset,
            out RemoteAgencyReturnValueInfoBase responseEntityProperty)
        {
            var name = GetValueFromAttribute<CustomizedPropertyGetResponsePropertyNameAttribute, string>(property,
                i => i.EntityPropertyName, out _, "Value");

            RemoteAgencyReturnValueInfoFromAssetPropertyReturnValue item =
                new RemoteAgencyReturnValueInfoFromAssetPropertyReturnValue()
                {
                    PropertyName = name,
                    ReturnValueDataType = property.PropertyType,
                    SerializerParameterLevelAttributesOnAsset = serializerParameterLevelAttributesOnAsset
                };
            responseEntityProperty = item;
        }


        //property
        void ProcessSettingResponseForNormalProperty(PropertyInfo assetProperty,
            List<Attribute> serializerParameterLevelAttributesOnAsset, Stack<MemberInfo> memberPath,
            out List<RemoteAgencyReturnValueInfoBase> responseEntityProperties)
        {
            responseEntityProperties = new List<RemoteAgencyReturnValueInfoBase>();

            //ParameterReturnRequiredPropertyAttribute
            var returnRequiredPropertyAttributes = assetProperty.GetCustomAttributes<ParameterReturnRequiredPropertyAttribute>().ToArray();
            if (returnRequiredPropertyAttributes.Length > 0)
            {
                HashSet<string> usedPropertyNamesInReturnValueEntity = new HashSet<string>();
                HashSet<string> processedProperties = new HashSet<string>();
                HashSet<Type> processedHelpers = new HashSet<Type>();

                foreach (var returnRequiredPropertyAttribute in returnRequiredPropertyAttributes)
                {
                    if (returnRequiredPropertyAttribute.IsSimpleMode)
                    {
                        if (!processedProperties.Add(returnRequiredPropertyAttribute.PropertyNameInParameter))
                            continue;

                        var dataType = assetProperty.PropertyType;

                        var field = dataType.GetField(returnRequiredPropertyAttribute.PropertyNameInParameter);
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
                                        returnRequiredPropertyAttribute,
                                        EntityPropertyNameConflictExceptionCausedMemberType.Property, memberPath);
                                }
                            }
                            else
                            {
                                name = AutoNamePlaceHolder;
                            }

                            RemoteAgencyReturnValueInfoFromAssetPropertyValueField returnProperty =
                                new RemoteAgencyReturnValueInfoFromAssetPropertyValueField()
                                {
                                    PropertyName = name,
                                    IsIncludedWhenExceptionThrown =
                                        returnRequiredPropertyAttribute.IsIncludedWhenExceptionThrown,
                                    ParameterField = field,
                                    SerializerParameterLevelAttributesOnAsset = serializerParameterLevelAttributesOnAsset
                                };
                            if (_serializerParameterLevelAttributeBaseType != null)
                            {
                                returnProperty.SerializerParameterLevelAttributesOnField =
                                    field.GetCustomAttributes(_serializerParameterLevelAttributeBaseType, true)
                                        .Cast<Attribute>().ToList();
                            }

                            responseEntityProperties.Add(returnProperty);

                            #endregion
                        }
                        else
                        {
                            var property =
                                dataType.GetProperty(returnRequiredPropertyAttribute.PropertyNameInParameter);
                            if (property != null)
                            {
                                #region Simple mode on property

                                if (property.GetGetMethod() == null)
                                    throw new InvalidAttributeDataException(
                                        "The property name specified is not readable publicly.",
                                        returnRequiredPropertyAttribute, memberPath);
                                if (property.GetSetMethod() == null)
                                    throw new InvalidAttributeDataException(
                                        "The property name specified is not writable publicly.",
                                        returnRequiredPropertyAttribute, memberPath);

                                string name = returnRequiredPropertyAttribute.ResponseEntityPropertyName;
                                if (!string.IsNullOrEmpty(name))
                                {
                                    if (!usedPropertyNamesInReturnValueEntity.Add(name))
                                    {
                                        throw new EntityPropertyNameConflictException(
                                            "The property name specified conflicts with others in return value entity.",
                                            returnRequiredPropertyAttribute, EntityPropertyNameConflictExceptionCausedMemberType.Property, memberPath);
                                    }
                                }
                                else
                                {
                                    name = AutoNamePlaceHolder;
                                }

                                RemoteAgencyReturnValueInfoFromAssetPropertyValueProperty returnProperty =
                                    new RemoteAgencyReturnValueInfoFromAssetPropertyValueProperty()
                                    {
                                        PropertyName = name,
                                        IsIncludedWhenExceptionThrown =
                                            returnRequiredPropertyAttribute.IsIncludedWhenExceptionThrown,
                                        ParameterProperty = property,
                                        SerializerParameterLevelAttributesOnAsset = serializerParameterLevelAttributesOnAsset
                                    };
                                if (_serializerParameterLevelAttributeBaseType != null)
                                {
                                    returnProperty.SerializerParameterLevelAttributesOnProperty =
                                        property.GetCustomAttributes(_serializerParameterLevelAttributeBaseType, true)
                                            .Cast<Attribute>().ToList();
                                }

                                responseEntityProperties.Add(returnProperty);

                                #endregion
                            }
                            else
                            {
                                throw new InvalidAttributeDataException(
                                    "The property name specified is not a public property, nor a public field.",
                                    returnRequiredPropertyAttribute, memberPath);
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
                                    throw new InvalidAttributeDataException(
                                        $"The property {propertyInHelperClass.Name} in helper class {returnRequiredPropertyAttribute.HelperClass.FullName} is not readable publicly.",
                                        returnRequiredPropertyAttribute, memberPath);
                                if (propertyInHelperClass.GetSetMethod() == null)
                                    throw new InvalidAttributeDataException(
                                        $"The property {propertyInHelperClass.Name} in helper class {returnRequiredPropertyAttribute.HelperClass.FullName} is not readable publicly.",
                                        returnRequiredPropertyAttribute, memberPath);

                                string name = returnRequiredHelperAttribute.ResponseEntityPropertyName;
                                if (!string.IsNullOrEmpty(name))
                                {
                                    if (!usedPropertyNamesInReturnValueEntity.Add(name))
                                    {
                                        throw new EntityPropertyNameConflictException(
                                            $"The property name specified for property {propertyInHelperClass.Name} in helper class {returnRequiredPropertyAttribute.HelperClass.FullName} conflicts with others in return value entity.",
                                            returnRequiredPropertyAttribute, EntityPropertyNameConflictExceptionCausedMemberType.Property, memberPath);
                                    }
                                }
                                else
                                {
                                    name = AutoNamePlaceHolder;
                                }

                                RemoteAgencyReturnValueInfoFromAssetPropertyValueHelperProperty returnProperty =
                                    new RemoteAgencyReturnValueInfoFromAssetPropertyValueHelperProperty()
                                    {
                                        PropertyName = name,
                                        IsIncludedWhenExceptionThrown =
                                            returnRequiredHelperAttribute.IsIncludedWhenExceptionThrown,
                                        ParameterHelperClass = returnRequiredPropertyAttribute.HelperClass,
                                        ParameterHelperProperty = propertyInHelperClass,
                                        SerializerParameterLevelAttributesOnAsset = serializerParameterLevelAttributesOnAsset
                                    };
                                if (_serializerParameterLevelAttributeBaseType != null)
                                {
                                    returnProperty.SerializerParameterLevelAttributesOnHelperProperty =
                                        propertyInHelperClass
                                            .GetCustomAttributes(_serializerParameterLevelAttributeBaseType, true)
                                            .Cast<Attribute>().ToList();
                                }

                                responseEntityProperties.Add(returnProperty);
                            }
                        }

                        #endregion
                    }
                }
            }
        }
    }
}