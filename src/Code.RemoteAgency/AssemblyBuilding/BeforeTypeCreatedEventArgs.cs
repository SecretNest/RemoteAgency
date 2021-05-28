using System;
using System.Reflection.Emit;

namespace SecretNest.RemoteAgency.AssemblyBuilding
{
    /// <summary>
    /// Represents an argument of the <see cref="RemoteAgencyBase.BeforeTypeCreated"/>.
    /// </summary>
    public class BeforeTypeCreatedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the type of source interface.
        /// </summary>
        public Type SourceInterface { get; }

        /// <summary>
        /// Gets the builder for building type.
        /// </summary>
        public TypeBuilder TypeBuilder { get; }

        /// <summary>
        /// Gets the type of the class to be built, proxy or service wrapper.
        /// </summary>
        public BuiltClassType BuiltClassType { get; }

        /// <summary>
        /// Initialize an instance of BeforeTypeCreatedEventArgs.
        /// </summary>
        /// <param name="typeBuilder">Builder for building type.</param>
        /// <param name="sourceInterface">Type of source interface.</param>
        /// <param name="builtClassType">Type of the class to be built, proxy or service wrapper.</param>
        public BeforeTypeCreatedEventArgs(TypeBuilder typeBuilder, Type sourceInterface, BuiltClassType builtClassType)
        {
            TypeBuilder = typeBuilder;
            SourceInterface = sourceInterface;
            BuiltClassType = builtClassType;
        }
    }
}
