using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using SecretNest.RemoteAgency.AssemblyBuilding;
using SecretNest.RemoteAgency.Inspecting;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgencyBase
    {
        /// <summary>
        /// Gets of sets default setting for method calling timeout in milliseconds. Default value is 0 (use default value while initializing). Only valid when building type (not on building instance).
        /// </summary>
        public int DefaultMethodCallingTimeoutForBuilding { get; set; }

        /// <summary>
        /// Gets or sets default setting for event adding timeout in milliseconds. Default value is 0 (use default value while initializing). Only valid when building type (not on building instance).
        /// </summary>
        public int DefaultEventAddingTimeoutForBuilding { get; set; }

        /// <summary>
        /// Gets or sets default setting for event removing timeout in milliseconds. Default value is 0 (use default value while initializing). Only valid when building type (not on building instance).
        /// </summary>
        public int DefaultEventRemovingTimeoutForBuilding { get; set; }

        /// <summary>
        /// Gets or sets default setting for event raising timeout in milliseconds. Default value is 0 (use default value while initializing). Only valid when building type (not on building instance).
        /// </summary>
        public int DefaultEventRaisingTimeoutForBuilding { get; set; }

        /// <summary>
        /// Gets or sets default setting for property getting timeout in milliseconds. Default value is 0 (use default value while initializing). Only valid when building type (not on building instance).
        /// </summary>
        public int DefaultPropertyGettingTimeoutForBuilding { get; set; }

        /// <summary>
        /// Gets or sets default setting for property setting timeout in milliseconds. Default value is 0 (use default value while initializing). Only valid when building type (not on building instance).
        /// </summary>
        public int DefaultPropertySettingTimeoutForBuilding { get; set; }

        void Emit(RemoteAgencyInterfaceBasicInfo basicInfo, 
            bool isProxyRequired, bool isServiceWrapperRequired,
            out Type builtProxy, out Type builtServiceWrapper, out List<Type> builtEntities, out AssemblyBuilder assemblyBuilder, out ModuleBuilder moduleBuilder)
        {
            var assemblyName = new AssemblyName(basicInfo.AssemblyName);

            assemblyBuilder = 
#if netfx
                System.Threading.Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
#else
                AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
#endif

            moduleBuilder =
#if netfx
                assemblyBuilder.DefineDynamicModule("RemoteAgency", basicInfo.ClassNameBase + ".dll");
#else
                assemblyBuilder.DefineDynamicModule("RemoteAgency");
#endif

            var inspector = new Inspector(basicInfo, isProxyRequired, isServiceWrapperRequired,
                EntityTypeBuilder.InterfaceLevelAttributeBaseType, EntityTypeBuilder.AssetLevelAttributeBaseType,
                EntityTypeBuilder.DelegateLevelAttributeBaseType, EntityTypeBuilder.ParameterLevelAttributeBaseType);

            inspector.Process();

            var info = inspector.InterfaceTypeInfo;
            info.DefaultMethodCallingTimeout = DefaultMethodCallingTimeoutForBuilding;
            info.DefaultEventAddingTimeout = DefaultEventAddingTimeoutForBuilding;
            info.DefaultEventRemovingTimeout = DefaultEventRemovingTimeoutForBuilding;
            info.DefaultEventRaisingTimeout = DefaultEventRaisingTimeoutForBuilding;
            info.DefaultPropertyGettingTimeout = DefaultPropertyGettingTimeoutForBuilding;
            info.DefaultPropertySettingTimeout = DefaultPropertySettingTimeoutForBuilding;

            var buildingEntityTasks = CreateEmitEntityTasks(moduleBuilder, info);
            buildingEntityTasks.ForEach(i => i.Start());
            // ReSharper disable once AsyncConverter.AsyncWait
            var entityBuildingTasks = buildingEntityTasks.Select(i => i.Result).ToList();
            builtEntities = new List<Type>(entityBuildingTasks.Count);
            foreach (var entityBuildingTask in entityBuildingTasks)
            {
                var type = entityBuildingTask.Item1.CreateType();
                entityBuildingTask.Item2.SetResultCallback(type);
                builtEntities.Add(type);
            }

            Task emitProxy, emitServiceWrapper;
            TypeBuilder proxyTypeBuilder, serviceWrapperTypeBuilder;

            if (isProxyRequired)
            {
                proxyTypeBuilder = moduleBuilder.DefineType(basicInfo.ProxyTypeName,
                    /*TypeAttributes.Class | */TypeAttributes.Public, EntityBase,
                    new[] {typeof(IProxyCommunicate), basicInfo.SourceInterface});

                EmitAttributePassThroughAttributes(proxyTypeBuilder, info.InterfaceLevelPassThroughAttributes);

                if (basicInfo.IsSourceInterfaceGenericType)
                {
                    EmitGenericParameters(proxyTypeBuilder, basicInfo.SourceInterfaceGenericDefinitionArguments, info.InterfaceLevelGenericParameterPassThroughAttributes);
                }

                emitProxy = Task.Run(() => EmitProxy(proxyTypeBuilder, info));
            }
            else
            {
                emitProxy = null;
                proxyTypeBuilder = null;
            }

            if (isServiceWrapperRequired) 
            {
                serviceWrapperTypeBuilder = moduleBuilder.DefineType(basicInfo.ServiceWrapperTypeName,
                    /*TypeAttributes.Class | */TypeAttributes.Public, typeof(object),
                    new[] {typeof(IServiceWrapperCommunicate), basicInfo.SourceInterface});

                EmitAttributePassThroughAttributes(serviceWrapperTypeBuilder, info.InterfaceLevelPassThroughAttributes);

                if (basicInfo.IsSourceInterfaceGenericType)
                {
                    EmitGenericParameters(serviceWrapperTypeBuilder, basicInfo.SourceInterfaceGenericDefinitionArguments, info.InterfaceLevelGenericParameterPassThroughAttributes);
                }

                emitServiceWrapper = Task.Run(() => EmitServiceWrapper(serviceWrapperTypeBuilder, info));
            }
            else
            {
                emitServiceWrapper = null;
                serviceWrapperTypeBuilder = null;
            }

            if (isProxyRequired) 
            {
                // ReSharper disable once AsyncConverter.AsyncWait
                emitProxy.Wait();

                BeforeTypeCreated?.Invoke(this, new BeforeTypeCreatedEventArgs(proxyTypeBuilder, basicInfo.SourceInterface, BuiltClassType.Proxy));

                builtProxy = proxyTypeBuilder.CreateType();
            }
            else
            {
                builtProxy = null;
            }

            if (isServiceWrapperRequired)
            {
                // ReSharper disable once AsyncConverter.AsyncWait
                emitServiceWrapper.Wait();

                BeforeTypeCreated?.Invoke(this, new BeforeTypeCreatedEventArgs(serviceWrapperTypeBuilder, basicInfo.SourceInterface, BuiltClassType.ServiceWrapper));

                builtServiceWrapper = serviceWrapperTypeBuilder.CreateType();
            }
            else
            {
                builtServiceWrapper = null;
            }
        }
    }
}
