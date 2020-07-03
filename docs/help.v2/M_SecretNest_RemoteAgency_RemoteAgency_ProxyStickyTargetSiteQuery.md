# RemoteAgency.ProxyStickyTargetSiteQuery Method 
 

Queries the proxy sticky target site setting state.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public void ProxyStickyTargetSiteQuery(
	Object proxy,
	out bool isEnabled,
	out Guid defaultTargetSiteId,
	out Nullable<Guid> stickyTargetSiteId
)
```

**VB**<br />
``` VB
Public Sub ProxyStickyTargetSiteQuery ( 
	proxy As Object,
	<OutAttribute> ByRef isEnabled As Boolean,
	<OutAttribute> ByRef defaultTargetSiteId As Guid,
	<OutAttribute> ByRef stickyTargetSiteId As Nullable(Of Guid)
)
```

**C++**<br />
``` C++
public:
void ProxyStickyTargetSiteQuery(
	Object^ proxy, 
	[OutAttribute] bool% isEnabled, 
	[OutAttribute] Guid% defaultTargetSiteId, 
	[OutAttribute] Nullable<Guid>% stickyTargetSiteId
)
```

**F#**<br />
``` F#
member ProxyStickyTargetSiteQuery : 
        proxy : Object * 
        isEnabled : bool byref * 
        defaultTargetSiteId : Guid byref * 
        stickyTargetSiteId : Nullable<Guid> byref -> unit 

```


#### Parameters
&nbsp;<dl><dt>proxy</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">System.Object</a><br />Proxy to be reset.</dd><dt>isEnabled</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">System.Boolean</a><br />Will be set as whether this function is enabled on this proxy.</dd><dt>defaultTargetSiteId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">System.Guid</a><br />Will be set as default target site id.</dd><dt>stickyTargetSiteId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.nullable-1" target="_blank">System.Nullable</a>(<a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">Guid</a>)<br />Will be set as sticky target site id. Value will be set to a null reference (`Nothing` in Visual Basic) if no sticky target set yet.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />