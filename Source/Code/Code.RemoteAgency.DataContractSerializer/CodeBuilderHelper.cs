using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SecretNest.RemoteAgency
{
    static class CodeBuilderHelper
    {
        internal static void ApplyConstraints(Dictionary<string, Type> types, StringBuilder builder)
        {
            if (types != null)
                foreach (var type in types)
                    ApplyConstraints(type.Key, type.Value, builder);
        }

        static void ApplyConstraints(string name, Type type, StringBuilder builder)
        {
            var typeInfo = type.GetTypeInfo();
            var typeConstraints = typeInfo.GetGenericParameterConstraints();
            var typeAttributes = typeInfo.GenericParameterAttributes;
            var valueType = typeof(ValueType);
            var words = typeConstraints.Where(i => i != valueType).Select(i => i.Name).ToList();
            if (typeAttributes.HasFlag(GenericParameterAttributes.NotNullableValueTypeConstraint))
                words.Add("struct");
            else
            {
                if (typeAttributes.HasFlag(GenericParameterAttributes.ReferenceTypeConstraint))
                    words.Add("class");
                if (typeAttributes.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint))
                    words.Add("new()");
            }
            if (words.Count > 0)
            {
                builder.Append(" where ").Append(name).Append(" : ")
                    .Append(string.Join(", ", words));
            }
        }
    }
}
