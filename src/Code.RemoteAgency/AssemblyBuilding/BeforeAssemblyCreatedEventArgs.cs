using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace SecretNest.RemoteAgency.AssemblyBuilding
{
    /// <summary>
    /// Represents an argument of the <see cref="RemoteAgency.BeforeAssemblyCreated"/>.
    /// </summary>
    public class BeforeAssemblyCreatedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the type of source interface.
        /// </summary>
        public Type SourceInterface { get; }

        /// <summary>
        /// Gets the builder for building assembly.
        /// </summary>
        public AssemblyBuilder AssemblyBuilder { get; }

        /// <summary>
        /// Gets the builder for building module.
        /// </summary>
        public ModuleBuilder ModuleBuilder { get; }

        /// <summary>
        /// Gets the type of built proxy. When proxy is not built, the value is <see langword="null"/>.
        /// </summary>
        public Type BuiltProxy { get; }

        /// <summary>
        /// Gets the type of built service wrapper. When service wrapper is not built, the value is <see langword="null"/>.
        /// </summary>
        public Type BuiltServiceWrapper { get; }

        /// <summary>
        /// Gets the types of built entities.
        /// </summary>
        public IReadOnlyList<Type> BuiltEntities { get; }

        /// <summary>
        /// Initialize an instance of BeforeAssemblyCreatedEventArgs.
        /// </summary>
        /// <param name="assemblyBuilder">Builder for building assembly.</param>
        /// <param name="moduleBuilder">Builder for building module.</param>
        /// <param name="sourceInterface">Type of source interface.</param>
        /// <param name="builtProxy">Type of built proxy</param>
        /// <param name="builtServiceWrapper">Type of built service wrapper.</param>
        /// <param name="builtEntities">Types of built entities.</param>
        public BeforeAssemblyCreatedEventArgs(AssemblyBuilder assemblyBuilder, ModuleBuilder moduleBuilder, Type sourceInterface, Type builtProxy, Type builtServiceWrapper, IReadOnlyList<Type> builtEntities)
        {
            AssemblyBuilder = assemblyBuilder;
            BuiltProxy = builtProxy;
            BuiltServiceWrapper = builtServiceWrapper;
            BuiltEntities = builtEntities;
            ModuleBuilder = moduleBuilder;
            SourceInterface = sourceInterface;
        }
    }
}
