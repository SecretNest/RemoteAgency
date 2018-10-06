using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using SecretNest.RemoteAgency.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;

namespace SecretNest.RemoteAgency
{
    public partial class ProxyCreator<TSerialized, TEntityBase> : IAssemblyCacheOperatings<TSerialized, TEntityBase> where TEntityBase : class
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

        Assembly CreateProxyAssembly(Type interfaceType, System.Reflection.TypeInfo interfaceTypeInfo, out bool disposeRequired, out byte[] image)
        {
            //Preparing
            StringBuilder totalSourceBuilder = new StringBuilder(); //extern alias
            StringBuilder processMessageSourceBuilder = new StringBuilder();
            StringBuilder processExceptionSourceBuilder = new StringBuilder();
            StringBuilder classLevelBuilder = new StringBuilder();
            StringBuilder memberLevelBuilder = new StringBuilder();
            StringBuilder afterInitializedSourceBuilder = new StringBuilder();

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
            var assemblyMameInterface = interfaceTypeInfo.Assembly.GetName();
            usedAssemblies.Add(assemblyMameInterface.FullName, new Tuple<AssemblyName, string>(assemblyMameInterface, null));
            GetTypeFullNameParameter getTypeFullNameParameter = new GetTypeFullNameParameter(usedTypes, usedAssemblies, totalSourceBuilder);
            var entityBaseTypeName = typeof(TEntityBase).GetFullName(getTypeFullNameParameter, null);
            var entitySerializedTypeName = typeof(TSerialized).GetFullName(getTypeFullNameParameter, null);
            var communicateInterfaceTypeName = typeof(ICommunicate<TSerialized>).GetFullName(getTypeFullNameParameter, null);
            var interfaceTypeFullName = interfaceType.GetFullName(getTypeFullNameParameter, null);
            var serializingHelperTypeName = remoteAgencySerializingHelperType.GetFullName(getTypeFullNameParameter, null);
            var wrappedExceptionTypeName = typeof(WrappedException).GetFullName(getTypeFullNameParameter, null);
            var assetNotFoundExceptionTypeName = typeof(AssetNotFoundException).GetFullName(getTypeFullNameParameter, null);
            var redirectedExceptionRaisedCallbackTypeName = typeof(RedirectedExceptionRaisedCallback).GetFullName(getTypeFullNameParameter, null);

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
            var hasMethod = allMethods.Any();
            var allEvents = interfaceType.GetRuntimeEvents();
            var hasEvent = allEvents.Any();
            var allProperties = interfaceType.GetRuntimeProperties();
            var hasProperty = allProperties.Any();

            LocalExceptionHandlingMode localExceptionHandlingMode = interfaceType.GetCustomAttribute<LocalExceptionHandlingAttribute>()?.LocalExceptionHandlingMode ?? LocalExceptionHandlingMode.Suppress;

            string serializingHelperName = NamingHelper.GetRandomName("serializingHelper");
            memberLevelBuilder.Append(serializingHelperTypeName).Append(" ").Append(serializingHelperName).Append(" = new ").Append(serializingHelperTypeName).AppendLine("();");

            string responderName;

            responderName = NamingHelper.GetRandomName("responder");
            var responderTypeName = typeof(Responder<TEntityBase>).GetFullName(getTypeFullNameParameter, null);
            memberLevelBuilder.Append(responderTypeName).Append(" ").Append(responderName).Append(" = new ").Append(responderTypeName).Append("();\nSecretNest.RemoteAgency.MessageWaitingTimedOutCallback ")
                .Append(communicateInterfaceTypeName).Append(".MessageWaitingTimedOutCallback { get { return ")
                .Append(responderName).Append(".MessageWaitingTimedOutCallback; } set { ")
                .Append(responderName).AppendLine(".MessageWaitingTimedOutCallback = value; } }");
            
            //Method
            if (hasMethod)
            {
                BuildMethodParameter buildMethodParameter = new BuildMethodParameter(
                    memberLevelBuilder, classLevelBuilder, getTypeFullNameParameter,
                    responderName, entityBaseTypeName, serializingHelperName,
                    communicateInterfaceTypeName, globalTimedoutSetting);
                foreach (var method in allMethods)
                    BuildMethod(method, buildMethodParameter);
                AssemblyBuilderHelper.MessageHandlersWrapper(processMessageSourceBuilder, false, MessageType.Method, buildMethodParameter.ProcessMessageSourceBuilder.ToString(), assetNotFoundExceptionTypeName, wrappedExceptionTypeName, serializingHelperName, communicateInterfaceTypeName, localExceptionHandlingMode, interfaceTypeFullName);
            }
            else
            {
                AssemblyBuilderHelper.MessageHandlersWrapper(processMessageSourceBuilder, false, MessageType.Method, "", assetNotFoundExceptionTypeName, wrappedExceptionTypeName, serializingHelperName, communicateInterfaceTypeName, localExceptionHandlingMode, interfaceTypeFullName);
            }

            //Event
            disposeRequired = hasEvent;
            string eventUnsubscribeConditionCode;
            if (hasEvent)
            {
                var joint = allEvents.Select(i =>
                {
                    var subscriptionMode = i.GetCustomAttribute<EventSubscriptionAttribute>()?.EventSubscritionMode ?? EventSubscritionMode.Dynamic;
                    return new { EventInfo = i, EventSubscritionMode = subscriptionMode, UnderlayDelegateName = subscriptionMode == EventSubscritionMode.SubscribeOnStartAndKeep ? null : NamingHelper.GetRandomName("underlayDelegate") };
                }).ToArray();

                string eventHandlerStateName, lockObjectName;
                if (joint.Any(i => i.EventSubscritionMode == EventSubscritionMode.SubscribeOnStartAndKeep))
                {
                    eventHandlerStateName = null;
                    eventUnsubscribeConditionCode = "";
                    if (joint.Any(i => i.EventSubscritionMode != EventSubscritionMode.SubscribeOnStartAndKeep))
                        lockObjectName = NamingHelper.GetRandomName("lock");
                    else
                        lockObjectName = null;
                }
                else if (joint.Any(i => i.EventSubscritionMode == EventSubscritionMode.SubscribeOnFirstUseAndKeep))
                {
                    lockObjectName = NamingHelper.GetRandomName("lock");
                    eventHandlerStateName = NamingHelper.GetRandomName("eventHandlerState");
                    memberLevelBuilder.Append("bool ").Append(eventHandlerStateName).AppendLine(" = false;");
                    var conditions = joint.Where(i=>i.EventSubscritionMode == EventSubscritionMode.Dynamic).Select(i => i.UnderlayDelegateName + " != null").ToList();
                    conditions.Insert(0, eventHandlerStateName);
                    eventUnsubscribeConditionCode = string.Format("if ({0})\n", string.Join(" || ", conditions));
                }
                else
                {
                    lockObjectName = NamingHelper.GetRandomName("lock");
                    eventHandlerStateName = null;
                    var conditions = joint.Select(i => i.UnderlayDelegateName + " != null");
                    eventUnsubscribeConditionCode = string.Format("if ({0})\n", string.Join(" || ", conditions));
                }

                if (lockObjectName != null)
                    memberLevelBuilder.Append("object ").Append(lockObjectName).AppendLine(" = new object();");

                BuildEventParameter buildEventParameter = new BuildEventParameter(
                    memberLevelBuilder, classLevelBuilder, getTypeFullNameParameter,
                    lockObjectName, eventHandlerStateName, responderName,
                    entityBaseTypeName, entitySerializedTypeName, serializingHelperName,
                    communicateInterfaceTypeName, globalTimedoutSetting, wrappedExceptionTypeName,
                    interfaceTypeFullName, localExceptionHandlingMode);
                
                foreach (var item in joint)
                    BuildEvent(item.EventInfo, item.UnderlayDelegateName, buildEventParameter, item.EventSubscritionMode);

                AssemblyBuilderHelper.MessageHandlersWrapper(processMessageSourceBuilder, true, MessageType.Event, buildEventParameter.ProcessRaiseMessageSourceBuilder.ToString(), assetNotFoundExceptionTypeName, wrappedExceptionTypeName, serializingHelperName, communicateInterfaceTypeName, localExceptionHandlingMode, interfaceTypeFullName);
                processMessageSourceBuilder.AppendLine("else if(messageType == SecretNest.RemoteAgency.MessageType.EventAdd || messageType == SecretNest.RemoteAgency.MessageType.EventRemove)\n{")
                    .Append(responderName).AppendLine(".SetDefaultResult(messageId);\n}");

                afterInitializedSourceBuilder.Append(buildEventParameter.AfterInitializedSourcePhase1Builder.ToString()).Append(buildEventParameter.AfterInitializedSourcePhase2Builder.ToString());
            }
            else
            {
                AssemblyBuilderHelper.MessageHandlersWrapper(processMessageSourceBuilder, true, MessageType.Event, "", assetNotFoundExceptionTypeName, wrappedExceptionTypeName, serializingHelperName, communicateInterfaceTypeName, localExceptionHandlingMode, interfaceTypeFullName);
                eventUnsubscribeConditionCode = null; //will not be used
            }

            //Property
            if (hasProperty)
            {
                BuildPropertyParameter buildPropertyParameter = new BuildPropertyParameter(
                    memberLevelBuilder, classLevelBuilder, getTypeFullNameParameter,
                    responderName, entityBaseTypeName, serializingHelperName,
                    communicateInterfaceTypeName, globalTimedoutSetting,
                    interfaceTypeFullName, localExceptionHandlingMode);

                foreach (var property in allProperties)
                    BuildProperty(property, buildPropertyParameter);

                AssemblyBuilderHelper.MessageHandlersWrapper(processMessageSourceBuilder, true, MessageType.PropertyGet, buildPropertyParameter.ProcessGetMessageSourceBuilder.ToString(), assetNotFoundExceptionTypeName, wrappedExceptionTypeName, serializingHelperName, communicateInterfaceTypeName, localExceptionHandlingMode, interfaceTypeFullName);
                AssemblyBuilderHelper.MessageHandlersWrapper(processMessageSourceBuilder, true, MessageType.PropertySet, buildPropertyParameter.ProcessSetMessageSourceBuilder.ToString(), assetNotFoundExceptionTypeName, wrappedExceptionTypeName, serializingHelperName, communicateInterfaceTypeName, localExceptionHandlingMode, interfaceTypeFullName);
            }
            else
            {
                AssemblyBuilderHelper.MessageHandlersWrapper(processMessageSourceBuilder, true, MessageType.PropertyGet, "", assetNotFoundExceptionTypeName, wrappedExceptionTypeName, serializingHelperName, communicateInterfaceTypeName, localExceptionHandlingMode, interfaceTypeFullName);
                AssemblyBuilderHelper.MessageHandlersWrapper(processMessageSourceBuilder, true, MessageType.PropertySet, "", assetNotFoundExceptionTypeName, wrappedExceptionTypeName, serializingHelperName, communicateInterfaceTypeName, localExceptionHandlingMode, interfaceTypeFullName);
            }

            AssemblyBuilderHelper.MessageHandlersWrapper(processMessageSourceBuilder, true, MessageType.EventAdd, "", assetNotFoundExceptionTypeName, wrappedExceptionTypeName, serializingHelperName, communicateInterfaceTypeName, localExceptionHandlingMode, interfaceTypeFullName);
            AssemblyBuilderHelper.MessageHandlersWrapper(processMessageSourceBuilder, true, MessageType.EventRemove, "", assetNotFoundExceptionTypeName, wrappedExceptionTypeName, serializingHelperName, communicateInterfaceTypeName, localExceptionHandlingMode, interfaceTypeFullName);

            //Exception Received
            string exceptionValueNameInProcessException = NamingHelper.GetRandomName("exception");
            string deserializeExceptionValueNameInProcessException = NamingHelper.GetRandomName("deserializeException");
            processExceptionSourceBuilder.Append("var ").Append(exceptionValueNameInProcessException).Append(" = ")
                .Append(serializingHelperName).Append(".DeserializeExceptionWithExceptionTolerance(serializedException, exceptionType, out var ")
                .Append(deserializeExceptionValueNameInProcessException).AppendLine(");")
                .Append(responderName).AppendLine(".SetException(messageId, ").Append(exceptionValueNameInProcessException).AppendLine(");");
            CodeBuilderHelper.BuildRaisingOrRedirectingExceptionWithNullCheckCode(processExceptionSourceBuilder,
                localExceptionHandlingMode,
                communicateInterfaceTypeName, interfaceTypeFullName,
                "messageType", deserializeExceptionValueNameInProcessException);

            //Build Preparing
            totalSourceBuilder.Append("using System;\nusing System.Runtime.Serialization;\nusing SecretNest.RemoteAgency;\n\nnamespace SecretNest.RemoteAgency.Created\n{\npublic class Proxy : ")
                .Append(communicateInterfaceTypeName).Append(", ").Append(interfaceTypeFullName);
            if (disposeRequired)
            {
                totalSourceBuilder.Append(", IDisposable");
            }
            totalSourceBuilder.AppendLine("\n{")
                .AppendLine(memberLevelBuilder.ToString())
                .Append("SecretNest.RemoteAgency.SendMessageCallback<").Append(entitySerializedTypeName).Append("> ")
                .Append(communicateInterfaceTypeName).Append(".SendMessageCallback { get; set; }\nSecretNest.RemoteAgency.SendExceptionCallback<").Append(entitySerializedTypeName).Append("> ")
                .Append(communicateInterfaceTypeName).Append(".SendExceptionCallback { get; set; }\nvoid ")
                .Append(communicateInterfaceTypeName).Append(".ProcessMessage(SecretNest.RemoteAgency.MessageType messageType, string assetName, System.Guid messageId, bool isOneWay, ").Append(entitySerializedTypeName).AppendLine(" data, Type[] genericArguments)\n{")
                .Append(processMessageSourceBuilder.ToString()).Append("}\nvoid ")
                .Append(communicateInterfaceTypeName).Append(".ProcessException(SecretNest.RemoteAgency.MessageType messageType, string assetName, System.Guid messageId, ").Append(entitySerializedTypeName).AppendLine(" serializedException, System.Type exceptionType)\n{")
                .Append(processExceptionSourceBuilder.ToString()).AppendLine("}")
                .Append(redirectedExceptionRaisedCallbackTypeName).Append(" ")
                .Append(communicateInterfaceTypeName).Append(".RedirectedExceptionRaisedCallback { get; set; }\nvoid ")
                .Append(communicateInterfaceTypeName).AppendLine(".AfterInitialized()\n{")
                .Append(afterInitializedSourceBuilder.ToString())
                .AppendLine("}");
            if (disposeRequired)
            {
                totalSourceBuilder.AppendLine("private bool disposedValue = false;\nprotected virtual void Dispose(bool disposing)\n{\nif (!disposedValue)\n{\nif (disposing)")
                    .Append(eventUnsubscribeConditionCode)
                    .Append("((").Append(communicateInterfaceTypeName).Append(")this).SendMessageCallback.Invoke(SecretNest.RemoteAgency.MessageType.SpecialCommand, \"Dispose\", System.Guid.NewGuid(), true, default(").Append(entitySerializedTypeName).AppendLine("), null);\ndisposedValue = true;\n}\n}\npublic void Dispose()\n{\nDispose(true);\n}\n}");
            }
            else
            {
                totalSourceBuilder.AppendLine("}");
            }
            totalSourceBuilder.AppendLine(classLevelBuilder.ToString()).AppendLine("}");

            var otherAssemblies = interfaceTypeInfo.GetCustomAttributes<ImportAssemblyAttribute>();

            return assemblyBuilderHelper.Build(totalSourceBuilder.ToString(), usedAssemblies, otherAssemblies, false, NamingHelper.GetRandomName("SecretNestRemoteAgency"), out image);
        }
    }
}
