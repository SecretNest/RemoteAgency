using SecretNest.RemoteAgency.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SecretNest.RemoteAgency
{
    public partial class ProxyCreator<TSerialized, TEntityBase> : IAssemblyCacheOperatings<TSerialized, TEntityBase> where TEntityBase : class
    {
        class BuildPropertyParameter
        {
            public readonly StringBuilder MemberLevelBuilder;
            public readonly StringBuilder ClassLevelBuilder;
            public readonly StringBuilder ProcessGetMessageSourceBuilder = new StringBuilder();
            public readonly StringBuilder ProcessSetMessageSourceBuilder = new StringBuilder();
            public readonly GetTypeFullNameParameter GetTypeFullNameParameter;
            public readonly string EntityBaseTypeName;
            public readonly string ResponderName;
            public readonly string SerializingHelperName;
            public readonly string CommunicateInterfaceTypeName;
            public readonly int GlobalTimedoutSetting;
            public readonly string InterfaceTypeName;
            public readonly LocalExceptionHandlingMode LocalExceptionHandlingMode;

            public BuildPropertyParameter(StringBuilder memberLevelBuilder, StringBuilder classLevelBuilder, GetTypeFullNameParameter getTypeFullNameParameter, string responderName, string entityBaseTypeName, string serializingHelperName, string communicateInterfaceTypeName, int globalTimedoutSetting, string interfaceTypeName, LocalExceptionHandlingMode localExceptionHandlingMode)
            {
                MemberLevelBuilder = memberLevelBuilder;
                ClassLevelBuilder = classLevelBuilder;
                GetTypeFullNameParameter = getTypeFullNameParameter;
                ResponderName = responderName;
                EntityBaseTypeName = entityBaseTypeName;
                SerializingHelperName = serializingHelperName;
                CommunicateInterfaceTypeName = communicateInterfaceTypeName;
                GlobalTimedoutSetting = globalTimedoutSetting;
                InterfaceTypeName = interfaceTypeName;
                LocalExceptionHandlingMode = localExceptionHandlingMode;
            }
        }

        void BuildProperty(PropertyInfo propertyInfo, BuildPropertyParameter parameter)
        {
            string propertyName = NamingHelper.GetAssetName(propertyInfo);

            //type
            var valueType = propertyInfo.PropertyType;
            var valueTypeName = valueType.GetFullName(parameter.GetTypeFullNameParameter, null);
            string messageIdValueName = NamingHelper.GetRandomName("messageId");
            string serializedRequestValueName = NamingHelper.GetRandomName("serializedRequestValue");
            string requestValueName = NamingHelper.GetRandomName("requestValue");
            string responseValueName = NamingHelper.GetRandomName("responseValue");
            bool isOneWay = propertyInfo.GetCustomAttribute<CustomizedOneWayOperatingAttribute>() != null;

            parameter.MemberLevelBuilder.Append("public ").Append(valueTypeName).Append(" ").Append(propertyInfo.Name).AppendLine(" \n{");

            if (propertyInfo.GetMethod != null)
            {
                List<ValueMapping> entityMappings = new List<ValueMapping>();
                string propertyGetRequestEntityName = propertyInfo.GetCustomAttribute<CustomizedPropertyGetRequestEntityAttribute>()?.EntityName ?? propertyName + "GetRequest";
                string propertyGetResponseEntityName = propertyInfo.GetCustomAttribute<CustomizedPropertyGetResponseEntityAttribute>()?.EntityName ?? propertyName + "GetResponse";
                string propertyGetResponseEntityPropertyName = propertyInfo.GetCustomAttribute<CustomizedPropertyGetResponsePropertyNameAttribute>()?.EntityPropertyName ?? "Value";
                propertyGetRequestEntityName = remoteAgencyEntityCodeBuilder.BuildEntity(propertyGetRequestEntityName, parameter.EntityBaseTypeName,
                    entityMappings, null, true,  parameter.ClassLevelBuilder, out string propertyGetConstructorCallerCode);
                entityMappings.Add(new ValueMapping("value", propertyGetResponseEntityPropertyName, valueTypeName, null));
                propertyGetResponseEntityName = remoteAgencyEntityCodeBuilder.BuildEntity(propertyGetResponseEntityName, parameter.EntityBaseTypeName,
                    entityMappings, null, false,  parameter.ClassLevelBuilder, out _);

                int timedoutSetting = propertyInfo.GetCustomAttribute<CustomizedOperatingTimedoutTimeAttribute>()?.MillisecondsTimeout ?? parameter.GlobalTimedoutSetting;
                parameter.MemberLevelBuilder.Append("get\n{\nSystem.Guid ").Append(messageIdValueName).Append(" = System.Guid.NewGuid();\nvar ")
                    .Append(requestValueName).Append(propertyGetConstructorCallerCode).Append("var ")
                    .Append(serializedRequestValueName).Append(" = ")
                    .Append(parameter.SerializingHelperName).Append(".Serialize(")
                    .Append(requestValueName).Append(", typeof(").Append(propertyGetRequestEntityName).AppendLine("));")
                    .Append(parameter.ResponderName).Append(".Prepare(")
                    .Append(messageIdValueName).Append(");\ntry\n{\n((")
                    .Append(parameter.CommunicateInterfaceTypeName).Append(")this).SendMessageCallback(SecretNest.RemoteAgency.MessageType.PropertyGet, \"")
                    .Append(propertyName).Append("\", ")
                    .Append(messageIdValueName).Append(", false, ")
                    .Append(serializedRequestValueName).AppendLine(", null);\n}\ncatch\n{")
                    .Append(parameter.ResponderName).Append(".Remove(").Append(messageIdValueName).Append(");\nthrow;\n}\nvar ")
                    .Append(responseValueName).Append(" = (")
                    .Append(propertyGetResponseEntityName).Append(")")
                    .Append(parameter.ResponderName).Append(".GetResult(").Append(messageIdValueName).Append(", ").Append(timedoutSetting).Append(");\nreturn ")
                    .Append(responseValueName).Append(".").Append(propertyGetResponseEntityPropertyName).AppendLine(";\n}");

                string deserializingExceptionValueName = NamingHelper.GetRandomName("deserializingException");
                string wrappedDeserializingExceptionValueName = NamingHelper.GetRandomName("wrappedDeserializingException");
                parameter.ProcessGetMessageSourceBuilder.Append("case \"").Append(propertyName).Append("\":\n{\nvar ")
                    .Append(responseValueName).Append(" = ")
                    .Append(parameter.SerializingHelperName)
                    .Append(".DeserializeWithExceptionTolerance(data, typeof(")
                    .Append(propertyGetResponseEntityName)
                    .Append("), out var ")
                    .Append(wrappedDeserializingExceptionValueName)
                    .Append(", out var ")
                    .Append(deserializingExceptionValueName)
                    .Append(") as ").Append(propertyGetResponseEntityName).Append(";\nif (")
                    .Append(wrappedDeserializingExceptionValueName)
                    .AppendLine(" == null)\n{")
                    .Append(parameter.ResponderName)
                    .Append(".SetResult(messageId, ")
                    .Append(responseValueName)
                    .AppendLine(");\n}\nelse\n{")
                    .Append(parameter.ResponderName)
                    .Append(".SetException(messageId, ")
                    .Append(wrappedDeserializingExceptionValueName)
                    .Append(");\nthrow ")
                    .Append(deserializingExceptionValueName)
                    .AppendLine(";\n}\n}\nbreak;");
            }

            if (propertyInfo.SetMethod != null)
            {
                List<ValueMapping> entityMappings = new List<ValueMapping>();
                string propertySetRequestEntityName = propertyInfo.GetCustomAttribute<CustomizedPropertySetRequestEntityAttribute>()?.EntityName ?? propertyName + "SetRequest";
                string propertySetRequestEntityPropertyName = propertyInfo.GetCustomAttribute<CustomizedPropertySetRequestPropertyNameAttribute>()?.EntityPropertyName ?? "Value";
                string propertySetResponseEntityName;
                if (!isOneWay)
                {
                    propertySetResponseEntityName = propertyInfo.GetCustomAttribute<CustomizedPropertySetResponseEntityAttribute>()?.EntityName ?? propertyName + "SetResponse";
                    propertySetResponseEntityName = remoteAgencyEntityCodeBuilder.BuildEntity(propertySetResponseEntityName, parameter.EntityBaseTypeName,
                    entityMappings, null, false, parameter.ClassLevelBuilder, out _);
                }
                else
                {
                    propertySetResponseEntityName = null;
                }
                entityMappings.Add(new ValueMapping("value", propertySetRequestEntityPropertyName, valueTypeName, "value"));
                propertySetRequestEntityName = remoteAgencyEntityCodeBuilder.BuildEntity(propertySetRequestEntityName, parameter.EntityBaseTypeName,
                    entityMappings, null, true, parameter.ClassLevelBuilder, out string propertySetConstructorCallerCode);

                StringBuilder sendingMessageBackBuilder = new StringBuilder("((")
                    .Append(parameter.CommunicateInterfaceTypeName).Append(")this).SendMessageCallback(SecretNest.RemoteAgency.MessageType.PropertySet, \"")
                    .Append(propertyName).Append("\", ")
                    .Append(messageIdValueName).Append(", ")
                    .Append(isOneWay ? "true" : "false").Append(", ")
                    .Append(serializedRequestValueName).AppendLine(", null);");
                string sendingMessageBack = sendingMessageBackBuilder.ToString();

                parameter.MemberLevelBuilder.Append("set\n{\nSystem.Guid ").Append(messageIdValueName).Append(" = System.Guid.NewGuid();\nvar ")
                    .Append(requestValueName).Append(propertySetConstructorCallerCode).Append("var ")
                    .Append(serializedRequestValueName).Append(" = ")
                    .Append(parameter.SerializingHelperName).Append(".Serialize(")
                    .Append(requestValueName).Append(", typeof(").Append(propertySetRequestEntityName).AppendLine("));");

                if (!isOneWay)
                {
                    int timedoutSetting = propertyInfo.GetCustomAttribute<CustomizedOperatingTimedoutTimeAttribute>()?.MillisecondsTimeout ?? parameter.GlobalTimedoutSetting;
                    string deserializingExceptionValueName = NamingHelper.GetRandomName("deserializingException");
                    string wrappedDeserializingExceptionValueName = NamingHelper.GetRandomName("wrappedDeserializingException");
                    parameter.ProcessSetMessageSourceBuilder.Append("case \"").Append(propertyName).Append("\":\n{\n var ")
                        .Append(responseValueName).Append(" = ")
                        .Append(parameter.SerializingHelperName)
                        .Append(".DeserializeWithExceptionTolerance(data, typeof(")
                        .Append(propertySetResponseEntityName)
                        .Append("), out var ")
                        .Append(wrappedDeserializingExceptionValueName)
                        .Append(", out var ")
                        .Append(deserializingExceptionValueName)
                        .Append(") as ").Append(propertySetResponseEntityName).Append(";\nif (")
                        .Append(wrappedDeserializingExceptionValueName)
                        .AppendLine(" == null)\n{")
                        .Append(parameter.ResponderName)
                        .Append(".SetResult(messageId, ")
                        .Append(responseValueName)
                        .AppendLine(");\n}\nelse\n{")
                        .Append(parameter.ResponderName)
                        .Append(".SetException(messageId, ")
                        .Append(wrappedDeserializingExceptionValueName)
                        .Append(");\nthrow ")
                        .Append(deserializingExceptionValueName)
                        .AppendLine(";\n}\n}\nbreak;");

                    parameter.MemberLevelBuilder.Append(parameter.ResponderName).Append(".Prepare(").Append(messageIdValueName).AppendLine(");\ntry\n{")
                        .Append(sendingMessageBack).Append("}\ncatch\n{")
                        .Append(parameter.ResponderName).Append(".Remove(").Append(messageIdValueName).Append(");\nthrow;\n}\nvar ").Append(responseValueName).Append(" = ")
                        .Append(parameter.ResponderName).Append(".GetResult(").Append(messageIdValueName).Append(", ").Append(timedoutSetting).AppendLine(");");
                }
                else
                {
                    parameter.MemberLevelBuilder.Append(sendingMessageBack);
                }
                parameter.MemberLevelBuilder.AppendLine("}");
            }
            parameter.MemberLevelBuilder.AppendLine("}");
        }
    }
}
