using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// The factory for creating a service wrapper object from the contact interfaces and link it to the existed service object.
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
    /// <seealso cref="ServiceWrapperCacheableAttribute"/>
    public partial class ServiceWrapperCreator<TSerialized, TEntityBase> : IAssemblyCacheOperatings<TSerialized, TEntityBase> where TEntityBase : class
    {
        readonly EntityCodeBuilderBase remoteAgencyEntityCodeBuilder;
        readonly Type remoteAgencySerializingHelperType;

        /// <summary>
        /// Initializes an instance of ServiceWrapperCreator.
        /// </summary>
        /// <param name="remoteAgencyEntityCodeBuilder">Entity code builder for building classes of entities in this service wrapper type.</param>
        /// <param name="remoteAgencySerializingHelperType">Type of the <see cref="SerializingHelperBase{TSerialized, TEntityBase}"/> for using in code which will be generated for this service wrapper type.</param>
        /// <seealso cref="EntityCodeBuilderBase"/>
        public ServiceWrapperCreator(EntityCodeBuilderBase remoteAgencyEntityCodeBuilder, Type remoteAgencySerializingHelperType)
        {
            this.remoteAgencyEntityCodeBuilder = remoteAgencyEntityCodeBuilder;
            this.remoteAgencySerializingHelperType = remoteAgencySerializingHelperType;
            assemblyBuilderHelper = new AssemblyBuilderHelper(e => MissingAssemblyRequesting?.Invoke(this, e));
        }

        /// <summary>
        /// Creates a service wrapper object based on one service contract interface.
        /// </summary>
        /// <typeparam name="TServiceObject">Type of service object.</typeparam>
        /// <param name="disposeRequired">Whether the service wrapper object need to be disposed.</param>
        /// <param name="serviceObject">Existed service object which will be linked to the new created service wrapper object.</param>
        /// <param name="interfaceContract">Type of service contract interface.</param>
        /// <returns>The service wrapper object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="interfaceContract"/> is set to null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="interfaceContract"/> is not an interface.</exception>
        /// <exception cref="TypeCreatingException">Thrown when exception thrown in type creating procedure.</exception>
        /// <seealso cref="CreateServiceWrapperObject{TServiceObject}(out bool, TServiceObject, Type[])"/>
        /// <seealso cref="ICommunicate{TSerialized}"/>
        public ICommunicate<TSerialized> CreateServiceWrapperObject<TServiceObject>(out bool disposeRequired, TServiceObject serviceObject, Type interfaceContract) where TServiceObject : class
        {
            if (interfaceContract == null)
                throw new ArgumentNullException(nameof(interfaceContract));
            var typeInfo = interfaceContract.GetTypeInfo();
            if (!typeInfo.IsInterface)
                throw new ArgumentException("Type must be an interface.", nameof(interfaceContract));
            return CreateServiceWrapperObject(out disposeRequired, serviceObject, new Type[] { interfaceContract }, new TypeInfo[] { typeInfo });
        }

        /// <summary>
        /// Creates a service wrapper object based on multiple service contract interfaces.
        /// </summary>
        /// <typeparam name="TServiceObject">Type of service object.</typeparam>
        /// <param name="disposeRequired">Whether the service wrapper object need to be disposed.</param>
        /// <param name="serviceObject">Existed service object which will be linked to the new created service wrapper object.</param>
        /// <param name="interfaceContracts">Types of service contract interfaces.</param>
        /// <returns>The service wrapper object.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="interfaceContracts"/> is set to null.</exception>
        /// <exception cref="ArgumentException">Thrown when there is no type specified in <paramref name="interfaceContracts"/>, or any of <paramref name="interfaceContracts"/> is not an interface.</exception>
        /// <exception cref="TypeCreatingException">Thrown when exception thrown in type creating procedure.</exception>
        /// <seealso cref="CreateServiceWrapperObject{TServiceObject}(out bool, TServiceObject, Type)"/>
        /// <seealso cref="ICommunicate{TSerialized}"/>
        public ICommunicate<TSerialized> CreateServiceWrapperObject<TServiceObject>(out bool disposeRequired, TServiceObject serviceObject, params Type[] interfaceContracts) where TServiceObject : class
        {
            if (interfaceContracts == null)
                throw new ArgumentNullException(nameof(interfaceContracts));
            if (interfaceContracts.Length == 0)
                throw new ArgumentException("None type specified.", nameof(interfaceContracts));
            var typeInfo = interfaceContracts.Select(i => i.GetTypeInfo()).ToArray();
            if (typeInfo.Any(i => !i.IsInterface))
                throw new ArgumentException("Type must be an interface.", nameof(interfaceContracts));
            return CreateServiceWrapperObject(out disposeRequired, serviceObject, interfaceContracts, typeInfo);
        }

        ICommunicate<TSerialized> CreateServiceWrapperObject<TServiceObject>(out bool disposeRequired, TServiceObject serviceObject, Type[] types, TypeInfo[] typeInfo) where TServiceObject : class
        {
            var assembly = LoadAssembly(types, typeInfo, serviceObject.GetType(), out disposeRequired);
            Type constructedType = assembly.GetType("SecretNest.RemoteAgency.Created.ServiceWrapper");
            return (ICommunicate<TSerialized>)FastActivator<TServiceObject>.CreateInstance(constructedType, serviceObject);
        }
    }
}
