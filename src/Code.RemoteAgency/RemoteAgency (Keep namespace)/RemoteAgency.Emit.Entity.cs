﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using SecretNest.RemoteAgency.Inspecting;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgency<TSerialized, TEntityBase>
    {
        List<Type> EmitEntities(ModuleBuilder moduleBuilder, RemoteAgencyInterfaceInfo info)
        {
            var entitiesInfo = info.GetEntities();

            var buildings = entitiesInfo.Select(entityInfo =>
            {
                var typeBuilder = moduleBuilder.DefineType(entityInfo.EntityClassName,
                    /*TypeAttributes.Class | */TypeAttributes.Public, typeof(TEntityBase),
                    new[] {typeof(IRemoteAgencyMessage)});

                if (entityInfo.GenericParameters.Count > 0)
                {
                    GenericTypeParameterBuilder[] typeParams =
                        typeBuilder.DefineGenericParameters(entityInfo.GenericParameters
                            .Select(i => i.GenericParameter.Name).ToArray());

                    for (int i = 0; i < entityInfo.GenericParameters.Count; i++)
                    {
                        var genericTypeInfo = entityInfo.GenericParameters[i];
                        foreach (var customAttribute in genericTypeInfo.PassThroughAttributes)
                        {
                            typeParams[i].SetCustomAttribute(customAttribute.GetAttributeBuilder());
                        }

                        typeParams[i]
                            .SetGenericParameterAttributes(genericTypeInfo.GenericParameter.GenericParameterAttributes);

                        var typeConstraints = genericTypeInfo.GenericParameter.GetGenericParameterConstraints();
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

                return new Task<Type>(() => _entityCodeBuilder.BuildEntity(typeBuilder, entityInfo));
            }).ToList();

            buildings.ForEach(i => i.Start());

            // ReSharper disable once AsyncConverter.AsyncWait
            return buildings.Select(i => i.Result).ToList();
        }
    }
}