# RemoteAgency Constructor 
 

Initializes the instance of Remote Agency.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
protected RemoteAgency(
	EntityCodeBuilderBase entityCodeBuilder,
	Guid siteId,
	Type entityBase
)
```

**VB**<br />
``` VB
Protected Sub New ( 
	entityCodeBuilder As EntityCodeBuilderBase,
	siteId As Guid,
	entityBase As Type
)
```

**C++**<br />
``` C++
protected:
RemoteAgency(
	EntityCodeBuilderBase^ entityCodeBuilder, 
	Guid siteId, 
	Type^ entityBase
)
```

**F#**<br />
``` F#
new : 
        entityCodeBuilder : EntityCodeBuilderBase * 
        siteId : Guid * 
        entityBase : Type -> RemoteAgency
```


#### Parameters
&nbsp;<dl><dt>entityCodeBuilder</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_EntityCodeBuilderBase">SecretNest.RemoteAgency.EntityCodeBuilderBase</a><br />Entity code builder.</dd><dt>siteId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">System.Guid</a><br />Site id. A randomized value is used when it is set to <a href="https://docs.microsoft.com/dotnet/api/system.guid.empty" target="_blank">Empty</a>.</dd><dt>entityBase</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.type" target="_blank">System.Type</a><br />Type of the entity base.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />