# SendEmptyMessageCallback Delegate
 

Sends an empty message out.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public delegate void SendEmptyMessageCallback(
	MessageType messageType,
	string assetName,
	int timeout
)
```

**VB**<br />
``` VB
Public Delegate Sub SendEmptyMessageCallback ( 
	messageType As MessageType,
	assetName As String,
	timeout As Integer
)
```

**C++**<br />
``` C++
public delegate void SendEmptyMessageCallback(
	MessageType messageType, 
	String^ assetName, 
	int timeout
)
```

**F#**<br />
``` F#
type SendEmptyMessageCallback = 
    delegate of 
        messageType : MessageType * 
        assetName : string * 
        timeout : int -> unit
```


#### Parameters
&nbsp;<dl><dt>messageType</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_MessageType">SecretNest.RemoteAgency.MessageType</a><br />Message type.</dd><dt>assetName</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Asset name.</dd><dt>timeout</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.int32" target="_blank">System.Int32</a><br />Timeout in millisecond.</dd></dl>

## Exceptions
&nbsp;<table><tr><th>Exception</th><th>Condition</th></tr><tr><td><a href="T_SecretNest_RemoteAgency_AccessingTimeOutException">AccessingTimeOutException</a></td><td>Thrown when timed out.</td></tr></table>

## See Also


#### Reference
<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />