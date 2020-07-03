# RemoteAgency(*TSerialized*, *TEntityBase*) Constructor 
 

Initializes an instance of Remote Agency.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public RemoteAgency(
	SerializingHelperBase<TSerialized, TEntityBase> serializingHelper,
	EntityCodeBuilderBase entityCodeBuilder,
	Guid siteId
)
```

**VB**<br />
``` VB
Public Sub New ( 
	serializingHelper As SerializingHelperBase(Of TSerialized, TEntityBase),
	entityCodeBuilder As EntityCodeBuilderBase,
	siteId As Guid
)
```

**C++**<br />
``` C++
public:
RemoteAgency(
	SerializingHelperBase<TSerialized, TEntityBase>^ serializingHelper, 
	EntityCodeBuilderBase^ entityCodeBuilder, 
	Guid siteId
)
```

**F#**<br />
``` F#
new : 
        serializingHelper : SerializingHelperBase<'TSerialized, 'TEntityBase> * 
        entityCodeBuilder : EntityCodeBuilderBase * 
        siteId : Guid -> RemoteAgency
```


#### Parameters
&nbsp;<dl><dt>serializingHelper</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_SerializingHelperBase_2">SecretNest.RemoteAgency.SerializingHelperBase</a>(<a href="T_SecretNest_RemoteAgency_RemoteAgency_2">*TSerialized*</a>, <a href="T_SecretNest_RemoteAgency_RemoteAgency_2">*TEntityBase*</a>)<br />Serializer helper.</dd><dt>entityCodeBuilder</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_EntityCodeBuilderBase">SecretNest.RemoteAgency.EntityCodeBuilderBase</a><br />Entity code builder.</dd><dt>siteId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">System.Guid</a><br />Site id. A randomized value is used when it is set to <a href="https://docs.microsoft.com/dotnet/api/system.guid.empty" target="_blank">Empty</a>.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency_2">RemoteAgency(TSerialized, TEntityBase) Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />