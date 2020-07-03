# ServiceWrapperEventHelper.OnRemoteProxyClosing Method 
 

Unlinks specified remote proxy from the event registered in service wrapper objects.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Helper">SecretNest.RemoteAgency.Helper</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public void OnRemoteProxyClosing(
	Guid siteId,
	Nullable<Guid> proxyInstanceId
)
```

**VB**<br />
``` VB
Public Sub OnRemoteProxyClosing ( 
	siteId As Guid,
	proxyInstanceId As Nullable(Of Guid)
)
```

**C++**<br />
``` C++
public:
void OnRemoteProxyClosing(
	Guid siteId, 
	Nullable<Guid> proxyInstanceId
)
```

**F#**<br />
``` F#
member OnRemoteProxyClosing : 
        siteId : Guid * 
        proxyInstanceId : Nullable<Guid> -> unit 

```


#### Parameters
&nbsp;<dl><dt>siteId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">System.Guid</a><br />The site id of the instance of the Remote Agency which managing the closing proxy.</dd><dt>proxyInstanceId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.nullable-1" target="_blank">System.Nullable</a>(<a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">Guid</a>)<br />The instance id of the closing proxy. When set to null, all proxies from the site specified by *siteId* will be unlinked.</dd></dl>

## Exceptions
&nbsp;<table><tr><th>Exception</th><th>Condition</th></tr><tr><td><a href="https://docs.microsoft.com/dotnet/api/system.aggregateexception" target="_blank">AggregateException</a></td><td>When exceptions occurred.</td></tr></table>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Helper_ServiceWrapperEventHelper">ServiceWrapperEventHelper Class</a><br /><a href="N_SecretNest_RemoteAgency_Helper">SecretNest.RemoteAgency.Helper Namespace</a><br />