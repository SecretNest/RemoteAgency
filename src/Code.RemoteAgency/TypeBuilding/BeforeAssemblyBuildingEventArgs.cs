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
    public class BeforeAssemblyBuildingEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the source type.
        /// </summary>
        /// <remarks><p>When building proxy, the value is the type of the interface.</p>
        /// <p>When building service wrapper, the value is the type of the service object.</p></remarks>
        public Type SourceType { get; }

        /// <summary>
        /// Gets of sets the constructed assembly.
        /// </summary>
        public Assembly Assembly { get; set; }

        /// <summary>
        /// Initializes an instance of BeforeAssemblyBuildingEventArgs.
        /// </summary>
        /// <param name="sourceType">Source type.</param>
        internal BeforeAssemblyBuildingEventArgs(Type sourceType)
        {
            SourceType = sourceType;
        }
    }
}
