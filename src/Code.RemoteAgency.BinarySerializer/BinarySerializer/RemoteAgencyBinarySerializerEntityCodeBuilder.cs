using SecretNest.RemoteAgency.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretNest.RemoteAgency.BinarySerializer
{    /// <summary>
    /// Provides code generating for entity classes working with <see cref="RemoteAgencyBinarySerializer"/>
    /// </summary>
    public class RemoteAgencyBinarySerializerEntityCodeBuilder : EntityCodeBuilderBase
    {
       /// <inheritdoc />
        public override string BuildEntity(string className, string entityBaseTypeFullName, string fullNameOfIRemoteAgencyMessage,
            List<Attribute> interfaceLevelAttributes, List<Attribute> assetLevelAttributes, List<Attribute> delegateLevelAttributes, List<ValueMapping> values,
            Dictionary<string, Type> usedGenerics, bool needValueBasedConstructor, StringBuilder sourceCodeBuilder,
            out string valueBasedConstructorCallerCode)
        {
            string entityClassName;

            sourceCodeBuilder.AppendLine("[Serializable()]").Append("public class ").Append(className);
            if (usedGenerics?.Count > 0)
            {
                var code = $"<{string.Join(", ", usedGenerics.Keys)}>";
                sourceCodeBuilder.Append(code);
                entityClassName = className + code;
            }
            else
            {
                entityClassName = className;
            }

            sourceCodeBuilder.Append(" : ")
                .Append(entityBaseTypeFullName).Append(", ").Append(fullNameOfIRemoteAgencyMessage);
            CodeBuilderHelper.ApplyConstraints(usedGenerics, sourceCodeBuilder);
            sourceCodeBuilder.AppendLine("\n{");

            //values
            foreach (var item in values)
            {
                sourceCodeBuilder.Append("public ").Append(item.TypeName).Append(" ").Append(item.PropertyName).AppendLine(" { get; set; }");
            }

            //constructor
            if (needValueBasedConstructor)
            {
                StringBuilder constructorCallerCodeBuilder = new StringBuilder(" = new ");
                constructorCallerCodeBuilder.Append(entityClassName).Append("(");
                sourceCodeBuilder.Append("public ").Append(className).Append("(");
                if (values.Any())
                {
                    var items = values.Select(i => i.TypeName + " " + i.UniqueName);
                    sourceCodeBuilder.Append(string.Join(", ", items));
                    constructorCallerCodeBuilder.Append(string.Join(", ", values.Select(i => i.NameInCode)));
                }
                constructorCallerCodeBuilder.AppendLine(");");
                valueBasedConstructorCallerCode = constructorCallerCodeBuilder.ToString();
                sourceCodeBuilder.AppendLine(")\n{");
                foreach (var item in values)
                {
                    sourceCodeBuilder.Append("this.").Append(item.PropertyName).Append(" = ").Append(item.UniqueName).AppendLine(";");
                }
                sourceCodeBuilder.AppendLine("}");
            }
            else
            {
                valueBasedConstructorCallerCode = null;
            }

            //interface members
            sourceCodeBuilder.Append(@"System.Guid IRemoteAgencyMessage.SenderSiteId { get; set; }
System.Guid IRemoteAgencyMessage.TargetSiteId { get; set; }
System.Guid IRemoteAgencyMessage.SenderInstanceId { get; set; }
System.Guid IRemoteAgencyMessage.TargetInstanceId { get; set; }
SecretNest.RemoteAgency.MessageType IRemoteAgencyMessage.MessageType { get; set; }
string IRemoteAgencyMessage.AssetName { get; set; }
System.Guid IRemoteAgencyMessage.MessageId { get; set; }
System.Exception IRemoteAgencyMessage.Exception { get; set; }
bool IRemoteAgencyMessage.IsOneWay { get; set; }
bool IRemoteAgencyMessage.IsEmptyMessage => false;
}"); //<--class end
            return entityClassName;
        }

        /// <inheritdoc />
        public override Type InterfaceLevelAttributeBaseType => null;
        /// <inheritdoc />
        public override Type AssetLevelAttributeBaseType => null;
        /// <inheritdoc />
        public override Type DelegateLevelAttributeBaseType => null;
        /// <inheritdoc />
        public override Type ParameterLevelAttributeBaseType => null;

        /// <inheritdoc />
        public override IRemoteAgencyMessage CreateEmptyMessage()
        {
            return new RemoteAgencyBinaryEmptyMessage();
        }
    }
}
