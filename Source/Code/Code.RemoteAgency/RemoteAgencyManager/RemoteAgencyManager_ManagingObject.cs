using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    public partial class RemoteAgencyManager<TNetworkMessage, TSerialized, TEntityBase> where TEntityBase : class
    {
        ConcurrentDictionary<Guid, RemoteAgencyManagingObject<TSerialized>> managingObjects = new ConcurrentDictionary<Guid, RemoteAgencyManagingObject<TSerialized>>();

        static WrappedException timeOutException = WrappedException.Create(new TimeoutException());

        /// <summary>
        /// Removes a managing object by instance id.
        /// </summary>
        /// <param name="instanceId">Instance id</param>
        /// <param name="cancelMessagesInWaiting">True: break all waiting messages (throwing timeout exceptions to all messages); False: wait for processing of all messages finished.</param>
        /// <seealso cref="AddProxy{TInterfaceContract}(ProxyCreator{TSerialized, TEntityBase}, Guid, Guid)"/>
        /// <seealso cref="AddProxy{TInterfaceContract}(ProxyCreator{TSerialized, TEntityBase}, Guid, out Guid)"/>
        /// <seealso cref="AddProxy{TInterfaceContract}(TInterfaceContract, bool, Guid, Guid?)"/>
        /// <seealso cref="GetAllProxies"/>
        /// <seealso cref="GetAllProxies{TInterfaceContract}"/>
        /// <seealso cref="AddServiceWrapper(object, IEnumerable{Type}, bool, Guid?)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(object, bool, Guid?)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(ServiceWrapperCreator{TSerialized, TEntityBase}, TInterfaceContract, Guid)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(ServiceWrapperCreator{TSerialized, TEntityBase}, TInterfaceContract, out Guid)"/>
        /// <seealso cref="AddServiceWrapper{TServiceObject}(ServiceWrapperCreator{TSerialized, TEntityBase}, TServiceObject, IEnumerable{Type}, Guid)"/>
        /// <seealso cref="AddServiceWrapper{TServiceObject}(ServiceWrapperCreator{TSerialized, TEntityBase}, TServiceObject, IEnumerable{Type}, out Guid)"/>
        /// <seealso cref="RemoveTypeFromServiceWrapper(Guid, Type, out bool, bool)"/>
        /// <seealso cref="GetAllServiceWrappers"/>
        /// <seealso cref="GetAllServiceWrappers{TInterfaceContract}"/>
        /// <seealso cref="RemoveAllManagingObjects(bool)"/>
        public void RemoveManagingObject(Guid instanceId, bool cancelMessagesInWaiting)
        {
            if (managingObjects.TryRemove(instanceId, out var item))
            {
                lock (item)
                {
                    
                    if (cancelMessagesInWaiting)
                        item.CancelWaiting();
                    else
                        item.WaitAll();
                    item.Dispose();
                }
            }
        }

        /// <summary>
        /// Removes all managing objects.
        /// </summary>
        /// <param name="cancelMessagesInWaiting">True: break all waiting messages (throwing timeout exceptions to all messages); False: wait for processing of all messages finished.</param>
        /// <seealso cref="AddProxy{TInterfaceContract}(ProxyCreator{TSerialized, TEntityBase}, Guid, Guid)"/>
        /// <seealso cref="AddProxy{TInterfaceContract}(ProxyCreator{TSerialized, TEntityBase}, Guid, out Guid)"/>
        /// <seealso cref="AddProxy{TInterfaceContract}(TInterfaceContract, bool, Guid, Guid?)"/>
        /// <seealso cref="GetAllProxies"/>
        /// <seealso cref="GetAllProxies{TInterfaceContract}"/>
        /// <seealso cref="AddServiceWrapper(object, IEnumerable{Type}, bool, Guid?)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(object, bool, Guid?)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(ServiceWrapperCreator{TSerialized, TEntityBase}, TInterfaceContract, Guid)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(ServiceWrapperCreator{TSerialized, TEntityBase}, TInterfaceContract, out Guid)"/>
        /// <seealso cref="AddServiceWrapper{TServiceObject}(ServiceWrapperCreator{TSerialized, TEntityBase}, TServiceObject, IEnumerable{Type}, Guid)"/>
        /// <seealso cref="AddServiceWrapper{TServiceObject}(ServiceWrapperCreator{TSerialized, TEntityBase}, TServiceObject, IEnumerable{Type}, out Guid)"/>
        /// <seealso cref="RemoveTypeFromServiceWrapper(Guid, Type, out bool, bool)"/>
        /// <seealso cref="GetAllServiceWrappers"/>
        /// <seealso cref="GetAllServiceWrappers{TInterfaceContract}"/>
        /// <seealso cref="RemoveManagingObject(Guid, bool)"/>
        public void RemoveAllManagingObjects(bool cancelMessagesInWaiting)
        {
            foreach(var key in managingObjects.Keys)
            {
                RemoveManagingObject(key, cancelMessagesInWaiting);
            }
        }

        TSerialized SerializeException(WrappedException exception)
        {
            return SerializingHelper.SerializeExceptionWithExceptionTolerance(exception, out _);
        }

        WrappedException DeserializeException(TSerialized exception, Type exceptionType)
        {
            return SerializingHelper.DeserializeExceptionWithExceptionTolerance(exception, exceptionType, out _);
        }
    }
}
