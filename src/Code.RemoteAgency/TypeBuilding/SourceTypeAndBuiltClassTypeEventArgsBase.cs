using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.TypeBuilding
{
    /// <summary>
    /// Represents an event argument contains <see cref="SourceType"/> and <see cref="BuiltClassType"/>. This is an abstract class.
    /// </summary>
    public abstract class SourceTypeAndBuiltClassTypeEventArgsBase : EventArgs
    {
        /// <summary>
        /// Gets the source type.
        /// </summary>
        /// <remarks><p>When building proxy, the value is the type of the interface.</p>
        /// <p>When building service wrapper, the value is the type of the service object.</p></remarks>
        public Type SourceType { get; }

        /// <summary>
        /// Get the type of the class, proxy or service wrapper.
        /// </summary>
        public BuiltClassType BuiltClassType { get; }

        /// <summary>
        /// Initializes an instance of SourceTypeAndBuiltClassTypeEventArgsBase.
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="builtClassType"></param>
        protected SourceTypeAndBuiltClassTypeEventArgsBase(Type sourceType, BuiltClassType builtClassType)
        {
            SourceType = sourceType;
            BuiltClassType = builtClassType;
        }
    }
}
