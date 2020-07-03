# RemoteAgency.GenerateEmptyMessage Method (Guid, Guid, MessageType, String, Guid, Exception)
 

Creates an empty message with sender instance id set to <a href="https://docs.microsoft.com/dotnet/api/system.guid.empty" target="_blank">Empty</a> and one way is `true` (`True` in Visual Basic).

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
protected IRemoteAgencyMessage GenerateEmptyMessage(
	Guid targetSiteId,
	Guid targetInstanceId,
	MessageType messageType,
	string assetName,
	Guid messageId,
	Exception exception
)
```

**VB**<br />
``` VB
Protected Function GenerateEmptyMessage ( 
	targetSiteId As Guid,
	targetInstanceId As Guid,
	messageType As MessageType,
	assetName As String,
	messageId As Guid,
	exception As Exception
) As IRemoteAgencyMessage
```

**C++**<br />
``` C++
protected:
IRemoteAgencyMessage^ GenerateEmptyMessage(
	Guid targetSiteId, 
	Guid targetInstanceId, 
	MessageType messageType, 
	String^ assetName, 
	Guid messageId, 
	Exception^ exception
)
```

**F#**<br />
``` F#
member GenerateEmptyMessage : 
        targetSiteId : Guid * 
        targetInstanceId : Guid * 
        messageType : MessageType * 
        assetName : string * 
        messageId : Guid * 
        exception : Exception -> IRemoteAgencyMessage 

```


#### Parameters
&nbsp;<dl><dt>targetSiteId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">System.Guid</a><br />Target site id.</dd><dt>targetInstanceId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">System.Guid</a><br />Target instance id.</dd><dt>messageType</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_MessageType">SecretNest.RemoteAgency.MessageType</a><br />Message type.</dd><dt>assetName</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Asset name.</dd><dt>messageId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">System.Guid</a><br />Message id.</dd><dt>exception</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">System.Exception</a><br />Exception.</dd></dl>

#### Return Value
Type: <a href="T_SecretNest_RemoteAgency_IRemoteAgencyMessage">IRemoteAgencyMessage</a><br />Empty message.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency Class</a><br /><a href="Overload_SecretNest_RemoteAgency_RemoteAgency_GenerateEmptyMessage">GenerateEmptyMessage Overload</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />