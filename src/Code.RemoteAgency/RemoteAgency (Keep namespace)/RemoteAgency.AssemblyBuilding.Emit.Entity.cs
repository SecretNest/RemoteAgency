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

                EmitGenericParameters(typeBuilder, entityInfo.GenericParameters,
                    entityInfo.GenericParameterPassThroughAttributes);

                return new Task<Type>(() =>
                {
                    var type = EntityTypeBuilder.BuildEntity(typeBuilder, entityInfo);
                    entityInfo.SetResultCallback(type);
                    return type;
                });
            }).ToList();

            return buildings;
        }
    }
}