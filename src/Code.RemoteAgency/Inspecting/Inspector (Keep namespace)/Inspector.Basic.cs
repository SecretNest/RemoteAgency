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
        public static InterfaceTypeBasicInfo GetBasicInfo(Type sourceInterface)
        {
            var interfaceTypeBasicInfo = new InterfaceTypeBasicInfo();

            SetInterfaceTypeBasicInfo(interfaceTypeBasicInfo, sourceInterface, sourceInterface.GetTypeInfo());

            return interfaceTypeBasicInfo;
        }

        static void SetInterfaceTypeBasicInfo(InterfaceTypeBasicInfo interfaceTypeBasicInfo, Type sourceInterface, TypeInfo typeInfo)
        {
            interfaceTypeBasicInfo.SourceInterface = sourceInterface;

            Lazy<string> classNameBase = new Lazy<string>(() => GetClassNameBase(sourceInterface));

            var customized = typeInfo.GetCustomAttribute<CustomizedClassNameAttribute>();

            interfaceTypeBasicInfo.AssemblyName = string.IsNullOrEmpty(customized?.AssemblyName)
                ? GetDefaultAssemblyName(classNameBase.Value)
                : customized.AssemblyName;
            interfaceTypeBasicInfo.ProxyTypeName = string.IsNullOrEmpty(customized?.ProxyClassName)
                ? GetDefaultProxyTypeName(classNameBase.Value)
                : customized.ProxyClassName;
            interfaceTypeBasicInfo.ServiceWrapperTypeName = string.IsNullOrEmpty(customized?.ServiceWrapperClassName)
                ? GetDefaultServiceWrapperTypeName(classNameBase.Value)
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
    }
}