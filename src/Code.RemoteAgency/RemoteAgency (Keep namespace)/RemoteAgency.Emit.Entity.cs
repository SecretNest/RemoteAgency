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
    partial class RemoteAgency<TSerialized, TEntityBase>
    {
        List<Type> EmitEntities(ModuleBuilder moduleBuilder, RemoteAgencyInterfaceInfo info)
        {
            var entitiesInfo = info.GetEntities();

            var buildings = entitiesInfo.Select(entityInfo => 
            {
                var typeBuilder = moduleBuilder.DefineType(entityInfo.EntityClassName,
                    TypeAttributes.Class | TypeAttributes.Public, typeof(TEntityBase),
                    new[] {typeof(IRemoteAgencyMessage)});
                return new Task<Type>(() => _entityCodeBuilder.BuildEntity(typeBuilder, entityInfo));
            }).ToList();

            buildings.ForEach(i=>i.Start());

            return buildings.Select(i => i.Result).ToList();
        }
    }
}