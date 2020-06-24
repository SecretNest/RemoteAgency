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
        List<AttributePassThrough> GetAttributePassThrough(MemberInfo memberInfo, Stack<MemberInfo> memberParentPath)
        {
            List<AttributePassThrough> result = new List<AttributePassThrough>();

            var attributePassThroughAttributes =
                memberInfo.GetCustomAttributes<AttributePassThroughAttribute>().ToArray();
            if (attributePassThroughAttributes.Length == 0)
                return result;

            var attributePassThroughIndexBasedParameterAttributes =
                memberInfo.GetCustomAttributes<AttributePassThroughIndexBasedParameterAttribute>()
                    .ToLookup(i => i.AttributeId);
            var attributePassThroughPropertyAttributes =
                memberInfo.GetCustomAttributes<AttributePassThroughPropertyAttribute>()
                    .ToLookup(i => i.AttributeId);

            HashSet<string> processedAttributeId = new HashSet<string>();
            foreach (var attributePassThroughAttribute in attributePassThroughAttributes)
            {
                //Avoid process with same attribute id.
                if (attributePassThroughAttribute.AttributeId != null)
                {
                    if (processedAttributeId.Contains(attributePassThroughAttribute.AttributeId))
                        continue;
                    processedAttributeId.Add(attributePassThroughAttribute.AttributeId);
                }

                var mainRecord = new AttributePassThrough
                {
                    Attribute = attributePassThroughAttribute.Attribute,
                    AttributeConstructorParameterTypes =
                        attributePassThroughAttribute.AttributeConstructorParameterTypes,
                    AttributeConstructorParameters = new List<KeyValuePair<int, object>>(),
                    AttributeProperties = new List<KeyValuePair<string, object>>()
                };

                if (attributePassThroughAttribute.AttributeConstructorParameterTypes.Length != 0)
                {
                    Dictionary<int, object> parameters =
                        new Dictionary<int, object>(attributePassThroughAttribute.AttributeConstructorParameterTypes
                            .Length);

                    if (attributePassThroughAttribute.AttributeConstructorParameters != null)
                    {
                        if (attributePassThroughAttribute.AttributeConstructorParameters.Length >
                            attributePassThroughAttribute.AttributeConstructorParameterTypes.Length)
                        {
                            throw new InvalidAttributeDataException(
                                $"Length of {nameof(attributePassThroughAttribute.AttributeConstructorParameters)} can not exceed the length of {nameof(attributePassThroughAttribute.AttributeConstructorParameterTypes)}.",
                                attributePassThroughAttribute, memberInfo, memberParentPath);
                        }

                        for (int i = 0; i < attributePassThroughAttribute.AttributeConstructorParameters.Length; i++)
                        {
                            parameters[i] = attributePassThroughAttribute.AttributeConstructorParameters[i];
                        }

                    }

                    if (attributePassThroughAttribute.AttributeId != null)
                    {
                        var linkedAttributePassThroughIndexBasedParameterAttributes =
                            attributePassThroughIndexBasedParameterAttributes[
                                attributePassThroughAttribute.AttributeId].Reverse(); //reverse to process from base to derived class.

                        foreach (var attributePassThroughIndexBasedParameterAttribute in
                            linkedAttributePassThroughIndexBasedParameterAttributes)
                        {
                            if (attributePassThroughIndexBasedParameterAttribute.ParameterIndex >=
                                attributePassThroughAttribute.AttributeConstructorParameterTypes.Length)
                            {
                                throw new InvalidAttributeDataException(
                                    $"{nameof(AttributePassThroughIndexBasedParameterAttribute.ParameterIndex)} cannot be equal or larger than the length of {nameof(AttributePassThroughAttribute.AttributeConstructorParameterTypes)}.",
                                    attributePassThroughIndexBasedParameterAttribute, memberInfo, memberParentPath);
                            }
                            else
                            {
                                parameters[attributePassThroughIndexBasedParameterAttribute.ParameterIndex] =
                                    attributePassThroughIndexBasedParameterAttribute.Value;
                            }
                        }
                    }

                    for (int i = 0; i < attributePassThroughAttribute.AttributeConstructorParameterTypes.Length; i++)
                    {
                        if (parameters.TryGetValue(i, out var value))
                            mainRecord.AttributeConstructorParameters.Add(new KeyValuePair<int, object>(i, value));
                    }
                }

                if (attributePassThroughAttribute.AttributeId != null)
                {
                    HashSet<string> setProperties = new HashSet<string>();

                    var linkedAttributePassThroughPropertyAttributes =
                        attributePassThroughPropertyAttributes[attributePassThroughAttribute.AttributeId]
                            .OrderBy(i => i.Order);

                    foreach (var attributePassThroughPropertyAttribute in linkedAttributePassThroughPropertyAttributes)
                    {
                        //Avoid process with same property.
                        if (setProperties.Contains(attributePassThroughPropertyAttribute.PropertyName))
                            continue;
                        setProperties.Add(attributePassThroughPropertyAttribute.PropertyName);

                        mainRecord.AttributeProperties.Add(new KeyValuePair<string, object>(
                            attributePassThroughPropertyAttribute.PropertyName,
                            attributePassThroughPropertyAttribute.Value));
                    }
                }

                result.Add(mainRecord);
            }

            return result;
        }

        InterfaceLevelAttributePassThrough GetInterfaceLevelAttributePassThrough(TypeInfo interfaceTypeInfo, Stack<MemberInfo> memberParentPath)
        {
            InterfaceLevelAttributePassThrough result = new InterfaceLevelAttributePassThrough
            {
                Interface = GetAttributePassThrough(interfaceTypeInfo, memberParentPath),
                GenericTypes = new Dictionary<Type, List<AttributePassThrough>>()
            };
            var generics = interfaceTypeInfo.GetGenericArguments();
            if (generics.Length > 0)
            {
                memberParentPath.Push(interfaceTypeInfo);
                foreach (var generic in generics)
                {
                    result.GenericTypes[generic] = GetAttributePassThrough(generic, memberParentPath);
                }
                memberParentPath.Pop();
            }

            return result;
        }
    }
}