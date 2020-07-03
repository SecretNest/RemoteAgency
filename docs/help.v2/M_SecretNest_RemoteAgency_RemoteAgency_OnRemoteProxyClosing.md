# RemoteAgency.OnRemoteProxyClosing Method 
 

Unlinks specified remote proxy from the event registered in service wrapper objects.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public abstract void OnRemoteProxyClosing(
	Guid siteId,
	Nullable<Guid> proxyInstanceId = null
)
```

**VB**<br />
``` VB
Public MustOverride Sub OnRemoteProxyClosing ( 
	siteId As Guid,
	Optional proxyInstanceId As Nullable(Of Guid) = Nothing
)
```

**C++**<br />
``` C++
public:
virtual void OnRemoteProxyClosing(
	Guid siteId, 
	Nullable<Guid> proxyInstanceId = nullptr
) abstract
```

**F#**<br />
``` F#
abstract OnRemoteProxyClosing : 
        siteId : Guid * 
        ?proxyInstanceId : Nullable<Guid> 
(* Defaults:
        let _proxyInstanceId = defaultArg proxyInstanceId null
*)
-> unit 

```


#### Parameters
&nbsp;<dl><dt>siteId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">System.Guid</a><br />The site id of the instance of the Remote Agency which managing the closing proxy.</dd><dt>proxyInstanceId (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.nullable-1" target="_blank">System.Nullable</a>(<a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">Guid</a>)<br />The instance id of the closing proxy. When set to null, all proxies from the site specified by *siteId* will be unlinked. Default value is null.</dd></dl>

## Remarks

Should be called when a remote proxy closing happened without messages routed to <a href="M_SecretNest_RemoteAgency_RemoteAgency_2_ProcessReceivedMessage_1">ProcessReceivedMessage(TEntityBase)</a> or <a href="M_SecretNest_RemoteAgency_RemoteAgency_2_ProcessReceivedMessage">ProcessReceivedMessage(IRemoteAgencyMessage)</a>.

Service wrapper object manages links of all proxies which need to handle events. When remote proxy is disposed, messages for removing event handlers are sent to the service wrapper object. But when something wrong happened, network disconnected or proxy object crashed for example, the crucial messages may not be able to transferred correctly. In this case, this method need to be called, or the obsolete links will stay in service wrapper object which may cause lags or exceptions while processing events.


## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />