using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using SecretNest.RemoteAgency.Inspecting;

namespace SecretNest.RemoteAgency
{
    partial class RemoteAgency
    {
        List<Task<Type>> CreateEmitEntityTasks(ModuleBuilder moduleBuilder, RemoteAgencyInterfaceInfo info)
        {
            var entitiesInfo = info.GetEntities();

            var buildings = entitiesInfo.Select(entityInfo =>
            {
                var typeBuilder = moduleBuilder.DefineType(entityInfo.EntityClassName,
                    /*TypeAttributes.Class | */TypeAttributes.Public, _entityBase,
                    new[] {typeof(IRemoteAgencyMessage)});

                if (entityInfo.GenericParameters.Length > 0)
                {
                    GenericTypeParameterBuilder[] typeParams = 
                        typeBuilder.DefineGenericParameters(entityInfo.GenericParameters
                            .Select(i => i.Name).ToArray());

                    for (int i = 0; i < entityInfo.GenericParameters.Length; i++)
                    {
                        var genericType = entityInfo.GenericParameters[i];
                        var passThroughAttributes = entityInfo.GenericParameterPassThroughAttributes[genericType.Name];
                        foreach (var customAttribute in passThroughAttributes)
                        {
                            typeParams[i].SetCustomAttribute(customAttribute.GetAttributeBuilder());
                        }

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

                return new Task<Type>(() =>
                {
                    var type = EntityCodeBuilder.BuildEntity(typeBuilder, entityInfo);
                    entityInfo.SetResultCallback(type);
                    return type;
                });
            }).ToList();

            return buildings;
        }
    }
}