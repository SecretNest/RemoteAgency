using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// The factory for creating a proxy object from the contract interface.
    /// </summary>
    /// <typeparam name="TSerialized">Type of the serialized data.</typeparam>
    /// <typeparam name="TEntityBase">Type of the parent class of all entities.</typeparam>
    /// <seealso cref="CustomizedAssetNameAttribute"/>
    /// <seealso cref="CustomizedOneWayOperatingAttribute"/>
    /// <seealso cref="CustomizedOperatingTimedoutTimeAttribute"/>
    /// <seealso cref="CustomizedEventParameterEntityPropertyNameAttribute"/>
    /// <seealso cref="CustomizedParameterEntityAttribute"/>
    /// <seealso cref="CustomizedParameterEntityPropertyNameAttribute"/>
    /// <seealso cref="CustomizedReturnEntityAttribute"/>
    /// <seealso cref="CustomizedReturnEntityPropertyNameAttribute"/>
    /// <seealso cref="CustomizedPropertyGetRequestEntityAttribute"/>
    /// <seealso cref="CustomizedPropertyGetResponseEntityAttribute"/>
    /// <seealso cref="CustomizedPropertyGetResponsePropertyNameAttribute"/>
    /// <seealso cref="CustomizedPropertySetRequestEntityAttribute"/>
    /// <seealso cref="CustomizedPropertySetRequestPropertyNameAttribute"/>
    /// <seealso cref="CustomizedPropertySetResponseEntityAttribute"/>
    /// <seealso cref="EventParameterTwoWayPropertyAttribute"/>
    /// <seealso cref="ParameterTwoWayPropertyAttribute"/>
    /// <seealso cref="EventParameterIgnoredAttribute"/>
    /// <seealso cref="ParameterIgnoredAttribute"/>
    /// <seealso cref="EventSubscriptionAttribute"/>
    /// <seealso cref="ImportAssemblyAttribute"/>
    /// <seealso cref="LocalExceptionHandlingAttribute"/>
    /// <seealso cref="EntityCodeBuilderBase"/>
    /// <seealso cref="SerializingHelperBase{TSerialized, TEntityBase}"/>
    /// <seealso cref="ProxyCacheableAttribute"/>
    public partial class ProxyCreator<TSerialized, TEntityBase> : IAssemblyCacheOperatings<TSerialized, TEntityBase> where TEntityBase : class
    {
        readonly EntityCodeBuilderBase remoteAgencyEntityCodeBuilder;
        readonly Type remoteAgencySerializingHelperType;

        /// <summary>
        /// Initializes an instance of ProxyCreator.
        /// </summary>
        /// <param name="remoteAgencyEntityCodeBuilder">Entity code builder for building classes of entities in this proxy type.</param>
        /// <param name="remoteAgencySerializingHelperType">Type of the <see cref="SerializingHelperBase{TSerialized, TEntityBase}"/> for using in code which will be generated for this proxy type.</param>
        /// <seealso cref="EntityCodeBuilderBase"/>
        public ProxyCreator(EntityCodeBuilderBase remoteAgencyEntityCodeBuilder, Type remoteAgencySerializingHelperType)
        {
            this.remoteAgencyEntityCodeBuilder = remoteAgencyEntityCodeBuilder;
            this.remoteAgencySerializingHelperType = remoteAgencySerializingHelperType;
            assemblyBuilderHelper = new AssemblyBuilderHelper(e => MissingAssemblyRequesting?.Invoke(this, e));
        }

        /// <summary>
        /// Creates a proxy object.
        /// </summary>
        /// <typeparam name="TInterfaceContract">Type of service contract interface.</typeparam>
        /// <param name="disposeRequired">Whether the proxy object need to be disposed.</param>
        /// <returns>The proxy object.</returns>
        /// <exception cref="ArgumentException">Thrown when <typeparamref name="TInterfaceContract"/> is not an interface.</exception>
        /// <exception cref="TypeCreatingException">Thrown when exception thrown in type creating procedure.</exception>
        public TInterfaceContract CreateProxyObject<TInterfaceContract>(out bool disposeRequired) where TInterfaceContract : class
        {
            var type = typeof(TInterfaceContract);
            var typeInfo = type.GetTypeInfo();
            if (!typeInfo.IsInterface)
                throw new ArgumentException("Generic type must be an interface.");

            var assembly = LoadAssembly(type, typeInfo, out disposeRequired);
            Type constructedType = assembly.GetType("SecretNest.RemoteAgency.Created.Proxy");
            return (TInterfaceContract)FastActivator.CreateInstance(constructedType);
        }
    }
}
