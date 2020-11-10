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
    partial class RemoteAgencyBase
    {
        List<Task<Tuple<TypeBuilder, EntityBuildingExtended>>> CreateEmitEntityTasks(ModuleBuilder moduleBuilder, RemoteAgencyInterfaceInfo info)
        {
            var entitiesInfo = info.GetEntities();

            var buildings = entitiesInfo.Select(entityInfo =>
            {
                var typeBuilder = moduleBuilder.DefineType(entityInfo.EntityClassName,
                    /*TypeAttributes.Class | */TypeAttributes.Public, _entityBase,
                    new[] {typeof(IRemoteAgencyMessage)});

                EmitGenericParameters(typeBuilder, entityInfo.GenericParameters,
                    entityInfo.GenericParameterPassThroughAttributes);

                return new Task<Tuple<TypeBuilder, EntityBuildingExtended>>(() =>
                {
                    EntityTypeBuilder.BuildEntity(typeBuilder, entityInfo);
                    return new Tuple<TypeBuilder, EntityBuildingExtended>(typeBuilder, entityInfo);
                });
            }).ToList();

            return buildings;
        }
    }
}