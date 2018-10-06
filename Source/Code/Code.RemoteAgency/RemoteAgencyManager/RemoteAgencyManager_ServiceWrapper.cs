using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretNest.RemoteAgency
{
    public partial class RemoteAgencyManager<TNetworkMessage, TSerialized, TEntityBase> where TEntityBase : class
    {
        /// <summary>
        /// Adds a service wrapper object.
        /// </summary>
        /// <param name="serviceWrapperObject">Service wrapper object.</param>
        /// <param name="interfaceContracts">All types of service contract interfaces.</param>
        /// <param name="shouldDisposeInnerObject">Whether this service wrapper object should be disposed when being removed from this manager.</param>
        /// <param name="serviceWrapperInstanceId">Preferred instance id of this service wrapper object.</param>
        /// <returns>Instance id of this service wrapper object.</returns>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(object, bool, Guid?)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(ServiceWrapperCreator{TSerialized, TEntityBase}, TInterfaceContract, Guid)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(ServiceWrapperCreator{TSerialized, TEntityBase}, TInterfaceContract, out Guid)"/>
        /// <seealso cref="AddServiceWrapper{TServiceObject}(ServiceWrapperCreator{TSerialized, TEntityBase}, TServiceObject, IEnumerable{Type}, Guid)"/>
        /// <seealso cref="AddServiceWrapper{TServiceObject}(ServiceWrapperCreator{TSerialized, TEntityBase}, TServiceObject, IEnumerable{Type}, out Guid)"/>
        /// <seealso cref="RemoveTypeFromServiceWrapper(Guid, Type, out bool, bool)"/>
        /// <seealso cref="GetAllServiceWrappers"/>
        /// <seealso cref="GetAllServiceWrappers{TInterfaceContract}"/>
        /// <seealso cref="RemoveManagingObject(Guid, bool)"/>
        /// <seealso cref="RemoveAllManagingObjects(bool)"/>
        public Guid AddServiceWrapper(object serviceWrapperObject, IEnumerable<Type> interfaceContracts, bool shouldDisposeInnerObject, Guid? serviceWrapperInstanceId = null)
        {
            if (!serviceWrapperInstanceId.HasValue) serviceWrapperInstanceId = Guid.NewGuid();
            RemoteAgencyManagingServiceWrapperObject<TSerialized> managing = new RemoteAgencyManagingServiceWrapperObject<TSerialized>(
                (ICommunicate<TSerialized>)serviceWrapperObject, shouldDisposeInnerObject, interfaceContracts, serviceWrapperInstanceId.Value, timeOutException, SendMessage, SendException, SerializeException, DeserializeException, RaiseRedirectedException, QueryTargetSite, QueryDefaultTargetSite, OnDisposingMessageRequiredProxyAdded);
            managingObjects.AddOrUpdate(serviceWrapperInstanceId.Value, managing, (i, j) => managing);
            return serviceWrapperInstanceId.Value;
        }

        /// <summary>
        /// Adds a service wrapper object.
        /// </summary>
        /// <typeparam name="TInterfaceContract">Type of service contract interface.</typeparam>
        /// <param name="serviceWrapperObject">Service wrapper object.</param>
        /// <param name="shouldDisposeInnerObject">Whether this service wrapper object should be disposed when being removed from this manager.</param>
        /// <param name="serviceWrapperInstanceId">Preferred instance id of this service wrapper object.</param>
        /// <returns>Instance id of this service wrapper object.</returns>
        /// <seealso cref="AddServiceWrapper(object, IEnumerable{Type}, bool, Guid?)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(ServiceWrapperCreator{TSerialized, TEntityBase}, TInterfaceContract, Guid)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(ServiceWrapperCreator{TSerialized, TEntityBase}, TInterfaceContract, out Guid)"/>
        /// <seealso cref="AddServiceWrapper{TServiceObject}(ServiceWrapperCreator{TSerialized, TEntityBase}, TServiceObject, IEnumerable{Type}, Guid)"/>
        /// <seealso cref="AddServiceWrapper{TServiceObject}(ServiceWrapperCreator{TSerialized, TEntityBase}, TServiceObject, IEnumerable{Type}, out Guid)"/>
        /// <seealso cref="RemoveTypeFromServiceWrapper(Guid, Type, out bool, bool)"/>
        /// <seealso cref="GetAllServiceWrappers"/>
        /// <seealso cref="GetAllServiceWrappers{TInterfaceContract}"/>
        /// <seealso cref="RemoveManagingObject(Guid, bool)"/>
        /// <seealso cref="RemoveAllManagingObjects(bool)"/>
        public Guid AddServiceWrapper<TInterfaceContract>(object serviceWrapperObject, bool shouldDisposeInnerObject, Guid? serviceWrapperInstanceId = null) where TInterfaceContract : class
            => AddServiceWrapper(serviceWrapperObject, new Type[] { typeof(TInterfaceContract) }, shouldDisposeInnerObject, serviceWrapperInstanceId);

        /// <summary>
        /// Creates a service wrapper object and adds it to this manager with a randomized instance id.
        /// </summary>
        /// <typeparam name="TServiceObject">Type of service object.</typeparam>
        /// <param name="creator">Service wrapper creator.</param>
        /// <param name="serviceObject">Existed service object which will be linked to the new created service wrapper object.</param>
        /// <param name="interfaceContracts">All types of service contract interfaces.</param>
        /// <param name="serviceWrapperInstanceId">Instance id of this service wrapper object.</param>
        /// <returns>Service wrapper object.</returns>
        /// <seealso cref="AddServiceWrapper(object, IEnumerable{Type}, bool, Guid?)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(object, bool, Guid?)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(ServiceWrapperCreator{TSerialized, TEntityBase}, TInterfaceContract, Guid)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(ServiceWrapperCreator{TSerialized, TEntityBase}, TInterfaceContract, out Guid)"/>
        /// <seealso cref="AddServiceWrapper{TServiceObject}(ServiceWrapperCreator{TSerialized, TEntityBase}, TServiceObject, IEnumerable{Type}, Guid)"/>
        /// <seealso cref="RemoveTypeFromServiceWrapper(Guid, Type, out bool, bool)"/>
        /// <seealso cref="GetAllServiceWrappers"/>
        /// <seealso cref="GetAllServiceWrappers{TInterfaceContract}"/>
        /// <seealso cref="RemoveManagingObject(Guid, bool)"/>
        /// <seealso cref="RemoveAllManagingObjects(bool)"/>
        /// <seealso cref="ICommunicate{TSerialized}"/>
        public ICommunicate<TSerialized> AddServiceWrapper<TServiceObject>(ServiceWrapperCreator<TSerialized, TEntityBase> creator, TServiceObject serviceObject, IEnumerable<Type> interfaceContracts, out Guid serviceWrapperInstanceId) where TServiceObject : class
        {
            var serviceWrapperObject = creator.CreateServiceWrapperObject(out bool isDisposable, serviceObject, interfaceContracts.ToArray());
            serviceWrapperInstanceId = AddServiceWrapper(serviceWrapperObject, interfaceContracts, isDisposable, null);
            return serviceWrapperObject;
        }

        /// <summary>
        /// Creates a service wrapper object and adds it to this manager with the instance id specified.
        /// </summary>
        /// <typeparam name="TServiceObject">Type of service object.</typeparam>
        /// <param name="creator">Service wrapper creator.</param>
        /// <param name="serviceObject">Existed service object which will be linked to the new created service wrapper object.</param>
        /// <param name="interfaceContracts">All types of service contract interfaces.</param>
        /// <param name="serviceWrapperInstanceId">Instance id of this service wrapper object.</param>
        /// <returns>Service wrapper object.</returns>
        /// <seealso cref="AddServiceWrapper(object, IEnumerable{Type}, bool, Guid?)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(object, bool, Guid?)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(ServiceWrapperCreator{TSerialized, TEntityBase}, TInterfaceContract, Guid)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(ServiceWrapperCreator{TSerialized, TEntityBase}, TInterfaceContract, out Guid)"/>
        /// <seealso cref="AddServiceWrapper{TServiceObject}(ServiceWrapperCreator{TSerialized, TEntityBase}, TServiceObject, IEnumerable{Type}, out Guid)"/>
        /// <seealso cref="RemoveTypeFromServiceWrapper(Guid, Type, out bool, bool)"/>
        /// <seealso cref="GetAllServiceWrappers"/>
        /// <seealso cref="GetAllServiceWrappers{TInterfaceContract}"/>
        /// <seealso cref="RemoveManagingObject(Guid, bool)"/>
        /// <seealso cref="RemoveAllManagingObjects(bool)"/>
        /// <seealso cref="ICommunicate{TSerialized}"/>
        public ICommunicate<TSerialized> AddServiceWrapper<TServiceObject>(ServiceWrapperCreator<TSerialized, TEntityBase> creator, TServiceObject serviceObject, IEnumerable<Type> interfaceContracts, Guid serviceWrapperInstanceId) where TServiceObject : class
        {
            var serviceWrapperObject = creator.CreateServiceWrapperObject(out bool isDisposable, serviceObject, interfaceContracts.ToArray());
            AddServiceWrapper(serviceWrapperObject, interfaceContracts, isDisposable, serviceWrapperInstanceId);
            return serviceWrapperObject;
        }

        /// <summary>
        /// Creates a service wrapper object and adds it to this manager with a randomized instance id.
        /// </summary>
        /// <typeparam name="TInterfaceContract">Type of service contract interface.</typeparam>
        /// <param name="creator">Service wrapper creator.</param>
        /// <param name="serviceObject">Existed service object which will be linked to the new created service wrapper object.</param>
        /// <param name="serviceWrapperInstanceId">Instance id of this service wrapper object.</param>
        /// <returns>Service wrapper object.</returns>
        /// <seealso cref="AddServiceWrapper(object, IEnumerable{Type}, bool, Guid?)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(object, bool, Guid?)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(ServiceWrapperCreator{TSerialized, TEntityBase}, TInterfaceContract, Guid)"/>
        /// <seealso cref="AddServiceWrapper{TServiceObject}(ServiceWrapperCreator{TSerialized, TEntityBase}, TServiceObject, IEnumerable{Type}, Guid)"/>
        /// <seealso cref="AddServiceWrapper{TServiceObject}(ServiceWrapperCreator{TSerialized, TEntityBase}, TServiceObject, IEnumerable{Type}, out Guid)"/>
        /// <seealso cref="RemoveTypeFromServiceWrapper(Guid, Type, out bool, bool)"/>
        /// <seealso cref="GetAllServiceWrappers"/>
        /// <seealso cref="GetAllServiceWrappers{TInterfaceContract}"/>
        /// <seealso cref="RemoveManagingObject(Guid, bool)"/>
        /// <seealso cref="RemoveAllManagingObjects(bool)"/>
        /// <seealso cref="ICommunicate{TSerialized}"/>
        public ICommunicate<TSerialized> AddServiceWrapper<TInterfaceContract>(ServiceWrapperCreator<TSerialized, TEntityBase> creator, TInterfaceContract serviceObject, out Guid serviceWrapperInstanceId) where TInterfaceContract : class
            => AddServiceWrapper(creator, serviceObject, new List<Type>() { typeof(TInterfaceContract) }, out serviceWrapperInstanceId);

        /// <summary>
        /// Creates a service wrapper object and adds it to this manager with the instance id specified.
        /// </summary>
        /// <typeparam name="TInterfaceContract">Type of service contract interface.</typeparam>
        /// <param name="creator">Service wrapper creator.</param>
        /// <param name="serviceObject">Existed service object which will be linked to the new created service wrapper object.</param>
        /// <param name="serviceWrapperInstanceId">Instance id of this service wrapper object.</param>
        /// <returns>Service wrapper object.</returns>
        /// <seealso cref="AddServiceWrapper(object, IEnumerable{Type}, bool, Guid?)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(object, bool, Guid?)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(ServiceWrapperCreator{TSerialized, TEntityBase}, TInterfaceContract, out Guid)"/>
        /// <seealso cref="AddServiceWrapper{TServiceObject}(ServiceWrapperCreator{TSerialized, TEntityBase}, TServiceObject, IEnumerable{Type}, Guid)"/>
        /// <seealso cref="AddServiceWrapper{TServiceObject}(ServiceWrapperCreator{TSerialized, TEntityBase}, TServiceObject, IEnumerable{Type}, out Guid)"/>
        /// <seealso cref="RemoveTypeFromServiceWrapper(Guid, Type, out bool, bool)"/>
        /// <seealso cref="GetAllServiceWrappers"/>
        /// <seealso cref="GetAllServiceWrappers{TInterfaceContract}"/>
        /// <seealso cref="RemoveManagingObject(Guid, bool)"/>
        /// <seealso cref="RemoveAllManagingObjects(bool)"/>
        /// <seealso cref="ICommunicate{TSerialized}"/>
        public ICommunicate<TSerialized> AddServiceWrapper<TInterfaceContract>(ServiceWrapperCreator<TSerialized, TEntityBase> creator, TInterfaceContract serviceObject, Guid serviceWrapperInstanceId) where TInterfaceContract : class
            => AddServiceWrapper(creator, serviceObject, new List<Type>() { typeof(TInterfaceContract) }, serviceWrapperInstanceId);

        /// <summary>
        /// Removes a service contract type from a existed and managed service wrapper.
        /// </summary>
        /// <param name="serviceWrapperInstanceId">Instance id of this service wrapper object.</param>
        /// <param name="interfaceContract">Type of the service contract interface to be removed.</param>
        /// <param name="serviceWrapperObjectRemoved">Whether the service wrapper has been removed from this manager due to the last service contract interface is removed.</param>
        /// <param name="cancelMessagesInWaiting">True: break all waiting messages (throwing timeout exceptions to all messages); False: wait for processing of all messages finished.</param>
        /// <seealso cref="AddServiceWrapper(object, IEnumerable{Type}, bool, Guid?)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(object, bool, Guid?)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(ServiceWrapperCreator{TSerialized, TEntityBase}, TInterfaceContract, Guid)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(ServiceWrapperCreator{TSerialized, TEntityBase}, TInterfaceContract, out Guid)"/>
        /// <seealso cref="AddServiceWrapper{TServiceObject}(ServiceWrapperCreator{TSerialized, TEntityBase}, TServiceObject, IEnumerable{Type}, Guid)"/>
        /// <seealso cref="AddServiceWrapper{TServiceObject}(ServiceWrapperCreator{TSerialized, TEntityBase}, TServiceObject, IEnumerable{Type}, out Guid)"/>
        /// <seealso cref="GetAllServiceWrappers"/>
        /// <seealso cref="GetAllServiceWrappers{TInterfaceContract}"/>
        /// <seealso cref="RemoveManagingObject(Guid, bool)"/>
        /// <seealso cref="RemoveAllManagingObjects(bool)"/>
        public void RemoveTypeFromServiceWrapper(Guid serviceWrapperInstanceId, Type interfaceContract, out bool serviceWrapperObjectRemoved, bool cancelMessagesInWaiting = false)
        {
            managingObjects.TryGetValue(serviceWrapperInstanceId, out var item);
            if (item.IsProxy)
            {
                serviceWrapperObjectRemoved = false;
                return;
            }
            var serviceWrapper = (RemoteAgencyManagingServiceWrapperObject<TSerialized>)item;
            lock (serviceWrapper)
            {
                serviceWrapper.InterfaceTypes.Remove(interfaceContract);
                if (serviceWrapper.InterfaceTypes.Count == 0)
                {
                    serviceWrapperObjectRemoved = true;
                    RemoveManagingObject(serviceWrapperInstanceId, cancelMessagesInWaiting);
                }
                else
                {
                    serviceWrapperObjectRemoved = false;
                }
            }
        }

        /// <summary>
        /// Get all managed service wrappers.
        /// </summary>
        /// <returns>All managed service wrappers.</returns>
        /// <seealso cref="AddServiceWrapper(object, IEnumerable{Type}, bool, Guid?)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(object, bool, Guid?)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(ServiceWrapperCreator{TSerialized, TEntityBase}, TInterfaceContract, Guid)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(ServiceWrapperCreator{TSerialized, TEntityBase}, TInterfaceContract, out Guid)"/>
        /// <seealso cref="AddServiceWrapper{TServiceObject}(ServiceWrapperCreator{TSerialized, TEntityBase}, TServiceObject, IEnumerable{Type}, Guid)"/>
        /// <seealso cref="AddServiceWrapper{TServiceObject}(ServiceWrapperCreator{TSerialized, TEntityBase}, TServiceObject, IEnumerable{Type}, out Guid)"/>
        /// <seealso cref="RemoveTypeFromServiceWrapper(Guid, Type, out bool, bool)"/>
        /// <seealso cref="GetAllServiceWrappers{TInterfaceContract}"/>
        /// <seealso cref="RemoveManagingObject(Guid, bool)"/>
        /// <seealso cref="RemoveAllManagingObjects(bool)"/>
        public IEnumerable<ManagingServiceWrapper> GetAllServiceWrappers()
        {
            return managingObjects.Values.Where(i => !i.IsProxy)
                .Select(i => new ManagingServiceWrapper(i.LocalInstanceId, i.InnerObject, ((RemoteAgencyManagingServiceWrapperObject<TSerialized>)i).InterfaceTypes));
        }

        /// <summary>
        /// Get all managed service wrappers linked to the service contract interface specified.
        /// </summary>
        /// <typeparam name="TInterfaceContract">Type of service contract interface.</typeparam>
        /// <returns>All managed service wrappers linked with the service contract interface specified.</returns>
        /// <seealso cref="AddServiceWrapper(object, IEnumerable{Type}, bool, Guid?)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(object, bool, Guid?)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(ServiceWrapperCreator{TSerialized, TEntityBase}, TInterfaceContract, Guid)"/>
        /// <seealso cref="AddServiceWrapper{TInterfaceContract}(ServiceWrapperCreator{TSerialized, TEntityBase}, TInterfaceContract, out Guid)"/>
        /// <seealso cref="AddServiceWrapper{TServiceObject}(ServiceWrapperCreator{TSerialized, TEntityBase}, TServiceObject, IEnumerable{Type}, Guid)"/>
        /// <seealso cref="AddServiceWrapper{TServiceObject}(ServiceWrapperCreator{TSerialized, TEntityBase}, TServiceObject, IEnumerable{Type}, out Guid)"/>
        /// <seealso cref="RemoveTypeFromServiceWrapper(Guid, Type, out bool, bool)"/>
        /// <seealso cref="GetAllServiceWrappers"/>
        /// <seealso cref="RemoveManagingObject(Guid, bool)"/>
        /// <seealso cref="RemoveAllManagingObjects(bool)"/>
        public IEnumerable<ManagingServiceWrapper> GetAllServiceWrappers<TInterfaceContract>() where TInterfaceContract : class
        {
            var type = typeof(TInterfaceContract);
            return managingObjects.Values.Where(i => !i.IsProxy && i.IsInterfaceImplemented(type))
                .Select(i => new ManagingServiceWrapper(i.LocalInstanceId, i.InnerObject, ((RemoteAgencyManagingServiceWrapperObject<TSerialized>)i).InterfaceTypes));
        }
    }

    /// <summary>
    /// Represents a managed service wrapper.
    /// </summary>
    public class ManagingServiceWrapper
    {
        /// <summary>
        /// Gets the instance id of the service wrapper object.
        /// </summary>
        public Guid ServiceWrapperInstanceId { get; }
        /// <summary>
        /// Gets the service wrapper object.
        /// </summary>
        public object ServiceWrapperObject { get; }
        /// <summary>
        /// Gets the collection of all service contract interfaces.
        /// </summary>
        public IReadOnlyCollection<Type> InterfaceContracts { get; }

        /// <summary>
        /// Initializes an instance of the ManagingServiceWrapper.
        /// </summary>
        /// <param name="serviceWrapperInstanceId">Instance id of the service wrapper object.</param>
        /// <param name="serviceWrapperObject">Service mapper object.</param>
        /// <param name="interfaceContracts">Collection of all service contract interfaces.</param>
        public ManagingServiceWrapper(Guid serviceWrapperInstanceId, object serviceWrapperObject, IReadOnlyCollection<Type> interfaceContracts)
        {
            ServiceWrapperInstanceId = serviceWrapperInstanceId;
            ServiceWrapperObject = serviceWrapperObject;
            InterfaceContracts = interfaceContracts;
        }
    }
}
