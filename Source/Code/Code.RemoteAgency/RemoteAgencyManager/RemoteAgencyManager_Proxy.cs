using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecretNest.RemoteAgency
{
    public partial class RemoteAgencyManager<TNetworkMessage, TSerialized, TEntityBase> where TEntityBase : class
    {
        /// <summary>
        /// Adds a proxy.
        /// </summary>
        /// <typeparam name="TInterfaceContract">Type of service contract interface.</typeparam>
        /// <param name="proxy">Proxy object.</param>
        /// <param name="shouldDisposeInnerObject">Whether this proxy object should be disposed when being removed from this manager.</param>
        /// <param name="serviceWrapperInstanceId">Instance id of the target service wrapper.</param>
        /// <param name="proxyInstanceId">Preferred instance id of this proxy object.</param>
        /// <returns>Instance id of this proxy object.</returns>
        /// <seealso cref="AddProxy{TInterfaceContract}(ProxyCreator{TSerialized, TEntityBase}, Guid, Guid)"/>
        /// <seealso cref="AddProxy{TInterfaceContract}(ProxyCreator{TSerialized, TEntityBase}, Guid, out Guid)"/>
        /// <seealso cref="GetAllProxies"/>
        /// <seealso cref="GetAllProxies{TInterfaceContract}"/>
        /// <seealso cref="RemoveManagingObject(Guid, bool)"/>
        /// <seealso cref="RemoveAllManagingObjects(bool)"/>
        public Guid AddProxy<TInterfaceContract>(TInterfaceContract proxy, bool shouldDisposeInnerObject, Guid serviceWrapperInstanceId, Guid? proxyInstanceId = null) where TInterfaceContract : class
        {
            if (!proxyInstanceId.HasValue) proxyInstanceId = Guid.NewGuid();
            RemoteAgencyManagingProxyObject<TSerialized> managing = new RemoteAgencyManagingProxyObject<TSerialized>(
                (ICommunicate<TSerialized>)proxy, shouldDisposeInnerObject, typeof(TInterfaceContract), proxyInstanceId.Value, serviceWrapperInstanceId, timeOutException, SendMessage, SendException, SerializeException, DeserializeException, RaiseRedirectedException, QueryTargetSite, QueryDefaultTargetSite);
            managingObjects.AddOrUpdate(proxyInstanceId.Value, managing, (i, j) => managing);
            return proxyInstanceId.Value;
        }

        /// <summary>
        /// Creates a proxy and adds it to this manager with a randomized instance id.
        /// </summary>
        /// <typeparam name="TInterfaceContract">Type of service contract interface.</typeparam>
        /// <param name="creator">Proxy creator.</param>
        /// <param name="serviceWrapperInstanceId">Instance id of the target service wrapper.</param>
        /// <param name="proxyInstanceId">Instance id of this proxy object.</param>
        /// <returns>Proxy object.</returns>
        /// <seealso cref="AddProxy{TInterfaceContract}(ProxyCreator{TSerialized, TEntityBase}, Guid, Guid)"/>
        /// <seealso cref="AddProxy{TInterfaceContract}(TInterfaceContract, bool, Guid, Guid?)"/>
        /// <seealso cref="GetAllProxies"/>
        /// <seealso cref="GetAllProxies{TInterfaceContract}"/>
        /// <seealso cref="RemoveManagingObject(Guid, bool)"/>
        /// <seealso cref="RemoveAllManagingObjects(bool)"/>
        public TInterfaceContract AddProxy<TInterfaceContract>(ProxyCreator<TSerialized, TEntityBase> creator, Guid serviceWrapperInstanceId, out Guid proxyInstanceId) where TInterfaceContract : class
        {
            var proxy = creator.CreateProxyObject<TInterfaceContract>(out bool isDisposable);
            proxyInstanceId = AddProxy(proxy, isDisposable, serviceWrapperInstanceId, null);
            return proxy;
        }

        /// <summary>
        /// Creates a proxy and adds it to this manager with the instance id specified.
        /// </summary>
        /// <typeparam name="TInterfaceContract">Type of service contract interface.</typeparam>
        /// <param name="creator">Proxy creator.</param>
        /// <param name="serviceWrapperInstanceId">Instance id of the target service wrapper.</param>
        /// <param name="proxyInstanceId">Instance id of this proxy object.</param>
        /// <returns>Proxy object.</returns>
        /// <seealso cref="AddProxy{TInterfaceContract}(TInterfaceContract, bool, Guid, Guid?)"/>
        /// <seealso cref="AddProxy{TInterfaceContract}(ProxyCreator{TSerialized, TEntityBase}, Guid, out Guid)"/>
        /// <seealso cref="GetAllProxies"/>
        /// <seealso cref="GetAllProxies{TInterfaceContract}"/>
        /// <seealso cref="RemoveManagingObject(Guid, bool)"/>
        /// <seealso cref="RemoveAllManagingObjects(bool)"/>
        public TInterfaceContract AddProxy<TInterfaceContract>(ProxyCreator<TSerialized, TEntityBase> creator, Guid serviceWrapperInstanceId, Guid proxyInstanceId) where TInterfaceContract : class
        {
            var proxy = creator.CreateProxyObject<TInterfaceContract>(out bool isDisposable);
            AddProxy(proxy, isDisposable, serviceWrapperInstanceId, proxyInstanceId);
            return proxy;
        }

        /// <summary>
        /// Gets all managed proxies.
        /// </summary>
        /// <returns>All managed proxies.</returns>
        /// <seealso cref="AddProxy{TInterfaceContract}(ProxyCreator{TSerialized, TEntityBase}, Guid, Guid)"/>
        /// <seealso cref="AddProxy{TInterfaceContract}(ProxyCreator{TSerialized, TEntityBase}, Guid, out Guid)"/>
        /// <seealso cref="AddProxy{TInterfaceContract}(TInterfaceContract, bool, Guid, Guid?)"/>
        /// <seealso cref="GetAllProxies{TInterfaceContract}"/>
        /// <seealso cref="RemoveManagingObject(Guid, bool)"/>
        /// <seealso cref="RemoveAllManagingObjects(bool)"/>
        public IEnumerable<ManagingProxy> GetAllProxies()
        {
            return managingObjects.Values.Where(i => i.IsProxy).Select(i => new ManagingProxy(i.LocalInstanceId, i.DefaultRemoteInstanceId, i.InnerObject));
        }

        /// <summary>
        /// Get all managed proxies by service contract interface specified.
        /// </summary>
        /// <typeparam name="TInterfaceContract">Type of service contract interface.</typeparam>
        /// <returns>All managed proxies linked with the service contract interface specified.</returns>
        /// <seealso cref="AddProxy{TInterfaceContract}(ProxyCreator{TSerialized, TEntityBase}, Guid, Guid)"/>
        /// <seealso cref="AddProxy{TInterfaceContract}(ProxyCreator{TSerialized, TEntityBase}, Guid, out Guid)"/>
        /// <seealso cref="AddProxy{TInterfaceContract}(TInterfaceContract, bool, Guid, Guid?)"/>
        /// <seealso cref="GetAllProxies"/>
        /// <seealso cref="RemoveManagingObject(Guid, bool)"/>
        /// <seealso cref="RemoveAllManagingObjects(bool)"/>
        public IEnumerable<ManagingProxy<TInterfaceContract>> GetAllProxies<TInterfaceContract>() where TInterfaceContract : class
        {
            var type = typeof(TInterfaceContract);
            return managingObjects.Values.Where(i => i.IsProxy && i.IsInterfaceImplemented(type))
                .Select(i => new ManagingProxy<TInterfaceContract>(i.LocalInstanceId, i.DefaultRemoteInstanceId, (TInterfaceContract)i.InnerObject));
        }
    }

    /// <summary>
    /// Represents a managed proxy.
    /// </summary>
    /// <typeparam name="TInterfaceContract">Type of service contract interface.</typeparam>
    public class ManagingProxy<TInterfaceContract>
    {
        /// <summary>
        /// Gets the instance id of the proxy object.
        /// </summary>
        public Guid ProxyInstanceId { get; }
        /// <summary>
        /// Gets the instance id of the target service wrapper object.
        /// </summary>
        public Guid TargetServiceWrapperInstanceId { get; }

        /// <summary>
        /// Gets the proxy object.
        /// </summary>
        public TInterfaceContract ProxyObject { get; }

        /// <summary>
        /// Initializes an instance of the ManagingProxy.
        /// </summary>
        /// <param name="proxyInstanceId">Instance id of the proxy object.</param>
        /// <param name="targetServiceWrapperInstanceId">Instance id of the target service wrapper object.</param>
        /// <param name="proxyObject">Proxy object.</param>
        public ManagingProxy(Guid proxyInstanceId, Guid targetServiceWrapperInstanceId, TInterfaceContract proxyObject)
        {
            ProxyInstanceId = proxyInstanceId;
            TargetServiceWrapperInstanceId = targetServiceWrapperInstanceId;
            ProxyObject = proxyObject;
        }
    }

    /// <summary>
    /// Represents a managed proxy.
    /// </summary>
    public class ManagingProxy : ManagingProxy<object>
    {
        /// <summary>
        /// Initializes an instance of the ManagingProxy.
        /// </summary>
        /// <param name="proxyInstanceId">Instance id of the proxy object.</param>
        /// <param name="targetServiceWrapperInstanceId">Instance id of the target service wrapper object.</param>
        /// <param name="proxyObject">Proxy object.</param>
        public ManagingProxy(Guid proxyInstanceId, Guid targetServiceWrapperInstanceId, object proxyObject) : base(proxyInstanceId, targetServiceWrapperInstanceId, proxyObject)
        { }
    }
}
