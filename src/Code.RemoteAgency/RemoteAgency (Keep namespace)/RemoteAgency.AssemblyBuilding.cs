using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using SecretNest.RemoteAgency.AssemblyBuilding;
using SecretNest.RemoteAgency.Inspecting;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgency
    {
        private const int DefaultTimeout = 90000;

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
        /// <param name="basicInfo">Basic info of the source interface.</param>
        /// <param name="isProxyRequired">Whether proxy is required to be built.</param>
        /// <param name="isServiceWrapperRequired">Whether service wrapper is required to be built.</param>
        /// <param name="builtProxy">Type of built proxy. When proxy is not built, the value is <see langword="null"/>.</param>
        /// <param name="builtServiceWrapper">Type of built service wrapper. When service wrapper is not built, the value is <see langword="null"/>.</param>
        /// <remarks>Caution: This should not be called if types exist in application domain.</remarks>
        private protected void BuildAssembly(RemoteAgencyInterfaceBasicInfo basicInfo,
            bool isProxyRequired, bool isServiceWrapperRequired,
            out Type builtProxy, out Type builtServiceWrapper)
        {
            bool needBuild = false;
            if (isProxyRequired)
            {
                if (!TryGetType(basicInfo.AssemblyName, basicInfo.ProxyTypeName, out builtProxy))
                {
                    needBuild = true;
                }
            }
            else
            {
                builtProxy = null;
            }

            if (isServiceWrapperRequired)
            {
                if (!TryGetType(basicInfo.AssemblyName, basicInfo.ServiceWrapperTypeName, out builtServiceWrapper))
                {
                    needBuild = true;
                }
            }
            else
            {
                builtServiceWrapper = null;
            }

            if (!needBuild)
                return;

            Emit(basicInfo, isProxyRequired, isServiceWrapperRequired, out builtProxy, out builtServiceWrapper, out var entities, out var assemblyBuilder, out var moduleBuilder);

            BeforeAssemblyCreated?.Invoke(this,
                new BeforeAssemblyCreatedEventArgs(assemblyBuilder, moduleBuilder, basicInfo.SourceInterface,
                    builtProxy, builtServiceWrapper, entities));

            AfterTypeAndAssemblyBuilt?.Invoke(this, new AfterTypeAndAssemblyBuiltEventArgs(basicInfo.SourceInterface, builtProxy, builtServiceWrapper, entities, assemblyBuilder, 
#if netfx
                assemblyBuilder.Save
#else
                null
#endif
                ));
        }

        /// <summary>
        /// Tries to get type specified.
        /// </summary>
        /// <param name="assemblyName">Assembly name.</param>
        /// <param name="typeName">Type name.</param>
        /// <param name="type">Type object specified.</param>
        /// <exception cref="TypeLoadException">Thrown when the type request cannot be found in the assembly specified.</exception>
        /// <returns>Result.</returns>
        private static bool TryGetType(string assemblyName, string typeName, out Type type)
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
