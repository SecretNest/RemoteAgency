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

        void ProcessParameterAndReturnValueForOneWayAsset(ParameterInfo[] parameters, Type returnType,
            Stack<MemberInfo> memberPath, out List<RemoteAgencyParameterInfo> parameterEntityProperties,
            out List<RemoteAgencyReturnValueInfoBase> returnValueEntityProperties, bool includeCallerOnlyInfo,
            Dictionary<string, ParameterIgnoredAttribute> eventLevelParameterIgnoredAttributes = null,
            Dictionary<string, CustomizedParameterEntityPropertyNameAttribute> eventLevelParameterEntityPropertyNameAttributes = null)
        {
            parameterEntityProperties = new List<RemoteAgencyParameterInfo>();
            returnValueEntityProperties = new List<RemoteAgencyReturnValueInfoBase>();

            HashSet<string> usedPropertyNamesInParameterEntity = new HashSet<string>(); //could be duplicated cause by the case changing :)

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
                            parameter.GetCustomAttributes(_serializerParameterLevelAttributeBaseType, true).Cast<Attribute>().ToList();
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
        void ProcessParameterAndReturnValueForNormalAsset(ParameterInfo[] parameters, Type returnType, ICustomAttributeProvider returnTypeCustomAttributes,
            Stack<MemberInfo> memberPath, ICustomAttributeProvider[] returnValueAttributeProviders,
            out List<RemoteAgencyParameterInfo> parameterEntityProperties,
            out List<RemoteAgencyReturnValueInfoBase> returnValueEntityProperties,
            Dictionary<string, ParameterIgnoredAttribute> eventLevelParameterIgnoredAttributes = null,
            Dictionary<string, CustomizedParameterEntityPropertyNameAttribute> eventLevelParameterEntityPropertyNameAttributes = null,
            Dictionary<string, ParameterTwoWayAttribute> eventLevelParameterTwoWayAttributes = null)
        {
            parameterEntityProperties = new List<RemoteAgencyParameterInfo>();
            returnValueEntityProperties = new List<RemoteAgencyReturnValueInfoBase>();

            HashSet<string> usedPropertyNamesInParameterEntity = new HashSet<string>(); //could be duplicated cause by the case changing :)
            HashSet<string> usedPropertyNamesInReturnValueEntity = new HashSet<string>();

            foreach (var parameter in parameters)
            {
                if (parameter.IsOut || GetValueFromAttribute(parameter, i => i.IsIgnored, out _, eventLevelParameterIgnoredAttributes))
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
                            parameter.GetCustomAttributes(_serializerParameterLevelAttributeBaseType, true).Cast<Attribute>().ToList();
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


                if (parameter.IsOut || parameter.ParameterType.IsByRef)
                {
                    #region Out/Ref

                    var isTwoWay = GetValueFromAttribute(parameter, i => i.IsTwoWay, out var twoWayAttribute,
                        eventLevelParameterTwoWayAttributes, true); //default value is true
                    if (isTwoWay)
                    {
                        string name = twoWayAttribute?.ResponseEntityPropertyName;
                        if (!string.IsNullOrEmpty(name))
                        {
                            if (!usedPropertyNamesInReturnValueEntity.Add(name))
                            {
                                throw new EntityPropertyNameConflictException(
                                    $"The property name specified conflicts with others in return value entity.",
                                    twoWayAttribute, parameter, memberPath);
                            }
                        }
                        else
                        {
                            name = AutoNamePlaceHolder;
                        }

                        RemoteAgencyReturnValueInfoFromParameter returnProperty = new RemoteAgencyReturnValueInfoFromParameter()
                        {
                            PropertyName = name,
                            IsIncludedWhenExceptionThrown = twoWayAttribute?.IsIncludedWhenExceptionThrown ?? false,
                            Parameter = parameter
                        };
                        if (_serializerParameterLevelAttributeBaseType != null)
                        {
                            returnProperty.SerializerParameterLevelAttributes =
                                parameter.GetCustomAttributes(_serializerParameterLevelAttributeBaseType, true).Cast<Attribute>().ToList();
                        }
                        returnValueEntityProperties.Add(returnProperty);
                    }

                    var twoWayPropertyAttribute = parameter.GetCustomAttributes<ParameterTwoWayPropertyAttribute>().FirstOrDefault();

                    if (twoWayPropertyAttribute != null)
                        throw new InvalidParameterAttributeDataException(
                            $"{nameof(ParameterTwoWayPropertyAttribute)} can only be used on the parameter without \"ref / ByRef\" and \"out / Out\"",
                            twoWayPropertyAttribute, parameter, memberPath);
                    #endregion
                }
                else
                {
                    #region Two way propery
                    var twoWayAttribute = parameter.GetCustomAttribute<ParameterTwoWayAttribute>();

                    if (twoWayAttribute != null)
                        throw new InvalidParameterAttributeDataException(
                            $"{nameof(ParameterTwoWayAttribute)} can only be used on the parameter with \"ref / ByRef\" and \"out / Out\".",
                            twoWayAttribute, parameter, memberPath);

                    var twoWayPropertyAttributes = parameter.GetCustomAttributes<ParameterTwoWayPropertyAttribute>().ToArray();
                    if (twoWayPropertyAttributes.Length > 0)
                    {
                        HashSet<string> processedProperties = new HashSet<string>();
                        HashSet<Type> processedHelpers = new HashSet<Type>();

                        foreach (var twoWayPropertyAttribute in twoWayPropertyAttributes)
                        {
                            if (twoWayPropertyAttribute.IsSimpleMode)
                            {
                                if (!processedProperties.Add(twoWayPropertyAttribute.PropertyNameInParameter))
                                    continue;

                                var parameterType = parameter.ParameterType;

                                var field = parameterType.GetField(twoWayPropertyAttribute.PropertyNameInParameter);
                                if (field != null)
                                {
                                    #region Simple mode on field

                                    string name = twoWayPropertyAttribute.ResponseEntityPropertyName;
                                    if (!string.IsNullOrEmpty(name))
                                    {
                                        if (!usedPropertyNamesInReturnValueEntity.Add(name))
                                        {
                                            throw new EntityPropertyNameConflictException(
                                                "The property name specified conflicts with others in return value entity.",
                                                twoWayPropertyAttribute, parameter, memberPath);
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
                                            IsIncludedWhenExceptionThrown = twoWayPropertyAttribute.IsIncludedWhenExceptionThrown,
                                            ParameterField = field,
                                            Parameter = parameter
                                        };
                                    if (_serializerParameterLevelAttributeBaseType != null)
                                    {
                                        returnProperty.SerializerParameterLevelAttributes =
                                            parameter.GetCustomAttributes(_serializerParameterLevelAttributeBaseType, true).Cast<Attribute>().ToList();
                                        returnProperty.SerializerParameterLevelAttributesOnField =
                                            field.GetCustomAttributes(_serializerParameterLevelAttributeBaseType, true).Cast<Attribute>().ToList();
                                    }
                                    returnValueEntityProperties.Add(returnProperty);
                                    #endregion
                                }
                                else
                                {
                                    var property =
                                        parameterType.GetProperty(twoWayPropertyAttribute.PropertyNameInParameter);
                                    if (property != null)
                                    {
                                        #region Simple mode on property
                                        if (!property.CanRead)
                                            throw new InvalidParameterAttributeDataException(
                                                "The property name specified is not readable.",
                                                twoWayPropertyAttribute, parameter, memberPath);
                                        if (!property.CanWrite)
                                            throw new InvalidParameterAttributeDataException(
                                                "The property name specified is not writable.",
                                                twoWayPropertyAttribute, parameter, memberPath);

                                        string name = twoWayPropertyAttribute.ResponseEntityPropertyName;
                                        if (!string.IsNullOrEmpty(name))
                                        {
                                            if (!usedPropertyNamesInReturnValueEntity.Add(name))
                                            {
                                                throw new EntityPropertyNameConflictException(
                                                    "The property name specified conflicts with others in return value entity.",
                                                    twoWayPropertyAttribute, parameter, memberPath);
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
                                                IsIncludedWhenExceptionThrown = twoWayPropertyAttribute.IsIncludedWhenExceptionThrown,
                                                ParameterProperty = property,
                                                Parameter = parameter
                                            };
                                        if (_serializerParameterLevelAttributeBaseType != null)
                                        {
                                            returnProperty.SerializerParameterLevelAttributes =
                                                parameter.GetCustomAttributes(_serializerParameterLevelAttributeBaseType, true).Cast<Attribute>().ToList();
                                            returnProperty.SerializerParameterLevelAttributesOnProperty =
                                                property.GetCustomAttributes(_serializerParameterLevelAttributeBaseType, true).Cast<Attribute>().ToList();
                                        }
                                        returnValueEntityProperties.Add(returnProperty);
                                        #endregion
                                    }
                                    else
                                    {
                                        throw new InvalidParameterAttributeDataException(
                                            "The property name specified is not a public property, nor a public field.",
                                            twoWayPropertyAttribute, parameter, memberPath);
                                    }
                                }
                            }
                            else
                            {
                                #region Helper class mode
                                if (!processedHelpers.Add(twoWayPropertyAttribute.HelperClass))
                                    continue;

                                var propertiesInHelperClass = twoWayPropertyAttribute.HelperClass.GetProperties();
                                foreach (var propertyInHelperClass in propertiesInHelperClass)
                                {
                                    if (GetValueFromAttribute<TwoWayHelperAttribute, bool>(propertyInHelperClass,
                                        i => i.IsTwoWay, out var twoWayHelperAttribute))
                                    {
                                        if (!propertyInHelperClass.CanRead)
                                            throw new InvalidParameterAttributeDataException(
                                                $"The property {propertyInHelperClass.Name} in helper class {twoWayPropertyAttribute.HelperClass.FullName} is not readable.",
                                                twoWayPropertyAttribute, parameter, memberPath);
                                        if (!propertyInHelperClass.CanWrite)
                                            throw new InvalidParameterAttributeDataException(
                                                $"The property {propertyInHelperClass.Name} in helper class {twoWayPropertyAttribute.HelperClass.FullName} is not readable.",
                                                twoWayPropertyAttribute, parameter, memberPath);

                                        string name = twoWayHelperAttribute.ResponseEntityPropertyName;
                                        if (!string.IsNullOrEmpty(name))
                                        {
                                            if (!usedPropertyNamesInReturnValueEntity.Add(name))
                                            {
                                                throw new EntityPropertyNameConflictException(
                                                    $"The property name specified for property {propertyInHelperClass.Name} in helper class {twoWayPropertyAttribute.HelperClass.FullName} conflicts with others in return value entity.",
                                                    twoWayPropertyAttribute, parameter, memberPath);
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
                                                IsIncludedWhenExceptionThrown = twoWayHelperAttribute.IsIncludedWhenExceptionThrown,
                                                ParameterHelperClass = twoWayPropertyAttribute.HelperClass,
                                                ParameterHelperProperty = propertyInHelperClass,
                                                Parameter = parameter
                                            };
                                        if (_serializerParameterLevelAttributeBaseType != null)
                                        {
                                            returnProperty.SerializerParameterLevelAttributes =
                                                parameter.GetCustomAttributes(_serializerParameterLevelAttributeBaseType, true).Cast<Attribute>().ToList();
                                            returnProperty.SerializerParameterLevelAttributesOnHelperProperty =
                                                propertyInHelperClass.GetCustomAttributes(_serializerParameterLevelAttributeBaseType, true).Cast<Attribute>().ToList();
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
                CustomizedReturnValueEntityPropertyNameAttribute customizedReturnValueEntityPropertyNameAttribute = null;
                foreach (var returnValueAttributeProvider in returnValueAttributeProviders)
                {
                    customizedReturnValueEntityPropertyNameAttribute = GetValueFromAttribute<CustomizedReturnValueEntityPropertyNameAttribute, CustomizedReturnValueEntityPropertyNameAttribute>(
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
                            customizedReturnValueEntityPropertyNameAttribute, memberPath);
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
                            returnTypeCustomAttributes.GetCustomAttributes(_serializerParameterLevelAttributeBaseType, true).Cast<Attribute>().ToList();
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
    }
}