# RemoteAgency.ResetProxyStickyTargetSite Method 
 

Resets sticky target site of the proxy specified.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public void ResetProxyStickyTargetSite(
	Object proxy
)
```

**VB**<br />
``` VB
Public Sub ResetProxyStickyTargetSite ( 
	proxy As Object
)
```

**C++**<br />
``` C++
public:
void ResetProxyStickyTargetSite(
	Object^ proxy
)
```

**F#**<br />
``` F#
member ResetProxyStickyTargetSite : 
        proxy : Object -> unit 

```


#### Parameters
&nbsp;<dl><dt>proxy</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">System.Object</a><br />Proxy to be reset.</dd></dl>

## Remarks
Should be called when a service wrapper is closing and this proxy has the sticky target site pointed to the site managing the closing service wrapper.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br /><a href="T_SecretNest_RemoteAgency_Attributes_ProxyStickyTargetSiteAttribute">SecretNest.RemoteAgency.Attributes.ProxyStickyTargetSiteAttribute</a><br />