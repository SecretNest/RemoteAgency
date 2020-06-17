using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SecretNest.RemoteAgency.Attributes;

namespace SecretNest.RemoteAgency.Helper
{
    static class NamingHelper
    {
        internal static string MakeFirstUpper(string original)
        {
            return original.First().ToString().ToUpper() + original.Substring(1);
        }

        internal static string GetRandomName(string prefix)
        {
            return $"{prefix}_{Guid.NewGuid():N}";
        }

        internal static string GetAssetName(MemberInfo memberInfo)
        {
            return memberInfo.GetCustomAttribute<CustomizedAssetNameAttribute>()?.AssetName ?? memberInfo.Name;
        }

        internal static string GetFullName(this Type type, GetTypeFullNameParameter parameter, Dictionary<string, Type> foundGenerics)
        {
            if (type.IsByRef)
            {
                type = type.GetElementType();
            }

            if (type.IsArray)
            {
                var elementType = type.GetElementType();
                return elementType.GetFullName(parameter, foundGenerics) + "[]";
            }

            if (foundGenerics != null && type.IsGenericParameter)
            {
                if (!foundGenerics.ContainsKey(type.Name))
                    foundGenerics.Add(type.Name, type);
                return type.Name;
            }

            if (type.IsConstructedGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();
                var baseTypeName = genericType.GetFullName(parameter, foundGenerics).Split('`')[0];
                var argNames = type.GenericTypeArguments.Select(i => i.GetFullName(parameter, foundGenerics)).ToArray();
                return baseTypeName + "<" + string.Join(", ", argNames) + ">";
            }

            if (type.IsNested)
            {
                var outerType = type.DeclaringType;
                var typeName = outerType.GetFullName(parameter, foundGenerics);
                return typeName + "." + type.Name;
            }

            if (parameter.UsedTypes.TryGetValue(type, out var result))
                return result;

            var assemblyName = type.GetTypeInfo().Assembly.GetName();
            var assemblyNameText = assemblyName.FullName;
            if (!parameter.UsedAssemblies.TryGetValue(assemblyNameText, out var assemblyPoint))
            {
                string alias = "alias_" + Guid.NewGuid().ToString("N");
                assemblyPoint = new Tuple<AssemblyName, string>(assemblyName, alias);
                parameter.UsedAssemblies.Add(assemblyNameText, assemblyPoint);
                parameter.TotalSourceBuilder.Append("extern alias ").Append(alias).AppendLine(";");
            }
            if (assemblyPoint.Item2 == null)
                result = type.FullName;
            else
                result = assemblyPoint.Item2 + "::" + type.FullName;
            // ReSharper disable once PossibleNullReferenceException
            if (result.EndsWith("&"))
                result = result.Substring(0, result.Length - 1);
            parameter.UsedTypes.Add(type, result);
            return result;
        }
    }
    internal class GetTypeFullNameParameter
    {
        public readonly Dictionary<Type, string> UsedTypes;
        public readonly Dictionary<string, Tuple<AssemblyName, string>> UsedAssemblies;
        public readonly StringBuilder TotalSourceBuilder;
        public GetTypeFullNameParameter(Dictionary<Type, string> usedTypes, Dictionary<string, Tuple<AssemblyName, string>> usedAssemblies, StringBuilder totalSourceBuilder)
        {
            UsedTypes = usedTypes;
            UsedAssemblies = usedAssemblies;
            TotalSourceBuilder = totalSourceBuilder;
        }
    }
}