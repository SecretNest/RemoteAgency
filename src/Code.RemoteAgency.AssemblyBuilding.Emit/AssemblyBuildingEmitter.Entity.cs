using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading.Tasks;
using SecretNest.RemoteAgency.Inspecting;

namespace SecretNest.RemoteAgency
{
    partial class AssemblyBuildingEmitter
    {
        internal List<Task<Tuple<TypeBuilder, EntityBuildingExtended>>> CreateEmitEntityTasks(ModuleBuilder moduleBuilder, EntityTypeBuilderBase entityTypeBuilder, Type entityBase)
        {
            var entitiesInfo = InterfaceInfo.GetEntities();

            var buildings = entitiesInfo.Select(entityInfo =>
            {
                var typeBuilder = moduleBuilder.DefineType(entityInfo.EntityClassName,
                    /*TypeAttributes.Class | */TypeAttributes.Public, entityBase,
                    new[] {typeof(IRemoteAgencyMessage)});

                typeBuilder.EmitGenericParameters(entityInfo.GenericParameters,
                    entityInfo.GenericParameterPassThroughAttributes);

                return new Task<Tuple<TypeBuilder, EntityBuildingExtended>>(() =>
                {
                    entityTypeBuilder.BuildEntity(typeBuilder, entityInfo);
                    return new Tuple<TypeBuilder, EntityBuildingExtended>(typeBuilder, entityInfo);
                });
            }).ToList();

            return buildings;
        }
    }
}
