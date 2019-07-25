using SecretNest.RemoteAgency.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SecretNest.RemoteAgency
{
    public partial class ServiceWrapperCreator<TSerialized, TEntityBase> : IAssemblyCacheOperatings<TSerialized, TEntityBase> where TEntityBase : class
    {
        class BuildMethodParameter
        {
            public readonly StringBuilder MemberLevelBuilder; //for generic only
            public readonly StringBuilder ClassLevelBuilder;
            public readonly StringBuilder ProcessMethodMessageSourceBuilder = new StringBuilder();
            public readonly HashSet<string> usedAssetNames = new HashSet<string>();
            public readonly GetTypeFullNameParameter GetTypeFullNameParameter;
            public readonly string EntityBaseTypeName;
            public readonly string EntitySerializedTypeName;
            public readonly string SerializingHelperName;
            public readonly string CommunicateInterfaceTypeName;
            public readonly string ServiceObjectName;
            public readonly string WrappedExceptionTypeName;

            public BuildMethodParameter(StringBuilder memberLevelBuilder, StringBuilder classLevelBuilder, GetTypeFullNameParameter getTypeFullNameParameter, 
                string entityBaseTypeName, string entitySerializedTypeName, string serializingHelperName, string communicateInterfaceTypeName,
                string serviceObjectName, string wrappedExceptionTypeName)
            {
                MemberLevelBuilder = memberLevelBuilder;
                ClassLevelBuilder = classLevelBuilder;
                GetTypeFullNameParameter = getTypeFullNameParameter;
                EntityBaseTypeName = entityBaseTypeName;
                EntitySerializedTypeName = entitySerializedTypeName;
                SerializingHelperName = serializingHelperName;
                CommunicateInterfaceTypeName = communicateInterfaceTypeName;
                ServiceObjectName = serviceObjectName;
                WrappedExceptionTypeName = wrappedExceptionTypeName;
            }
        }

        void BuildMethod(MethodInfo methodInfo, BuildMethodParameter parameter, string interfaceTypeName, LocalExceptionHandlingMode localExceptionHandlingModeDefault, ref bool dynamicKeywordUsed)
        {
            string methodName = NamingHelper.GetAssetName(methodInfo);
            if (parameter.usedAssetNames.Contains(methodName))
            {
                throw new TypeCreatingException(new TypeCreatingExceptionRecord[] { new TypeCreatingExceptionRecord(
                    "-1",
                    "Method name duplications (overload included) detected.\nUsing CustomizedAssetName attribute on this method with unique name is required.")
                });
            }
            parameter.usedAssetNames.Add(methodName);

            //type
            var returnType = methodInfo.ReturnType;
            string returnTypeName;
            Dictionary<string, Type> returnTypeUsedGenerics = new Dictionary<string, Type>();
            List<ValueMapping> returnMappings = new List<ValueMapping>();
            string returnValueName;
            LocalExceptionHandlingMode localExceptionHandlingMode = methodInfo.GetCustomAttribute<LocalExceptionHandlingAttribute>()?.LocalExceptionHandlingMode ?? localExceptionHandlingModeDefault;
            
            //Return
            if (returnType == typeof(void))
            {
                returnTypeName = null;
                returnValueName = null;
            }
            else
            {
                returnTypeName = returnType.GetFullName(parameter.GetTypeFullNameParameter, returnTypeUsedGenerics);
                returnValueName = NamingHelper.GetRandomName("result");
                string returnPropertyName = methodInfo.GetCustomAttribute<CustomizedReturnEntityPropertyNameAttribute>()?.EntityPropertyName ?? "Result";
                returnMappings.Add(new ValueMapping(returnValueName, returnPropertyName, returnTypeName, returnValueName));
            }

            //Generic
            Dictionary<string, Type> methodGenericArguments;
            if (methodInfo.IsGenericMethod)
            {
                var args = methodInfo.GetGenericArguments();
                methodGenericArguments = new Dictionary<string, Type>(args.Length);
                foreach (var arg in args)
                    methodGenericArguments.Add(arg.Name, arg);
            }
            else
            {
                methodGenericArguments = null;
            }

            string parameterValueName = NamingHelper.GetRandomName("parameter");
            //Parameter
            List<ValueMapping> parameterMappings = new List<ValueMapping>();
            var parameters = methodInfo.GetParameters();
            string preCallCode;
            string serviceMethodParameter;
            if (parameters.Length > 0)
            {
                StringBuilder preCallBuilder = new StringBuilder();
                List<string> serviceMethodParameters = new List<string>();
                for (int parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
                {
                    var para = parameters[parameterIndex];
                    var parameterType = para.ParameterType;
                    //var parameterTypeInfo = parameterType.GetTypeInfo();

                    Dictionary<string, Type> oneParameterTypeUsedGenerics = new Dictionary<string, Type>();
                    string typeName = parameterType.GetFullName(parameter.GetTypeFullNameParameter, oneParameterTypeUsedGenerics);
                    string propertyName = para.GetCustomAttribute<CustomizedParameterEntityPropertyNameAttribute>()?.EntityPropertyName;
                    bool isNotIgnoredParameter, isNotIgnoredReturn;
                    {
                        var ignoredParameter = para.GetCustomAttribute<ParameterIgnoredAttribute>();
                        if (ignoredParameter != null)
                        {
                            isNotIgnoredParameter = !ignoredParameter.IgnoredInParameter;
                            isNotIgnoredReturn = !ignoredParameter.IgnoredInReturn;
                        }
                        else
                        {
                            isNotIgnoredParameter = true;
                            isNotIgnoredReturn = true;
                        }
                    }

                    if (parameterType.IsByRef)
                    {
                        string refValueName = NamingHelper.GetRandomName(para.Name);
                        if (isNotIgnoredReturn)
                        {
                            returnMappings.Add(para.Name, typeName, refValueName, propertyName);
                            returnTypeUsedGenerics.Merge(oneParameterTypeUsedGenerics);
                        }

                        if (para.IsOut)
                        {
                            preCallBuilder.Append(typeName).Append(" ").Append(refValueName).Append(" = default(").Append(typeName).AppendLine(");");
                            serviceMethodParameters.Add(string.Format("out {0}", refValueName));
                        }
                        else
                        {
                            if (isNotIgnoredParameter)
                            {
                                propertyName = parameterMappings.Add(para.Name, typeName, null, propertyName);
                                preCallBuilder.Append(typeName).Append(" ").Append(refValueName).Append(" = ").Append(parameterValueName).Append(".").Append(propertyName).AppendLine(";");
                            }
                            else
                            {
                                preCallBuilder.Append(typeName).Append(" ").Append(refValueName).Append(" = default(").Append(typeName).AppendLine(");");
                            }
                            serviceMethodParameters.Add(string.Format("ref {0}", refValueName));
                        }
                    }
                    else if (isNotIgnoredParameter)
                    {
                        propertyName = parameterMappings.Add(para.Name, typeName, null, propertyName);
                        string nameInCode = string.Format("{0}.{1}", parameterValueName, propertyName);
                        serviceMethodParameters.Add(nameInCode);

                        if (isNotIgnoredReturn)
                        {
                            var twoWayProperties = para.GetCustomAttributes<ParameterTwoWayPropertyAttribute>().Distinct();
                            foreach (var property in twoWayProperties)
                            {
                                var mapping = property.ExtractTwoWaySetting(nameInCode, parameterType, parameter.GetTypeFullNameParameter);
                                if (mapping != null)
                                    returnMappings.Add(mapping);
                            }
                        }
                    }
                    else
                    {
                        serviceMethodParameters.Add(string.Format("default({0})", typeName));
                    }
                }
                serviceMethodParameter = string.Join(", ", serviceMethodParameters);
                preCallCode = preCallBuilder.ToString();
            }
            else
            {
                serviceMethodParameter = "";
                preCallCode = "";
            }

            string serviceMethodName = methodInfo.Name;

            //Parameter Entity
            string parameterEntityNameWithoutGeneric = methodInfo.GetCustomAttribute<CustomizedParameterEntityAttribute>()?.EntityName ?? methodName + "Parameter";
            string parameterEntityName = remoteAgencyEntityCodeBuilder.BuildEntity(parameterEntityNameWithoutGeneric, parameter.EntityBaseTypeName,
                parameterMappings, methodGenericArguments, false, parameter.ClassLevelBuilder,
                out _);
            if (methodGenericArguments != null)
            {
                parameterEntityNameWithoutGeneric += "<" + new string(',', methodGenericArguments.Count - 1) + ">";
                serviceMethodName += "<" + string.Join(", ", methodGenericArguments.Keys) + ">";
            }

            //Return Entity
            string returnEntityValueBasedConstructorCallerCode;
            string genericReturnEntityTypeName;
            string returnEntityName = methodInfo.GetCustomAttribute<CustomizedReturnEntityAttribute>()?.EntityName ?? methodName + "Return";
            if (returnTypeUsedGenerics.Count > 0)
                genericReturnEntityTypeName = returnEntityName + "<" + new string(',', returnTypeUsedGenerics.Count - 1) + ">";
            else
                genericReturnEntityTypeName = null;
            returnEntityName = remoteAgencyEntityCodeBuilder.BuildEntity(returnEntityName, parameter.EntityBaseTypeName,
                returnMappings, returnTypeUsedGenerics, true, parameter.ClassLevelBuilder,
                out returnEntityValueBasedConstructorCallerCode);

            //Method
            string helperMethodName;
            string serializedResultValueName = NamingHelper.GetRandomName("serializedResult");
            string needSendValueName = NamingHelper.GetRandomName("needSend");
            string catchedExceptionValueName = NamingHelper.GetRandomName("exception");
            string wrappedCatchedExceptionValueName = NamingHelper.GetRandomName("wrappedException");
            string resultValueName = NamingHelper.GetRandomName("result");
            string deserializingExceptionValueName = NamingHelper.GetRandomName("deserializingException");
            string wrappedDeserializingExceptionValueName = NamingHelper.GetRandomName("wrappedDeserializingException");
            string serializingExceptionValueName = NamingHelper.GetRandomName("serializingException");
            string wrappedSerializingExceptionValueName = NamingHelper.GetRandomName("wrappedSerializingException");
            parameter.ProcessMethodMessageSourceBuilder.Append("case \"").Append(methodName).AppendLine("\":\n{")
                .Append(parameter.EntitySerializedTypeName).Append(" ")
                .Append(serializedResultValueName).Append(" = default(")
                .Append(parameter.EntitySerializedTypeName).Append(");\nbool ")
                .Append(needSendValueName).Append(" = false;\nvar ")
                .Append(parameterValueName).Append(" = ")
                .Append(parameter.SerializingHelperName).Append(".DeserializeWithExceptionTolerance(data, typeof(").Append(parameterEntityNameWithoutGeneric).Append(")");

            StringBuilder innerCodeBuilder = new StringBuilder(preCallCode);
            if (returnTypeName != null)
            {
                innerCodeBuilder.Append("var ").Append(returnValueName).Append(" = ");
            }
            innerCodeBuilder.Append("((").Append(interfaceTypeName).Append(")")
                .Append(parameter.ServiceObjectName).Append(").")
                .Append(serviceMethodName).Append("(").Append(serviceMethodParameter).Append(");\nif (!isOneWay)\n{\nvar ")
                .Append(resultValueName).Append(returnEntityValueBasedConstructorCallerCode)
                .Append(serializedResultValueName).Append(" = ")
                .Append(parameter.SerializingHelperName).Append(".SerializeWithExceptionTolerance(")
                .Append(resultValueName).Append(", typeof(").Append(returnEntityName).AppendLine("), out var ")
                .Append(serializingExceptionValueName).Append(");\nif (")
                .Append(serializingExceptionValueName).AppendLine(" != null)\n{\nvar ")
                .Append(wrappedSerializingExceptionValueName).Append(" = ").Append(parameter.WrappedExceptionTypeName).Append(".Create(").Append(serializingExceptionValueName).AppendLine(");");
            CodeBuilderHelper.BuildSendingExceptionCode(innerCodeBuilder,
                parameter.SerializingHelperName, localExceptionHandlingMode,
                parameter.CommunicateInterfaceTypeName, interfaceTypeName, parameter.WrappedExceptionTypeName,
                "SecretNest.RemoteAgency.MessageType.Method", wrappedSerializingExceptionValueName);
            CodeBuilderHelper.BuildRaisingOrRedirectingExceptionWithNullCheckCode(innerCodeBuilder,
                localExceptionHandlingMode,
                parameter.CommunicateInterfaceTypeName, interfaceTypeName,
                "SecretNest.RemoteAgency.MessageType.Method", serializingExceptionValueName);
            innerCodeBuilder.AppendLine("}\nelse\n{")
                .Append(needSendValueName).AppendLine(" = true;\n}\n}"); //}: else of serializing error, if (!isOneWay)

            if (methodInfo.IsGenericMethod)
            {
                dynamicKeywordUsed = true;
                parameter.ProcessMethodMessageSourceBuilder.Append(".MakeGenericType(genericArguments), out var ")
                    .Append(wrappedDeserializingExceptionValueName).Append(", out var ")
                    .Append(deserializingExceptionValueName).AppendLine(");");
            }
            else
            {
                helperMethodName = null;
                parameter.ProcessMethodMessageSourceBuilder.Append(", out var ")
                    .Append(wrappedDeserializingExceptionValueName).Append(", out var ")
                    .Append(deserializingExceptionValueName).AppendLine(") as ")
                    .Append(parameterEntityName).AppendLine(";");
            }
            parameter.ProcessMethodMessageSourceBuilder.Append("if (")
                .Append(wrappedDeserializingExceptionValueName).AppendLine(" != null)\n{\nif (!isOneWay)\n{");
            CodeBuilderHelper.BuildSendingExceptionCode(parameter.ProcessMethodMessageSourceBuilder,
                parameter.SerializingHelperName, localExceptionHandlingMode,
                parameter.CommunicateInterfaceTypeName, interfaceTypeName, parameter.WrappedExceptionTypeName,
                "SecretNest.RemoteAgency.MessageType.Method", wrappedDeserializingExceptionValueName);
            parameter.ProcessMethodMessageSourceBuilder.AppendLine("}"); //}: if (!isOneWay)
            CodeBuilderHelper.BuildRaisingOrRedirectingExceptionWithNullCheckCode(parameter.ProcessMethodMessageSourceBuilder,
                localExceptionHandlingMode,
                parameter.CommunicateInterfaceTypeName, interfaceTypeName,
                "SecretNest.RemoteAgency.MessageType.Method", deserializingExceptionValueName);
            parameter.ProcessMethodMessageSourceBuilder.AppendLine("}\nelse\n{\ntry\n{");
            if (methodInfo.IsGenericMethod)
            {
                helperMethodName = NamingHelper.GetRandomName("GenericHelper");
                parameter.ProcessMethodMessageSourceBuilder.Append(helperMethodName).Append("((dynamic)")
                    .Append(parameterValueName).Append(", messageId, isOneWay, genericArguments, ref ")
                    .Append(needSendValueName).Append(", ref ").Append(serializedResultValueName).AppendLine(");");

                //helper Method
                parameter.MemberLevelBuilder.Append("void ").Append(helperMethodName).Append("<")
                    .Append(string.Join(", ", methodGenericArguments.Keys)).Append(">(")
                    .Append(parameterEntityName).Append(" ")
                    .Append(parameterValueName).Append(", System.Guid messageId, bool isOneWay, Type[] genericArguments, ref bool ")
                    .Append(needSendValueName).Append(", ref ").Append(parameter.EntitySerializedTypeName).Append(" ").Append(serializedResultValueName).Append(")");
                CodeBuilderHelper.ApplyConstraints(methodGenericArguments, parameter.MemberLevelBuilder);
                parameter.MemberLevelBuilder.Append("\n{\nconst string assetName = \"").Append(methodName).AppendLine("\";")
                    .Append(innerCodeBuilder.ToString()).AppendLine("}");
            }
            else
            {
                parameter.ProcessMethodMessageSourceBuilder.Append(innerCodeBuilder.ToString());
            }

            parameter.ProcessMethodMessageSourceBuilder.Append("}\ncatch (Exception ").Append(catchedExceptionValueName).Append(")\n{\nif (!isOneWay)\n{\n var ")
                .Append(wrappedCatchedExceptionValueName).Append(" = ").Append(parameter.WrappedExceptionTypeName).Append(".Create(").Append(catchedExceptionValueName).AppendLine(");");
            CodeBuilderHelper.BuildSendingExceptionCode(parameter.ProcessMethodMessageSourceBuilder,
                parameter.SerializingHelperName, localExceptionHandlingMode,
                parameter.CommunicateInterfaceTypeName, interfaceTypeName, parameter.WrappedExceptionTypeName,
                "SecretNest.RemoteAgency.MessageType.Method", wrappedCatchedExceptionValueName);
            parameter.ProcessMethodMessageSourceBuilder.AppendLine("}"); //}: if (!isOneWay)
            CodeBuilderHelper.BuildRaisingOrRedirectingExceptionWithNullCheckCode(parameter.ProcessMethodMessageSourceBuilder,
                localExceptionHandlingMode,
                parameter.CommunicateInterfaceTypeName, interfaceTypeName,
                "SecretNest.RemoteAgency.MessageType.Method", catchedExceptionValueName);
            parameter.ProcessMethodMessageSourceBuilder.Append("}\nif (") //}: catch
                .Append(needSendValueName).AppendLine(")\n{");

            string genericsInReturn;
            if (genericReturnEntityTypeName == null)
            {
                genericsInReturn = "null"; //Must be string "null", not null
            }
            else
            {
                List<string> genericTypeNames = new List<string>(returnTypeUsedGenerics.Count);
                var allGenericTypeNames = methodGenericArguments.Keys.ToArray();
                foreach (var key in returnTypeUsedGenerics.Keys)
                {
                    var index = Array.IndexOf(allGenericTypeNames, key);
                    genericTypeNames.Add(string.Format("genericArguments[{0}]", index));
                }
                genericsInReturn = "new Type[] { " +
                    string.Join(", ", genericTypeNames) + " }";
            }
            CodeBuilderHelper.BuildSendingSerializedValueCode(parameter.ProcessMethodMessageSourceBuilder,
            localExceptionHandlingMode,
            parameter.CommunicateInterfaceTypeName, interfaceTypeName,
            "SecretNest.RemoteAgency.MessageType.Method", serializedResultValueName, genericsInReturn);
            parameter.ProcessMethodMessageSourceBuilder.AppendLine("}\n}\n}\nbreak;"); //}: if, else(no error in deserialize), case
        }
    }
}
