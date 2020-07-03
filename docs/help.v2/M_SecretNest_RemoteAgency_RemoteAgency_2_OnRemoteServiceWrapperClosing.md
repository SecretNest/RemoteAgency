# RemoteAgency(*TSerialized*, *TEntityBase*).OnRemoteServiceWrapperClosing Method 
 

Resets sticky target site of all affected proxies when the service wrapper is closing.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public override void OnRemoteServiceWrapperClosing(
	Guid siteId,
	Nullable<Guid> serviceWrapperInstanceId
)
```

**VB**<br />
``` VB
Public Overrides Sub OnRemoteServiceWrapperClosing ( 
	siteId As Guid,
	serviceWrapperInstanceId As Nullable(Of Guid)
)
```

**C++**<br />
``` C++
public:
virtual void OnRemoteServiceWrapperClosing(
	Guid siteId, 
	Nullable<Guid> serviceWrapperInstanceId
) override
```

**F#**<br />
``` F#
abstract OnRemoteServiceWrapperClosing : 
        siteId : Guid * 
        serviceWrapperInstanceId : Nullable<Guid> -> unit 
override OnRemoteServiceWrapperClosing : 
        siteId : Guid * 
        serviceWrapperInstanceId : Nullable<Guid> -> unit 
```


#### Parameters
&nbsp;<dl><dt>siteId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">System.Guid</a><br />The site id of the instance of the Remote Agency which managing the closing service wrapper.</dd><dt>serviceWrapperInstanceId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.nullable-1" target="_blank">System.Nullable</a>(<a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">Guid</a>)<br />The instance id of the closing service wrapper. When set to null, all proxies with sticky target site specified by *siteId* will be reset. Default value is null.</dd></dl>

## Remarks
Should be called when a service wrapper is closing and some proxies managed by the local Remote Agency instance have the sticky target site pointed to the site managing the closing service wrapper.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency_2">RemoteAgency(TSerialized, TEntityBase) Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br /><a href="T_SecretNest_RemoteAgency_Attributes_ProxyStickyTargetSiteAttribute">SecretNest.RemoteAgency.Attributes.ProxyStickyTargetSiteAttribute</a><br />