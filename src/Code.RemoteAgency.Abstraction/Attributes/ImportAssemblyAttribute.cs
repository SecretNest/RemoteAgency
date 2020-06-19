using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency.Attributes
{
    /// <summary>
    /// Specifies more assemblies to be loaded with new created assembly.
    /// </summary>
    /// <remarks>By default, all detected used assemblies will be loaded with new created assembly automatically. You may want to use this attribute when some more assemblies should be loaded also.</remarks>
    [AttributeUsage(AttributeTargets.Interface, Inherited = true, AllowMultiple = true)]
    public class ImportAssemblyAttribute : Attribute
    {
        /// <summary>
        /// Gets the full name of the assembly.
        /// </summary>
        public string AssemblyFullName { get; }

        /// <summary>
        /// Gets whether the image of the assembly will be loaded to compiler by event handler in user code before compiling.
        /// </summary>
        // ReSharper disable once IdentifierTypo
        public bool Preloading { get; }

        /// <summary>
        /// Gets the assembly aliases.
        /// </summary>
        public string[] Aliases { get; }

        /// <summary>
        /// Gets whether this is a module.
        /// </summary>
        public bool IsModule { get; }

        /// <summary>
        /// Gets whether this is a embed interop types from the referenced assembly to the referencing compilation.
        /// </summary>
        public bool EmbedInteropTypes { get; }

        /// <summary>
        /// Initializes an instance of the ImportAssemblyAttribute.
        /// </summary>
        /// <param name="assemblyFullName">The full name of the assembly.</param>
        /// <param name="preloading">Whether the image of the assembly will be loaded to compiler by event handler in user code before compiling.</param>
        /// <param name="isModule">Whether this is a module.</param>
        /// <param name="aliases">Assembly aliases.</param>
        /// <param name="embedInteropTypes">True to embed interop types from the referenced assembly to the referencing compilation.</param>
        // ReSharper disable once IdentifierTypo
        public ImportAssemblyAttribute(string assemblyFullName, bool preloading = false, bool isModule = false, string[] aliases = null, bool embedInteropTypes = false)
        {
            AssemblyFullName = assemblyFullName;
            Preloading = preloading;
            IsModule = isModule;
            Aliases = aliases;
            EmbedInteropTypes = embedInteropTypes;
        }
    }
}
