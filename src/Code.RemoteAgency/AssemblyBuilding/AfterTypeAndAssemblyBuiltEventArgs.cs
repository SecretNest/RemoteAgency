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
    public class AfterTypeAndAssemblyBuiltEventArgs : EventArgs
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

        private readonly Lazy<byte[]> _image;

        /// <summary>
        /// Gets the image of the built assembly.
        /// </summary>
        public byte[] AssemblyImage => _image.Value;

        /// <summary>
        /// Initializes an instance of AfterTypeAndAssemblyBuiltEventArgs.
        /// </summary>
        /// <param name="sourceInterface">Type of source interface.</param>
        /// <param name="builtProxy">Type of built proxy</param>
        /// <param name="builtServiceWrapper">Type of built service wrapper.</param>
        /// <param name="builtEntities">Types of built entities.</param>
        /// <param name="assembly">Built assembly.</param>
        internal AfterTypeAndAssemblyBuiltEventArgs(Type sourceInterface, Type builtProxy, Type builtServiceWrapper, IReadOnlyList<Type> builtEntities, Assembly assembly)
        {
            SourceInterface = sourceInterface;
            BuiltProxy = builtProxy;
            BuiltServiceWrapper = builtServiceWrapper;
            BuiltEntities = builtEntities;
            Assembly = assembly;

            _image = new Lazy<byte[]>(() =>
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, Assembly);
                    return stream.ToArray();
                }
            });
        }
    }
}
