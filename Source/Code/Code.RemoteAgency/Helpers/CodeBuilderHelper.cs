using SecretNest.RemoteAgency.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SecretNest.RemoteAgency
{
    static class CodeBuilderHelper
    {
        internal static void ApplyConstraints(Dictionary<string, Type> types, StringBuilder builder)
        {
            if (types != null)
                foreach (var type in types)
                    ApplyConstraints(type.Key, type.Value, builder);
        }

        static void ApplyConstraints(string name, Type type, StringBuilder builder)
        {
            var typeInfo = type.GetTypeInfo();
            var typeConstraints = typeInfo.GetGenericParameterConstraints();
            var typeAttributes = typeInfo.GenericParameterAttributes;
            var valueType = typeof(ValueType);
            var words = typeConstraints.Where(i => i != valueType).Select(i => i.Name).ToList();
            if (typeAttributes.HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint))
                words.Add("struct");
            else
            {
                if (typeAttributes.HasFlag(GenericParameterAttributes.ReferenceTypeConstraint))
                    words.Add("class");
                if (typeAttributes.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint))
                    words.Add("new()");
            }
            if (words.Count > 0)
            {
                builder.Append(" where ").Append(name).Append(" : ")
                    .Append(string.Join(", ", words));
            }
        }

        internal static void BuildSendingSerializedValueCode(StringBuilder target,
            LocalExceptionHandlingMode localExceptionHandlingMode,
            string communicateInterfaceTypeName, string interfaceTypeName,
            string messageType, string serializedValueName, string genericArguments = "null") //null need to be a string
        {
            /*
             * try
             *   SendMessage
             * catch
             *   SendException
             *   ProcessException
             */
            target.Append("try\n{((")
                .Append(communicateInterfaceTypeName)
                .Append(")this).SendMessageCallback(")
                .Append(messageType)
                .Append(", assetName, messageId, true, ")
                .Append(serializedValueName)
                .Append(", ")
                .Append(genericArguments)
                .AppendLine(");}"); //}: try

            BuildRaisingOrRedirectingExceptionWithCatchCode(target, localExceptionHandlingMode, communicateInterfaceTypeName, interfaceTypeName, messageType);
        }

        //SecretNest.RemoteAgency.MessageType.
        internal static void BuildSendingExceptionCode(StringBuilder target, 
            string serializingHelperName, LocalExceptionHandlingMode localExceptionHandlingMode,
            string communicateInterfaceTypeName, string interfaceTypeName, string wrappedExceptionTypeName,
            string messageType, string wrappedExceptionName)
        {
            /* SerializeException
             * SendException
             */
            string serializedExceptionValueName = NamingHelper.GetRandomName("serializedException");
            string exceptionTypeValueName = NamingHelper.GetRandomName("exceptionType");
            string serializingExceptionValueName = NamingHelper.GetRandomName("serializingException");
            target.Append("System.Type ")
                .Append(exceptionTypeValueName)
                .Append(";\nvar ")
                .Append(serializedExceptionValueName)
                .Append(" = ")
                .Append(serializingHelperName)
                .Append(".SerializeExceptionWithExceptionTolerance(")
                .Append(wrappedExceptionName)
                .Append(", out var ")
                .Append(serializingExceptionValueName)
                .Append(");\nif (")
                .Append(serializingExceptionValueName)
                .AppendLine(" != null)\n{")
                .Append(exceptionTypeValueName)
                .Append(" = ")
                .Append(serializingExceptionValueName)
                .AppendLine(".GetType();")
                .Append(serializedExceptionValueName)
                .Append(" = ")
                .Append(serializingHelperName)
                .Append(".SerializeExceptionWithExceptionTolerance(")
                .Append(wrappedExceptionTypeName)
                .Append(".Create(")
                .Append(serializingExceptionValueName)
                .AppendLine("), out _);\n}\nelse\n{")
                .Append(exceptionTypeValueName)
                .Append(" = ")
                .Append(wrappedExceptionName)
                .Append(".ExceptionType;\n}\n((")
                .Append(communicateInterfaceTypeName)
                .Append(")this).SendExceptionCallback(")
                .Append(messageType)
                .Append(", assetName, messageId, ")
                .Append(serializedExceptionValueName)
                .Append(", ")
                .Append(exceptionTypeValueName)
                .AppendLine(");");

            //Process the exception occurred in serializing only.
            BuildRaisingOrRedirectingExceptionWithNullCheckCode(target, localExceptionHandlingMode, communicateInterfaceTypeName, interfaceTypeName, messageType, serializingExceptionValueName);
        }

        internal static void BuildRaisingOrRedirectingExceptionWithNullCheckCode(StringBuilder target,
            LocalExceptionHandlingMode localExceptionHandlingMode,
            string communicateInterfaceTypeName, string interfaceTypeName,
            string messageType, string exceptionValueName)
        {
            if (localExceptionHandlingMode == LocalExceptionHandlingMode.Redirect)
            {
                target.Append("if (")
                    .Append(exceptionValueName)
                    .AppendLine(" != null)\n{");
                BuildRedirectingExceptionCode(target, communicateInterfaceTypeName, interfaceTypeName, messageType, exceptionValueName);
                target.AppendLine("}");
            }
            else if (localExceptionHandlingMode == LocalExceptionHandlingMode.Throw)
            {
                target.Append("if (")
                    .Append(exceptionValueName)
                    .AppendLine(" != null)\n{");
                BuildRaisingExceptionCode(target, exceptionValueName);
                target.AppendLine("}");
            }
        }

        internal static void BuildRaisingOrRedirectingExceptionWithCatchCode(StringBuilder target,
            LocalExceptionHandlingMode localExceptionHandlingMode,
            string communicateInterfaceTypeName, string interfaceTypeName,
            string messageType)
        {
            if (localExceptionHandlingMode == LocalExceptionHandlingMode.Redirect)
            {
                string catchedExceptionValueName = NamingHelper.GetRandomName("exception");
                target.Append("catch (Exception ")
                    .Append(catchedExceptionValueName)
                    .AppendLine(")\n{");
                BuildRedirectingExceptionCode(target, communicateInterfaceTypeName, interfaceTypeName, messageType, catchedExceptionValueName);
                target.AppendLine("}");
            }
            else if (localExceptionHandlingMode == LocalExceptionHandlingMode.Throw)
            {
                string catchedExceptionValueName = NamingHelper.GetRandomName("exception");
                target.Append("catch (Exception ")
                    .Append(catchedExceptionValueName)
                    .AppendLine(")\n{");
                BuildRaisingExceptionCode(target, catchedExceptionValueName);
                target.AppendLine("}");
            }
            else //suppress
            {
                target.AppendLine("catch { }");
            }
        }

        static void BuildRedirectingExceptionCode(StringBuilder target,
            string communicateInterfaceTypeName, string interfaceTypeName,
            string messageType, string exceptionValueName)
        {
            target.Append("if (((")
                .Append(communicateInterfaceTypeName)
                .Append(")this).RedirectedExceptionRaisedCallback != null)\n{\n((")
                .Append(communicateInterfaceTypeName)
                .Append(")this).RedirectedExceptionRaisedCallback(")
                .Append(messageType)
                .Append(", assetName, messageId, typeof(")
                .Append(interfaceTypeName)
                .Append("), ")
                .Append(exceptionValueName)
                .Append(");\n}\nelse\n{\nthrow ")
                .Append(exceptionValueName)
                .AppendLine(";\n}");
        }

        static void BuildRaisingExceptionCode(StringBuilder target, string exceptionValueName)
        {
            target.Append("throw ")
                .Append(exceptionValueName)
                .AppendLine(";");
        }
    }
}
