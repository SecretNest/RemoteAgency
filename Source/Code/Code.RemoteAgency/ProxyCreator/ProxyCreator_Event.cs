using SecretNest.RemoteAgency.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SecretNest.RemoteAgency
{
    public partial class ProxyCreator<TSerialized, TEntityBase> : IAssemblyCacheOperatings<TSerialized, TEntityBase> where TEntityBase : class
    {
        private class BuildEventParameter
        {
            public readonly StringBuilder MemberLevelBuilder;
            public readonly StringBuilder ClassLevelBuilder;
            public readonly StringBuilder ProcessRaiseMessageSourceBuilder = new StringBuilder();
            public readonly StringBuilder AfterInitializedSourcePhase1Builder = new StringBuilder();
            public readonly StringBuilder AfterInitializedSourcePhase2Builder = new StringBuilder();
            public readonly GetTypeFullNameParameter GetTypeFullNameParameter;
            public readonly string LockObjectName;
            public readonly string EventHandlerStateName;
            public readonly string ResponderName;
            public readonly string EntityBaseTypeName;
            public readonly string EntitySerializedTypeName;
            public readonly string SerializingHelperName;
            public readonly int GlobalTimedoutSetting;
            public readonly string CommunicateInterfaceTypeName;
            public readonly string WrappedExceptionTypeName;
            public readonly string InterfaceTypeName;
            public readonly LocalExceptionHandlingMode LocalExceptionHandlingMode;

            public BuildEventParameter(StringBuilder memberLevelBuilder, StringBuilder classLevelBuilder, GetTypeFullNameParameter getTypeFullNameParameter, string lockObjectName, string eventHandlerStateName, string responderName, string entityBaseTypeName, string entitySerializedTypeName, string serializingHelperName, string communicateInterfaceTypeName, int globalTimedoutSetting, string wrappedExceptionTypeName, string interfaceTypeName, LocalExceptionHandlingMode localExceptionHandlingMode)
            {
                ClassLevelBuilder = classLevelBuilder;
                MemberLevelBuilder = memberLevelBuilder;
                GetTypeFullNameParameter = getTypeFullNameParameter;
                ResponderName = responderName;
                LockObjectName = lockObjectName;
                EventHandlerStateName = eventHandlerStateName;
                EntityBaseTypeName = entityBaseTypeName;
                EntitySerializedTypeName = entitySerializedTypeName;
                SerializingHelperName = serializingHelperName;
                CommunicateInterfaceTypeName = communicateInterfaceTypeName;
                GlobalTimedoutSetting = globalTimedoutSetting;
                WrappedExceptionTypeName = wrappedExceptionTypeName;
                InterfaceTypeName = interfaceTypeName;
                LocalExceptionHandlingMode = localExceptionHandlingMode;
            }
        }

        void BuildEvent(EventInfo eventInfo, string underlayDelegateName, BuildEventParameter parameter, EventSubscritionMode eventSubscritionMode)
        {
            string eventName = NamingHelper.GetAssetName(eventInfo);

            //type
            var eventHandlerType = eventInfo.EventHandlerType;
            var eventHandlerTypeInfo = eventHandlerType.GetTypeInfo();
            var declaredMethodInfo = eventHandlerTypeInfo.GetDeclaredMethod("Invoke");
            var eventDelegateTypeName = eventHandlerType.GetFullName(parameter.GetTypeFullNameParameter, null);
            int timedoutSetting = eventInfo.GetCustomAttribute<CustomizedOperatingTimedoutTimeAttribute>()?.MillisecondsTimeout ?? parameter.GlobalTimedoutSetting;

            //event and delegate object define
            if (eventSubscritionMode == EventSubscritionMode.SubscribeOnStartAndKeep)
            {
                underlayDelegateName = eventInfo.Name;
            }
            else
            {
                parameter.MemberLevelBuilder.Append(eventDelegateTypeName).Append(" ").Append(underlayDelegateName).AppendLine(";");
            }
            parameter.MemberLevelBuilder.Append("public event ").Append(eventDelegateTypeName).Append(" ").Append(eventInfo.Name);
            if (eventSubscritionMode == EventSubscritionMode.SubscribeOnStartAndKeep)
            {
                parameter.MemberLevelBuilder.AppendLine(";");
                string messageIdValueName = NamingHelper.GetRandomName("messageId");
                parameter.AfterInitializedSourcePhase1Builder
                    .Append("var ").Append(messageIdValueName).AppendLine(" = System.Guid.NewGuid();")
                    .Append(parameter.ResponderName).Append(".Prepare(").Append(messageIdValueName).Append(");\ntry\n{\n((")
                    .Append(parameter.CommunicateInterfaceTypeName)
                    .Append(")this).SendMessageCallback(SecretNest.RemoteAgency.MessageType.EventAdd, \"")
                    .Append(eventName).Append("\", ")
                    .Append(messageIdValueName).Append(", false, default(")
                    .Append(parameter.EntitySerializedTypeName).AppendLine("), null);\n}\ncatch\n{")
                    .Append(parameter.ResponderName).Append(".Remove(").Append(messageIdValueName).AppendLine(");\nthrow;\n}");
                parameter.AfterInitializedSourcePhase2Builder
                    .Append(parameter.ResponderName).Append(".GetResult(").Append(messageIdValueName).Append(", ").Append(timedoutSetting)
                    .AppendLine(");");
            }
            else if (eventSubscritionMode == EventSubscritionMode.SubscribeOnFirstUseAndKeep)
            {
                string eventSubscribedName = NamingHelper.GetRandomName("eventSubscribed");
                string messageIdValueName = NamingHelper.GetRandomName("messageId");
                parameter.MemberLevelBuilder
                    .Append("\n{\nadd\n{\nlock (").Append(parameter.LockObjectName)
                    .Append(")\n{\nif (!").Append(eventSubscribedName).Append(")\n{\nvar ")
                    .Append(messageIdValueName).AppendLine(" = System.Guid.NewGuid();")
                    .Append(parameter.ResponderName).Append(".Prepare(").Append(messageIdValueName).Append(");\ntry\n{\n((")
                    .Append(parameter.CommunicateInterfaceTypeName)
                    .Append(")this).SendMessageCallback(SecretNest.RemoteAgency.MessageType.EventAdd, \"")
                    .Append(eventName).Append("\", ")
                    .Append(messageIdValueName).Append(", false, default(")
                    .Append(parameter.EntitySerializedTypeName).AppendLine("), null);\n}\ncatch\n{")
                    .Append(parameter.ResponderName).Append(".Remove(").Append(messageIdValueName).AppendLine(");\nthrow;\n}")
                    .Append(parameter.ResponderName).Append(".GetResult(").Append(messageIdValueName).Append(", ").Append(timedoutSetting).AppendLine(");")
                    .Append(eventSubscribedName).AppendLine(" = true;")
                    .Append(parameter.EventHandlerStateName).AppendLine(" = true;\n}")
                    .Append(underlayDelegateName).Append(" += value;\n}\n}\nremove\n{\nlock (").Append(parameter.LockObjectName)
                    .AppendLine(")\n{")
                    .Append(underlayDelegateName).AppendLine(" -= value;\n}\n}\n}");
                parameter.MemberLevelBuilder.Append("bool ").Append(eventSubscribedName).AppendLine(" = false;");
            }
            else //Dynamic
            {
                string messageIdValueName = NamingHelper.GetRandomName("messageId");
                parameter.MemberLevelBuilder
                    .Append("\n{\nadd\n{\nlock (").Append(parameter.LockObjectName)
                    .Append(")\n{\nif (").Append(underlayDelegateName).Append(" == null)\n{\nvar ")
                    .Append(messageIdValueName).AppendLine(" = System.Guid.NewGuid();")
                    .Append(parameter.ResponderName).Append(".Prepare(").Append(messageIdValueName).Append(");\ntry\n{\n((")
                    .Append(parameter.CommunicateInterfaceTypeName)
                    .Append(")this).SendMessageCallback(SecretNest.RemoteAgency.MessageType.EventAdd, \"")
                    .Append(eventName).Append("\", ")
                    .Append(messageIdValueName).Append(", false, default(")
                    .Append(parameter.EntitySerializedTypeName).AppendLine("), null);\n}\ncatch\n{")
                    .Append(parameter.ResponderName).Append(".Remove(").Append(messageIdValueName).AppendLine(");\nthrow;\n}")
                    .Append(parameter.ResponderName).Append(".GetResult(").Append(messageIdValueName).Append(", ").Append(timedoutSetting).AppendLine(");\n}")
                    .Append(underlayDelegateName).Append(" += value;\n}\n}\nremove\n{\nlock (").Append(parameter.LockObjectName)
                    .AppendLine(")\n{")
                    .Append(underlayDelegateName).Append(" -= value;\nif (").Append(underlayDelegateName).Append(" == null)\n{\nvar ")
                    .Append(messageIdValueName).AppendLine(" = System.Guid.NewGuid();")
                    .Append(parameter.ResponderName).Append(".Prepare(").Append(messageIdValueName).Append(");\ntry\n{\n((")
                    .Append(parameter.CommunicateInterfaceTypeName)
                    .Append(")this).SendMessageCallback(SecretNest.RemoteAgency.MessageType.EventRemove, \"")
                    .Append(eventName).Append("\", ")
                    .Append(messageIdValueName).Append(", false, default(")
                    .Append(parameter.EntitySerializedTypeName).AppendLine("), null);\n}\ncatch\n{")
                    .Append(parameter.ResponderName).Append(".Remove(").Append(messageIdValueName).AppendLine(");\nthrow;\n}")
                    .Append(parameter.ResponderName).Append(".GetResult(").Append(messageIdValueName).Append(", ").Append(timedoutSetting).AppendLine(");\n}\n}\n}\n}");
            }

            var declaredReturnType = declaredMethodInfo.ReturnType;
            string eventReturnValueName;

            //Parameters -> Properties
            List<ValueMapping> parameterMappings = new List<ValueMapping>();
            List<ValueMapping> returnMappings = new List<ValueMapping>();

            if (declaredReturnType == typeof(void))
            {
                eventReturnValueName = null;
            }
            else
            {
                string returnTypeName = declaredReturnType.GetFullName(parameter.GetTypeFullNameParameter, null);
                eventReturnValueName = NamingHelper.GetRandomName("result");
                string returnPropertyName = eventInfo.GetCustomAttribute<CustomizedReturnEntityPropertyNameAttribute>()?.EntityPropertyName ??
                    eventHandlerTypeInfo.GetCustomAttribute<CustomizedReturnEntityPropertyNameAttribute>()?.EntityPropertyName ?? "Result";
                returnMappings.Add(new ValueMapping(eventReturnValueName, returnPropertyName, returnTypeName, eventReturnValueName));
            }

            string parameterValueName = NamingHelper.GetRandomName("parameter");
            var parameters = declaredMethodInfo.GetParameters();
            string preCallCode;
            string invokeParameter;
            if (parameters.Length > 0)
            {
                StringBuilder preCallBuilder = new StringBuilder();
                List<string> invokeParameters = new List<string>();
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
                foreach (var attribute in eventInfo.GetCustomAttributes<EventParameterIgnoredAttribute>())
                {
                    if (!eventIgnoredParameters.ContainsKey(attribute.ParameterName))
                        eventIgnoredParameters.Add(attribute.ParameterName, new ParameterIgnoredAttribute(attribute));
                }
                for (int parameterIndex = 0; parameterIndex < parameters.Length; parameterIndex++)
                {
                    var para = parameters[parameterIndex];
                    var parameterType = para.ParameterType;
                    var parameterTypeInfo = parameterType.GetTypeInfo();

                    string typeName = parameterType.GetFullName(parameter.GetTypeFullNameParameter, null);
                    string propertyName;
                    if (!customizedEventParameterEntityPropertyNames.TryGetValue(para.Name, out propertyName))
                        propertyName = para.GetCustomAttribute<CustomizedParameterEntityPropertyNameAttribute>()?.EntityPropertyName ?? NamingHelper.MakeFirstUpper(para.Name);
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

                    if (parameterType.IsByRef)
                    {
                        string refValueName = NamingHelper.GetRandomName(para.Name);
                        if (isNotIgnoredReturn)
                            returnMappings.Add(para.Name, typeName, refValueName, propertyName);
                        if (para.IsOut)
                        {
                            preCallBuilder.Append(typeName).Append(" ").Append(refValueName).Append(" = default(").Append(typeName).AppendLine(");");
                            invokeParameters.Add(string.Format("out {0}", refValueName));
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
                            invokeParameters.Add(string.Format("ref {0}", refValueName));
                        }
                    }
                    else if (isNotIgnoredParameter)
                    {
                        propertyName = parameterMappings.Add(para.Name, typeName, null, propertyName);
                        invokeParameters.Add(string.Format("{0}.{1}", parameterValueName, propertyName));

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
                                var mapping = property.ExtractTwoWaySetting(parameterValueName + "." + propertyName, parameterType, parameter.GetTypeFullNameParameter);
                                if (mapping != null)
                                    returnMappings.Add(mapping);
                            }
                        }
                    }
                    else
                    {
                        invokeParameters.Add(string.Format("default({0})", typeName));
                    }
                }
                invokeParameter = string.Join(", ", invokeParameters);
                preCallCode = preCallBuilder.ToString();
            }
            else
            {
                invokeParameter = "";
                preCallCode = "";
            }

            //Parameter Entity
            string parameterEntityName = eventInfo.GetCustomAttribute<CustomizedParameterEntityAttribute>()?.EntityName ?? eventName + "Parameter";
            parameterEntityName = remoteAgencyEntityCodeBuilder.BuildEntity(parameterEntityName, parameter.EntityBaseTypeName,
                parameterMappings, null, false, parameter.ClassLevelBuilder,
                out _);

            //Return Entity
            string returnEntityName = eventInfo.GetCustomAttribute<CustomizedReturnEntityAttribute>()?.EntityName ?? eventName + "Return";
            returnEntityName = remoteAgencyEntityCodeBuilder.BuildEntity(returnEntityName, parameter.EntityBaseTypeName,
                returnMappings, null, true, parameter.ClassLevelBuilder,
                out string returnEntityValueBasedConstructorCallerCode);

            //Method
            string serializedResultValueName = NamingHelper.GetRandomName("serializedResult");
            string needSendValueName = NamingHelper.GetRandomName("needSend");
            string catchedExceptionValueName = NamingHelper.GetRandomName("exception");
            string wrappedCatchedExceptionValueName = NamingHelper.GetRandomName("wrappedException");
            string deserializingExceptionValueName = NamingHelper.GetRandomName("deserializingException");
            string wrappedDeserializingExceptionValueName = NamingHelper.GetRandomName("wrappedDeserializingException");
            string serializingExceptionValueName = NamingHelper.GetRandomName("serializingException");
            string wrappedSerializingExceptionValueName = NamingHelper.GetRandomName("wrappedSerializingException");
            string resultValueName = NamingHelper.GetRandomName("result");
            LocalExceptionHandlingMode localExceptionHandlingMode = eventInfo.GetCustomAttribute<LocalExceptionHandlingAttribute>()?.LocalExceptionHandlingMode ?? parameter.LocalExceptionHandlingMode;
            parameter.ProcessRaiseMessageSourceBuilder.Append("case \"").Append(eventName).AppendLine("\":\n{")
                .Append(parameter.EntitySerializedTypeName).Append(" ")
                .Append(serializedResultValueName).Append(" = default(")
                .Append(parameter.EntitySerializedTypeName).Append(");\nbool ")
                .Append(needSendValueName).Append(" = false;\nvar ")
                .Append(parameterValueName).Append(" = ")
                .Append(parameter.SerializingHelperName).Append(".DeserializeWithExceptionTolerance(data, typeof(")
                .Append(parameterEntityName).Append("), out var ")
                .Append(wrappedDeserializingExceptionValueName).Append(", out var ")
                .Append(deserializingExceptionValueName).Append(") as ").Append(parameterEntityName).Append(";\nif (")
                .Append(wrappedDeserializingExceptionValueName).AppendLine(" != null)\n{\nif (!isOneWay)\n{");
            CodeBuilderHelper.BuildSendingExceptionCode(parameter.ProcessRaiseMessageSourceBuilder,
                parameter.SerializingHelperName, localExceptionHandlingMode,
                parameter.CommunicateInterfaceTypeName, parameter.InterfaceTypeName, parameter.WrappedExceptionTypeName,
                "SecretNest.RemoteAgency.MessageType.Event", wrappedDeserializingExceptionValueName);
            parameter.ProcessRaiseMessageSourceBuilder.AppendLine("}"); //}: if (!isOneWay)
            CodeBuilderHelper.BuildRaisingOrRedirectingExceptionWithNullCheckCode(parameter.ProcessRaiseMessageSourceBuilder,
                localExceptionHandlingMode,
                parameter.CommunicateInterfaceTypeName, parameter.InterfaceTypeName,
                "SecretNest.RemoteAgency.MessageType.Event", deserializingExceptionValueName);
            parameter.ProcessRaiseMessageSourceBuilder.AppendLine("}\nelse\n{\ntry\n{")
                .Append(preCallCode);
            if (eventReturnValueName != null)
            {
                parameter.ProcessRaiseMessageSourceBuilder.Append("var ").Append(eventReturnValueName).Append(" = ");
            }
            parameter.ProcessRaiseMessageSourceBuilder
                .Append(underlayDelegateName).Append("?.Invoke(").Append(invokeParameter).Append(");\nif (!isOneWay)\n{\nvar ")
                .Append(resultValueName).Append(returnEntityValueBasedConstructorCallerCode)
                .Append(serializedResultValueName).Append(" = ")
                .Append(parameter.SerializingHelperName).Append(".SerializeWithExceptionTolerance(")
                .Append(resultValueName).Append(", typeof(").Append(returnEntityName).Append("), out var ")
                .Append(serializingExceptionValueName).Append(");\nif (")
                .Append(serializingExceptionValueName).AppendLine(" != null)\n{\nvar ")
                .Append(wrappedSerializingExceptionValueName).Append(" = ").Append(parameter.WrappedExceptionTypeName).Append(".Create(").Append(serializingExceptionValueName).AppendLine(");");
            CodeBuilderHelper.BuildSendingExceptionCode(parameter.ProcessRaiseMessageSourceBuilder,
                parameter.SerializingHelperName, localExceptionHandlingMode,
                parameter.CommunicateInterfaceTypeName, parameter.InterfaceTypeName, parameter.WrappedExceptionTypeName,
                "SecretNest.RemoteAgency.MessageType.Event", wrappedSerializingExceptionValueName);
            CodeBuilderHelper.BuildRaisingOrRedirectingExceptionWithNullCheckCode(parameter.ProcessRaiseMessageSourceBuilder,
                localExceptionHandlingMode,
                parameter.CommunicateInterfaceTypeName, parameter.InterfaceTypeName,
                "SecretNest.RemoteAgency.MessageType.Event", serializingExceptionValueName);
            parameter.ProcessRaiseMessageSourceBuilder.AppendLine("}\nelse\n{")
                .Append(needSendValueName).Append(" = true;\n}\n}\n}\ncatch (Exception ")
                .Append(catchedExceptionValueName).Append(")\n{\nif (!isOneWay)\n{\n var ")
                .Append(wrappedCatchedExceptionValueName).Append(" = ").Append(parameter.WrappedExceptionTypeName).Append(".Create(").Append(catchedExceptionValueName).AppendLine(");");
            CodeBuilderHelper.BuildSendingExceptionCode(parameter.ProcessRaiseMessageSourceBuilder,
                parameter.SerializingHelperName, localExceptionHandlingMode,
                parameter.CommunicateInterfaceTypeName, parameter.InterfaceTypeName, parameter.WrappedExceptionTypeName,
                "SecretNest.RemoteAgency.MessageType.Event", wrappedCatchedExceptionValueName);
            parameter.ProcessRaiseMessageSourceBuilder.AppendLine("}"); //}: if (!isOneWay)
            CodeBuilderHelper.BuildRaisingOrRedirectingExceptionWithNullCheckCode(parameter.ProcessRaiseMessageSourceBuilder,
                localExceptionHandlingMode,
                parameter.CommunicateInterfaceTypeName, parameter.InterfaceTypeName,
                "SecretNest.RemoteAgency.MessageType.Event", catchedExceptionValueName);
            parameter.ProcessRaiseMessageSourceBuilder.Append("}\nif (") //}: catch
                .Append(needSendValueName).AppendLine(")\n{");
            CodeBuilderHelper.BuildSendingSerializedValueCode(parameter.ProcessRaiseMessageSourceBuilder,
                localExceptionHandlingMode,
                parameter.CommunicateInterfaceTypeName, parameter.InterfaceTypeName,
                "SecretNest.RemoteAgency.MessageType.Event", serializedResultValueName);
            parameter.ProcessRaiseMessageSourceBuilder.AppendLine("}\n}\n}\nbreak;"); //}: if, else(no error in deserialize), case
        }
    }
}
