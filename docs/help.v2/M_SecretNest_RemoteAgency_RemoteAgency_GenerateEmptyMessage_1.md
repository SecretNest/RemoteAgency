# RemoteAgency.GenerateEmptyMessage Method (Guid, Guid, Guid, MessageType, String, Guid, Exception, Boolean)
 

Creates an empty message.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
protected IRemoteAgencyMessage GenerateEmptyMessage(
	Guid senderInstanceId,
	Guid targetSiteId,
	Guid targetInstanceId,
	MessageType messageType,
	string assetName,
	Guid messageId,
	Exception exception,
	bool isOneWay
)
```

**VB**<br />
``` VB
Protected Function GenerateEmptyMessage ( 
	senderInstanceId As Guid,
	targetSiteId As Guid,
	targetInstanceId As Guid,
	messageType As MessageType,
	assetName As String,
	messageId As Guid,
	exception As Exception,
	isOneWay As Boolean
) As IRemoteAgencyMessage
```

**C++**<br />
``` C++
protected:
IRemoteAgencyMessage^ GenerateEmptyMessage(
	Guid senderInstanceId, 
	Guid targetSiteId, 
	Guid targetInstanceId, 
	MessageType messageType, 
	String^ assetName, 
	Guid messageId, 
	Exception^ exception, 
	bool isOneWay
)
```

**F#**<br />
``` F#
member GenerateEmptyMessage : 
        senderInstanceId : Guid * 
        targetSiteId : Guid * 
        targetInstanceId : Guid * 
        messageType : MessageType * 
        assetName : string * 
        messageId : Guid * 
        exception : Exception * 
        isOneWay : bool -> IRemoteAgencyMessage 

```


#### Parameters
&nbsp;<dl><dt>senderInstanceId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">System.Guid</a><br />Sender instance id.</dd><dt>targetSiteId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">System.Guid</a><br />Target site id.</dd><dt>targetInstanceId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">System.Guid</a><br />Target instance id.</dd><dt>messageType</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_MessageType">SecretNest.RemoteAgency.MessageType</a><br />Message type.</dd><dt>assetName</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Asset name.</dd><dt>messageId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">System.Guid</a><br />Message id.</dd><dt>exception</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">System.Exception</a><br />Exception.</dd><dt>isOneWay</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">System.Boolean</a><br />Whether the message is one way.</dd></dl>

#### Return Value
Type: <a href="T_SecretNest_RemoteAgency_IRemoteAgencyMessage">IRemoteAgencyMessage</a><br />Empty message.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency Class</a><br /><a href="Overload_SecretNest_RemoteAgency_RemoteAgency_GenerateEmptyMessage">GenerateEmptyMessage Overload</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />