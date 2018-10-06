using SecretNest.RemoteAgency.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace SecretNest.RemoteAgency
{
    public partial class ServiceWrapperCreator<TSerialized, TEntityBase> : IAssemblyCacheOperatings<TSerialized, TEntityBase> where TEntityBase : class
    {
        AssemblyBuilderHelper assemblyBuilderHelper;

        /// <summary>
        /// Occurs when a missing assembly / module needs to be resolved.
        /// </summary>
        public event EventHandler<AssemblyRequestingEventArgs> MissingAssemblyRequesting;

        /// <summary>
        /// Clear cached assemblies using in Roslyn.
        /// </summary>
        public void ClearBuilderAssemblyCache()
        {
            assemblyBuilderHelper.ClearBuilderAssemblyCache();
        }
        Assembly CreateServiceWrapperAssembly(Type[] interfaceTypes, TypeInfo[] interfaceTypesInfo, out bool disposeRequired, out byte[] image)
        {
            //Preparing
            StringBuilder totalSourceBuilder = new StringBuilder(); //extern alias
            StringBuilder classLevelBuilder = new StringBuilder();
            StringBuilder memberLevelBuilder = new StringBuilder();
            StringBuilder afterInitializedSourceBuilder = new StringBuilder();
            StringBuilder processExceptionSourceBuilder = new StringBuilder();
            bool dynamicKeywordUsed = false;
            disposeRequired = false;

            Dictionary<Type, string> usedTypes = new Dictionary<Type, string>();
            usedTypes.Add(typeof(object), "object");
            usedTypes.Add(typeof(WrappedException), "SecretNest.RemoteAgency.WrappedException");
            usedTypes.Add(typeof(DataContractAttribute), "System.Runtime.Serialization.DataContractAttribute");
            Dictionary<string, Tuple<AssemblyName, string>> usedAssemblies = new Dictionary<string, Tuple<AssemblyName, string>>();
            var assemblyNameObject = typeof(object).GetTypeInfo().Assembly.GetName();
            usedAssemblies.Add(assemblyNameObject.FullName, new Tuple<AssemblyName, string>(assemblyNameObject, null));
            var assemblyNameRemoteAgencyBase = typeof(WrappedException).GetTypeInfo().Assembly.GetName();
            usedAssemblies.Add(assemblyNameRemoteAgencyBase.FullName, new Tuple<AssemblyName, string>(assemblyNameRemoteAgencyBase, null));
            var assemblyNameDataContract = typeof(DataContractAttribute).GetTypeInfo().Assembly.GetName();
            usedAssemblies.Add(assemblyNameDataContract.FullName, new Tuple<AssemblyName, string>(assemblyNameDataContract, null));
            foreach (var typeInfo in interfaceTypesInfo)
            {
                var assemblyName = typeInfo.Assembly.GetName();
                var assemblyNameText = assemblyName.FullName;
                if (!usedAssemblies.ContainsKey(assemblyNameText))
                    usedAssemblies.Add(assemblyNameText, new Tuple<AssemblyName, string>(assemblyName, null));
            }

            GetTypeFullNameParameter getTypeFullNameParameter = new GetTypeFullNameParameter(usedTypes, usedAssemblies, totalSourceBuilder);
            var entityBaseTypeName = typeof(TEntityBase).GetFullName(getTypeFullNameParameter, null);
            var entitySerializedTypeName = typeof(TSerialized).GetFullName(getTypeFullNameParameter, null);
            var communicateInterfaceTypeName = typeof(ICommunicate<TSerialized>).GetFullName(getTypeFullNameParameter, null);
            var serializingHelperTypeName = remoteAgencySerializingHelperType.GetFullName(getTypeFullNameParameter, null);
            var wrappedExceptionTypeName = typeof(WrappedException).GetFullName(getTypeFullNameParameter, null);
            var assetNotFoundExceptionTypeName = typeof(AssetNotFoundException).GetFullName(getTypeFullNameParameter, null);
            var redirectedExceptionRaisedCallbackTypeName = typeof(RedirectedExceptionRaisedCallback).GetFullName(getTypeFullNameParameter, null);
            List<ImportAssemblyAttribute> importAssemblyAttributes = new List<ImportAssemblyAttribute>();

            var serviceObjectName = NamingHelper.GetRandomName("serviceObject");
            memberLevelBuilder.Append("readonly object ").Append(serviceObjectName).Append(";\npublic ServiceWrapper(object serviceObject)\n{\nthis.")
                .Append(serviceObjectName).AppendLine(" = serviceObject;\n}");

            string serializingHelperName = NamingHelper.GetRandomName("serializingHelper");
            memberLevelBuilder.Append(serializingHelperTypeName).Append(" ").Append(serializingHelperName).Append(" = new ").Append(serializingHelperTypeName).AppendLine("();");
            string responderName = NamingHelper.GetRandomName("responder");
            string lockObjectName = NamingHelper.GetRandomName("lock");
            string responderTypeName = typeof(Responder<TEntityBase>).GetFullName(getTypeFullNameParameter, null);
            memberLevelBuilder.Append(responderTypeName).Append(" ").Append(responderName).Append(" = new ").Append(responderTypeName).AppendLine("();");
            BuildMethodParameter buildMethodParameter = new BuildMethodParameter(
                memberLevelBuilder, classLevelBuilder, getTypeFullNameParameter,
                entityBaseTypeName, entitySerializedTypeName, serializingHelperName,
                communicateInterfaceTypeName, serviceObjectName, wrappedExceptionTypeName);
            BuildEventParameter buildEventParameter = new BuildEventParameter(
                memberLevelBuilder, classLevelBuilder, getTypeFullNameParameter,
                lockObjectName, responderName, entityBaseTypeName,
                entitySerializedTypeName, serializingHelperName, communicateInterfaceTypeName,
                serviceObjectName, wrappedExceptionTypeName);
            BuildPropertyParameter buildPropertyParameter = new BuildPropertyParameter(
                classLevelBuilder, getTypeFullNameParameter, entityBaseTypeName,
                entitySerializedTypeName, serializingHelperName, communicateInterfaceTypeName,
                serviceObjectName, wrappedExceptionTypeName);

            string unifiedInterfaceTypeName;
            if (interfaceTypes.Length == 1)
                unifiedInterfaceTypeName = interfaceTypes[0].GetFullName(getTypeFullNameParameter, null);
            else
                unifiedInterfaceTypeName = null;
            LocalExceptionHandlingMode unifiedLocalExceptionHandlingMode
                = interfaceTypes[0].GetCustomAttribute<LocalExceptionHandlingAttribute>()?.LocalExceptionHandlingMode ?? LocalExceptionHandlingMode.Suppress;

            for (int interfaceIndex = 0; interfaceIndex < interfaceTypes.Length; interfaceIndex++)
            {
                Type interfaceType = interfaceTypes[interfaceIndex];
                TypeInfo interfaceTypeInfo = interfaceTypesInfo[interfaceIndex];
                var interfaceTypeFullName = interfaceType.GetFullName(getTypeFullNameParameter, null);
                int globalTimedoutSetting = interfaceTypeInfo.GetCustomAttribute<CustomizedOperatingTimedoutTimeAttribute>()?.MillisecondsTimeout ?? -1;

                //Generic types @ interface level
                if (interfaceType.IsConstructedGenericType)
                {
                    Queue<Type> types = new Queue<Type>(interfaceType.GenericTypeArguments);

                    while (types.Count > 0)
                    {
                        var type = types.Dequeue();
                        while (type.IsArray)
                        {
                            type = type.GetElementType();
                        }

                        type.GetFullName(getTypeFullNameParameter, null);

                        if (type.IsConstructedGenericType)
                        {
                            foreach (var subType in type.GenericTypeArguments)
                            {
                                types.Enqueue(subType);
                            }
                        }
                    }
                }

                var allMethods = interfaceType.GetRuntimeMethods().Where(i => !i.IsSpecialName);
                var allEvents = interfaceType.GetRuntimeEvents();
                var hasEvent = allEvents.Any();
                var allProperties = interfaceType.GetRuntimeProperties();

                LocalExceptionHandlingMode localExceptionHandlingMode = interfaceType.GetCustomAttribute<LocalExceptionHandlingAttribute>()?.LocalExceptionHandlingMode ?? LocalExceptionHandlingMode.Suppress;

                //Method
                foreach (var method in allMethods)
                    BuildMethod(method, buildMethodParameter, interfaceTypeFullName, localExceptionHandlingMode, ref dynamicKeywordUsed);

                //Event
                if (hasEvent)
                {
                    if (!disposeRequired)
                    {
                        disposeRequired = true;
                        memberLevelBuilder.Append("SecretNest.RemoteAgency.MessageWaitingTimedOutCallback ")
                            .Append(communicateInterfaceTypeName).Append(".MessageWaitingTimedOutCallback { get { return ")
                            .Append(responderName).Append(".MessageWaitingTimedOutCallback; } set { ")
                            .Append(responderName).Append(".MessageWaitingTimedOutCallback = value; } }\nobject ")
                            .Append(lockObjectName).AppendLine(" = new object();");
                    }

                    foreach (var eventInfo in allEvents)
                        BuildEvent(eventInfo, buildEventParameter, interfaceTypeFullName, globalTimedoutSetting, localExceptionHandlingMode);
                }

                //Property
                foreach (var property in allProperties)
                    BuildProperty(property, buildPropertyParameter, interfaceTypeFullName);

                importAssemblyAttributes.AddRange(interfaceTypeInfo.GetCustomAttributes<ImportAssemblyAttribute>());
            }

            //Exception Received
            string exceptionValueNameInProcessException = NamingHelper.GetRandomName("exception");
            string deserializeExceptionValueNameInProcessException = NamingHelper.GetRandomName("deserializeException");
            processExceptionSourceBuilder.Append("var ").Append(exceptionValueNameInProcessException).Append(" = ")
                .Append(serializingHelperName).Append(".DeserializeExceptionWithExceptionTolerance(serializedException, exceptionType, out var ")
                .Append(deserializeExceptionValueNameInProcessException).AppendLine(");")
                .Append(responderName).AppendLine(".SetException(messageId, ").Append(exceptionValueNameInProcessException).AppendLine(");");
            CodeBuilderHelper.BuildRaisingOrRedirectingExceptionWithNullCheckCode(processExceptionSourceBuilder,
                unifiedLocalExceptionHandlingMode, 
                communicateInterfaceTypeName, unifiedInterfaceTypeName,
                "messageType", deserializeExceptionValueNameInProcessException);

            //Build Preparing
            totalSourceBuilder.Append("using System;\nusing System.Runtime.Serialization;\nusing SecretNest.RemoteAgency;\n\nnamespace SecretNest.RemoteAgency.Created\n{\npublic class ServiceWrapper : ")
                .Append(communicateInterfaceTypeName);
            if (disposeRequired)
            {
                totalSourceBuilder.Append(", IDisposable");
            }
            totalSourceBuilder.AppendLine("\n{")
                .AppendLine(memberLevelBuilder.ToString())
                .Append("SecretNest.RemoteAgency.SendMessageCallback<").Append(entitySerializedTypeName).Append("> ")
                .Append(communicateInterfaceTypeName).Append(".SendMessageCallback { get; set; }\nSecretNest.RemoteAgency.SendExceptionCallback<").Append(entitySerializedTypeName).Append("> ")
                .Append(communicateInterfaceTypeName).Append(".SendExceptionCallback { get; set; }\nvoid ")
                .Append(communicateInterfaceTypeName).Append(".ProcessMessage(SecretNest.RemoteAgency.MessageType messageType, string assetName, System.Guid messageId, bool isOneWay, ").Append(entitySerializedTypeName).AppendLine(" data, Type[] genericArguments)\n{");
            AssemblyBuilderHelper.MessageHandlersWrapper(totalSourceBuilder, false, MessageType.Method, buildMethodParameter.ProcessMethodMessageSourceBuilder.ToString(), assetNotFoundExceptionTypeName, wrappedExceptionTypeName, serializingHelperName, communicateInterfaceTypeName, unifiedLocalExceptionHandlingMode, unifiedInterfaceTypeName);
            AssemblyBuilderHelper.MessageHandlersWrapper(totalSourceBuilder, true, MessageType.Event, buildEventParameter.ProcessEventMessageSourceBuilder.ToString(), assetNotFoundExceptionTypeName, wrappedExceptionTypeName, serializingHelperName, communicateInterfaceTypeName, unifiedLocalExceptionHandlingMode, unifiedInterfaceTypeName);
            AssemblyBuilderHelper.MessageHandlersWrapper(totalSourceBuilder, true, MessageType.EventAdd, buildEventParameter.ProcessEventAddMessageSourceBuilder.ToString(), assetNotFoundExceptionTypeName, wrappedExceptionTypeName, serializingHelperName, communicateInterfaceTypeName, unifiedLocalExceptionHandlingMode, unifiedInterfaceTypeName);
            AssemblyBuilderHelper.MessageHandlersWrapper(totalSourceBuilder, true, MessageType.EventRemove, buildEventParameter.ProcessEventRemoveMessageSourceBuilder.ToString(), assetNotFoundExceptionTypeName, wrappedExceptionTypeName, serializingHelperName, communicateInterfaceTypeName, unifiedLocalExceptionHandlingMode, unifiedInterfaceTypeName);
            AssemblyBuilderHelper.MessageHandlersWrapper(totalSourceBuilder, true, MessageType.PropertyGet, buildPropertyParameter.ProcessGetMessageSourceBuilder.ToString(), assetNotFoundExceptionTypeName, wrappedExceptionTypeName, serializingHelperName, communicateInterfaceTypeName, unifiedLocalExceptionHandlingMode, unifiedInterfaceTypeName);
            AssemblyBuilderHelper.MessageHandlersWrapper(totalSourceBuilder, true, MessageType.PropertySet, buildPropertyParameter.ProcessSetMessageSourceBuilder.ToString(), assetNotFoundExceptionTypeName, wrappedExceptionTypeName, serializingHelperName, communicateInterfaceTypeName, unifiedLocalExceptionHandlingMode, unifiedInterfaceTypeName);
            totalSourceBuilder.Append("}\nvoid ")
                .Append(communicateInterfaceTypeName).Append(".ProcessException(SecretNest.RemoteAgency.MessageType messageType, string assetName, System.Guid messageId, ").Append(entitySerializedTypeName).AppendLine(" serializedException, System.Type exceptionType)\n{")
                .Append(processExceptionSourceBuilder).AppendLine("}")
                .Append(redirectedExceptionRaisedCallbackTypeName).Append(" ")
                .Append(communicateInterfaceTypeName).Append(".RedirectedExceptionRaisedCallback { get; set; }\nvoid ")
                .Append(communicateInterfaceTypeName).AppendLine(".AfterInitialized() { }");
            if (disposeRequired)
            {
                totalSourceBuilder.AppendLine("private bool disposedValue = false;\nprotected virtual void Dispose(bool disposing)\n{\nif (!disposedValue)\n{\nif (disposing)\n{")
                    .Append(buildEventParameter.DisposingSourceBuilder.ToString()).AppendLine("}\ndisposedValue = true;\n}\n}\npublic void Dispose()\n{\nDispose(true);\n}\n}");
            }
            else
            {
                totalSourceBuilder.Append("SecretNest.RemoteAgency.MessageWaitingTimedOutCallback ")
                    .Append(communicateInterfaceTypeName).AppendLine(".MessageWaitingTimedOutCallback { get; set; }\n}");
            }
            totalSourceBuilder.AppendLine(classLevelBuilder.ToString()).AppendLine("}");

            var enumerableAssembly = typeof(Enumerable).GetTypeInfo().Assembly;
            if (!usedAssemblies.ContainsKey(enumerableAssembly.FullName))
                usedAssemblies.Add(enumerableAssembly.FullName, new Tuple<AssemblyName, string>(enumerableAssembly.GetName(), null));

            return assemblyBuilderHelper.Build(totalSourceBuilder.ToString(), usedAssemblies, importAssemblyAttributes, dynamicKeywordUsed, NamingHelper.GetRandomName("SecretNestRemoteAgency"), out image);
        }
    }
}
