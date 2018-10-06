using SecretNest.CSharpRoslynAgency;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Represents an argument of the MissingAssemblyRequesting.
    /// </summary>
    public class AssemblyRequestingEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the path or name of the assembly / module which reference this missing assembly.
        /// </summary>
        public string Display { get; }

        /// <summary>
        /// Gets whether the assembly / module which reference this missing assembly is a module.
        /// </summary>
        public bool IsModule { get; }

        /// <summary>
        /// Gets whether the assembly / module which reference this missing assembly is EmbedInteropTypes.
        /// </summary>
        public bool EmbedInteropTypes { get; }

        /// <summary>
        /// Gets the aliases of the assembly / module which reference this missing assembly.
        /// </summary>
        public IEnumerable<string> Aliases { get; }

        /// <summary>
        /// Gets whether the assembly requested is loaded by <see cref="Load(byte[])"/>.
        /// </summary>
        public bool IsLoaded { get; private set; }

        /// <summary>
        /// Gets the image of the missing assembly / module, set by <see cref="Load(byte[])"/>.
        /// </summary>
        public byte[] MissingAssemblyImage { get; private set; }

        /// <summary>
        /// Gets the name of the assembly / module which is missing.
        /// </summary>
        public AssemblyName AssemblyName { get; }

        /// <summary>
        /// Load image of missing assembly / module into <see cref="MissingAssemblyImage"/> and set <see cref="IsLoaded"/> to true.
        /// </summary>
        /// <param name="image">Image of the assembly / module</param>
        public void Load(byte[] image)
        {
            if (image != null)
            {
                MissingAssemblyImage = image;
                IsLoaded = true;
            }
        }

        internal AssemblyRequestingEventArgs(MissingAssemblyResolvingEventArgs missingAssemblyResolvingEventArgs)
        {
            Display = missingAssemblyResolvingEventArgs.Display;
            AssemblyName = missingAssemblyResolvingEventArgs.AssemblyName;
            IsModule = missingAssemblyResolvingEventArgs.IsModule;
            EmbedInteropTypes = missingAssemblyResolvingEventArgs.EmbedInteropTypes;
            Aliases = missingAssemblyResolvingEventArgs.Aliases;
        }

        internal AssemblyRequestingEventArgs(AssemblyName assemblyName, bool isModule, bool embedInteropTypes, IEnumerable<string> aliases)
        {
            Display = assemblyName.FullName;
            AssemblyName = assemblyName;
            IsModule = isModule;
            EmbedInteropTypes = embedInteropTypes;
            Aliases = aliases;
        }
    }
}
