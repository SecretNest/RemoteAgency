# RemoteAgency.SetExceptionAsResponse Method 
 

Sets an exception as the result of a message waiting for response and break the waiting.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public void SetExceptionAsResponse(
	Guid instanceId,
	Guid messageId,
	Exception exception
)
```

**VB**<br />
``` VB
Public Sub SetExceptionAsResponse ( 
	instanceId As Guid,
	messageId As Guid,
	exception As Exception
)
```

**C++**<br />
``` C++
public:
void SetExceptionAsResponse(
	Guid instanceId, 
	Guid messageId, 
	Exception^ exception
)
```

**F#**<br />
``` F#
member SetExceptionAsResponse : 
        instanceId : Guid * 
        messageId : Guid * 
        exception : Exception -> unit 

```


#### Parameters
&nbsp;<dl><dt>instanceId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">System.Guid</a><br />Id of proxy or service wrapper instance.</dd><dt>messageId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">System.Guid</a><br />Message id.</dd><dt>exception</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">System.Exception</a><br />The exception object to be passed.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br /><a href="T_SecretNest_RemoteAgency_Attributes_OperatingTimeoutTimeAttribute">SecretNest.RemoteAgency.Attributes.OperatingTimeoutTimeAttribute</a><br />