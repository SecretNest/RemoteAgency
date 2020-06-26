using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency.Inspecting
{
    partial class Inspector
    {
        public static RemoteAgencyInterfaceBasicInfo GetBasicInfo(Type sourceInterface)
        {
            var basicInfo = new RemoteAgencyInterfaceBasicInfo();

            SetInterfaceTypeBasicInfo(basicInfo, sourceInterface, sourceInterface.GetTypeInfo());

            return basicInfo;
        }

        static void SetInterfaceTypeBasicInfo(RemoteAgencyInterfaceBasicInfo basicInfo, Type sourceInterface, TypeInfo typeInfo)
        {
            if (sourceInterface.IsGenericType)
            {
                basicInfo.SourceInterface = sourceInterface.GetGenericTypeDefinition();
                basicInfo.IsSourceInterfaceGenericType = true;
                basicInfo.SourceInterfaceGenericArguments = sourceInterface.GetGenericArguments();
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
                return @namespace + typeName.Substring(1);
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

        static string GetDefaultEntityTypeName(string classNameBase, string assetName, string usage)
        {
            return $"{classNameBase}_{assetName}_{usage}";
        }
    }
}