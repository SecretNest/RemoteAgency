using SecretNest.RemoteAgency.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SecretNest.RemoteAgency
{
    public partial class ServiceWrapperCreator<TSerialized, TEntityBase> : IAssemblyCacheOperatings<TSerialized, TEntityBase> where TEntityBase : class
    {
        class BuildPropertyParameter
        {
            public readonly StringBuilder ClassLevelBuilder;
            public readonly StringBuilder ProcessGetMessageSourceBuilder = new StringBuilder();
            public readonly StringBuilder ProcessSetMessageSourceBuilder = new StringBuilder();
            public readonly HashSet<string> usedAssetNames = new HashSet<string>();
            public readonly GetTypeFullNameParameter GetTypeFullNameParameter;
            public readonly string EntityBaseTypeName;
            public readonly string EntitySerializedTypeName;
            public readonly string SerializingHelperName;
            public readonly string CommunicateInterfaceTypeName;
            public readonly string ServiceObjectName;
            public readonly string WrappedExceptionTypeName;

            public BuildPropertyParameter(StringBuilder classLevelBuilder, GetTypeFullNameParameter getTypeFullNameParameter, string entityBaseTypeName,
                string entitySerializedTypeName, string serializingHelperName, string communicateInterfaceTypeName, string serviceObjectName, 
                string wrappedExceptionTypeName)
            {
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

        void BuildProperty(PropertyInfo propertyInfo, BuildPropertyParameter parameter, string interfaceTypeName)
        {
            string propertyName = NamingHelper.GetAssetName(propertyInfo);
            if (parameter.usedAssetNames.Contains(propertyName))
            {
                throw new TypeCreatingException(new TypeCreatingExceptionRecord[] { new TypeCreatingExceptionRecord(
                    "-1",
                    "Property name duplications detected.\nUsing CustomizedAssetName attribute on this property with unique name is required.")
                });
            }
            parameter.usedAssetNames.Add(propertyName);

            var valueType = propertyInfo.PropertyType;
            var valueTypeName = valueType.GetFullName(parameter.GetTypeFullNameParameter, null);
            string messageIdValueName = NamingHelper.GetRandomName("messageId");
            string serializedResponseValueName = NamingHelper.GetRandomName("serializedResponseValue");
            string requestValueName = NamingHelper.GetRandomName("requestValue");
            string responseValueName = NamingHelper.GetRandomName("responseValue");
            string catchedExceptionValueName = NamingHelper.GetRandomName("exception");
            string wrappedCatchedExceptionValueName = NamingHelper.GetRandomName("wrappedException");
            string needSendValueName = NamingHelper.GetRandomName("needSend");
            LocalExceptionHandlingMode localExceptionHandlingMode = propertyInfo.GetCustomAttribute<LocalExceptionHandlingAttribute>()?.LocalExceptionHandlingMode ?? LocalExceptionHandlingMode.Suppress;

            if (propertyInfo.GetMethod != null)
            {
                List<ValueMapping> entityMappings = new List<ValueMapping>();
                string propertyGetRequestEntityName = propertyInfo.GetCustomAttribute<CustomizedPropertyGetRequestEntityAttribute>()?.EntityName ?? propertyName + "GetRequest";
                string propertyGetResponseEntityName = propertyInfo.GetCustomAttribute<CustomizedPropertyGetResponseEntityAttribute>()?.EntityName ?? propertyName + "GetResponse";
                string propertyGetResponseEntityPropertyName = propertyInfo.GetCustomAttribute<CustomizedPropertyGetResponsePropertyNameAttribute>()?.EntityPropertyName ?? "Value";
                string valueName = NamingHelper.GetRandomName("value");
                string deserializingExceptionValueName = NamingHelper.GetRandomName("deserializingException");
                string wrappedDeserializingExceptionValueName = NamingHelper.GetRandomName("wrappedDeserializingException");
                string serializingExceptionValueName = NamingHelper.GetRandomName("serializingException");
                string wrappedSerializingExceptionValueName = NamingHelper.GetRandomName("wrappedSerializingException");

                propertyGetRequestEntityName = remoteAgencyEntityCodeBuilder.BuildEntity(propertyGetRequestEntityName, parameter.EntityBaseTypeName,
                    entityMappings, null, false, parameter.ClassLevelBuilder, out _);
                entityMappings.Add(new ValueMapping("value", propertyGetResponseEntityPropertyName, valueTypeName, valueName));
                propertyGetResponseEntityName = remoteAgencyEntityCodeBuilder.BuildEntity(propertyGetResponseEntityName, parameter.EntityBaseTypeName,
                    entityMappings, null, true, parameter.ClassLevelBuilder, out string propertyGetConstructorCallerCode);

                parameter.ProcessGetMessageSourceBuilder.Append("case \"").Append(propertyName).AppendLine("\":\n{")
                    .Append(parameter.EntitySerializedTypeName).Append(" ")
                    .Append(serializedResponseValueName).Append(" = default(")
                    .Append(parameter.EntitySerializedTypeName).Append(");\nbool ")
                    .Append(needSendValueName).Append(" = false;\nvar ")
                    .Append(requestValueName).Append(" = ")
                    .Append(parameter.SerializingHelperName).Append(".DeserializeWithExceptionTolerance(data, typeof(")
                    .Append(propertyGetRequestEntityName).Append("), out var ")
                    .Append(wrappedDeserializingExceptionValueName).Append(", out var ")
                    .Append(deserializingExceptionValueName).Append(") as ")
                    .Append(propertyGetRequestEntityName).Append(";\nif (")
                    .Append(wrappedDeserializingExceptionValueName).AppendLine(" != null)\n{");
                CodeBuilderHelper.BuildSendingExceptionCode(parameter.ProcessGetMessageSourceBuilder,
                    parameter.SerializingHelperName, localExceptionHandlingMode,
                    parameter.CommunicateInterfaceTypeName, interfaceTypeName, parameter.WrappedExceptionTypeName,
                    "SecretNest.RemoteAgency.MessageType.PropertyGet", wrappedDeserializingExceptionValueName);
                CodeBuilderHelper.BuildRaisingOrRedirectingExceptionWithNullCheckCode(parameter.ProcessGetMessageSourceBuilder,
                    localExceptionHandlingMode,
                    parameter.CommunicateInterfaceTypeName, interfaceTypeName,
                    "SecretNest.RemoteAgency.MessageType.PropertyGet", deserializingExceptionValueName);
                parameter.ProcessGetMessageSourceBuilder.Append("}\nelse\n{\ntry\n{\nvar ")
                    .Append(valueName).Append(" = ((")
                    .Append(interfaceTypeName).Append(")")
                    .Append(parameter.ServiceObjectName).Append(").").Append(propertyInfo.Name).Append(";\nvar ")
                    .Append(responseValueName).Append(propertyGetConstructorCallerCode)
                    .Append(serializedResponseValueName).Append(" = ")
                    .Append(parameter.SerializingHelperName).Append(".SerializeWithExceptionTolerance(")
                    .Append(responseValueName).Append(", typeof(").Append(propertyGetResponseEntityName).Append("), out var ")
                    .Append(serializingExceptionValueName).Append(");\nif (")
                    .Append(serializingExceptionValueName).Append(" != null)\n{\nvar ")
                    .Append(wrappedSerializingExceptionValueName).Append(" = ").Append(parameter.WrappedExceptionTypeName).Append(".Create(").Append(serializingExceptionValueName).AppendLine(");");
                CodeBuilderHelper.BuildSendingExceptionCode(parameter.ProcessGetMessageSourceBuilder,
                    parameter.SerializingHelperName, localExceptionHandlingMode,
                    parameter.CommunicateInterfaceTypeName, interfaceTypeName, parameter.WrappedExceptionTypeName,
                    "SecretNest.RemoteAgency.MessageType.PropertyGet", wrappedSerializingExceptionValueName);
                CodeBuilderHelper.BuildRaisingOrRedirectingExceptionWithNullCheckCode(parameter.ProcessGetMessageSourceBuilder,
                    localExceptionHandlingMode,
                    parameter.CommunicateInterfaceTypeName, interfaceTypeName,
                    "SecretNest.RemoteAgency.MessageType.PropertyGet", serializingExceptionValueName);
                parameter.ProcessGetMessageSourceBuilder.AppendLine("}\nelse\n{")
                    .Append(needSendValueName).Append(" = true;\n}\n}\ncatch (Exception ").Append(catchedExceptionValueName).Append(")\n{var ")
                    .Append(wrappedCatchedExceptionValueName).Append(" = ").Append(parameter.WrappedExceptionTypeName).Append(".Create(").Append(catchedExceptionValueName).AppendLine(");");
                CodeBuilderHelper.BuildSendingExceptionCode(parameter.ProcessGetMessageSourceBuilder,
                    parameter.SerializingHelperName, localExceptionHandlingMode,
                    parameter.CommunicateInterfaceTypeName, interfaceTypeName, parameter.WrappedExceptionTypeName,
                    "SecretNest.RemoteAgency.MessageType.PropertyGet", wrappedCatchedExceptionValueName);
                CodeBuilderHelper.BuildRaisingOrRedirectingExceptionWithNullCheckCode(parameter.ProcessGetMessageSourceBuilder,
                    localExceptionHandlingMode,
                    parameter.CommunicateInterfaceTypeName, interfaceTypeName,
                    "SecretNest.RemoteAgency.MessageType.PropertyGet", catchedExceptionValueName);
                parameter.ProcessGetMessageSourceBuilder.Append("}\nif (") //}: catch
                    .Append(needSendValueName).AppendLine(")\n{");
                CodeBuilderHelper.BuildSendingSerializedValueCode(parameter.ProcessGetMessageSourceBuilder,
                    localExceptionHandlingMode,
                    parameter.CommunicateInterfaceTypeName, interfaceTypeName,
                    "SecretNest.RemoteAgency.MessageType.PropertyGet", serializedResponseValueName);
                parameter.ProcessGetMessageSourceBuilder.AppendLine("}\n}\n}\nbreak;"); //}: if, else(no error in deserialize), case
            }

            if (propertyInfo.SetMethod != null)
            {
                List<ValueMapping> entityMappings = new List<ValueMapping>();
                string propertySetRequestEntityName = propertyInfo.GetCustomAttribute<CustomizedPropertySetRequestEntityAttribute>()?.EntityName ?? propertyName + "SetRequest";
                string propertySetRequestEntityPropertyName = propertyInfo.GetCustomAttribute<CustomizedPropertySetRequestPropertyNameAttribute>()?.EntityPropertyName ?? "Value";
                string propertySetResponseEntityName = propertyInfo.GetCustomAttribute<CustomizedPropertySetResponseEntityAttribute>()?.EntityName ?? propertyName + "SetResponse";
                string deserializingExceptionValueName = NamingHelper.GetRandomName("deserializingException");
                string wrappedDeserializingExceptionValueName = NamingHelper.GetRandomName("wrappedDeserializingException");
                string serializingExceptionValueName = NamingHelper.GetRandomName("serializingException");
                string wrappedSerializingExceptionValueName = NamingHelper.GetRandomName("wrappedSerializingException");
                propertySetResponseEntityName = remoteAgencyEntityCodeBuilder.BuildEntity(propertySetResponseEntityName, parameter.EntityBaseTypeName,
                    entityMappings, null, true, parameter.ClassLevelBuilder, out string propertySetConstructorCallerCode);
                entityMappings.Add(new ValueMapping("value", propertySetRequestEntityPropertyName, valueTypeName, null));
                propertySetRequestEntityName = remoteAgencyEntityCodeBuilder.BuildEntity(propertySetRequestEntityName, parameter.EntityBaseTypeName,
                    entityMappings, null, false, parameter.ClassLevelBuilder, out _);

                parameter.ProcessSetMessageSourceBuilder.Append("case \"").Append(propertyName).AppendLine("\":\n{")
                    .Append(parameter.EntitySerializedTypeName).Append(" ")
                    .Append(serializedResponseValueName).Append(" = default(")
                    .Append(parameter.EntitySerializedTypeName).Append(");\nbool ")
                    .Append(needSendValueName).Append(" = false;\nvar ")
                    .Append(requestValueName).Append(" = ")
                    .Append(parameter.SerializingHelperName).Append(".DeserializeWithExceptionTolerance(data, typeof(")
                    .Append(propertySetRequestEntityName).Append("), out var ")
                    .Append(wrappedDeserializingExceptionValueName).Append(", out var ")
                    .Append(deserializingExceptionValueName).Append(") as ")
                    .Append(propertySetRequestEntityName).Append(";\nif (")
                    .Append(wrappedDeserializingExceptionValueName).AppendLine(" != null)\n{\nif (!isOneWay)\n{");
                CodeBuilderHelper.BuildSendingExceptionCode(parameter.ProcessSetMessageSourceBuilder,
                    parameter.SerializingHelperName, localExceptionHandlingMode,
                    parameter.CommunicateInterfaceTypeName, interfaceTypeName, parameter.WrappedExceptionTypeName,
                    "SecretNest.RemoteAgency.MessageType.PropertySet", wrappedDeserializingExceptionValueName);
                parameter.ProcessSetMessageSourceBuilder.AppendLine("}"); //}: if (!isOneWay)
                CodeBuilderHelper.BuildRaisingOrRedirectingExceptionWithNullCheckCode(parameter.ProcessSetMessageSourceBuilder,
                    localExceptionHandlingMode,
                    parameter.CommunicateInterfaceTypeName, interfaceTypeName,
                    "SecretNest.RemoteAgency.MessageType.PropertySet", deserializingExceptionValueName);
                parameter.ProcessSetMessageSourceBuilder.AppendLine("}\nelse\n{");
                parameter.ProcessSetMessageSourceBuilder.Append("try\n{\n((")
                    .Append(interfaceTypeName).Append(")")
                    .Append(parameter.ServiceObjectName).Append(").").Append(propertyInfo.Name).Append(" = ")
                    .Append(requestValueName).Append(".")
                    .Append(propertySetRequestEntityPropertyName).Append(";\nif (!isOneWay)\n{\nvar ")
                    .Append(responseValueName).Append(propertySetConstructorCallerCode)
                    .Append(serializedResponseValueName).Append(" = ")
                    .Append(parameter.SerializingHelperName).Append(".SerializeWithExceptionTolerance(")
                    .Append(responseValueName).Append(", typeof(").Append(propertySetResponseEntityName).Append("), out var ")
                    .Append(serializingExceptionValueName).Append(");\nif (")
                    .Append(serializingExceptionValueName).Append(" != null)\n{\nvar ")
                    .Append(wrappedSerializingExceptionValueName).Append(" = ").Append(parameter.WrappedExceptionTypeName).Append(".Create(").Append(serializingExceptionValueName).AppendLine(");");
                CodeBuilderHelper.BuildSendingExceptionCode(parameter.ProcessSetMessageSourceBuilder,
                    parameter.SerializingHelperName, localExceptionHandlingMode,
                    parameter.CommunicateInterfaceTypeName, interfaceTypeName, parameter.WrappedExceptionTypeName,
                    "SecretNest.RemoteAgency.MessageType.PropertySet", wrappedSerializingExceptionValueName);
                CodeBuilderHelper.BuildRaisingOrRedirectingExceptionWithNullCheckCode(parameter.ProcessSetMessageSourceBuilder,
                    localExceptionHandlingMode,
                    parameter.CommunicateInterfaceTypeName, interfaceTypeName,
                    "SecretNest.RemoteAgency.MessageType.PropertySet", serializingExceptionValueName);
                parameter.ProcessSetMessageSourceBuilder.AppendLine("}\nelse\n{")
                    .Append(needSendValueName).Append(" = true;\n}\n}\n}\ncatch (Exception ").Append(catchedExceptionValueName).Append(")\n{\nif (!isOneWay)\n{\n var ")
                    .Append(wrappedCatchedExceptionValueName).Append(" = ").Append(parameter.WrappedExceptionTypeName).Append(".Create(").Append(catchedExceptionValueName).AppendLine(");");
                CodeBuilderHelper.BuildSendingExceptionCode(parameter.ProcessSetMessageSourceBuilder,
                    parameter.SerializingHelperName, localExceptionHandlingMode,
                    parameter.CommunicateInterfaceTypeName, interfaceTypeName, parameter.WrappedExceptionTypeName,
                    "SecretNest.RemoteAgency.MessageType.PropertySet", wrappedCatchedExceptionValueName);
                parameter.ProcessSetMessageSourceBuilder.AppendLine("}"); //}: if (!isOneWay)
                CodeBuilderHelper.BuildRaisingOrRedirectingExceptionWithNullCheckCode(parameter.ProcessSetMessageSourceBuilder,
                    localExceptionHandlingMode,
                    parameter.CommunicateInterfaceTypeName, interfaceTypeName,
                    "SecretNest.RemoteAgency.MessageType.PropertySet", catchedExceptionValueName);
                parameter.ProcessSetMessageSourceBuilder.Append("}\nif (") //}: catch
                    .Append(needSendValueName).AppendLine(")\n{");
                CodeBuilderHelper.BuildSendingSerializedValueCode(parameter.ProcessSetMessageSourceBuilder,
                    localExceptionHandlingMode,
                    parameter.CommunicateInterfaceTypeName, interfaceTypeName,
                    "SecretNest.RemoteAgency.MessageType.PropertySet", serializedResponseValueName);
                parameter.ProcessSetMessageSourceBuilder.AppendLine("}\n}\n}\nbreak;"); //}: if, else(no error in deserialize), case
            }
        }
    }
}
