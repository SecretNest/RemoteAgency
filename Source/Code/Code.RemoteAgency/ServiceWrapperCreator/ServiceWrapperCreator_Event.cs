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
        class BuildEventParameter
        {
            public readonly StringBuilder MemberLevelBuilder;
            public readonly StringBuilder ClassLevelBuilder;
            public readonly StringBuilder ProcessEventMessageSourceBuilder = new StringBuilder();
            public readonly StringBuilder ProcessEventAddMessageSourceBuilder = new StringBuilder();
            public readonly StringBuilder ProcessEventRemoveMessageSourceBuilder = new StringBuilder();
            public readonly StringBuilder DisposingSourceBuilder = new StringBuilder();
            public readonly HashSet<string> usedAssetNames = new HashSet<string>();
            public readonly GetTypeFullNameParameter GetTypeFullNameParameter;
            public readonly string LockObjectName;
            public readonly string ResponderName;
            public readonly string EntityBaseTypeName;
            public readonly string EntitySerializedTypeName;
            public readonly string SerializingHelperName;
            public readonly string CommunicateInterfaceTypeName;
            public readonly string ServiceObjectName;
            public readonly string WrappedExceptionTypeName;

            public BuildEventParameter(StringBuilder memberLevelBuilder, StringBuilder classLevelBuilder, GetTypeFullNameParameter getTypeFullNameParameter, 
                string lockObjectName, string responderName, string entityBaseTypeName, string entitySerializedTypeName, string serializingHelperName, 
                string communicateInterfaceTypeName, string serviceObjectName, string wrappedExceptionTypeName)
            {
                MemberLevelBuilder = memberLevelBuilder;
                ClassLevelBuilder = classLevelBuilder;
                GetTypeFullNameParameter = getTypeFullNameParameter;
                ResponderName = responderName;
                LockObjectName = lockObjectName;
                EntityBaseTypeName = entityBaseTypeName;
                EntitySerializedTypeName = entitySerializedTypeName;
                SerializingHelperName = serializingHelperName;
                CommunicateInterfaceTypeName = communicateInterfaceTypeName;
                ServiceObjectName = serviceObjectName;
                WrappedExceptionTypeName = wrappedExceptionTypeName;
            }
        }

        void BuildEvent(EventInfo eventInfo, BuildEventParameter parameter, string interfaceTypeName, int globalTimedoutSetting, LocalExceptionHandlingMode localExceptionHandlingModeDefault)
        {
            string eventName = NamingHelper.GetAssetName(eventInfo);
            if (parameter.usedAssetNames.Contains(eventName))
            {
                throw new TypeCreatingException(new TypeCreatingExceptionRecord[] { new TypeCreatingExceptionRecord(
                    "-1",
                    "Event name duplications detected.\nUsing CustomizedAssetName attribute on this event with unique name is required.")
                });
            }
            parameter.usedAssetNames.Add(eventName);

            //type
            var eventHandlerType = eventInfo.EventHandlerType;
            var eventHandlerTypeInfo = eventHandlerType.GetTypeInfo();
            var declaredMethodInfo = eventHandlerTypeInfo.GetDeclaredMethod("Invoke");
            var methodCodeName = NamingHelper.GetRandomName(eventInfo.Name);
            //var eventDelegateTypeName = eventHandlerType.GetFullName(parameter.GetTypeFullNameParameter, null);
            bool isOneWay;
            LocalExceptionHandlingMode localExceptionHandlingMode = eventInfo.GetCustomAttribute<LocalExceptionHandlingAttribute>()?.LocalExceptionHandlingMode ?? localExceptionHandlingModeDefault;

            //Method (Entities included)
            var methodReturnType = declaredMethodInfo.ReturnType;
            string methodReturnTypeName;
            string methodReturnPropertyName;
            List<ValueMapping> methodReturnMappings = new List<ValueMapping>();
            string methodReturnEntityName;

            if (methodReturnType == typeof(void))
            {
                if (eventInfo.GetCustomAttribute<CustomizedOneWayOperatingAttribute>() != null)
                    isOneWay = true;
                else if (eventHandlerTypeInfo.GetCustomAttribute<CustomizedOneWayOperatingAttribute>() != null)
                    isOneWay = true;
                else
                    isOneWay = false;
                parameter.MemberLevelBuilder.Append("void ");
                methodReturnTypeName = null;
                methodReturnPropertyName = null;
            }
            else
            {
                isOneWay = false;
                methodReturnTypeName = methodReturnType.GetFullName(parameter.GetTypeFullNameParameter, null);
                string returnValueName = NamingHelper.GetRandomName("result");
                methodReturnPropertyName = eventInfo.GetCustomAttribute<CustomizedReturnEntityPropertyNameAttribute>()?.EntityPropertyName ??
                    eventHandlerTypeInfo.GetCustomAttribute<CustomizedReturnEntityPropertyNameAttribute>()?.EntityPropertyName ?? "Result";
                methodReturnMappings.Add(new ValueMapping(returnValueName, methodReturnPropertyName, methodReturnTypeName, returnValueName));
                parameter.MemberLevelBuilder.Append(methodReturnTypeName).Append(" ");
            }

            parameter.MemberLevelBuilder.Append(methodCodeName).Append("(");

            List<ValueMapping> methodParameterMappings = new List<ValueMapping>();
            var methodParameters = declaredMethodInfo.GetParameters();
            if (methodParameters.Length > 0)
            {
                Dictionary<string, string> customizedEventParameterEntityPropertyNames = new Dictionary<string, string>();
                foreach (var attribute in eventInfo.GetCustomAttributes<CustomizedEventParameterEntityPropertyNameAttribute>().Distinct())
                {
                    customizedEventParameterEntityPropertyNames.Add(attribute.ParameterName, attribute.EntityPropertyName);
                }
                Dictionary<string, List<ParameterTwoWayPropertyAttribute>> eventParameterTwoWayProperties = new Dictionary<string, List<ParameterTwoWayPropertyAttribute>>();
                foreach (var attribute in eventInfo.GetCustomAttributes<EventParameterTwoWayPropertyAttribute>().Distinct())
                {
                    List<ParameterTwoWayPropertyAttribute> list;
                    if (!eventParameterTwoWayProperties.TryGetValue(attribute.ParameterName, out list))
                    {
                        list = new List<ParameterTwoWayPropertyAttribute>();
                        eventParameterTwoWayProperties.Add(attribute.ParameterName, list);
                    }
                    list.Add(new ParameterTwoWayPropertyAttribute(attribute));
                    
                }
                Dictionary<string, ParameterIgnoredAttribute> eventIgnoredParameters = new Dictionary<string, ParameterIgnoredAttribute>();
                foreach(var attribute in eventInfo.GetCustomAttributes<EventParameterIgnoredAttribute>())
                {
                    if (!eventIgnoredParameters.ContainsKey(attribute.ParameterName))
                        eventIgnoredParameters.Add(attribute.ParameterName, new ParameterIgnoredAttribute(attribute));
                }
                List<string> parameterStrings = new List<string>();
                for (int parameterIndex = 0; parameterIndex < methodParameters.Length; parameterIndex++)
                {
                    var para = methodParameters[parameterIndex];
                    var parameterType = para.ParameterType;
                    var parameterTypeInfo = parameterType.GetTypeInfo();

                    string typeName = parameterType.GetFullName(parameter.GetTypeFullNameParameter, null);
                    if (!customizedEventParameterEntityPropertyNames.TryGetValue(para.Name, out string preferredPropertyName))
                        preferredPropertyName = para.GetCustomAttribute<CustomizedParameterEntityPropertyNameAttribute>()?.EntityPropertyName;
                    bool isNotIgnoredParameter, isNotIgnoredReturn;
                    {
                        ParameterIgnoredAttribute ignoredParameter;
                        if (eventIgnoredParameters.ContainsKey(para.Name))
                        {
                            ignoredParameter = eventIgnoredParameters[para.Name];
                        }
                        else
                        {
                            ignoredParameter = para.GetCustomAttribute<ParameterIgnoredAttribute>();
                        }
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
                            methodReturnMappings.Add(para.Name, typeName, para.Name, preferredPropertyName);

                        if (para.IsOut)
                        {
                            builder.Append("out ");
                        }
                        else
                        {
                            builder.Append("ref ");
                            if (isNotIgnoredParameter)
                                methodParameterMappings.Add(para.Name, typeName, para.Name, preferredPropertyName);
                        }
                    }
                    else if (isNotIgnoredParameter)
                    {
                        methodParameterMappings.Add(para.Name, typeName, para.Name, preferredPropertyName);

                        if (isNotIgnoredReturn)
                        {
                            List<ParameterTwoWayPropertyAttribute> twoWayProperties;
                            if (!eventParameterTwoWayProperties.TryGetValue(para.Name, out twoWayProperties))
                            {
                                twoWayProperties = new List<ParameterTwoWayPropertyAttribute>();
                            }
                            foreach (var attribute in para.GetCustomAttributes<ParameterTwoWayPropertyAttribute>())
                            {
                                if (!twoWayProperties.Contains(attribute))
                                    twoWayProperties.Add(attribute);
                            }

                            foreach (var property in twoWayProperties)
                            {
                                var mapping = property.ExtractTwoWaySetting(para.Name, parameterType, parameter.GetTypeFullNameParameter);
                                if (mapping != null)
                                    methodReturnMappings.Add(mapping);
                            }
                        }
                    }

                    builder.Append(typeName).Append(" ").Append(para.Name);
                    parameterStrings.Add(builder.ToString());
                }
                parameter.MemberLevelBuilder.Append(string.Join(", ", parameterStrings));
            }
            parameter.MemberLevelBuilder.Append(")");

            string parameterEntityValueBasedConstructorCallerCode;
            string parameterEntityName = eventInfo.GetCustomAttribute<CustomizedParameterEntityAttribute>()?.EntityName ?? eventName + "Parameter";
            parameterEntityName = remoteAgencyEntityCodeBuilder.BuildEntity(parameterEntityName, parameter.EntityBaseTypeName,
                methodParameterMappings, null, true, parameter.ClassLevelBuilder,
                out parameterEntityValueBasedConstructorCallerCode);
            if (methodReturnMappings.Count > 0)
            {
                isOneWay = false;
            }
            if (!isOneWay)
            {
                methodReturnEntityName = eventInfo.GetCustomAttribute<CustomizedReturnEntityAttribute>()?.EntityName ?? eventName + "Return";
                methodReturnEntityName = remoteAgencyEntityCodeBuilder.BuildEntity(methodReturnEntityName, parameter.EntityBaseTypeName,
                    methodReturnMappings, null, false, parameter.ClassLevelBuilder,
                    out _);
            }
            else
            {
                methodReturnEntityName = null;
            }

            string methodMessageIdValueName = NamingHelper.GetRandomName("messageId");
            string methodParameterValueName = NamingHelper.GetRandomName("parameter");
            string methodSerializedParameterValueName = NamingHelper.GetRandomName("serializedParameter");
            string deserializingExceptionValueName = NamingHelper.GetRandomName("deserializingException");
            string wrappedDeserializingExceptionValueName = NamingHelper.GetRandomName("wrappedDeserializingException");
            parameter.MemberLevelBuilder.Append("\n{\nSystem.Guid ").Append(methodMessageIdValueName).Append(" = System.Guid.NewGuid();\nvar ")
                .Append(methodParameterValueName).Append(parameterEntityValueBasedConstructorCallerCode)
                .Append("var ").Append(methodSerializedParameterValueName).Append(" = ").Append(parameter.SerializingHelperName)
                .Append(".Serialize(").Append(methodParameterValueName).Append(", typeof(").Append(parameterEntityName).AppendLine("));");
            StringBuilder sendingMessageBackBuilder = new StringBuilder("((")
                .Append(parameter.CommunicateInterfaceTypeName).Append(")this).SendMessageCallback(SecretNest.RemoteAgency.MessageType.Event, \"")
                .Append(eventName).Append("\", ")
                .Append(methodMessageIdValueName).Append(", ")
                .Append(isOneWay ? "true" : "false").Append(", ")
                .Append(methodSerializedParameterValueName).AppendLine(", null);");
            string sendingMessageBack = sendingMessageBackBuilder.ToString();
            if (!isOneWay)
            {
                string methodResultValueName;
                parameter.ProcessEventMessageSourceBuilder.Append("case \"").Append(eventName).AppendLine("\":\n{");
                if (methodReturnMappings.Count > 0)
                {
                    methodResultValueName = NamingHelper.GetRandomName("result");
                    parameter.ProcessEventMessageSourceBuilder.Append("var ").Append(methodResultValueName).Append(" = ")
                        .Append(parameter.SerializingHelperName).Append(".DeserializeWithExceptionTolerance(data, typeof(")
                        .Append(methodReturnEntityName).Append("), out var ")
                        .Append(wrappedDeserializingExceptionValueName)
                        .Append(", out var ")
                        .Append(deserializingExceptionValueName)
                        .Append(") as ").Append(methodReturnEntityName).Append(";\nif (")
                        .Append(wrappedDeserializingExceptionValueName)
                        .AppendLine(" == null)\n{")
                        .Append(parameter.ResponderName)
                        .Append(".SetResult(messageId, ")
                        .Append(methodResultValueName)
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
                    methodResultValueName = null;
                    parameter.ProcessEventMessageSourceBuilder.Append(parameter.ResponderName).AppendLine(".SetDefaultResult(messageId);");
                }
                parameter.ProcessEventMessageSourceBuilder.AppendLine("}\nbreak;");

                parameter.MemberLevelBuilder.Append(parameter.ResponderName).Append(".Prepare(").Append(methodMessageIdValueName).AppendLine(");\ntry\n{")
                    .Append(sendingMessageBack).Append("}\ncatch\n{")
                    .Append(parameter.ResponderName).Append(".Remove(").Append(methodMessageIdValueName).AppendLine(");\nthrow;\n}");
                int timedoutSetting = eventInfo.GetCustomAttribute<CustomizedOperatingTimedoutTimeAttribute>()?.MillisecondsTimeout ?? globalTimedoutSetting;
                if (methodReturnMappings.Count > 0)
                {
                    parameter.MemberLevelBuilder.Append("var ").Append(methodResultValueName).Append(" = (")
                        .Append(methodReturnEntityName).Append(")")
                        .Append(parameter.ResponderName).Append(".GetResult(").Append(methodMessageIdValueName).Append(", ").Append(timedoutSetting)
                        .AppendLine(");");

                    foreach (var item in methodReturnMappings)
                    {
                        if (item.PropertyName != methodReturnPropertyName)
                        {
                            parameter.MemberLevelBuilder.Append(item.NameInCode).Append(" = ").Append(methodResultValueName).Append(".").Append(item.PropertyName).AppendLine(";");
                        }
                    }
                    if (methodReturnPropertyName != null)
                    {
                        parameter.MemberLevelBuilder.Append("return ").Append(methodResultValueName).Append(".").Append(methodReturnPropertyName).AppendLine(";");
                    }
                }
                else
                {
                    parameter.MemberLevelBuilder.Append(parameter.ResponderName).Append(".GetResult(").Append(methodMessageIdValueName).Append(", ").Append(timedoutSetting).AppendLine(");");
                }
            }
            else
            {
                parameter.MemberLevelBuilder.Append(sendingMessageBack);
            }

            var eventHandlerCountName = NamingHelper.GetRandomName("eventHandlerCount");
            parameter.MemberLevelBuilder.Append("}\nint ")
                .Append(eventHandlerCountName).AppendLine(" = 0;");

            //EventAdd
            string needSendValueName = NamingHelper.GetRandomName("needSend");
            string catchedExceptionValueName = NamingHelper.GetRandomName("exception");
            string wrappedCatchedExceptionValueName = NamingHelper.GetRandomName("wrappedException");
            //string serializedExceptionValueName = NamingHelper.GetRandomName("serializedException");
            parameter.ProcessEventAddMessageSourceBuilder.Append("case \"").Append(eventName).Append("\":\n{\nbool ")
                .Append(needSendValueName).Append(" = false;\ntry\n{\nlock (")
                .Append(parameter.LockObjectName).Append(")\n{\n((")
                .Append(interfaceTypeName).Append(")")
                .Append(parameter.ServiceObjectName).Append(").")
                .Append(eventInfo.Name).Append(" += ")
                .Append(methodCodeName).AppendLine(";")
                .Append(eventHandlerCountName).AppendLine(" += 1;\n}")
                .Append(needSendValueName).Append(" = true;\n}\ncatch (Exception ")
                .Append(catchedExceptionValueName).Append(")\n{\nvar ")
                .Append(wrappedCatchedExceptionValueName).Append(" = ").Append(parameter.WrappedExceptionTypeName).Append(".Create(").Append(catchedExceptionValueName).AppendLine(");");
            CodeBuilderHelper.BuildSendingExceptionCode(parameter.ProcessEventAddMessageSourceBuilder,
                parameter.SerializingHelperName, localExceptionHandlingMode,
                parameter.CommunicateInterfaceTypeName, interfaceTypeName, parameter.WrappedExceptionTypeName,
                "SecretNest.RemoteAgency.MessageType.EventAdd", wrappedCatchedExceptionValueName);
            CodeBuilderHelper.BuildRaisingOrRedirectingExceptionWithNullCheckCode(parameter.ProcessEventAddMessageSourceBuilder,
                localExceptionHandlingMode,
                parameter.CommunicateInterfaceTypeName, interfaceTypeName,
                "SecretNest.RemoteAgency.MessageType.EventAdd", catchedExceptionValueName);
            parameter.ProcessEventAddMessageSourceBuilder.Append("}\nif (") //}: catch
                .Append(needSendValueName).AppendLine(")\n{");
            CodeBuilderHelper.BuildSendingSerializedValueCode(parameter.ProcessEventAddMessageSourceBuilder,
                localExceptionHandlingMode,
                parameter.CommunicateInterfaceTypeName, interfaceTypeName,
                "SecretNest.RemoteAgency.MessageType.EventAdd", "default(" + parameter.EntitySerializedTypeName + ")");
            parameter.ProcessEventAddMessageSourceBuilder.Append("}\n}\nbreak;");//}: if, case

            //EventRemove
            parameter.ProcessEventRemoveMessageSourceBuilder.Append("case \"").Append(eventName).Append("\":\n{\nbool ")
                .Append(needSendValueName).Append(" = false;\ntry\n{\nlock (")
                .Append(parameter.LockObjectName).Append(")\n{\nif (")
                .Append(eventHandlerCountName).Append(" > 0)\n{\n((")
                .Append(interfaceTypeName).Append(")")
                .Append(parameter.ServiceObjectName).Append(").")
                .Append(eventInfo.Name).Append(" -= ")
                .Append(methodCodeName).AppendLine(";")
                .Append(eventHandlerCountName).AppendLine(" -= 1;\n}\n}")
                .Append(needSendValueName).Append(" = true;\n}\ncatch (Exception ")
                .Append(catchedExceptionValueName).Append(")\n{\nvar ")
                .Append(wrappedCatchedExceptionValueName).Append(" = ").Append(parameter.WrappedExceptionTypeName).Append(".Create(").Append(catchedExceptionValueName).AppendLine(");");
            CodeBuilderHelper.BuildSendingExceptionCode(parameter.ProcessEventRemoveMessageSourceBuilder,
                parameter.SerializingHelperName, localExceptionHandlingMode,
                parameter.CommunicateInterfaceTypeName, interfaceTypeName, parameter.WrappedExceptionTypeName,
                "SecretNest.RemoteAgency.MessageType.EventRemove", wrappedCatchedExceptionValueName);
            CodeBuilderHelper.BuildRaisingOrRedirectingExceptionWithNullCheckCode(parameter.ProcessEventRemoveMessageSourceBuilder,
                localExceptionHandlingMode,
                parameter.CommunicateInterfaceTypeName, interfaceTypeName,
                "SecretNest.RemoteAgency.MessageType.EventRemove", catchedExceptionValueName);
            parameter.ProcessEventRemoveMessageSourceBuilder.Append("}\nif (") //}: catch
                .Append(needSendValueName).AppendLine(")\n{");
            CodeBuilderHelper.BuildSendingSerializedValueCode(parameter.ProcessEventRemoveMessageSourceBuilder,
                localExceptionHandlingMode,
                parameter.CommunicateInterfaceTypeName, interfaceTypeName,
                "SecretNest.RemoteAgency.MessageType.EventRemove", "default(" + parameter.EntitySerializedTypeName + ")");
            parameter.ProcessEventRemoveMessageSourceBuilder.AppendLine("}\n}\nbreak;");//}: if, case

            //Disposing
            parameter.DisposingSourceBuilder.Append("while(")
                .Append(eventHandlerCountName).Append(" > 0)\n{\n((")
                .Append(interfaceTypeName).Append(")")
                .Append(parameter.ServiceObjectName).Append(").")
                .Append(eventInfo.Name).Append(" -= ")
                .Append(methodCodeName).AppendLine(";")
                .Append(eventHandlerCountName).AppendLine(" -= 1;\n}");
        }
    }
}
