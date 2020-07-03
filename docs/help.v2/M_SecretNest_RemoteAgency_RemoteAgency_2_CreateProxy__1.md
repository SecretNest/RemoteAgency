# RemoteAgency(*TSerialized*, *TEntityBase*).CreateProxy(*TInterface*) Method (Guid, Guid, Guid, Int32, Boolean)
 

Creates proxy of the interface specified.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public TInterface CreateProxy<TInterface>(
	Guid targetSiteId,
	Guid targetInstanceId,
	ref Guid instanceId,
	int defaultTimeout = 90000,
	bool buildServiceWrapperWithProxy = true
)

```

**VB**<br />
``` VB
Public Function CreateProxy(Of TInterface) ( 
	targetSiteId As Guid,
	targetInstanceId As Guid,
	ByRef instanceId As Guid,
	Optional defaultTimeout As Integer = 90000,
	Optional buildServiceWrapperWithProxy As Boolean = true
) As TInterface
```

**C++**<br />
``` C++
public:
generic<typename TInterface>
TInterface CreateProxy(
	Guid targetSiteId, 
	Guid targetInstanceId, 
	Guid% instanceId, 
	int defaultTimeout = 90000, 
	bool buildServiceWrapperWithProxy = true
)
```

**F#**<br />
``` F#
member CreateProxy : 
        targetSiteId : Guid * 
        targetInstanceId : Guid * 
        instanceId : Guid byref * 
        ?defaultTimeout : int * 
        ?buildServiceWrapperWithProxy : bool 
(* Defaults:
        let _defaultTimeout = defaultArg defaultTimeout 90000
        let _buildServiceWrapperWithProxy = defaultArg buildServiceWrapperWithProxy true
*)
-> 'TInterface 

```


#### Parameters
&nbsp;<dl><dt>targetSiteId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">System.Guid</a><br />Target site id of the created proxy instance.</dd><dt>targetInstanceId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">System.Guid</a><br />Target instance id of the created proxy instance.</dd><dt>instanceId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">System.Guid</a><br />Id of the created proxy instance. A new id is generated if value is set to <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">Guid</a>.Empty.</dd><dt>defaultTimeout (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.int32" target="_blank">System.Int32</a><br />Default timeout in milliseconds for all operations; or -1 to indicate that the waiting does not time out. Default value is 90000 (90 sec).</dd><dt>buildServiceWrapperWithProxy (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">System.Boolean</a><br />When building is required, builds service wrapper and its required entities in the same assembly. Default value is `true` (`True` in Visual Basic).</dd></dl>

#### Type Parameters
&nbsp;<dl><dt>TInterface</dt><dd>Service contract interface to be implemented by this proxy.</dd></dl>

#### Return Value
Type: *TInterface*<br />Proxy instance.

## Events
&nbsp;<table><tr><th>Event Type</th><th>Reason</th></tr><tr><td><a href="E_SecretNest_RemoteAgency_RemoteAgency_BeforeTypeCreated">RemoteAgency.BeforeTypeCreated</a></td><td>Raised before type building finished when a type is required for building.</td></tr><tr><td><a href="E_SecretNest_RemoteAgency_RemoteAgency_BeforeAssemblyCreated">RemoteAgency.BeforeAssemblyCreated</a></td><td>Raised before module and assembly building finished when a type is required for building.</td></tr><tr><td><a href="E_SecretNest_RemoteAgency_RemoteAgency_AfterTypeAndAssemblyBuilt">RemoteAgency.AfterTypeAndAssemblyBuilt</a></td><td>Raised after the assembly built when a type is required for building.</td></tr></table>

## Remarks
The types required will be created when necessary.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency_2">RemoteAgency(TSerialized, TEntityBase) Class</a><br /><a href="Overload_SecretNest_RemoteAgency_RemoteAgency_2_CreateProxy">CreateProxy Overload</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />