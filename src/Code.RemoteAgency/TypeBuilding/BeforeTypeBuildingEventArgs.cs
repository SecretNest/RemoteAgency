using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.TypeBuilding
{
    /// <summary>
    /// Represents an argument of the <see cref="RemoteAgency.BeforeAssemblyBuilding"/>.
    /// </summary>
    /// <remarks>The handler should set the <see cref="ConstructedType"/> when matching item found.</remarks>
    public class BeforeTypeBuildingEventArgs : SourceTypeAndBuiltClassTypeEventArgsBase
    {
        /// <summary>
        /// Gets of sets the type of the constructed object.
        /// </summary>
        public Type ConstructedType { get; set; }

        /// <summary>
        /// Initializes an instance of BeforeTypeBuildingEventArgs.
        /// </summary>
        /// <param name="sourceType">Source type.</param>
        /// <param name="builtClassType">Type of the class to be built, proxy or service wrapper.</param>
        internal BeforeTypeBuildingEventArgs(Type sourceType, BuiltClassType builtClassType) : base(sourceType, builtClassType)
        {
        }
    }
}
