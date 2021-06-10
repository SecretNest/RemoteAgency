using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SecretNest.RemoteAgency.Inspecting
{
    static class GetValueFromAttributeExtensions
    {
        public static TValue GetValueFromAttribute<TAttribute, TValue>(this ICustomAttributeProvider memberInfo,
            Func<TAttribute, TValue> selector, out TAttribute attribute,
            TValue defaultValue = default)
            where TAttribute : Attribute
        {
            attribute = memberInfo.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>().FirstOrDefault();
            return attribute == null ? defaultValue : selector(attribute);
        }

        public static TValue GetValueFromAttribute<TAttribute, TValue>(this ParameterInfo parameterInfo,
            Func<TAttribute, TValue> selector, out TAttribute attribute, Dictionary<string, TAttribute> overrides,
            TValue defaultValue = default)
            where TAttribute : Attribute
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            if (overrides == null || !overrides.TryGetValue(parameterInfo.Name, out attribute))
                attribute = parameterInfo.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>()
                    .FirstOrDefault();
            return attribute == null ? defaultValue : selector(attribute);
        }

        public static List<TAttribute> GetAttributes<TAttribute>(this ParameterInfo parameterInfo,
            Dictionary<string, List<TAttribute>> overrides)
            where TAttribute : Attribute
        {
            if (overrides == null || !overrides.TryGetValue(parameterInfo.Name!, out var attribute))
            {
                return parameterInfo.GetCustomAttributes<TAttribute>().ToList();
            }
            else
            {
                return attribute;
            }
        }

        public static TValue GetValueFromAttribute<TAttribute, TValue>(this EventInfo memberInfo, Type @delegate,
            Func<TAttribute, TValue> selector, out TAttribute attribute,
            TValue defaultValue = default)
            where TAttribute : Attribute
        {
            attribute = memberInfo.GetCustomAttribute<TAttribute>();
            if (attribute == null)
            {
                return @delegate.GetValueFromAttribute(selector, out attribute, defaultValue);
            }
            else
            {
                var value = selector(attribute);
                return value.Equals(defaultValue) ? @delegate.GetValueFromAttribute(selector, out attribute, defaultValue) : value;
            }
        }
    }
}