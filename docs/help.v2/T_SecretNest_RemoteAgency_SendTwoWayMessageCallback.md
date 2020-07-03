# SendTwoWayMessageCallback Delegate
 

Sends a message out and gets a response message.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public delegate IRemoteAgencyMessage SendTwoWayMessageCallback(
	IRemoteAgencyMessage message,
	int timeout
)
```

**VB**<br />
``` VB
Public Delegate Function SendTwoWayMessageCallback ( 
	message As IRemoteAgencyMessage,
	timeout As Integer
) As IRemoteAgencyMessage
```

**C++**<br />
``` C++
public delegate IRemoteAgencyMessage^ SendTwoWayMessageCallback(
	IRemoteAgencyMessage^ message, 
	int timeout
)
```

**F#**<br />
``` F#
type SendTwoWayMessageCallback = 
    delegate of 
        message : IRemoteAgencyMessage * 
        timeout : int -> IRemoteAgencyMessage
```


#### Parameters
&nbsp;<dl><dt>message</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_IRemoteAgencyMessage">SecretNest.RemoteAgency.IRemoteAgencyMessage</a><br />Message to be sent.</dd><dt>timeout</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.int32" target="_blank">System.Int32</a><br />Timeout in millisecond.</dd></dl>

#### Return Value
Type: <a href="T_SecretNest_RemoteAgency_IRemoteAgencyMessage">IRemoteAgencyMessage</a><br />Response message. Value a null reference (`Nothing` in Visual Basic) will be returned if no return required by *message*.

## Exceptions
&nbsp;<table><tr><th>Exception</th><th>Condition</th></tr><tr><td><a href="T_SecretNest_RemoteAgency_AccessingTimeOutException">AccessingTimeOutException</a></td><td>Thrown when timed out.</td></tr></table>

## See Also


#### Reference
<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />