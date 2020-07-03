# RemoteAgency(*TSerialized*, *TEntityBase*).ProcessReceivedSerializedMessage Method 
 

Processes a serialized message received.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public void ProcessReceivedSerializedMessage(
	TSerialized serializedMessage
)
```

**VB**<br />
``` VB
Public Sub ProcessReceivedSerializedMessage ( 
	serializedMessage As TSerialized
)
```

**C++**<br />
``` C++
public:
void ProcessReceivedSerializedMessage(
	TSerialized serializedMessage
)
```

**F#**<br />
``` F#
member ProcessReceivedSerializedMessage : 
        serializedMessage : 'TSerialized -> unit 

```


#### Parameters
&nbsp;<dl><dt>serializedMessage</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_RemoteAgency_2">*TSerialized*</a><br />Received serialized message.</dd></dl>

## Events
&nbsp;<table><tr><th>Event Type</th><th>Reason</th></tr><tr><td><a href="E_SecretNest_RemoteAgency_RemoteAgency_2_AfterMessageReceived">RemoteAgency(TSerialized, TEntityBase).AfterMessageReceived</a></td><td>Raised after deserialized before further processing.</td></tr></table>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency_2">RemoteAgency(TSerialized, TEntityBase) Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />