using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.SqlServer.Server;
using SecretNest.RemoteAgency.Helper;
using SecretNest.RemoteAgency.AssemblyBuilding;
using SecretNest.RemoteAgency.Inspecting;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgency
    {
        /// <summary>
        /// Occurs before type building finished.
        /// </summary>
        /// <remarks>Additional code can be added to the type through <see cref="BeforeTypeCreatedEventArgs.TypeBuilder"/>.</remarks>
        public event EventHandler<BeforeTypeCreatedEventArgs> BeforeTypeCreated;

        /// <summary>
        /// Occurs before module and assembly building finished.
        /// </summary>
        /// <remarks>Additional code can be added to the type through <see cref="BeforeAssemblyCreatedEventArgs.ModuleBuilder"/> and <see cref="BeforeAssemblyCreatedEventArgs.AssemblyBuilder"/>.</remarks>
        public event EventHandler<BeforeAssemblyCreatedEventArgs> BeforeAssemblyCreated;

        /// <summary>
        /// Occurs when an assembly is built.
        /// </summary>
        /// <remarks>The handler of this event can contains the code for saving assembly for further use, aka caching.</remarks>
        public event EventHandler<AfterTypeAndAssemblyBuiltEventArgs> AfterTypeAndAssemblyBuilt;

        /// <summary>
        /// Builds an assembly contains built types.
        /// </summary>
        /// <param name="sourceInterface">Type of the source interface.</param>
        /// <param name="isProxyRequired">Whether proxy is required to be built.</param>
        /// <param name="isServiceWrapperRequired">Whether service wrapper is required to be built.</param>
        /// <param name="builtProxy">Type of built proxy. When proxy is not built, the value is <see langword="null"/>.</param>
        /// <param name="builtServiceWrapper">Type of built service wrapper. When service wrapper is not built, the value is <see langword="null"/>.</param>
        /// <remarks>Caution: This should not be called if types exist in application domain.</remarks>
        protected void BuildAssembly(Type sourceInterface,
            bool isProxyRequired, bool isServiceWrapperRequired,
            out Type builtProxy, out Type builtServiceWrapper)
        {
            //TODO: write code here.

            throw new NotImplementedException();
        }

        /// <summary>
        /// Tries to get type specified.
        /// </summary>
        /// <param name="assemblyName">Assembly name.</param>
        /// <param name="typeName">Type name.</param>
        /// <param name="type">Type object specified.</param>
        /// <exception cref="TypeLoadException">Thrown when the type request cannot be found in the assembly specified.</exception>
        /// <returns>Result.</returns>
        protected bool TryGetType(string assemblyName, string typeName, out Type type)
        {
            Assembly assembly;
            try
            {
                assembly = Assembly.Load(assemblyName);
            }
            catch
            {
                type = default;
                return false;
            }

            type = assembly.GetType(typeName);
            return type != null;
        }
    }
}
