using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

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

            MethodInfo methodInfo = (MethodInfo) method.Asset;
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
                        method.ParameterEntityProperties =
                            ProcessParameterForIgnoredAndThrowExceptionAsset(methodInfo.GetParameters(), parentPath);
                        method.ReturnValueEntityProperties =
                            ProcessReturnValueForIgnoredAndThrowExceptionAsset();
                    }
                    else
                    {
                        method.ParameterEntityProperties = ProcessParameterForIgnoredAsset(methodInfo.GetParameters(),
                            parentPath, out var outputParameterNamesForSettingDefault);
                        method.ReturnValueEntityProperties =
                            ProcessReturnValueForIgnoredOrOneWayAsset(methodInfo, outputParameterNamesForSettingDefault);
                    }
                }
                else
                {
                    method.ParameterEntityProperties = new List<RemoteAgencyParameterInfo>();
                    method.ReturnValueEntityProperties = new List<RemoteAgencyReturnValueInfo>();
                }
            }
            else if (method.IsOneWay)
            {
                method.ParameterEntityProperties = ProcessParameterForOneWayAsset(methodInfo.GetParameters(),
                    parentPath, out var outputParameterNamesForSettingDefault);
                if (_includesProxyOnlyInfo)
                {
                    method.ReturnValueEntityProperties = ProcessReturnValueForIgnoredOrOneWayAsset(methodInfo,
                        outputParameterNamesForSettingDefault);
                }
                else
                {
                    method.ReturnValueEntityProperties = new List<RemoteAgencyReturnValueInfo>();
                }
            }
            else
            {
                //normal
                method.ParameterEntityProperties = ProcessParameter(methodInfo.GetParameters(), parentPath,
                    out var outputParameters, out var usedPropertyNamesInReturnValue);
                method.ReturnValueEntityProperties = ProcessReturnValue(methodInfo, parentPath, outputParameters,
                    usedPropertyNamesInReturnValue);
            }
        }

        List<RemoteAgencyParameterInfo> ProcessParameterForIgnoredAsset(ParameterInfo[] parameters, Stack<MemberInfo> memberPath,
            out List<Tuple<Type, string>> outputParameterNamesForSettingDefault)
        {
            List<RemoteAgencyParameterInfo> result = new List<RemoteAgencyParameterInfo>();
            outputParameterNamesForSettingDefault = new List<Tuple<Type, string>>();

            foreach (var parameter in parameters)
            {
                var parameterInfo = new RemoteAgencyParameterInfo
                {
                    Parameter = parameter,
                    PassThroughAttributes = GetAttributePassThrough(parameter, (m, a) => new InvalidParameterAttributeDataException(m, a, parameter, memberPath))
                };

                result.Add(parameterInfo);

                if (parameter.IsOut)
                {
                    outputParameterNamesForSettingDefault.Add(new Tuple<Type, string>(parameter.ParameterType, parameter.Name));
                }
            }

            return result;
        }

        List<RemoteAgencyParameterInfo> ProcessParameterForIgnoredAndThrowExceptionAsset(ParameterInfo[] parameters, Stack<MemberInfo> memberPath)
        {
            List<RemoteAgencyParameterInfo> result = new List<RemoteAgencyParameterInfo>();

            foreach (var parameter in parameters)
            {
                var parameterInfo = new RemoteAgencyParameterInfo
                {
                    Parameter = parameter,
                    PassThroughAttributes = GetAttributePassThrough(parameter, (m, a) => new InvalidParameterAttributeDataException(m, a, parameter, memberPath))
                };

                result.Add(parameterInfo);
            }

            return result;
        }
        
        List<RemoteAgencyParameterInfo> ProcessParameterForOneWayAsset(ParameterInfo[] parameters, Stack<MemberInfo> memberPath,
            out List<Tuple<Type, string>> outputParameterNamesForSettingDefault)
        {
            List<RemoteAgencyParameterInfo> result = new List<RemoteAgencyParameterInfo>();
            outputParameterNamesForSettingDefault = new List<Tuple<Type, string>>();

            HashSet<string> usedPropertyNamesInParameterEntity = new HashSet<string>(); //could be duplicated cause by the case changing :)
            HashSet<string> ignoredParameters= new HashSet<string>();

            foreach (var parameter in parameters)
            {
                var parameterInfo = new RemoteAgencyParameterInfo
                {
                    Parameter = parameter,
                    PassThroughAttributes = GetAttributePassThrough(parameter, (m, a) => new InvalidParameterAttributeDataException(m, a, parameter, memberPath))
                };

                if (GetValueFromAttribute<ParameterIgnoredAttribute, bool>(parameter, i => i.IsIgnored, out _))
                {
                    ignoredParameters.Add(parameter.Name);
                }
                else if (parameter.IsOut)
                {
                    ignoredParameters.Add(parameter.Name);
                    outputParameterNamesForSettingDefault.Add(new Tuple<Type, string>(parameter.ParameterType, parameter.Name));
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
                    }

                    parameterInfo.PropertyName = requiredPropertyName;
                }

                result.Add(parameterInfo);
            }

            //assign auto name
            foreach (var parameter in result)
            {
                if (string.IsNullOrEmpty(parameter.PropertyName))
                {
                    var parameterName = parameter.Parameter.Name;
                    if (!ignoredParameters.Contains(parameterName))
                        parameter.PropertyName =
                            GetPropertyAutoName(parameterName, usedPropertyNamesInParameterEntity);
                }
            }

            return result;
        }


        List<RemoteAgencyParameterInfo> ProcessParameter(ParameterInfo[] parameters, Stack<MemberInfo> memberPath,
            out List<FoundOutputParameter> outputParameters, out HashSet<string> usedPropertyNamesInReturnValue)
        {
            List<RemoteAgencyParameterInfo> result = new List<RemoteAgencyParameterInfo>();
            outputParameters = new List<FoundOutputParameter>();
            usedPropertyNamesInReturnValue = new HashSet<string>();

            HashSet<string> usedPropertyNamesInParameterEntity = new HashSet<string>(); //could be duplicated cause by the case changing :)
            HashSet<string> ignoredParameters= new HashSet<string>();
        
            foreach (var parameter in parameters)
            {
                var parameterInfo = new RemoteAgencyParameterInfo
                {
                    Parameter = parameter,
                    PassThroughAttributes = GetAttributePassThrough(parameter, (m, a) => new InvalidParameterAttributeDataException(m, a, parameter, memberPath))
                };

                if (parameter.IsOut || GetValueFromAttribute<ParameterIgnoredAttribute, bool>(parameter, i => i.IsIgnored, out _))
                {
                    ignoredParameters.Add(parameter.Name);
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
                    }

                    parameterInfo.PropertyName = requiredPropertyName;
                }
                
                result.Add(parameterInfo);

                if (parameter.IsOut || parameterInfo.GetType().IsByRef)
                {
                    #region Out/Ref
                    var isTwoWay = GetValueFromAttribute<ParameterTwoWayAttribute, bool>(parameter, i => i.IsTwoWay,
                        out var twoWayAttribute, true); //default value is true
                    if (isTwoWay)
                    {
                        string name = twoWayAttribute?.ResponseEntityPropertyName;
                        if (!string.IsNullOrEmpty(name))
                            if (!usedPropertyNamesInReturnValue.Add(name))
                            {
                                throw new EntityPropertyNameConflictException(
                                    $"The property name specified conflicts with others in return value entity.",
                                    twoWayAttribute, parameter, memberPath);
                            }


                        FoundOutputParameter outputParameter = new FoundOutputParameter()
                        {
                            Parameter = parameter,
                            IsIncludedWhenExceptionThrown =
                                twoWayAttribute?.IsIncludedWhenExceptionThrown ?? false,
                            ResponseEntityPropertyName = name,
                            DataType = parameter.ParameterType
                        };
                        outputParameters.Add(outputParameter);
                    }

                    var twoWayPropertyAttribute = parameter.GetCustomAttributes<ParameterTwoWayPropertyAttribute>().FirstOrDefault();

                    if (twoWayPropertyAttribute != null)
                        throw new InvalidParameterAttributeDataException(
                            $"{nameof(ParameterTwoWayPropertyAttribute)} can only be used on the parameter without \"ref / ByRef\" and \"out / Out\"",
                            twoWayPropertyAttribute, parameter, memberPath);
                    #endregion
                }
                else if (!ignoredParameters.Contains(parameter.Name)) //not out, not byref, not ignored => check two way
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
                                    if (!string.IsNullOrEmpty(twoWayPropertyAttribute.ResponseEntityPropertyName))
                                        if (!usedPropertyNamesInReturnValue.Add(
                                            twoWayPropertyAttribute.ResponseEntityPropertyName))
                                        {
                                            throw new EntityPropertyNameConflictException(
                                                "The property name specified conflicts with others in return value entity.",
                                                twoWayPropertyAttribute, parameter, memberPath);
                                        }

                                    FoundOutputParameter outputParameter = new FoundOutputParameter()
                                    {
                                        Parameter = parameter,
                                        IsField = true,
                                        DataType = field.FieldType,
                                        ResponseEntityPropertyName = twoWayPropertyAttribute.ResponseEntityPropertyName,
                                        IsIncludedWhenExceptionThrown = twoWayPropertyAttribute.IsIncludedWhenExceptionThrown
                                    };
                                    outputParameters.Add(outputParameter);
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

                                        if (!string.IsNullOrEmpty(twoWayPropertyAttribute.ResponseEntityPropertyName))
                                            if (!usedPropertyNamesInReturnValue.Add(
                                                twoWayPropertyAttribute.ResponseEntityPropertyName))
                                            {
                                                throw new EntityPropertyNameConflictException(
                                                    "The property name specified conflicts with others in return value entity.",
                                                    twoWayPropertyAttribute, parameter, memberPath);
                                            }

                                        FoundOutputParameter outputParameter = new FoundOutputParameter()
                                        {
                                            Parameter = parameter,
                                            IsProperty = true,
                                            DataType = property.PropertyType,
                                            ResponseEntityPropertyName = twoWayPropertyAttribute.ResponseEntityPropertyName,
                                            IsIncludedWhenExceptionThrown = twoWayPropertyAttribute.IsIncludedWhenExceptionThrown
                                        };
                                        outputParameters.Add(outputParameter);
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
                                      
                                        if (!string.IsNullOrEmpty(twoWayHelperAttribute.ResponseEntityPropertyName))
                                            if (!usedPropertyNamesInReturnValue.Add(
                                                twoWayHelperAttribute.ResponseEntityPropertyName))
                                            {
                                                throw new EntityPropertyNameConflictException(
                                                    $"The property name specified for property {propertyInHelperClass.Name} in helper class {twoWayPropertyAttribute.HelperClass.FullName} conflicts with others in return value entity.",
                                                    twoWayPropertyAttribute, parameter, memberPath);
                                            }

                                        FoundOutputParameter outputParameter = new FoundOutputParameter()
                                        {
                                            Parameter = parameter,
                                            IsHelperClass = true,
                                            HelperClass = twoWayPropertyAttribute.HelperClass,
                                            PropertyInHelperClass = propertyInHelperClass,
                                            DataType = propertyInHelperClass.PropertyType,
                                            ResponseEntityPropertyName = twoWayHelperAttribute.ResponseEntityPropertyName,
                                            IsIncludedWhenExceptionThrown = twoWayHelperAttribute.IsIncludedWhenExceptionThrown
                                        };
                                        outputParameters.Add(outputParameter);
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                    #endregion
                }
            }

            //assign auto name
            foreach (var parameter in result)
            {
                if (string.IsNullOrEmpty(parameter.PropertyName))
                {
                    var parameterName = parameter.Parameter.Name;
                    if (!ignoredParameters.Contains(parameterName))
                        parameter.PropertyName =
                            GetPropertyAutoName(parameterName, usedPropertyNamesInParameterEntity);
                }
            }

            return result;
        }

        class FoundOutputParameter
        {
            public ParameterInfo Parameter { get; set; }
            public bool IsIncludedWhenExceptionThrown { get; set; }
            public string ResponseEntityPropertyName { get; set; }

            public bool IsProperty { get; set; }
            public bool IsField { get; set; }
            public bool IsHelperClass { get; set; }
            public PropertyInfo PropertyInHelperClass { get; set; }
            public Type HelperClass { get; set; }
            public Type DataType { get; set; }
        }

        List<RemoteAgencyReturnValueInfo> ProcessReturnValueForIgnoredOrOneWayAsset(MethodInfo asset,
            List<Tuple<Type, string>> outputParameterNamesForSettingDefault)
        {
            List<RemoteAgencyReturnValueInfo> result = new List<RemoteAgencyReturnValueInfo>();

            foreach (var output in outputParameterNamesForSettingDefault)
            {
                RemoteAgencyReturnValueInfo item = new RemoteAgencyReturnValueInfo()
                {
                    ParameterName = output.Item2,
                    DataType = output.Item1,
                    ShouldSetToDefault = true
                };
                result.Add(item);
            }

            if (asset.ReturnType != typeof(void))
            {
                RemoteAgencyReturnValueInfo item = new RemoteAgencyReturnValueInfo()
                {
                    DataType = asset.ReturnType,
                    ShouldSetToDefault = true,
                    IsReturnValue = true
                };
                result.Add(item);
            }

            return result;
        }

        List<RemoteAgencyReturnValueInfo> ProcessReturnValueForIgnoredAndThrowExceptionAsset()
        {
            List<RemoteAgencyReturnValueInfo> result = new List<RemoteAgencyReturnValueInfo>();

            return result;
        }

        List<RemoteAgencyReturnValueInfo> ProcessReturnValue(MethodInfo asset, Stack<MemberInfo> memberPath,
            List<FoundOutputParameter> outputParameters, HashSet<string> usedPropertyNamesInReturnValue)
        {



            
            //if (!GetValueFromAttribute<ReturnIgnoredAttribute, bool>(asset.ReturnTypeCustomAttributes, i => i.IsIgnored,
            //    out _) &&
            //    !GetValueFromAttribute<ReturnIgnoredAttribute, bool>(asset, i => i.IsIgnored, out _))
            //{
            //    GetValueFromAttribute<CustomizedReturnValueEntityPropertyNameAttribute, string>(asset.ReturnTypeCustomAttributes,
            //            i => i.EntityPropertyName, out var customizedReturnValueEntityPropertyNameAttribute);
            //    if (customizedReturnValueEntityPropertyNameAttribute == null)
            //        GetValueFromAttribute(asset, i => i.EntityPropertyName,
            //            out customizedReturnValueEntityPropertyNameAttribute);

            //    if (asset.ReturnType != typeof(void))
            //    {
            //        RemoteAgencyReturnValueInfo item = new RemoteAgencyReturnValueInfo()
            //        {
            //            DataType = asset.ReturnType,
            //            ShouldSetToDefault = true,
            //            IsReturnValue = true
            //        };
            //        result.Add(item);
            //    }
            //    else
            //    {
            //        throw new InvalidReturnValueAttributeDataException(
            //            $"{nameof(CustomizedReturnValueEntityPropertyNameAttribute)} can only be used on the asset with return value.",
            //            customizedReturnValueEntityPropertyNameAttribute, memberPath);
            //    }
            //}


            //after processing return value, dont forget FoundOutputParameter   for each assign name (ResponseEntityPropertyName)  GetPropertyAutoName(parameter.Name, usedPropertyNames)   
        }
    }
}