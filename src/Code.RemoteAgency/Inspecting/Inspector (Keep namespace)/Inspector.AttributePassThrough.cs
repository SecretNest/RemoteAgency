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
        List<RemoteAgencyAttributePassThrough> GetAttributePassThrough(ICustomAttributeProvider dataSource, Func<string, Attribute, InvalidAttributeDataException> creatingExceptionCallback)
        {
            List<RemoteAgencyAttributePassThrough> result = new List<RemoteAgencyAttributePassThrough>();

            if (!_includesProxyOnlyInfo)
                return result;

            var attributePassThroughAttributes =
                dataSource.GetCustomAttributes(typeof(AttributePassThroughAttribute), true).Cast<AttributePassThroughAttribute>().ToArray();
            if (attributePassThroughAttributes.Length == 0)
                return result;

            var attributePassThroughIndexBasedParameterAttributes =
                dataSource.GetCustomAttributes(typeof(AttributePassThroughIndexBasedParameterAttribute), true)
                    .ToLookup(i => ((AttributePassThroughIndexBasedParameterAttribute) i).AttributeId,
                        i => (AttributePassThroughIndexBasedParameterAttribute) i);
            var attributePassThroughPropertyAttributes =
                dataSource.GetCustomAttributes(typeof(AttributePassThroughPropertyAttribute), true)
                    .ToLookup(i => ((AttributePassThroughPropertyAttribute) i).AttributeId,
                        i => (AttributePassThroughPropertyAttribute) i);

            HashSet<string> processedAttributeId = new HashSet<string>();
            foreach (var attributePassThroughAttribute in attributePassThroughAttributes)
            {
                //Avoid process with same attribute id.
                if (attributePassThroughAttribute.AttributeId != null)
                {
                    if (!processedAttributeId.Add(attributePassThroughAttribute.AttributeId))
                        continue;
                }

                var mainRecord = new RemoteAgencyAttributePassThrough
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
                            throw creatingExceptionCallback(
                                $"Length of {nameof(attributePassThroughAttribute.AttributeConstructorParameters)} can not exceed the length of {nameof(attributePassThroughAttribute.AttributeConstructorParameterTypes)}.",
                                attributePassThroughAttribute);
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
                                attributePassThroughAttribute.AttributeId];

                        HashSet<int> setIndices = new HashSet<int>();

                        foreach (var attributePassThroughIndexBasedParameterAttribute in
                            linkedAttributePassThroughIndexBasedParameterAttributes)
                        {
                            //Avoid process with same index.
                            if (!setIndices.Add(attributePassThroughIndexBasedParameterAttribute.ParameterIndex))
                                continue;
                            else if (attributePassThroughIndexBasedParameterAttribute.ParameterIndex >=
                                attributePassThroughAttribute.AttributeConstructorParameterTypes.Length)
                            {
                                throw creatingExceptionCallback(
                                    $"{nameof(AttributePassThroughIndexBasedParameterAttribute.ParameterIndex)} cannot be equal or larger than the length of {nameof(AttributePassThroughAttribute.AttributeConstructorParameterTypes)}.",
                                    attributePassThroughIndexBasedParameterAttribute);
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
                        if (!setProperties.Add(attributePassThroughPropertyAttribute.PropertyName))
                            continue;

                        mainRecord.AttributeProperties.Add(new KeyValuePair<string, object>(
                            attributePassThroughPropertyAttribute.PropertyName,
                            attributePassThroughPropertyAttribute.Value));
                    }
                }

                result.Add(mainRecord);
            }

            return result;
        }

        void FillAttributePassThroughOnParameters(List<RemoteAgencyParameterInfo> parameterEntityProperties, Func<string, Attribute, ParameterInfo, InvalidAttributeDataException> creatingExceptionCallback)
        {
            foreach (var parameterInfo in parameterEntityProperties)
            {
                parameterInfo.PassThroughAttributes = GetAttributePassThrough(parameterInfo.Parameter,
                    (m, a) => creatingExceptionCallback(m, a, parameterInfo.Parameter));
            }
        }
    }
}