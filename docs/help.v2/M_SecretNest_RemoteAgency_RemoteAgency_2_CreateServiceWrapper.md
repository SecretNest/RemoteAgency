# RemoteAgency(*TSerialized*, *TEntityBase*).CreateServiceWrapper Method (Type, Object, Guid, Int32, Boolean)
 

Creates service wrapper of the interface and the service object specified.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public override Guid CreateServiceWrapper(
	Type sourceInterface,
	Object serviceObject,
	ref Guid instanceId,
	int defaultTimeout = 90000,
	bool buildProxyWithServiceWrapper = true
)
```

**VB**<br />
``` VB
Public Overrides Function CreateServiceWrapper ( 
	sourceInterface As Type,
	serviceObject As Object,
	ByRef instanceId As Guid,
	Optional defaultTimeout As Integer = 90000,
	Optional buildProxyWithServiceWrapper As Boolean = true
) As Guid
```

**C++**<br />
``` C++
public:
virtual Guid CreateServiceWrapper(
	Type^ sourceInterface, 
	Object^ serviceObject, 
	Guid% instanceId, 
	int defaultTimeout = 90000, 
	bool buildProxyWithServiceWrapper = true
) override
```

**F#**<br />
``` F#
abstract CreateServiceWrapper : 
        sourceInterface : Type * 
        serviceObject : Object * 
        instanceId : Guid byref * 
        ?defaultTimeout : int * 
        ?buildProxyWithServiceWrapper : bool 
(* Defaults:
        let _defaultTimeout = defaultArg defaultTimeout 90000
        let _buildProxyWithServiceWrapper = defaultArg buildProxyWithServiceWrapper true
*)
-> Guid 
override CreateServiceWrapper : 
        sourceInterface : Type * 
        serviceObject : Object * 
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
&nbsp;<dl><dt>sourceInterface</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.type" target="_blank">System.Type</a><br />Type of service contract interface to be implemented by this service wrapper and have been implemented by the *serviceObject*.</dd><dt>serviceObject</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">System.Object</a><br />The service object to be wrapped.</dd><dt>instanceId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">System.Guid</a><br />Id of the created service wrapper instance. A new id is generated if value is set to <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">Guid</a>.Empty.</dd><dt>defaultTimeout (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.int32" target="_blank">System.Int32</a><br />Default timeout in milliseconds for all operations; or -1 to indicate that the waiting does not time out. Default value is 90000 (90 sec).</dd><dt>buildProxyWithServiceWrapper (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">System.Boolean</a><br />When building is required, builds proxy and its required entities in the same assembly. Default value is `true` (`True` in Visual Basic).</dd></dl>

#### Return Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">Guid</a><br />The id of the service wrapper instance created.

## Events
&nbsp;<table><tr><th>Event Type</th><th>Reason</th></tr><tr><td><a href="E_SecretNest_RemoteAgency_RemoteAgency_BeforeTypeCreated">RemoteAgency.BeforeTypeCreated</a></td><td>Raised before type building finished when a type is required for building.</td></tr><tr><td><a href="E_SecretNest_RemoteAgency_RemoteAgency_BeforeAssemblyCreated">RemoteAgency.BeforeAssemblyCreated</a></td><td>Raised before module and assembly building finished when a type is required for building.</td></tr><tr><td><a href="E_SecretNest_RemoteAgency_RemoteAgency_AfterTypeAndAssemblyBuilt">RemoteAgency.AfterTypeAndAssemblyBuilt</a></td><td>Raised after the assembly built when a type is required for building.</td></tr></table>

## Remarks
The types required will be created when necessary.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency_2">RemoteAgency(TSerialized, TEntityBase) Class</a><br /><a href="Overload_SecretNest_RemoteAgency_RemoteAgency_2_CreateServiceWrapper">CreateServiceWrapper Overload</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />