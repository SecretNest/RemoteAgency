# RemoteAgency.Create(*TSerialized*, *TEntityBase*) Method 
 

Creates an instance of Remote Agency.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public static RemoteAgency<TSerialized, TEntityBase> Create<TSerialized, TEntityBase>(
	SerializingHelperBase<TSerialized, TEntityBase> serializingHelper,
	EntityCodeBuilderBase entityCodeBuilder,
	Nullable<Guid> siteId = null
)

```

**VB**<br />
``` VB
Public Shared Function Create(Of TSerialized, TEntityBase) ( 
	serializingHelper As SerializingHelperBase(Of TSerialized, TEntityBase),
	entityCodeBuilder As EntityCodeBuilderBase,
	Optional siteId As Nullable(Of Guid) = Nothing
) As RemoteAgency(Of TSerialized, TEntityBase)
```

**C++**<br />
``` C++
public:
generic<typename TSerialized, typename TEntityBase>
static RemoteAgency<TSerialized, TEntityBase>^ Create(
	SerializingHelperBase<TSerialized, TEntityBase>^ serializingHelper, 
	EntityCodeBuilderBase^ entityCodeBuilder, 
	Nullable<Guid> siteId = nullptr
)
```

**F#**<br />
``` F#
static member Create : 
        serializingHelper : SerializingHelperBase<'TSerialized, 'TEntityBase> * 
        entityCodeBuilder : EntityCodeBuilderBase * 
        ?siteId : Nullable<Guid> 
(* Defaults:
        let _siteId = defaultArg siteId null
*)
-> RemoteAgency<'TSerialized, 'TEntityBase> 

```


#### Parameters
&nbsp;<dl><dt>serializingHelper</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_SerializingHelperBase_2">SecretNest.RemoteAgency.SerializingHelperBase</a>(*TSerialized*, *TEntityBase*)<br />Serializer helper.</dd><dt>entityCodeBuilder</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_EntityCodeBuilderBase">SecretNest.RemoteAgency.EntityCodeBuilderBase</a><br />Entity code builder.</dd><dt>siteId (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.nullable-1" target="_blank">System.Nullable</a>(<a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">Guid</a>)<br />Site id. A randomized value is used when it is set to <a href="https://docs.microsoft.com/dotnet/api/system.guid.empty" target="_blank">Empty</a> or absent.</dd></dl>

#### Type Parameters
&nbsp;<dl><dt>TSerialized</dt><dd>Type of the serialized data.</dd><dt>TEntityBase</dt><dd>Type of the parent class of all entities.</dd></dl>

#### Return Value
Type: <a href="T_SecretNest_RemoteAgency_RemoteAgency_2">RemoteAgency</a>(*TSerialized*, *TEntityBase*)<br />Created Remote Agency instance.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />