using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace SecretNest.RemoteAgency
{
    static partial class AssemblyBuildingEmitExtensions
    {
        internal static void EmitGenericParameters(this TypeBuilder typeBuilder, Type[] genericParameters,
            IReadOnlyDictionary<string, List<CustomAttributeBuilder>> genericParameterPassThroughAttributes)
        {
            if (genericParameters.Length > 0)
            {
                var typeParams = typeBuilder.DefineGenericParameters(genericParameters.Select(i => i.Name).ToArray());

                for (var i = 0; i < genericParameters.Length; i++)
                {
                    var genericType = genericParameters[i];

                    var passThroughAttributes = genericParameterPassThroughAttributes[genericType.Name];
                    typeParams[i].EmitAttributePassThroughAttributes(passThroughAttributes);

                    typeParams[i].SetGenericParameterAttributes(genericType.GenericParameterAttributes);

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

        //internal static void EmitGenericParameters(this TypeBuilder typeBuilder, Type[] genericParameters)
        //{
        //    if (genericParameters.Length > 0)
        //    {
        //        var typeParams = typeBuilder.DefineGenericParameters(genericParameters.Select(i => i.Name).ToArray());

        //        for (var i = 0; i < genericParameters.Length; i++)
        //        {
        //            var genericType = genericParameters[i];

        //            typeParams[i]
        //                .SetGenericParameterAttributes(genericType.GenericParameterAttributes);

        //            var typeConstraints = genericType.GetGenericParameterConstraints();
        //            if (typeConstraints.Length > 0)
        //            {
        //                var baseType = typeConstraints.FirstOrDefault(t => t.IsClass);
        //                if (baseType != null)
        //                    typeParams[i].SetBaseTypeConstraint(baseType);

        //                var interfaces = typeConstraints.Where(t => t != baseType).ToArray();
        //                if (interfaces.Length > 0)
        //                    typeParams[i].SetInterfaceConstraints(interfaces);
        //            }
        //        }
        //    }
        //}
    }
}
