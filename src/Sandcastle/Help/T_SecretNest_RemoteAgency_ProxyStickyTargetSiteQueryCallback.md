# ProxyStickyTargetSiteQueryCallback Delegate
 

Queries the proxy sticky target site setting state.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public delegate void ProxyStickyTargetSiteQueryCallback(
	out bool isEnabled,
	out Guid defaultTargetSiteId,
	out Nullable<Guid> stickyTargetSiteId
)
```

**VB**<br />
``` VB
Public Delegate Sub ProxyStickyTargetSiteQueryCallback ( 
	<OutAttribute> ByRef isEnabled As Boolean,
	<OutAttribute> ByRef defaultTargetSiteId As Guid,
	<OutAttribute> ByRef stickyTargetSiteId As Nullable(Of Guid)
)
```

**C++**<br />
``` C++
public delegate void ProxyStickyTargetSiteQueryCallback(
	[OutAttribute] bool% isEnabled, 
	[OutAttribute] Guid% defaultTargetSiteId, 
	[OutAttribute] Nullable<Guid>% stickyTargetSiteId
)
```

**F#**<br />
``` F#
type ProxyStickyTargetSiteQueryCallback = 
    delegate of 
        isEnabled : bool byref * 
        defaultTargetSiteId : Guid byref * 
        stickyTargetSiteId : Nullable<Guid> byref -> unit
```


#### Parameters
&nbsp;<dl><dt>isEnabled</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">System.Boolean</a><br />Will be set as whether this function is enabled on this proxy.</dd><dt>defaultTargetSiteId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">System.Guid</a><br />Will be set as default target site id.</dd><dt>stickyTargetSiteId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.nullable-1" target="_blank">System.Nullable</a>(<a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">Guid</a>)<br />Will be set as sticky target site id. Value will be set to a null reference (`Nothing` in Visual Basic) if no sticky target set yet.</dd></dl>

## See Also


#### Reference
<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />