using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.TypeBuilding
{
    /// <summary>
    /// Represents an argument of the <see cref="RemoteAgency.AfterTypeAndAssemblyBuilt"/>.
    /// </summary>
    public class AfterTypeAndAssemblyBuiltEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the source type.
        /// </summary>
        /// <remarks><p>When building proxy, the value is the type of the interface.</p>
        /// <p>When building service wrapper, the value is the type of the service object.</p></remarks>
        public Type SourceType { get; }

        /// <summary>
        /// Gets the type of the built object.
        /// </summary>
        public Type ConstructedType { get; }

        /// <summary>
        /// Gets the image of the built assembly.
        /// </summary>
        public byte[] AssemblyImage { get; }

        /// <summary>
        /// Initializes an instance of AfterTypeAndAssemblyBuiltEventArgs.
        /// </summary>
        /// <param name="sourceType">Source type.</param>
        /// <param name="constructedType">TYpe of the built object.</param>
        /// <param name="assemblyImage">Built assembly.</param>
        internal AfterTypeAndAssemblyBuiltEventArgs(Type sourceType, Type constructedType, byte[] assemblyImage)
        {
            SourceType = sourceType;
            ConstructedType = constructedType;
            AssemblyImage = assemblyImage;
        }
    }
}
