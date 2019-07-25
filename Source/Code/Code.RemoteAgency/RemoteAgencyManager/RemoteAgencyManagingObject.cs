using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SecretNest.RemoteAgency
{
    delegate TSerialized SerializeException<TSerialized>(WrappedException exception);
    delegate WrappedException DeserializeException<TSerialized>(TSerialized exception, Type exceptionType);

    abstract class RemoteAgencyManagingObject<TSerialized> : IDisposable
    {
        ConcurrentDictionary<Guid, Tuple<Guid, Guid>> messageSourceId = new ConcurrentDictionary<Guid, Tuple<Guid, Guid>>();
        public Guid LocalInstanceId { get; }
        public Guid DefaultRemoteInstanceId { get; }
        public abstract bool IsProxy { get; }
        public abstract bool IsInterfaceImplemented(Type interfaceType);
        protected readonly WrappedException timeOutException;

        public ICommunicate<TSerialized> InnerObject { get; private set; }

        readonly bool shouldDisposeInnerObject;
        SendMessage<TSerialized> sendMessageCallback;
        SendException<TSerialized> sendExceptionCallback;
        SerializeException<TSerialized> serializeExceptionCallback;
        DeserializeException<TSerialized> deserializeExceptionCallback;
        RedirectedExceptionRaisedCallback redirectedExceptionRaisedCallback;
        QueryTargetSite queryTargetSiteCallback;
        QueryDefaultTargetSite queryDefaultTargetSiteCallback;

        protected RemoteAgencyManagingObject(ICommunicate<TSerialized> innerObject, bool shouldDisposeInnerObject,
            Guid localInstanceId, Guid defaultRemoteInstanceId, WrappedException timeOutException,
            SendMessage<TSerialized> sendMessageCallback, SendException<TSerialized> sendExceptionCallback,
            SerializeException<TSerialized> serializeExceptionCallback, DeserializeException<TSerialized> deserializeExceptionCallback,
            RedirectedExceptionRaisedCallback redirectedExceptionRaisedCallback, QueryTargetSite queryTargetSiteCallback, QueryDefaultTargetSite queryDefaultTargetSiteCallback)
        {
            LocalInstanceId = localInstanceId;
            DefaultRemoteInstanceId = defaultRemoteInstanceId;
            this.timeOutException = timeOutException;
            InnerObject = innerObject;
            this.shouldDisposeInnerObject = shouldDisposeInnerObject;
            this.sendMessageCallback = sendMessageCallback;
            this.sendExceptionCallback = sendExceptionCallback;
            this.serializeExceptionCallback = serializeExceptionCallback;
            this.deserializeExceptionCallback = deserializeExceptionCallback;
            this.redirectedExceptionRaisedCallback = redirectedExceptionRaisedCallback;
            this.queryTargetSiteCallback = queryTargetSiteCallback;
            this.queryDefaultTargetSiteCallback = queryDefaultTargetSiteCallback;
            innerObject.SendMessageCallback += SendMessageHandler;
            innerObject.SendExceptionCallback += SendExceptionHandler;
            innerObject.MessageWaitingTimedOutCallback += MessageWaitingTimedOut;
            if (redirectedExceptionRaisedCallback != null)
                innerObject.RedirectedExceptionRaisedCallback += redirectedExceptionRaisedCallback;
            innerObject.AfterInitialized();
        }

        ConcurrentDictionary<Guid, WaitingResponseMessageEntity> waitingResponseMessages = new ConcurrentDictionary<Guid, WaitingResponseMessageEntity>();

        class WaitingResponseMessageEntity
        {
            public MessageType MessageType { get; }
            public string AssetName { get; }

            public WaitingResponseMessageEntity(MessageType messageType, string assetName)
            {
                MessageType = messageType;
                AssetName = assetName;
            }
        }

        public virtual void ProcessMessage(Guid senderSiteId, Guid senderInstanceId, MessageType messageType, string assetName, Guid messageId, bool isOneWay, TSerialized serialized, Type[] genericArguments)
        {
            if (!isOneWay)
            {
                var sender = new Tuple<Guid, Guid>(senderSiteId, senderInstanceId);
                messageSourceId.AddOrUpdate(messageId, sender, (i, j) => sender);
            }
            waitingResponseMessages.TryRemove(messageId, out _);
            InnerObject.ProcessMessage(messageType, assetName, messageId, isOneWay, serialized, genericArguments);
        }

        public virtual void ProcessException(MessageType messageType, string assetName, Guid messageId, TSerialized serializedException, Type exceptionType)
        {
            InnerObject.ProcessException(messageType, assetName, messageId, serializedException, exceptionType);
        }

        protected abstract void SendMessageHandler(MessageType messageType, string assetName, Guid messageId, bool isOneWay, TSerialized serialized, Type[] genericArguments);
        protected abstract void SendExceptionHandler(MessageType messageType, string assetName, Guid messageId, TSerialized serializedException, Type exceptionType);

        void GetTargetId(Guid messageId, out Guid targetInstanceId, out Guid targetSiteId)
        {
            if (messageSourceId.TryRemove(messageId, out var targetId))
            {
                targetSiteId = targetId.Item1;
                targetInstanceId = targetId.Item2;
            }
            else
            {
                targetInstanceId = DefaultRemoteInstanceId;
                targetSiteId = queryTargetSiteCallback(targetInstanceId);
                if (targetSiteId == Guid.Empty)
                    targetSiteId = queryDefaultTargetSiteCallback();
            }
        }

        protected void DirectSendMessage(MessageType messageType, string assetName, Guid messageId, bool isOneWay, TSerialized serialized, Type[] genericArguments)
        {
            GetTargetId(messageId, out Guid targetInstanceId, out Guid targetSiteId);
            DirectSendMessage(targetSiteId, targetInstanceId, messageType, assetName, messageId, isOneWay, serialized, genericArguments);
        }

        protected void DirectSendMessage(Guid targetSiteId, Guid targetInstanceId, MessageType messageType, string assetName, Guid messageId, bool isOneWay, TSerialized serialized, Type[] genericArguments)
        {
            //try
            //{
            sendMessageCallback(targetSiteId, LocalInstanceId, targetInstanceId, messageType, assetName, messageId, isOneWay, serialized, genericArguments);
            //}
            //catch(Exception ex) when (!isOneWay)
            //{
            //    ProcessException(messageType, assetName, messageId, SerializeException(WrappedException.Create(ex)), ex.GetType());
            //    throw;
            //}
        }

        protected void DirectSendException(MessageType messageType, string assetName, Guid messageId, TSerialized serializedException, Type exceptionType)
        {
            GetTargetId(messageId, out Guid targetInstanceId, out Guid targetSiteId);
            DirectSendException(targetSiteId, targetInstanceId, messageType, assetName, messageId, serializedException, exceptionType);
        }

        protected void DirectSendException(Guid targetSiteId, Guid targetInstanceId, MessageType messageType, string assetName, Guid messageId, TSerialized serializedException, Type exceptionType)
        {
            sendExceptionCallback(targetSiteId, LocalInstanceId,  targetInstanceId, messageType, assetName, messageId, serializedException, exceptionType);
        }

        protected TSerialized SerializeException(WrappedException ex)
        {
            return serializeExceptionCallback(ex);
        }

        protected WrappedException DeserializeException(TSerialized ex, Type exceptionType)
        {
            return deserializeExceptionCallback(ex, exceptionType);
        }

        void MessageWaitingTimedOut(Guid messageId)
        {
            waitingResponseMessages.TryRemove(messageId, out _);
            MessageWaitingTimedOutMoreHandling(messageId);
        }

        protected virtual void MessageWaitingTimedOutMoreHandling(Guid messageId) { }

        public void CancelWaiting()
        {
            if (waitingResponseMessages.Count > 0)
            {
                var timeOutExceptionSerialized = serializeExceptionCallback(timeOutException);
                foreach (var messageId in waitingResponseMessages.Keys)
                {
                    if (waitingResponseMessages.TryRemove(messageId, out var message))
                    {
                        InnerObject.SendExceptionCallback(message.MessageType, message.AssetName, messageId, timeOutExceptionSerialized, typeof(TimeoutException));
                    }
                }
            }
        }

        public void WaitAll()
        {
            //it's shitty, but no better choice IMHO...
            while(waitingResponseMessages.Count > 0)
            {
                Task.Delay(100).Wait();
            }
        }

        protected abstract void DisposeCalled();
        public abstract void TargetProxyDisposed(Guid instanceId);

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    DisposeCalled();
                    var innerObject = InnerObject;
                    if (shouldDisposeInnerObject)
                        ((IDisposable)innerObject).Dispose();
                    innerObject.SendExceptionCallback -= SendExceptionHandler;
                    innerObject.SendMessageCallback -= SendMessageHandler;
                    innerObject.MessageWaitingTimedOutCallback -= MessageWaitingTimedOut;
                    if (redirectedExceptionRaisedCallback != null)
                        innerObject.RedirectedExceptionRaisedCallback -= redirectedExceptionRaisedCallback;
                    innerObject = null;
                    InnerObject = null;
                    redirectedExceptionRaisedCallback = null;
                    sendMessageCallback = null;
                    sendExceptionCallback = null;
                    serializeExceptionCallback = null;
                    deserializeExceptionCallback = null;
                    queryTargetSiteCallback = null;
                    queryDefaultTargetSiteCallback = null;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~RemoteAgencyManagingObject() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }

    delegate void SendMessage<TSerialized>(Guid targetSiteId, Guid senderInstanceId, Guid targetInstanceId, MessageType messageType, string assetName, Guid messageId, bool isOneWay, TSerialized serialized, Type[] genericArguments);
    delegate void SendException<TSerialized>(Guid targetSiteId, Guid senderInstanceId, Guid targetInstanceId, MessageType messageType, string assetName, Guid messageId, TSerialized serializedException, Type exceptionType);
    delegate Guid QueryTargetSite(Guid instanceId);
    delegate Guid QueryDefaultTargetSite();

    class RemoteAgencyManagingProxyObject<TSerialized> : RemoteAgencyManagingObject<TSerialized>
    {
        internal Type InterfaceType { get; private set; }
        internal RemoteAgencyManagingProxyObject(ICommunicate<TSerialized> innerObject, bool isDisposable, Type interfaceType,
            Guid localInstanceId, Guid defaultRemoteInstanceId, WrappedException timeOutException,
            SendMessage<TSerialized> sendMessageCallback, SendException<TSerialized> sendExceptionCallback,
            SerializeException<TSerialized> serializeExceptionCallback, DeserializeException<TSerialized> deserializeExceptionCallback,
            RedirectedExceptionRaisedCallback redirectedExceptionRaisedCallback, QueryTargetSite queryTargetSiteCallback, QueryDefaultTargetSite queryDefaultTargetSiteCallback)
            : base(innerObject, isDisposable, localInstanceId, defaultRemoteInstanceId, timeOutException, sendMessageCallback, sendExceptionCallback, serializeExceptionCallback, deserializeExceptionCallback, 
                  redirectedExceptionRaisedCallback, queryTargetSiteCallback, queryDefaultTargetSiteCallback)
        {
            InterfaceType = interfaceType;
        }
        protected override void DisposeCalled()
        {
            InterfaceType = null;
        }

        public override void TargetProxyDisposed(Guid instanceId)
        {
        }

        public override bool IsProxy => true;

        public override bool IsInterfaceImplemented(Type interfaceType) => InterfaceType == interfaceType;

        protected override void SendMessageHandler(MessageType messageType, string assetName, Guid messageId, bool isOneWay, TSerialized serialized, Type[] genericArguments)
            => DirectSendMessage(messageType, assetName, messageId, isOneWay, serialized, genericArguments);

        protected override void SendExceptionHandler(MessageType messageType, string assetName, Guid messageId, TSerialized serializedException, Type exceptionType)
            => DirectSendException(messageType, assetName, messageId, serializedException, exceptionType);
    }

    delegate void DisposingMessageRequiredProxyAddedCallback(Guid siteId, Guid proxyInstanceId, Guid serviceWrapperInstanceId);

    class RemoteAgencyManagingServiceWrapperObject<TSerialized> : RemoteAgencyManagingObject<TSerialized>
    {
        internal List<Type> InterfaceTypes { get; private set; }

        public override bool IsProxy => false;
        DisposingMessageRequiredProxyAddedCallback disposingMessageRequiredProxyAddedCallback;

        public RemoteAgencyManagingServiceWrapperObject(ICommunicate<TSerialized> innerObject, bool isDisposable, IEnumerable<Type> interfaceTypes,
            Guid localInstanceId, WrappedException timeOutException,
            SendMessage<TSerialized> sendMessageCallback, SendException<TSerialized> sendExceptionCallback, 
            SerializeException<TSerialized> serializeExceptionCallback, DeserializeException<TSerialized> deserializeExceptionCallback,
            RedirectedExceptionRaisedCallback redirectedExceptionRaisedCallback, QueryTargetSite queryTargetSiteCallback, QueryDefaultTargetSite queryDefaultTargetSiteCallback,
            DisposingMessageRequiredProxyAddedCallback disposingMessageRequiredProxyAddedCallback)
            : base(innerObject, isDisposable, localInstanceId, Guid.Empty, timeOutException, sendMessageCallback, sendExceptionCallback, serializeExceptionCallback, deserializeExceptionCallback,
                  redirectedExceptionRaisedCallback, queryTargetSiteCallback, queryDefaultTargetSiteCallback)
        {
            InterfaceTypes = new List<Type>(interfaceTypes);
            this.disposingMessageRequiredProxyAddedCallback = disposingMessageRequiredProxyAddedCallback;
        }

        protected override void DisposeCalled()
        {
            InterfaceTypes = null;
            disposingMessageRequiredProxyAddedCallback = null;
            eventResponder.Dispose();
        }

        public override void TargetProxyDisposed(Guid instanceId)
        {
            senderSite.TryRemove(instanceId, out _);
            var events = registeredEvents.Where(i => i.Value.Contains(instanceId)).ToArray();
            foreach (var registeredEvent in events)
            {
                var instances = registeredEvent.Value;
                var assetName = registeredEvent.Key;
                lock (instances)
                {
                    if (instances.Count == 0)
                        continue; //should not be fired. keep for safe
                    while (true)
                    {
                        var index = instances.IndexOf(instanceId);
                        if (index == -1) break;
                        instances.RemoveAt(index);
                    }
                    if (instances.Count == 0)
                    {
                        base.ProcessMessage(Guid.Empty, Guid.Empty, MessageType.EventRemove, assetName, Guid.NewGuid(), true, defaultTSerialized, null); //no need for response.
                    }
                }
            }
        }

        public override bool IsInterfaceImplemented(Type interfaceType) => InterfaceTypes.Contains(interfaceType);

        protected override void SendMessageHandler(MessageType messageType, string assetName, Guid messageId, bool isOneWay, TSerialized serialized, Type[] genericArguments)
        {
            if (messageType == MessageType.Event)
            {
                EventRaised(assetName, messageId, isOneWay, serialized, genericArguments);
            }
            else if (messageType == MessageType.EventAdd || messageType == MessageType.EventRemove)
            {
                eventResponder.SetResult(messageId, serialized);
            }
            else
            {
                DirectSendMessage(messageType, assetName, messageId, isOneWay, serialized, genericArguments);
            }
        }

        protected override void SendExceptionHandler(MessageType messageType, string assetName, Guid messageId, TSerialized serializedException, Type exceptionType)
        {
            if (messageType == MessageType.EventAdd || messageType == MessageType.EventRemove)
            {
                eventResponder.SetException(messageId, DeserializeException(serializedException, exceptionType));
            }
            else
            {
                DirectSendException(messageType, assetName, messageId, serializedException, exceptionType);
            }
        }

        public override void ProcessMessage(Guid senderSiteId, Guid senderInstanceId, MessageType messageType, string assetName, Guid messageId, bool isOneWay, TSerialized serialized, Type[] genericArguments)
        {
            if (messageType == MessageType.Event)
            {
                eventResponder.SetResult(messageId, serialized);
                return;
            }
            else if (messageType == MessageType.EventAdd)
            {
                EventAdd(senderSiteId, senderInstanceId, assetName, messageId);
                return;
            }
            else if (messageType == MessageType.EventRemove)
            {
                EventRemove(senderSiteId, senderInstanceId, assetName, messageId);
                return;
            }
            base.ProcessMessage(senderSiteId, senderInstanceId, messageType, assetName, messageId, isOneWay, serialized, genericArguments);
        }
        public override void ProcessException(MessageType messageType, string assetName, Guid messageId, TSerialized serializedException, Type exceptionType)
        {
            if (messageType == MessageType.Event)
            {
                eventResponder.SetException(messageId, DeserializeException(serializedException, exceptionType));
                return;
            }
            base.ProcessException(messageType, assetName, messageId, serializedException, exceptionType);
        }


        ConcurrentDictionary<Guid, List<Guid>> eventSubMessageId = new ConcurrentDictionary<Guid, List<Guid>>();
        ConcurrentDictionary<Guid, Guid> senderSite = new ConcurrentDictionary<Guid, Guid>();

        void EventRaised(string assetName, Guid messageId, bool isOneWay, TSerialized serialized, Type[] genericArguments)
        {
            List<Guid> subMessageId = null;
            Guid[] instanceId = null;
            if (registeredEvents.TryGetValue(assetName, out var instances))
            {
                lock (instances)
                {
                    if (instances.Count != 0)
                    {
                        instanceId = instances.ToArray();
                        subMessageId = new List<Guid>(instances.Count);
                    }
                }
            }
            if (subMessageId == null)
            {
                //This event should not be raised. Handler is not linked.
                throw new EventHandlerRemovedException(assetName);
                //if (!isOneWay)
                //{
                //    var serializedException = SerializeException(new WrappedException<EventHandlerRemovedException>(new EventHandlerRemovedException(assetName)));
                //    base.ProcessException(MessageType.Event, assetName, messageId, serializedException, typeof(EventHandlerRemovedException));
                //}
                //return;
            }

            for (int index = 0; index < instanceId.Length; index++)
            {
                var instance = instanceId[index];
                if (senderSite.TryGetValue(instance, out Guid senderSiteId))
                {
                    var id = Guid.NewGuid();
                    if (!isOneWay) eventResponder.Prepare(id);
                    DirectSendMessage(senderSiteId, instance, MessageType.Event, assetName, id, isOneWay, serialized, genericArguments);
                    subMessageId.Add(id);
                }
            }

            if (!isOneWay && subMessageId != null && subMessageId.Count > 0)
            {
                eventSubMessageId.TryAdd(messageId, subMessageId);
                Task waiting = Task.Run(() => EventWaitingReturns(messageId, subMessageId, assetName));
            }
        }

        protected override void MessageWaitingTimedOutMoreHandling(Guid messageId)
        {
            if (!eventSubMessageId.TryRemove(messageId, out var subMessageId))
            {
                return;
            }

            foreach(var id in subMessageId)
            {
                eventResponder.SetException(id, timeOutException);
            }
        }

        void EventWaitingReturns(Guid messageId, List<Guid> subMessageId, string assetName)
        {
            TSerialized result = defaultTSerialized;
            bool processed = false;
            Queue<Guid> subMessageIdQueue = new Queue<Guid>(subMessageId);
            try
            {
                while (subMessageIdQueue.Count > 0)
                {
                    try
                    {
                        var id = subMessageIdQueue.Dequeue();
                        result = eventResponder.GetResult(id, -1);
                        processed = true;
                    }
                    catch (ObjectNotFoundException)
                    {
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                if (eventSubMessageId.ContainsKey(messageId))
                {
                    TSerialized exception = SerializeException(WrappedException.Create(ex));
                    base.ProcessException(MessageType.Event, assetName, messageId, exception, ex.GetType());
                }
                return;
            }
            finally
            {
                eventSubMessageId.TryRemove(messageId, out _);
                while (subMessageIdQueue.Count > 0)
                {
                    var id = subMessageIdQueue.Dequeue();
                    eventResponder.Remove(id);
                }
            }
            if (processed)
                base.ProcessMessage(Guid.Empty, Guid.Empty, MessageType.Event, assetName, messageId, false, result, null);
            else
            {
                var serializedException = SerializeException(new WrappedException<EventHandlerRemovedException>(new EventHandlerRemovedException(assetName)));
                base.ProcessException(MessageType.Event, assetName, messageId, serializedException, typeof(EventHandlerRemovedException));
            }
        }

        ConcurrentDictionary<string, List<Guid>> registeredEvents = new ConcurrentDictionary<string, List<Guid>>(); //asset name, instance id list
        TSerialized defaultTSerialized = default(TSerialized);

        void EventAdd(Guid senderSiteId, Guid instanceId, string assetName, Guid messageId)
        {
            List<Guid> instances = registeredEvents.GetOrAdd(assetName, i => new List<Guid>());
            lock(instances)
            {
                TSerialized result = defaultTSerialized;
                if (instances.Count == 0)
                {
                    try
                    {
                        eventResponder.Prepare(messageId);
                        base.ProcessMessage(Guid.Empty, Guid.Empty, MessageType.EventAdd, assetName, messageId, true, defaultTSerialized, null);
                        result = eventResponder.GetResult(messageId, -1);
                    }
                    catch (Exception ex)
                    {
                        TSerialized exception = SerializeException(WrappedException.Create(ex));
                        DirectSendException(senderSiteId, instanceId, MessageType.EventAdd, assetName, messageId, exception, ex.GetType());
                        return;
                    }
                }
                senderSite.AddOrUpdate(instanceId, senderSiteId, (i, j) => senderSiteId);
                instances.Add(instanceId);
                disposingMessageRequiredProxyAddedCallback(senderSiteId, instanceId, LocalInstanceId);
                DirectSendMessage(senderSiteId, instanceId, MessageType.EventAdd, assetName, messageId, true, result, null);   
            }
        }

        void EventRemove(Guid senderSiteId, Guid instanceId, string assetName, Guid messageId)
        {
            List<Guid> instances;
            TSerialized result = defaultTSerialized;
            if (registeredEvents.TryGetValue(assetName, out instances))
            {
                lock(instances)
                {
                    if (instances.Count == 1)
                    {
                        var index = instances.IndexOf(instanceId);
                        if (index == 0)
                        {
                            try
                            {
                                eventResponder.Prepare(messageId);
                                base.ProcessMessage(Guid.Empty, Guid.Empty, MessageType.EventRemove, assetName, messageId, true, defaultTSerialized, null);
                                result = eventResponder.GetResult(messageId, -1);
                            }
                            catch (Exception ex)
                            {
                                TSerialized exception = SerializeException(WrappedException.Create(ex));
                                DirectSendException(senderSiteId, instanceId, MessageType.EventRemove, assetName, messageId, exception, ex.GetType());
                                return;
                            }
                            instances.Clear();
                        }
                    }
                    else if (instances.Count > 1)
                    {
                        instances.Remove(instanceId);
                    }
                }
            }
            DirectSendMessage(senderSiteId, instanceId, MessageType.EventRemove, assetName, messageId, true, result, null);
        }

        Responder<TSerialized> eventResponder = new Responder<TSerialized>();
    }
}
