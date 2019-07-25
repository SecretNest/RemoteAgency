using SecretNest.RemoteAgency.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace SecretNest.RemoteAgency
{
    public partial class ProxyCreator<TSerialized, TEntityBase> : IAssemblyCacheOperatings<TSerialized, TEntityBase> where TEntityBase : class
    {
        class BuildMethodParameter
        {
            public readonly StringBuilder MemberLevelBuilder;
            public readonly StringBuilder ClassLevelBuilder;
            public readonly StringBuilder ProcessMessageSourceBuilder = new StringBuilder();
            public readonly HashSet<string> usedAssetNames = new HashSet<string>();
            public readonly GetTypeFullNameParameter GetTypeFullNameParameter;
            public readonly string EntityBaseTypeName;
            public readonly string ResponderName;
            public readonly string SerializingHelperName;
            public readonly string CommunicateInterfaceTypeName;
            public readonly int GlobalTimedoutSetting;

            public BuildMethodParameter(StringBuilder memberLevelBuilder, StringBuilder classLevelBuilder, GetTypeFullNameParameter getTypeFullNameParameter, string responderName, string entityBaseTypeName, string serializingHelperName, string communicateInterfaceTypeName, int globalTimedoutSetting)
            {
                MemberLevelBuilder = memberLevelBuilder;
                ClassLevelBuilder = classLevelBuilder;
                GetTypeFullNameParameter = getTypeFullNameParameter;
                ResponderName = responderName;
                EntityBaseTypeName = entityBaseTypeName;
                SerializingHelperName = serializingHelperName;
                CommunicateInterfaceTypeName = communicateInterfaceTypeName;
                GlobalTimedoutSetting = globalTimedoutSetting;
            }
        }

        void BuildMethod(MethodInfo methodInfo, BuildMethodParameter parameter)
        {
            string methodName = NamingHelper.GetAssetName(methodInfo);
            if (parameter.usedAssetNames.Contains(methodName))
            {
                throw new TypeCreatingException(new TypeCreatingExceptionRecord[] { new TypeCreatingExceptionRecord(
                    "-1",
                    "Method name duplications (overload) detected.\nUsing CustomizedAssetName attribute on this method with unique name is required.")
                });
            }
            parameter.usedAssetNames.Add(methodName);
            bool isOneWay;

            parameter.MemberLevelBuilder.Append("public ");

            //return type
            var returnType = methodInfo.ReturnType;
            string returnTypeName;
            string returnPropertyName;
            Dictionary<string, Type> returnTypeUsedGenerics = new Dictionary<string, Type>();
            List<ValueMapping> returnMappings = new List<ValueMapping>();
            string returnEntityName;

            if (returnType == typeof(void))
            {
                isOneWay = methodInfo.GetCustomAttribute<CustomizedOneWayOperatingAttribute>() != null;
                parameter.MemberLevelBuilder.Append("void ");
                returnTypeName = null;
                returnPropertyName = null;
            }
            else
            {
                isOneWay = false;
                returnTypeName = returnType.GetFullName(parameter.GetTypeFullNameParameter, returnTypeUsedGenerics);
                string returnValueName = NamingHelper.GetRandomName("result");
                returnPropertyName = methodInfo.GetCustomAttribute<CustomizedReturnEntityPropertyNameAttribute>()?.EntityPropertyName ?? "Result";
                returnMappings.Add(new ValueMapping(returnValueName, returnPropertyName, returnTypeName, returnValueName));
                parameter.MemberLevelBuilder.Append(returnTypeName).Append(" ");
            }

            //Main name
            parameter.MemberLevelBuilder.Append(methodInfo.Name);

            //Generic
            Dictionary<string, Type> methodGenericArguments;
            if (methodInfo.IsGenericMethod)
            {
                var args = methodInfo.GetGenericArguments();
                methodGenericArguments = new Dictionary<string, Type>(args.Length);
                foreach (var arg in args)
                    methodGenericArguments.Add(arg.Name, arg);
                parameter.MemberLevelBuilder.Append("<").Append(string.Join(", ", methodGenericArguments.Keys)).Append(">");
            }
            else
            {
                methodGenericArguments = null;
            }

            //Parameter
            List<ValueMapping> parameterMappings = new List<ValueMapping>();
            parameter.MemberLevelBuilder.Append("(");
            var parameters = methodInfo.GetParameters();
            if (parameters.Length > 0)
            {
                List<string> parameterStrings = new List<string>();
                for (int parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
                {
                    var para = parameters[parameterIndex];
                    var parameterType = para.ParameterType;
                    var parameterTypeInfo = parameterType.GetTypeInfo();

                    Dictionary<string, Type> oneParameterTypeUsedGenerics = new Dictionary<string, Type>();
                    string typeName = parameterType.GetFullName(parameter.GetTypeFullNameParameter, oneParameterTypeUsedGenerics);
                    string preferredPropertyName = para.GetCustomAttribute<CustomizedParameterEntityPropertyNameAttribute>()?.EntityPropertyName;
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

                    StringBuilder builder = new StringBuilder();
                    if (parameterType.IsByRef)
                    {
                        if (isNotIgnoredReturn)
                        {
                            returnMappings.Add(para.Name, typeName, para.Name, preferredPropertyName);
                            returnTypeUsedGenerics.Merge(oneParameterTypeUsedGenerics);
                        }

                        if (para.IsOut)
                        {
                            builder.Append("out ");
                        }
                        else
                        {
                            builder.Append("ref ");
                            if (isNotIgnoredParameter)
                                parameterMappings.Add(para.Name, typeName, para.Name, preferredPropertyName);
                        }
                    }
                    else if (isNotIgnoredParameter)
                    {
                        parameterMappings.Add(para.Name, typeName, para.Name, preferredPropertyName);

                        if (isNotIgnoredReturn)
                        {
                            var twoWayProperties = para.GetCustomAttributes<ParameterTwoWayPropertyAttribute>().Distinct();
                            foreach (var property in twoWayProperties)
                            {
                                var mapping = property.ExtractTwoWaySetting(para.Name, parameterType, parameter.GetTypeFullNameParameter);
                                if (mapping != null)
                                    returnMappings.Add(mapping);
                            }
                        }
                    }

                    if (parameterTypeInfo.GetCustomAttribute<ParamArrayAttribute>() != null)
                        builder.Append("params ");

                    builder.Append(typeName).Append(" ").Append(para.Name);

                    if (para.IsOptional)
                    {
                        builder.Append(" = ");
                        if (para.RawDefaultValue != null)
                        {
                            builder.Append(para.RawDefaultValue);
                        }
                        else
                        {
                            builder.Append("default(").Append(typeName).Append(")");
                        }
                    }

                    parameterStrings.Add(builder.ToString());
                }
                parameter.MemberLevelBuilder.Append(string.Join(", ", parameterStrings));
            }
            parameter.MemberLevelBuilder.Append(")");

            //Generic Constraints
            CodeBuilderHelper.ApplyConstraints(methodGenericArguments, parameter.MemberLevelBuilder);

            //Parameter Entity
            string parameterEntityValueBasedConstructorCallerCode;
            string parameterEntityName = methodInfo.GetCustomAttribute<CustomizedParameterEntityAttribute>()?.EntityName ?? methodName + "Parameter";
            parameterEntityName = remoteAgencyEntityCodeBuilder.BuildEntity(parameterEntityName, parameter.EntityBaseTypeName,
                parameterMappings, methodGenericArguments, true, parameter.ClassLevelBuilder,
                out parameterEntityValueBasedConstructorCallerCode);

            //Return Entity
            if (returnMappings.Count > 0)
            {
                isOneWay = false;
            }
            string genericReturnEntityTypeName;
            if (!isOneWay)
            {
                returnEntityName = methodInfo.GetCustomAttribute<CustomizedReturnEntityAttribute>()?.EntityName ?? methodName + "Return";
                if (returnTypeUsedGenerics.Count > 0)
                    genericReturnEntityTypeName = returnEntityName + "<"+ new string(',', returnTypeUsedGenerics.Count - 1) + ">";
                else
                    genericReturnEntityTypeName = null;
                returnEntityName = remoteAgencyEntityCodeBuilder.BuildEntity(returnEntityName, parameter.EntityBaseTypeName,
                    returnMappings, returnTypeUsedGenerics, false, parameter.ClassLevelBuilder,
                    out _);
            }
            else
            {
                genericReturnEntityTypeName = null;
                returnEntityName = null;
            }

            //Method Body
            string messageIdValueName = NamingHelper.GetRandomName("messageId");
            string parameterValueName = NamingHelper.GetRandomName("parameter");
            string serializedParameterValueName = NamingHelper.GetRandomName("serializedParameter");
            string deserializingExceptionValueName = NamingHelper.GetRandomName("deserializingException");
            string wrappedDeserializingExceptionValueName = NamingHelper.GetRandomName("wrappedDeserializingException");
            parameter.MemberLevelBuilder.Append("\n{\nSystem.Guid ").Append(messageIdValueName).Append(" = System.Guid.NewGuid();\nvar ")
                .Append(parameterValueName).Append(parameterEntityValueBasedConstructorCallerCode)
                .Append("var ").Append(serializedParameterValueName).Append(" = ").Append(parameter.SerializingHelperName)
                .Append(".Serialize(").Append(parameterValueName).Append(", typeof(").Append(parameterEntityName).AppendLine("));");
            StringBuilder sendingMessageBackBuilder = new StringBuilder("((")
                .Append(parameter.CommunicateInterfaceTypeName)
                .Append(")this).SendMessageCallback(SecretNest.RemoteAgency.MessageType.Method, \"")
                .Append(methodName).Append("\", ")
                .Append(messageIdValueName).Append(", ")
                .Append(isOneWay ? "true" : "false").Append(", ")
                .Append(serializedParameterValueName).Append(", ");
            if (methodGenericArguments.Count == 0)
                sendingMessageBackBuilder.Append("null");
            else
                sendingMessageBackBuilder.Append("new Type[] { ").Append(string.Join(", ", methodGenericArguments.Keys.Select(i => "typeof(" + i + ")"))).Append(" }");
            sendingMessageBackBuilder.AppendLine(");");
            string sendingMessageBack = sendingMessageBackBuilder.ToString();

            if (!isOneWay)
            {
                string resultValueName;
                parameter.ProcessMessageSourceBuilder.Append("case \"").Append(methodName).AppendLine("\":\n{");
                if (returnMappings.Count > 0)
                {
                    resultValueName = NamingHelper.GetRandomName("result");
                    parameter.ProcessMessageSourceBuilder.Append("var ").Append(resultValueName).Append(" = ")
                        .Append(parameter.SerializingHelperName).Append(".DeserializeWithExceptionTolerance(data, typeof(");
                    if (genericReturnEntityTypeName == null)
                    {
                        parameter.ProcessMessageSourceBuilder
                            .Append(returnEntityName).Append("), out var ")
                            .Append(wrappedDeserializingExceptionValueName)
                            .Append(", out var ")
                            .Append(deserializingExceptionValueName)
                            .Append(") as ").Append(returnEntityName);
                    }
                    else
                    {
                        parameter.ProcessMessageSourceBuilder
                            .Append(genericReturnEntityTypeName).Append("), genericArguments, out var ")
                            .Append(wrappedDeserializingExceptionValueName)
                            .Append(", out var ")
                            .Append(deserializingExceptionValueName)
                            .Append(")");
                    }
                    parameter.ProcessMessageSourceBuilder.Append(";\nif (")
                        .Append(wrappedDeserializingExceptionValueName)
                        .AppendLine(" == null)\n{")
                        .Append(parameter.ResponderName)
                        .Append(".SetResult(messageId, ")
                        .Append(resultValueName)
                        .AppendLine(");\n}\nelse\n{")
                        .Append(parameter.ResponderName)
                        .Append(".SetException(messageId, ")
                        .Append(wrappedDeserializingExceptionValueName)
                        .Append(");\nthrow ")
                        .Append(deserializingExceptionValueName)
                        .AppendLine(";\n}");
                }
                else
                {
                    resultValueName = null;
                    parameter.ProcessMessageSourceBuilder.Append(parameter.ResponderName).AppendLine(".SetDefaultResult(messageId);");
                }
                parameter.ProcessMessageSourceBuilder.AppendLine("}\nbreak;");

                parameter.MemberLevelBuilder.Append(parameter.ResponderName).Append(".Prepare(").Append(messageIdValueName).AppendLine(");\ntry\n{")
                    .Append(sendingMessageBack).Append("}\ncatch\n{")
                    .Append(parameter.ResponderName).Append(".Remove(").Append(messageIdValueName).AppendLine(");\nthrow;\n}");
                int timedoutSetting = methodInfo.GetCustomAttribute<CustomizedOperatingTimedoutTimeAttribute>()?.MillisecondsTimeout ?? parameter.GlobalTimedoutSetting;
                if (resultValueName != null)
                {
                    parameter.MemberLevelBuilder.Append("var ").Append(resultValueName).Append(" = (")
                        .Append(returnEntityName).Append(")")
                        .Append(parameter.ResponderName).Append(".GetResult(").Append(messageIdValueName).Append(", ").Append(timedoutSetting)
                        .AppendLine(");");

                    foreach (var item in returnMappings)
                    {
                        if (item.PropertyName != returnPropertyName)
                        {
                            parameter.MemberLevelBuilder.Append(item.NameInCode).Append(" = ").Append(resultValueName).Append(".").Append(item.PropertyName).AppendLine(";");
                        }
                    }
                    if (returnPropertyName != null)
                    {
                        parameter.MemberLevelBuilder.Append("return ").Append(resultValueName).Append(".").Append(returnPropertyName).AppendLine(";");
                    }
                }
                else
                {
                    parameter.MemberLevelBuilder.Append(parameter.ResponderName).Append(".GetResult(").Append(messageIdValueName).Append(", ").Append(timedoutSetting).AppendLine(");");
                }
            }
            else
            {
                parameter.MemberLevelBuilder.Append(sendingMessageBack);
            }

            parameter.MemberLevelBuilder.AppendLine("}");
        }


    }
}
