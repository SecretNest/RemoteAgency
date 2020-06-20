using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SecretNest.RemoteAgency.TypeBuilding
{ 
    /// <summary>
    /// Represents an argument of the <see cref="RemoteAgency.BeforeAssemblyBuilding"/>.
    /// </summary>
    /// <remarks>The handler should set the <see cref="Assembly"/> when matching item found.</remarks>
    public class BeforeAssemblyBuildingEventArgs : SourceTypeAndBuiltClassTypeEventArgsBase
    {
        /// <summary>
        /// Gets of sets the constructed assembly.
        /// </summary>
        public Assembly Assembly { get; set; }

        /// <summary>
        /// Initializes an instance of BeforeAssemblyBuildingEventArgs.
        /// </summary>
        /// <param name="sourceType">Source type.</param>
        /// <param name="builtClassType">Type of the class, proxy or service wrapper.</param>
        internal BeforeAssemblyBuildingEventArgs(Type sourceType, BuiltClassType builtClassType) : base(sourceType, builtClassType)
        {
        }
    }
}
