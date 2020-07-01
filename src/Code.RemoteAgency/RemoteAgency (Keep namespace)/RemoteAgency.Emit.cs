﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SecretNest.RemoteAgency.AssemblyBuilding;
using SecretNest.RemoteAgency.Inspecting;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgency
    {
        /// <summary>
        /// Gets of sets default setting for method calling timeout in milliseconds. Default value is 90000. Only valid when building type (not on building instance).
        /// </summary>
        public int DefaultMethodCallingTimeout { get; set; } = 90000;

        /// <summary>
        /// Gets or sets default setting for event adding timeout in milliseconds. Default value is 90000. Only valid when building type (not on building instance).
        /// </summary>
        public int DefaultEventAddingTimeout { get; set; } = 90000;

        /// <summary>
        /// Gets or sets default setting for event removing timeout in milliseconds. Default value is 90000. Only valid when building type (not on building instance).
        /// </summary>
        public int DefaultEventRemovingTimeout { get; set; } = 90000;

        /// <summary>
        /// Gets or sets default setting for event raising timeout in milliseconds. Default value is 90000. Only valid when building type (not on building instance).
        /// </summary>
        public int DefaultEventRaisingTimeout { get; set; } = 90000;

        /// <summary>
        /// Gets or sets default setting for property getting timeout in milliseconds. Default value is 90000. Only valid when building type (not on building instance).
        /// </summary>
        public int DefaultPropertyGettingTimeout { get; set; } = 90000;

        /// <summary>
        /// Gets or sets default setting for property setting timeout in milliseconds. Default value is 90000. Only valid when building type (not on building instance).
        /// </summary>
        public int DefaultPropertySettingTimeout { get; set; } = 90000;

        void Emit(RemoteAgencyInterfaceBasicInfo basicInfo, 
            bool isProxyRequired, bool isServiceWrapperRequired,
            out Type builtProxy, out Type builtServiceWrapper, out List<Type> builtEntities, out AssemblyBuilder assemblyBuilder, out ModuleBuilder moduleBuilder)
        {
            AssemblyName assemblyName = new AssemblyName(basicInfo.AssemblyName);

            assemblyBuilder = 
#if netfx
                Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
#else
                AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
#endif

            moduleBuilder =
#if netfx
                assemblyBuilder.DefineDynamicModule("RemoteAgency", basicInfo.ClassNameBase + ".dll");
#else
                assemblyBuilder.DefineDynamicModule("RemoteAgency");
#endif

            Inspector inspector = new Inspector(basicInfo, isProxyRequired, isServiceWrapperRequired,
                _entityCodeBuilder.InterfaceLevelAttributeBaseType, _entityCodeBuilder.AssetLevelAttributeBaseType,
                _entityCodeBuilder.DelegateLevelAttributeBaseType, _entityCodeBuilder.ParameterLevelAttributeBaseType);

            var info = inspector.InterfaceTypeInfo;
            info.DefaultMethodCallingTimeout = DefaultMethodCallingTimeout;
            info.DefaultEventAddingTimeout = DefaultEventAddingTimeout;
            info.DefaultEventRemovingTimeout = DefaultEventRemovingTimeout;
            info.DefaultEventRaisingTimeout = DefaultEventRaisingTimeout;
            info.DefaultPropertyGettingTimeout = DefaultPropertyGettingTimeout;
            info.DefaultPropertySettingTimeout = DefaultPropertySettingTimeout;

            builtEntities = EmitEntities(moduleBuilder, info);

            Task emitProxy;
            TypeBuilder proxyTypeBuilder;

            if (isProxyRequired) //star task
            {
                proxyTypeBuilder = moduleBuilder.DefineType(basicInfo.ProxyTypeName,
                    /*TypeAttributes.Class | */TypeAttributes.Public, _entityBase,
                    new[] {typeof(IProxyCommunicate), basicInfo.SourceInterface});

                emitProxy = Task.Run(() => EmitProxy(proxyTypeBuilder, info));
            }
            else
            {
                emitProxy = null;
                proxyTypeBuilder = null;
            }

            if (isServiceWrapperRequired) //run
            {
                var typeBuilder = moduleBuilder.DefineType(basicInfo.ServiceWrapperTypeName,
                    /*TypeAttributes.Class | */TypeAttributes.Public, typeof(object),
                    new[] {typeof(IServiceWrapperCommunicate), basicInfo.SourceInterface});

                EmitServiceWrapper(typeBuilder, info);

                BeforeTypeCreated?.Invoke(this, new BeforeTypeCreatedEventArgs(typeBuilder, basicInfo.SourceInterface, BuiltClassType.ServiceWrapper));

                builtServiceWrapper = typeBuilder.CreateType();
            }
            else
            {
                builtServiceWrapper = null;
            }

            if (isProxyRequired) //finish task
            {
                emitProxy.Wait();

                BeforeTypeCreated?.Invoke(this, new BeforeTypeCreatedEventArgs(proxyTypeBuilder, basicInfo.SourceInterface, BuiltClassType.Proxy));

                builtProxy = proxyTypeBuilder.CreateType();
            }
            else
            {
                builtProxy = null;
            }
        }
    }
}
