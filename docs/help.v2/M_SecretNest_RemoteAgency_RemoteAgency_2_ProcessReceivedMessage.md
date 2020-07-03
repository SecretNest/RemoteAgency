# RemoteAgency(*TSerialized*, *TEntityBase*).ProcessReceivedMessage Method (IRemoteAgencyMessage)
 

Processes a message received.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public void ProcessReceivedMessage(
	IRemoteAgencyMessage message
)
```

**VB**<br />
``` VB
Public Sub ProcessReceivedMessage ( 
	message As IRemoteAgencyMessage
)
```

**C++**<br />
``` C++
public:
void ProcessReceivedMessage(
	IRemoteAgencyMessage^ message
)
```

**F#**<br />
``` F#
member ProcessReceivedMessage : 
        message : IRemoteAgencyMessage -> unit 

```


#### Parameters
&nbsp;<dl><dt>message</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_IRemoteAgencyMessage">SecretNest.RemoteAgency.IRemoteAgencyMessage</a><br />Received message.</dd></dl>

## Events
&nbsp;<table><tr><th>Event Type</th><th>Reason</th></tr><tr><td><a href="E_SecretNest_RemoteAgency_RemoteAgency_2_AfterMessageReceived">RemoteAgency(TSerialized, TEntityBase).AfterMessageReceived</a></td><td>Raised after deserialized before further processing.</td></tr></table>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency_2">RemoteAgency(TSerialized, TEntityBase) Class</a><br /><a href="Overload_SecretNest_RemoteAgency_RemoteAgency_2_ProcessReceivedMessage">ProcessReceivedMessage Overload</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />