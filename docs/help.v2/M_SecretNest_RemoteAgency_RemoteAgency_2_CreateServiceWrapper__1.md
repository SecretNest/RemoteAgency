# RemoteAgency(*TSerialized*, *TEntityBase*).CreateServiceWrapper(*TInterface*) Method (*TInterface*, Guid, Int32, Boolean)
 

Creates service wrapper of the interface and the service object specified.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public Guid CreateServiceWrapper<TInterface>(
	TInterface serviceObject,
	ref Guid instanceId,
	int defaultTimeout = 90000,
	bool buildProxyWithServiceWrapper = true
)

```

**VB**<br />
``` VB
Public Function CreateServiceWrapper(Of TInterface) ( 
	serviceObject As TInterface,
	ByRef instanceId As Guid,
	Optional defaultTimeout As Integer = 90000,
	Optional buildProxyWithServiceWrapper As Boolean = true
) As Guid
```

**C++**<br />
``` C++
public:
generic<typename TInterface>
Guid CreateServiceWrapper(
	TInterface serviceObject, 
	Guid% instanceId, 
	int defaultTimeout = 90000, 
	bool buildProxyWithServiceWrapper = true
)
```

**F#**<br />
``` F#
member CreateServiceWrapper : 
        serviceObject : 'TInterface * 
        instanceId : Guid byref * 
        ?defaultTimeout : int * 
        ?buildProxyWithServiceWrapper : bool 
(* Defaults:
        let _defaultTimeout = defaultArg defaultTimeout 90000
        let _buildProxyWithServiceWrapper = defaultArg buildProxyWithServiceWrapper true
*)
-> Guid 

```


#### Parameters
&nbsp;<dl><dt>serviceObject</dt><dd>Type: *TInterface*<br />The service object to be wrapped.</dd><dt>instanceId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">System.Guid</a><br />Id of the created service wrapper instance. A new id is generated if value is set to <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">Guid</a>.Empty.</dd><dt>defaultTimeout (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.int32" target="_blank">System.Int32</a><br />Default timeout in milliseconds for all operations; or -1 to indicate that the waiting does not time out. Default value is 90000 (90 sec).</dd><dt>buildProxyWithServiceWrapper (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">System.Boolean</a><br />When building is required, builds proxy and its required entities in the same assembly. Default value is `true` (`True` in Visual Basic).</dd></dl>

#### Type Parameters
&nbsp;<dl><dt>TInterface</dt><dd>Service contract interface of the service to be implemented by this service wrapper and have been implemented by the *serviceObject*.</dd></dl>

#### Return Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">Guid</a><br />The id of the service wrapper instance created.

## Events
&nbsp;<table><tr><th>Event Type</th><th>Reason</th></tr><tr><td><a href="E_SecretNest_RemoteAgency_RemoteAgency_BeforeTypeCreated">RemoteAgency.BeforeTypeCreated</a></td><td>Raised before type building finished when a type is required for building.</td></tr><tr><td><a href="E_SecretNest_RemoteAgency_RemoteAgency_BeforeAssemblyCreated">RemoteAgency.BeforeAssemblyCreated</a></td><td>Raised before module and assembly building finished when a type is required for building.</td></tr><tr><td><a href="E_SecretNest_RemoteAgency_RemoteAgency_AfterTypeAndAssemblyBuilt">RemoteAgency.AfterTypeAndAssemblyBuilt</a></td><td>Raised after the assembly built when a type is required for building.</td></tr></table>

## Remarks
The types required will be created when necessary.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency_2">RemoteAgency(TSerialized, TEntityBase) Class</a><br /><a href="Overload_SecretNest_RemoteAgency_RemoteAgency_2_CreateServiceWrapper">CreateServiceWrapper Overload</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />