using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.TypeBuilding
{
    /// <summary>
    /// Represents an argument of the <see cref="RemoteAgency.AfterTypeAndAssemblyBuilt"/>.
    /// </summary>
    public class AfterTypeAndAssemblyBuiltEventArgs : SourceTypeAndBuiltClassTypeEventArgsBase
    {
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
        /// <param name="builtClassType">Type of the built class, proxy or service wrapper.</param>
        /// <param name="constructedType">TYpe of the built object.</param>
        /// <param name="assemblyImage">Built assembly.</param>
        internal AfterTypeAndAssemblyBuiltEventArgs(Type sourceType, BuiltClassType builtClassType, Type constructedType, byte[] assemblyImage) : base(sourceType, builtClassType)
        {
            ConstructedType = constructedType;
            AssemblyImage = assemblyImage;
        }
    }
}
