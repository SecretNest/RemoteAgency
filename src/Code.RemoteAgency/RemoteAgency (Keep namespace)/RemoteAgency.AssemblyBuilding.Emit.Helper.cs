using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using SecretNest.RemoteAgency.Inspecting;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgency
    {
        void EmitGenericParameters(TypeBuilder typeBuilder, Type[] genericParameters,
            Dictionary<string, List<RemoteAgencyAttributePassThrough>> genericParameterPassThroughAttributes)
        {
            if (genericParameters.Length > 0)
            {
                GenericTypeParameterBuilder[] typeParams =
                    typeBuilder.DefineGenericParameters(genericParameters.Select(i => i.Name).ToArray());

                for (int i = 0; i < genericParameters.Length; i++)
                {
                    var genericType = genericParameters[i];

                    var passThroughAttributes = genericParameterPassThroughAttributes[genericType.Name];
                    EmitAttributePassThroughAttributes(typeParams[i], passThroughAttributes);

                    typeParams[i]
                        .SetGenericParameterAttributes(genericType.GenericParameterAttributes);

                    var typeConstraints = genericType.GetGenericParameterConstraints();
                    if (typeConstraints.Length > 0)
                    {
                        var baseType = typeConstraints.FirstOrDefault(t => t.IsClass);
                        if (baseType != null)
                            typeParams[i].SetBaseTypeConstraint(baseType);

                        var interfaces = typeConstraints.Where(t => t != baseType).ToArray();
                        if (interfaces.Length > 0)
                            typeParams[i].SetInterfaceConstraints(interfaces);
                    }
                }
            }
        }

        void EmitAttributePassThroughAttributes(GenericTypeParameterBuilder typeBuilder,
            List<RemoteAgencyAttributePassThrough> passThroughAttributes)
        {
            foreach (var customAttribute in passThroughAttributes)
            {
                typeBuilder.SetCustomAttribute(customAttribute.GetAttributeBuilder());
            }
        }

        void EmitAttributePassThroughAttributes(TypeBuilder typeBuilder,
            List<RemoteAgencyAttributePassThrough> passThroughAttributes)
        {
            foreach (var customAttribute in passThroughAttributes)
            {
                typeBuilder.SetCustomAttribute(customAttribute.GetAttributeBuilder());
            }
        }

        void EmitParameterPassThroughAttributes(ParameterBuilder typeBuilder,
            List<RemoteAgencyAttributePassThrough> passThroughAttributes)
        {
            foreach (var customAttribute in passThroughAttributes)
            {
                typeBuilder.SetCustomAttribute(customAttribute.GetAttributeBuilder());
            }
        }
    }
}
