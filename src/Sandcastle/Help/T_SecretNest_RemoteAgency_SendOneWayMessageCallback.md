# SendOneWayMessageCallback Delegate
 

Sends a message out.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public delegate void SendOneWayMessageCallback(
	IRemoteAgencyMessage message
)
```

**VB**<br />
``` VB
Public Delegate Sub SendOneWayMessageCallback ( 
	message As IRemoteAgencyMessage
)
```

**C++**<br />
``` C++
public delegate void SendOneWayMessageCallback(
	IRemoteAgencyMessage^ message
)
```

**F#**<br />
``` F#
type SendOneWayMessageCallback = 
    delegate of 
        message : IRemoteAgencyMessage -> unit
```


#### Parameters
&nbsp;<dl><dt>message</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_IRemoteAgencyMessage">SecretNest.RemoteAgency.IRemoteAgencyMessage</a><br />Message to be sent.</dd></dl>

## See Also


#### Reference
<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />