using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SecretNest.RemoteAgency.TypeBuilding
{
    /// <summary>
    /// Represents an argument of the <see cref="RemoteAgency.CustomizedAssemblyBuildingRequested"/>.
    /// </summary>
    /// <remarks>The handler should set the <see cref="BuiltAssembly"/> after built.</remarks>
    public class CustomizedAssemblyBuildingEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the source code for building assembly.
        /// </summary>
        public string SourceCode { get; }

        /// <summary>
        /// Gets all references used by the source code.
        /// </summary>
        public IReadOnlyList<AssemblyReference> AssemblyReferences { get; }

        /// <summary>
        /// Gets or sets the built assembly.
        /// </summary>
        public Assembly BuiltAssembly { get; set; }

        /// <summary>
        /// Initializes an instance of CustomizedAssemblyBuildingEventArgs.
        /// </summary>
        /// <param name="sourceCode">Source code.</param>
        /// <param name="assemblyReferences">References used by the source code.</param>
        internal CustomizedAssemblyBuildingEventArgs(string sourceCode,
            IReadOnlyList<AssemblyReference> assemblyReferences)
        {
            SourceCode = sourceCode;
            AssemblyReferences = assemblyReferences;
        }
    }
}
