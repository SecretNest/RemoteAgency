using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using SecretNest.RemoteAgency.Helper;
using SecretNest.RemoteAgency.TypeBuilding;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgency
    {
        /// <summary>
        /// Occurs when a type is required for constructing.
        /// </summary>
        /// <remarks><p>The handler of this event can contains the code for providing type built previously, aka from caching.</p>
        /// <p>This event is raised before <see cref="BeforeAssemblyBuilding"/>.</p></remarks>
        public event EventHandler<BeforeTypeBuildingEventArgs> BeforeTypeBuilding;

        /// <summary>
        /// Occurs when an assembly is required for building.
        /// </summary>
        /// <remarks><p>The handler of this event can contains the code for providing assembly built previously, aka from caching.</p>
        /// <p>This event is raised when no type is provided from <see cref="BeforeTypeBuilding"/>.</p></remarks>
        public event EventHandler<BeforeAssemblyBuildingEventArgs> BeforeAssemblyBuilding;

        /// <summary>
        /// Occurs before compiling the assembly.
        /// </summary>
        /// <remarks><p>By handling this, the built-in compiling component will be skipped.</p>
        /// <p>Handling of this event is required when using Neat version of RemoteAgency, which does not shipped with built-in assembly builder.</p></remarks>
        public event EventHandler<CustomizedAssemblyBuildingEventArgs> CustomizedAssemblyBuildingRequested; 

        /// <summary>
        /// Occurs when an assembly is built.
        /// </summary>
        /// <remarks>The handler of this event can contains the code for saving assembly for further use, aka caching.</remarks>
        public event EventHandler<AfterTypeAndAssemblyBuiltEventArgs> AfterTypeAndAssemblyBuilt;

        /// <summary>
        /// Defines a method for generating source code.
        /// </summary>
        /// <param name="sourceCode">Generated source code.</param>
        /// <param name="assemblyReferences">References used by the source code.</param>
        protected delegate void SourceCodeBuilderFromConstructTypeCallback(out string sourceCode,
            out List<AssemblyReference> assemblyReferences);

        /// <summary>
        /// Constructs an object of a type.
        /// </summary>
        /// <typeparam name="T">Type of object to be built.</typeparam>
        /// <param name="sourceType">Source type.</param>
        /// <param name="constructedTypeName">Full name of the constructed type.</param>
        /// <param name="assemblyName">Assembly name to be built.</param>
        /// <param name="builtClassType">Type of the class to be built, proxy or service wrapper.</param>
        /// <param name="inMemoryCache">In memory cache of the built type.</param>
        /// <param name="creatingInstanceCallback">Callback for creating instance of specified type.</param>
        /// <param name="sourceCodeBuilderCallback">Callback for creating source code of the constructed type and related.</param>
        /// <returns>Instance of the type constructed or should be constructed.</returns>
        private T ConstructTypeInstance<T>(Type sourceType, string constructedTypeName, string assemblyName,
            BuiltClassType builtClassType, ConcurrentDictionary<Type, Type> inMemoryCache,
            Func<Type, T> creatingInstanceCallback,
            SourceCodeBuilderFromConstructTypeCallback sourceCodeBuilderCallback)
        {
            Type constructedType;

            if (inMemoryCache != null)
            {
                inMemoryCache.TryGetValue(sourceType, out constructedType);
            }
            else
            {
                constructedType = null;
            }

            if (constructedType == null && BeforeTypeBuilding != null)
            {
                var e = new BeforeTypeBuildingEventArgs(sourceType, builtClassType);
                BeforeTypeBuilding(this, e);
                constructedType = e.ConstructedType;
            }

            if (constructedType == null && BeforeAssemblyBuilding != null)
            {
                var e = new BeforeAssemblyBuildingEventArgs(sourceType, builtClassType);
                BeforeAssemblyBuilding(this, e);
                constructedType = e.Assembly?.GetType(constructedTypeName);
            }

            if (constructedType == null)
            {
                sourceCodeBuilderCallback(out var sourceCode, out var assemblyReferences);

                if (CustomizedAssemblyBuildingRequested != null)
                {
                    var e = new CustomizedAssemblyBuildingEventArgs(sourceCode, assemblyReferences);
                    CustomizedAssemblyBuildingRequested(this, e);
                    if (e.BuiltAssembly != null)
                    {
                        constructedType = e.BuiltAssembly.GetType(constructedTypeName);
                    }
                    else
                    {
                        throw new InvalidOperationException(
                            $"Property {nameof(CustomizedAssemblyBuildingEventArgs.BuiltAssembly)} in argument e of event {nameof(CustomizedAssemblyBuildingRequested)} should not be set to null.");
                    }
                }
                else if (BuildAssembly(assemblyName, new string[] {sourceCode},
                    assemblyReferences, out var assemblyImage, out var buildingError))
                {
                    var assembly = Assembly.Load(assemblyImage);
                    constructedType = assembly.GetType(constructedTypeName);

                    if (AfterTypeAndAssemblyBuilt != null)
                    {
                        var e = new AfterTypeAndAssemblyBuiltEventArgs(sourceType, builtClassType, constructedType,
                            assemblyImage);
                        AfterTypeAndAssemblyBuilt(this, e);
                    }
                }
                else
                {
                    throw buildingError;
                }
            }

            if (inMemoryCache != null)
            {
                inMemoryCache[sourceType] = constructedType;
            }

            return creatingInstanceCallback(constructedType);
        }

        Guid GetSiteId() => SiteId;

        /// <summary>
        /// Constructs a proxy object of a type.
        /// </summary>
        /// <param name="sourceType">Source type.</param>
        /// <param name="constructedTypeName">Full name of the constructed type.</param>
        /// <param name="assemblyName">Assembly name to be built.</param>
        /// <param name="sourceCodeBuilderCallback">Callback for creating source code of the constructed type and related.</param>
        /// <returns>Instance of the type constructed or should be constructed.</returns>
        protected object ConstructProxyInstance(Type sourceType, string constructedTypeName, string assemblyName,
            SourceCodeBuilderFromConstructTypeCallback sourceCodeBuilderCallback)
        {
            var proxy = ConstructTypeInstance(sourceType, constructedTypeName, assemblyName, BuiltClassType.Proxy,
                _inMemoryProxyTypeCache, FastActivator.CreateInstance<IProxyCommunicate>, sourceCodeBuilderCallback);
            proxy.GetSiteIdCallback = GetSiteId;
            return proxy;
        }

        /// <summary>
        /// Constructs a service wrapper object of a type.
        /// </summary>
        /// <param name="sourceType">Source type.</param>
        /// <param name="constructedTypeName">Full name of the constructed type.</param>
        /// <param name="assemblyName">Assembly name to be built.</param>
        /// <param name="serviceObject">Target service object to be wrapped.</param>
        /// <param name="sourceCodeBuilderCallback">Callback for creating source code of the constructed type and related.</param>
        /// <returns>Instance of the type constructed or should be constructed.</returns>
        protected object ConstructServiceWrapperInstance(Type sourceType, string constructedTypeName, string assemblyName,
            object serviceObject, SourceCodeBuilderFromConstructTypeCallback sourceCodeBuilderCallback)
        {
            var serviceWrapper = ConstructTypeInstance(sourceType, constructedTypeName, assemblyName, BuiltClassType.ServiceWrapper,
                _inMemoryServiceWrapperTypeCache, t => FastActivator<object>.CreateInstance<IServiceWrapperCommunicate>(t, serviceObject),
                sourceCodeBuilderCallback);
            serviceWrapper.GetSiteIdCallback = GetSiteId;
            return serviceWrapper;
        }
    }
}
