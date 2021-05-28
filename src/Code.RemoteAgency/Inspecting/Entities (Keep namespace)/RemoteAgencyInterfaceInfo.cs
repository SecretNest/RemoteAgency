﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace SecretNest.RemoteAgency.Inspecting
{
    class RemoteAgencyInterfaceInfo : RemoteAgencyInterfaceBasicInfo
    {
        public RemoteAgencyInterfaceInfo(RemoteAgencyInterfaceBasicInfo basicInfo)
        {
            SourceInterface = basicInfo.SourceInterface;
            IsSourceInterfaceGenericType = basicInfo.IsSourceInterfaceGenericType;
            SourceInterfaceGenericArguments = basicInfo.SourceInterfaceGenericArguments;
            SourceInterfaceGenericDefinitionArguments = basicInfo.SourceInterfaceGenericDefinitionArguments;
            AssemblyName = basicInfo.AssemblyName;
            ClassNameBase = basicInfo.ClassNameBase;
            ProxyTypeName = basicInfo.ProxyTypeName;
            ServiceWrapperTypeName = basicInfo.ServiceWrapperTypeName;
            ThreadLockMode = basicInfo.ThreadLockMode;
            TaskSchedulerName = basicInfo.TaskSchedulerName;
        }

        public List<RemoteAgencyMethodInfo> Methods { get; set; }
        public List<RemoteAgencyEventInfo> Events { get; set; }
        public List<RemoteAgencyPropertyInfo> Properties { get; set; }

        public bool NeedEventHelper => Events.Any(i => !i.IsIgnored);

        public List<CustomAttributeBuilder> InterfaceLevelPassThroughAttributes { get; set; }
        public Type[] InterfaceLevelGenericParameters { get; set; }

        public Dictionary<string, List<CustomAttributeBuilder>>
            InterfaceLevelGenericParameterPassThroughAttributes { get; set; }

        public List<Attribute> SerializerInterfaceLevelAttributes { get; set; }

        public int DefaultMethodCallingTimeout { get; set; } //set before building
        public int DefaultEventAddingTimeout { get; set; } //set before building
        public int DefaultEventRemovingTimeout { get; set; } //set before building
        public int DefaultEventRaisingTimeout { get; set; } //set before building
        public int DefaultPropertyGettingTimeout { get; set; } //set before building
        public int DefaultPropertySettingTimeout { get; set; } //set before building

        public IEnumerable<EntityBuildingExtended> GetEntities()
        {
            foreach (var asset in Methods)
            foreach (var entity in asset.GetEntities(SerializerInterfaceLevelAttributes,
                InterfaceLevelGenericParameters, InterfaceLevelGenericParameterPassThroughAttributes))
                yield return entity;
            foreach (var asset in Events)
            foreach (var entity in asset.GetEntities(SerializerInterfaceLevelAttributes,
                InterfaceLevelGenericParameters, InterfaceLevelGenericParameterPassThroughAttributes))
                yield return entity;
            foreach (var asset in Properties)
            foreach (var entity in asset.GetEntities(SerializerInterfaceLevelAttributes,
                InterfaceLevelGenericParameters, InterfaceLevelGenericParameterPassThroughAttributes))
                yield return entity;
        }
    }
}
