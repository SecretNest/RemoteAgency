using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace SecretNest.RemoteAgency.AssemblyBuilding
{
    /// <summary>
    /// Represents an argument of the <see cref="RemoteAgency.AfterTypeAndAssemblyBuilt"/>.
    /// </summary>
    public class AfterTypeAndAssemblyBuiltEventArgs : EventArgs, IDisposable
    {
        /// <summary>
        /// Gets the type of source interface.
        /// </summary>
        public Type SourceInterface { get; }

        /// <summary>
        /// Gets the type of built proxy. When proxy is not built, the value is <see langword="null"/>.
        /// </summary>
        public Type BuiltProxy { get; }

        /// <summary>
        /// Gets the type of built service wrapper. When service wrapper is not built, the value is <see langword="null"/>.
        /// </summary>
        public Type BuiltServiceWrapper { get; }

        /// <summary>
        /// Gets the types of built entities.
        /// </summary>
        public IReadOnlyList<Type> BuiltEntities { get; }

        /// <summary>
        /// Gets the built assembly.
        /// </summary>
        public Assembly Assembly { get; }

        /// <summary>
        /// Saves the built assembly to the file specified.
        /// </summary>
        /// <param name="assemblyFileName">File name to be written to.</param>
        /// <remarks>Assembly saving is not supported by .net core. This method is only for .net framework.</remarks>
        /// <exception cref="NotSupportedException">Thrown when called in .net core app.</exception>
        public void Save(string assemblyFileName)
        {
#if netfx
            _saveFileCallback(assemblyFileName);
#else
            throw new NotSupportedException("Assembly saving is not supported by .net core.");
#endif
        }

        private Action<string> _saveFileCallback;

        /// <summary>
        /// Initializes an instance of AfterTypeAndAssemblyBuiltEventArgs.
        /// </summary>
        /// <param name="sourceInterface">Type of source interface.</param>
        /// <param name="builtProxy">Type of built proxy</param>
        /// <param name="builtServiceWrapper">Type of built service wrapper.</param>
        /// <param name="builtEntities">Types of built entities.</param>
        /// <param name="assembly">Built assembly.</param>
        /// <param name="saveFileCallback">Callback for saving assembly to file.</param>
        internal AfterTypeAndAssemblyBuiltEventArgs(Type sourceInterface, Type builtProxy, Type builtServiceWrapper, IReadOnlyList<Type> builtEntities, Assembly assembly, Action<string> saveFileCallback)
        {
            SourceInterface = sourceInterface;
            BuiltProxy = builtProxy;
            BuiltServiceWrapper = builtServiceWrapper;
            BuiltEntities = builtEntities;
            Assembly = assembly;
            _saveFileCallback = saveFileCallback;
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by this instance.
        /// </summary>
        /// <param name="disposing">True: release both managed and unmanaged resources; False: release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _saveFileCallback = null;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
