# RemoteAgencyBinarySerializerEntityCodeBuilder.BuildEntity Method (TypeBuilder, EntityBuilding)
 

Builds an entity class type.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_BinarySerializer">SecretNest.RemoteAgency.BinarySerializer</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public virtual Type BuildEntity(
	TypeBuilder typeBuilder,
	EntityBuilding entityBuilding
)
```

**VB**<br />
``` VB
Public Overridable Function BuildEntity ( 
	typeBuilder As TypeBuilder,
	entityBuilding As EntityBuilding
) As Type
```

**C++**<br />
``` C++
public:
virtual Type^ BuildEntity(
	TypeBuilder^ typeBuilder, 
	EntityBuilding^ entityBuilding
)
```

**F#**<br />
``` F#
abstract BuildEntity : 
        typeBuilder : TypeBuilder * 
        entityBuilding : EntityBuilding -> Type 
override BuildEntity : 
        typeBuilder : TypeBuilder * 
        entityBuilding : EntityBuilding -> Type 
```


#### Parameters
&nbsp;<dl><dt>typeBuilder</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.reflection.emit.typebuilder" target="_blank">System.Reflection.Emit.TypeBuilder</a><br />Builder of the entity class.</dd><dt>entityBuilding</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_EntityBuilding">SecretNest.RemoteAgency.EntityBuilding</a><br />Info of entity to be built in this method.</dd></dl>

#### Return Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.type" target="_blank">Type</a><br />Type of the built entity class.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_BinarySerializer_RemoteAgencyBinarySerializerEntityCodeBuilder">RemoteAgencyBinarySerializerEntityCodeBuilder Class</a><br /><a href="Overload_SecretNest_RemoteAgency_BinarySerializer_RemoteAgencyBinarySerializerEntityCodeBuilder_BuildEntity">BuildEntity Overload</a><br /><a href="N_SecretNest_RemoteAgency_BinarySerializer">SecretNest.RemoteAgency.BinarySerializer Namespace</a><br />