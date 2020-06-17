using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using SecretNest.RemoteAgency.Helper;
using SecretNest.RemoteAgency.TypeBuilding;

namespace SecretNest.RemoteAgency
{
    public partial class RemoteAgency
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
        /// Constructs a type.
        /// </summary>
        /// <param name="sourceType">Source type.</param>
        /// <param name="constructedTypeName">Full name of the constructed type.</param>
        /// <param name="creatingInstanceCallback">Callback for creating instance of specified type.</param>
        /// <param name="sourceCodeBuilderCallback">Callback for creating source code of the constructed type and related.</param>
        /// <returns></returns>
        protected object ConstructType(Type sourceType, string constructedTypeName, Func<Type, object> creatingInstanceCallback, SourceCodeBuilderFromConstructTypeCallback sourceCodeBuilderCallback) 
        {
            if (BeforeTypeBuilding != null)
            {
                var e = new BeforeTypeBuildingEventArgs(sourceType);
                BeforeTypeBuilding(this, e);
                if (e.ConstructedType != null)
                {
                    return creatingInstanceCallback(e.ConstructedType);
                }
            }

            if (BeforeAssemblyBuilding != null)
            {
                var e = new BeforeAssemblyBuildingEventArgs(sourceType);
                BeforeAssemblyBuilding(this, e);
                Type constructedType = e.Assembly.GetType(constructedTypeName);
                return creatingInstanceCallback(constructedType);
            }

            sourceCodeBuilderCallback(out var sourceCode, out var assemblyReferences);

            if (CustomizedAssemblyBuildingRequested != null)
            {
                var e = new CustomizedAssemblyBuildingEventArgs(sourceCode, assemblyReferences);
                CustomizedAssemblyBuildingRequested(this, e);
                if (e.BuiltAssembly != null)
                {
                    Type constructedType = e.BuiltAssembly.GetType(constructedTypeName);
                    return creatingInstanceCallback(constructedType);
                }

                throw new InvalidOperationException(
                    $"Property {nameof(CustomizedAssemblyBuildingEventArgs.BuiltAssembly)} in argument e of event {nameof(CustomizedAssemblyBuildingRequested)} should not be set to null.");
            }


            if (BuildAssembly(NamingHelper.GetRandomName("Assembly"), new string[] {sourceCode},
                assemblyReferences, out var assemblyImage, out var buildingError))
            {
                var assembly = Assembly.Load(assemblyImage);
                Type constructedType = assembly.GetType(constructedTypeName);

                if (AfterTypeAndAssemblyBuilt != null)
                {
                    var e = new AfterTypeAndAssemblyBuiltEventArgs(sourceType, constructedType, assemblyImage);
                    AfterTypeAndAssemblyBuilt(this, e);
                }

                return creatingInstanceCallback(constructedType);
            }

            throw buildingError;
        }
    }
}
