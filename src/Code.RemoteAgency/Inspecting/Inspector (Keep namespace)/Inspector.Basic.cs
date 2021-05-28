using System;
using System.Reflection;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency.Inspecting
{
    partial class Inspector
    {
        public static RemoteAgencyInterfaceBasicInfo GetBasicInfo(Type sourceInterface/*, bool includesProxyOnlyInfo*/)
        {
            var basicInfo = new RemoteAgencyInterfaceBasicInfo();

            SetInterfaceTypeBasicInfo(basicInfo, sourceInterface, sourceInterface.GetTypeInfo()/*, includesProxyOnlyInfo*/);

            return basicInfo;
        }

        static void SetInterfaceTypeBasicInfo(RemoteAgencyInterfaceBasicInfo basicInfo, Type sourceInterface, TypeInfo typeInfo/*, bool includesProxyOnlyInfo*/)
        {
            if (sourceInterface.IsGenericType)
            {
                basicInfo.SourceInterface = sourceInterface.GetGenericTypeDefinition();
                basicInfo.IsSourceInterfaceGenericType = true;
                basicInfo.SourceInterfaceGenericArguments = sourceInterface.GetGenericArguments();
                basicInfo.SourceInterfaceGenericDefinitionArguments = basicInfo.SourceInterface.GetGenericArguments();
            }
            else
            {
                basicInfo.SourceInterface = sourceInterface;
            }

            basicInfo.ClassNameBase = GetClassNameBase(sourceInterface);

            var customized = typeInfo.GetCustomAttribute<CustomizedClassNameAttribute>();

            basicInfo.AssemblyName = string.IsNullOrEmpty(customized?.AssemblyName)
                ? GetDefaultAssemblyName(basicInfo.ClassNameBase)
                : customized.AssemblyName;
            basicInfo.ProxyTypeName = string.IsNullOrEmpty(customized?.ProxyClassName)
                ? GetDefaultProxyTypeName(basicInfo.ClassNameBase)
                : customized.ProxyClassName;
            basicInfo.ServiceWrapperTypeName = string.IsNullOrEmpty(customized?.ServiceWrapperClassName)
                ? GetDefaultServiceWrapperTypeName(basicInfo.ClassNameBase)
                : customized.ServiceWrapperClassName;

            basicInfo.ThreadLockMode =
                GetValueFromAttribute<ThreadLockAttribute, ThreadLockMode>(basicInfo.SourceInterface, i => i.ThreadLockMode,
                    out var threadLockAttribute);
            if (basicInfo.ThreadLockMode == ThreadLockMode.TaskSchedulerSpecified)
                basicInfo.TaskSchedulerName = threadLockAttribute.TaskSchedulerName;
        }

        static string GetClassNameBase(Type sourceInterface)
        {
            var typeName = sourceInterface.Name;
            var @namespace = sourceInterface.Namespace;

            if (string.IsNullOrEmpty(@namespace))
            {
                @namespace = "";
            }
            else
            {
                @namespace += ".";
            }
            if (typeName.StartsWith("I") && typeName.Length > 1 && !char.IsUpper(typeName[1]))
            {
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable IDE0057 // Use range operator
                return @namespace + typeName.Substring(1);
#pragma warning restore IDE0057 // Use range operator
#pragma warning restore IDE0079 // Remove unnecessary suppression
            }
            else
            {
                return @namespace + typeName;
            }
        }

        static string GetDefaultAssemblyName(string classNameBase)
        {
            return "SecretNest.RemoteAgency.BuiltAssembly." + classNameBase;
        }

        static string GetDefaultProxyTypeName(string classNameBase)
        {
            return classNameBase + "_Proxy";
        }

        static string GetDefaultServiceWrapperTypeName(string classNameBase)
        {
            return classNameBase + "_ServiceWrapper";
        }

    }
}