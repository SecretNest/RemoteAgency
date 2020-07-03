# MessageProcessTerminatedException Constructor 
 

Initializes an instance of the MessageProcessTerminatedException.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public MessageProcessTerminatedException(
	string message,
	MessageProcessTerminatedPosition terminatedPosition,
	IRemoteAgencyMessage terminatedMessage
)
```

**VB**<br />
``` VB
Public Sub New ( 
	message As String,
	terminatedPosition As MessageProcessTerminatedPosition,
	terminatedMessage As IRemoteAgencyMessage
)
```

**C++**<br />
``` C++
public:
MessageProcessTerminatedException(
	String^ message, 
	MessageProcessTerminatedPosition terminatedPosition, 
	IRemoteAgencyMessage^ terminatedMessage
)
```

**F#**<br />
``` F#
new : 
        message : string * 
        terminatedPosition : MessageProcessTerminatedPosition * 
        terminatedMessage : IRemoteAgencyMessage -> MessageProcessTerminatedException
```


#### Parameters
&nbsp;<dl><dt>message</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Error message.</dd><dt>terminatedPosition</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_MessageProcessTerminatedPosition">SecretNest.RemoteAgency.MessageProcessTerminatedPosition</a><br />Position where the message is terminated.</dd><dt>terminatedMessage</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_IRemoteAgencyMessage">SecretNest.RemoteAgency.IRemoteAgencyMessage</a><br />Terminated message.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_MessageProcessTerminatedException">MessageProcessTerminatedException Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />