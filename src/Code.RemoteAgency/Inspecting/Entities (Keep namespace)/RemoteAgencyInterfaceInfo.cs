using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency.Inspecting
{
    class RemoteAgencyInterfaceInfo : RemoteAgencyInterfaceBasicInfo
    {
        public RemoteAgencyInterfaceInfo(RemoteAgencyInterfaceBasicInfo basicInfo)
        {
            SourceInterface = basicInfo.SourceInterface;
            IsSourceInterfaceGenericType = basicInfo.IsSourceInterfaceGenericType;
            SourceInterfaceGenericArguments = basicInfo.SourceInterfaceGenericArguments;
            AssemblyName = basicInfo.AssemblyName;
            ClassNameBase = basicInfo.ClassNameBase;
            ProxyTypeName = basicInfo.ProxyTypeName;
            ServiceWrapperTypeName = basicInfo.ServiceWrapperTypeName;
            IsProxyStickyTargetSite = basicInfo.IsProxyStickyTargetSite;
            ThreadLockMode = basicInfo.ThreadLockMode;
            TaskSchedulerName = basicInfo.TaskSchedulerName;
        }

        public List<RemoteAgencyMethodInfo> Methods { get; set; }
        public List<RemoteAgencyEventInfo> Events { get; set; }
        public List<RemoteAgencyPropertyInfo> Properties { get; set; }

        public List<RemoteAgencyAttributePassThrough> InterfaceLevelPassThroughAttributes { get; set; }
        public List<RemoteAgencyGenericParameterInfo> InterfaceLevelGenericParameters { get; set; }

        public List<Attribute> SerializerInterfaceLevelAttributes { get; set; }

        public int DefaultMethodCallingTimeout { get; set; } //set before building
        public int DefaultEventAddingTimeout { get; set; } //set before building
        public int DefaultEventRemovingTimeout { get; set; } //set before building
        public int DefaultEventRaisingTimeout { get; set; } //set before building
        public int DefaultPropertyGettingTimeout { get; set; } //set before building
        public int DefaultPropertySettingTimeout { get; set; } //set before building

        public IEnumerable<EntityBuildingExtended> GetEntities()
        {
            foreach(var asset in Methods)
            foreach (var entity in asset.GetEntities(SerializerInterfaceLevelAttributes, InterfaceLevelGenericParameters))
                yield return entity;
            foreach (var asset in Events)
            foreach (var entity in asset.GetEntities(SerializerInterfaceLevelAttributes, InterfaceLevelGenericParameters))
                yield return entity;
            foreach (var asset in Properties)
            foreach (var entity in asset.GetEntities(SerializerInterfaceLevelAttributes, InterfaceLevelGenericParameters))
                yield return entity;
        }
    }
}
