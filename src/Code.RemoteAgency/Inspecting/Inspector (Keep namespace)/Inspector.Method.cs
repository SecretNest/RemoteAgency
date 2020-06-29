using SecretNest.RemoteAgency.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SecretNest.RemoteAgency.Inspecting
{
    partial class Inspector
    {
        void ProcessMethod(RemoteAgencyMethodInfo method, TypeInfo @interface,
            LocalExceptionHandlingMode interfaceLevelLocalExceptionHandlingMode,
            int interfaceLevelMethodCallingTimeout)
        {
            Stack<MemberInfo> parentPath = new Stack<MemberInfo>();
            parentPath.Push(@interface);

            MethodInfo methodInfo = (MethodInfo)method.Asset;
            method.LocalExceptionHandlingMode =
                GetValueFromAttribute<LocalExceptionHandlingAttribute, LocalExceptionHandlingMode>(methodInfo,
                    i => i.LocalExceptionHandlingMode, out _, interfaceLevelLocalExceptionHandlingMode);

            method.MethodCallingTimeout =
                GetValueFromAttribute<OperatingTimeoutTimeAttribute, int>(methodInfo,
                    i => i.Timeout, out _, interfaceLevelMethodCallingTimeout);

            //generic parameter
            method.AssetLevelGenericArguments = ProcessGenericArgument(method.Asset, methodInfo.GetGenericArguments(), parentPath);

            //NOTE: at this point, method is pushed into parentPath.
            parentPath.Push(method.Asset);

            //asset level pass through attributes
            method.AssetLevelPassThroughAttributes = GetAttributePassThrough(methodInfo,
                (m, a) => new InvalidAttributeDataException(m, a, parentPath));
            method.ReturnValuePassThroughAttributes = GetAttributePassThrough(methodInfo.ReturnTypeCustomAttributes,
                (m, a) => new InvalidReturnValueAttributeDataException(m, a, parentPath));

            if (method.IsIgnored)
            {
                if (_includesProxyOnlyInfo)
                {
                    if (method.WillThrowExceptionWhileCalling)
                    {
                        ProcessParameterForIgnoredAndThrowExceptionAsset(methodInfo.GetParameters(),
                            parentPath, out var parameters);
                        method.ParameterEntityProperties = parameters;
                        method.ReturnValueEntityProperties = new List<RemoteAgencyReturnValueInfoBase>();
                    }
                    else
                    {
                        ProcessParameterAndReturnValueForIgnoredAsset(methodInfo.GetParameters(), methodInfo.ReturnType,
                            parentPath, out var parameters, out var returnValues);
                        method.ParameterEntityProperties = parameters;
                        method.ReturnValueEntityProperties = returnValues;
                    }
                }
                else
                {
                    method.ParameterEntityProperties = new List<RemoteAgencyParameterInfo>();
                    method.ReturnValueEntityProperties = new List<RemoteAgencyReturnValueInfoBase>();
                }
            }
            else if (method.IsOneWay)
            {
                ProcessParameterAndReturnValueForOneWayAsset(methodInfo.GetParameters(), methodInfo.ReturnType,
                    parentPath, out var parameters, out var returnValues);
                method.ParameterEntityProperties = parameters;
                method.ReturnValueEntityProperties = returnValues;
            }
            else
            {
                //normal
                ProcessParameterAndReturnValueForNormalAsset(methodInfo.GetParameters(), methodInfo.ReturnType,
                    parentPath, methodInfo.ReturnTypeCustomAttributes, methodInfo, out var parameters,
                    out var returnValues);
                method.ParameterEntityProperties = parameters;
                method.ReturnValueEntityProperties = returnValues;
            }
        }

        void ProcessParameterAndReturnValueForIgnoredAsset(ParameterInfo[] parameters, Type returnType,
            Stack<MemberInfo> memberPath, out List<RemoteAgencyParameterInfo> parameterEntityProperties,
            out List<RemoteAgencyReturnValueInfoBase> returnValueEntityProperties)
        {
            parameterEntityProperties = new List<RemoteAgencyParameterInfo>();
            returnValueEntityProperties = new List<RemoteAgencyReturnValueInfoBase>();

            foreach (var parameter in parameters)
            {
                var parameterInfo = new RemoteAgencyParameterInfo
                {
                    Parameter = parameter,
                    PassThroughAttributes = GetAttributePassThrough(parameter,
                        (m, a) => new InvalidParameterAttributeDataException(m, a, parameter, memberPath))
                };

                parameterEntityProperties.Add(parameterInfo);

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

        void ProcessParameterForIgnoredAndThrowExceptionAsset(ParameterInfo[] parameters,
            Stack<MemberInfo> memberPath, out List<RemoteAgencyParameterInfo> parameterEntityProperties)
        {
            parameterEntityProperties = new List<RemoteAgencyParameterInfo>();

            foreach (var parameter in parameters)
            {
                var parameterInfo = new RemoteAgencyParameterInfo
                {
                    Parameter = parameter,
                    PassThroughAttributes = GetAttributePassThrough(parameter, (m, a) => new InvalidParameterAttributeDataException(m, a, parameter, memberPath))
                };

                parameterEntityProperties.Add(parameterInfo);
            }
        }


        void ProcessParameterAndReturnValueForOneWayAsset(ParameterInfo[] parameters, Type returnType,
            Stack<MemberInfo> memberPath, out List<RemoteAgencyParameterInfo> parameterEntityProperties,
            out List<RemoteAgencyReturnValueInfoBase> returnValueEntityProperties)
        {
            parameterEntityProperties = new List<RemoteAgencyParameterInfo>();
            returnValueEntityProperties = new List<RemoteAgencyReturnValueInfoBase>();

            HashSet<string> usedPropertyNamesInParameterEntity = new HashSet<string>(); //could be duplicated cause by the case changing :)

            foreach (var parameter in parameters)
            {
                var parameterInfo = new RemoteAgencyParameterInfo
                {
                    Parameter = parameter,
                    PassThroughAttributes = GetAttributePassThrough(parameter, (m, a) => new InvalidParameterAttributeDataException(m, a, parameter, memberPath))
                };

                if (GetValueFromAttribute<ParameterIgnoredAttribute, bool>(parameter, i => i.IsIgnored, out _))
                {
                    //ignored
                }
                else if (parameter.IsOut)
                {
                    if (_includesProxyOnlyInfo) //one way server won't process return value
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
                    var requiredPropertyName =
                        GetValueFromAttribute<CustomizedParameterEntityPropertyNameAttribute, string>(parameter,
                            i => i.EntityPropertyName, out var customizedParameterEntityPropertyNameAttribute);
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
                }

                parameterEntityProperties.Add(parameterInfo);
            }

            if (_includesProxyOnlyInfo) //one way server won't process return value
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

        void ProcessParameterAndReturnValueForNormalAsset(ParameterInfo[] parameters, Type returnType,
            Stack<MemberInfo> memberPath, ICustomAttributeProvider returnValueAttributeProviderHighPriority,
            ICustomAttributeProvider returnValueAttributeProvider, out List<RemoteAgencyParameterInfo> parameterEntityProperties,
            out List<RemoteAgencyReturnValueInfoBase> returnValueEntityProperties)
        {
            parameterEntityProperties = new List<RemoteAgencyParameterInfo>();
            returnValueEntityProperties = new List<RemoteAgencyReturnValueInfoBase>();

            HashSet<string> usedPropertyNamesInParameterEntity = new HashSet<string>(); //could be duplicated cause by the case changing :)
            HashSet<string> usedPropertyNamesInReturnValueEntity = new HashSet<string>();

            foreach (var parameter in parameters)
            {
                var parameterInfo = new RemoteAgencyParameterInfo
                {
                    Parameter = parameter,
                    PassThroughAttributes = GetAttributePassThrough(parameter, (m, a) => new InvalidParameterAttributeDataException(m, a, parameter, memberPath))
                };

                if (parameter.IsOut || GetValueFromAttribute<ParameterIgnoredAttribute, bool>(parameter, i => i.IsIgnored, out _))
                {
                    //ignored in parameter entity
                }
                else
                {
                    var requiredPropertyName =
                        GetValueFromAttribute<CustomizedParameterEntityPropertyNameAttribute, string>(parameter,
                            i => i.EntityPropertyName, out var customizedParameterEntityPropertyNameAttribute);
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
                }

                parameterEntityProperties.Add(parameterInfo);

                if (parameter.IsOut || parameter.ParameterType.IsByRef)
                {
                    #region Out/Ref
                    var isTwoWay = GetValueFromAttribute<ParameterTwoWayAttribute, bool>(parameter, i => i.IsTwoWay,
                        out var twoWayAttribute, true); //default value is true
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
            if (!GetValueFromAttribute<ReturnIgnoredAttribute, bool>(returnValueAttributeProviderHighPriority, i => i.IsIgnored,
                    out _) &&
                !GetValueFromAttribute<ReturnIgnoredAttribute, bool>(returnValueAttributeProvider, i => i.IsIgnored, out _))
            {
                var returnValuePropertyName = GetValueFromAttribute<CustomizedReturnValueEntityPropertyNameAttribute, string>(returnValueAttributeProviderHighPriority,
                    i => i.EntityPropertyName, out var customizedReturnValueEntityPropertyNameAttribute);
                if (customizedReturnValueEntityPropertyNameAttribute == null)
                    returnValuePropertyName = GetValueFromAttribute(returnValueAttributeProvider, i => i.EntityPropertyName,
                        out customizedReturnValueEntityPropertyNameAttribute);

                if (returnType != typeof(void))
                {
                    RemoteAgencyReturnValueInfoFromReturnValue item = new RemoteAgencyReturnValueInfoFromReturnValue()
                    {
                        PropertyName = returnValuePropertyName,
                        ReturnValueDataType = returnType
                    };
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