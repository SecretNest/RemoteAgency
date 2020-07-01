using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
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
            out Type builtProxy, out Type builtServiceWrapper, out List<Type> builtEntities, out AssemblyBuilder assemblyBuilder)
        {
            AssemblyName assemblyName = new AssemblyName(basicInfo.AssemblyName);

            assemblyBuilder =
                Thread.GetDomain().DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);

            ModuleBuilder moduleBuilder =
                assemblyBuilder.DefineDynamicModule("RemoteAgency", basicInfo.ClassNameBase + ".dll");

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

            builtProxy = isProxyRequired ? EmitProxy(moduleBuilder, info) : null;

            builtServiceWrapper = isServiceWrapperRequired ? EmitServiceWrapper(moduleBuilder, info) : null;
        }
    }
}
