# IServiceWrapperCommunicate.SendOneWayEventMessageCallback Property 
 

Will be called while an event raising message need to be sent to a remote site without getting response.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
SendOneWayMessageCallback SendOneWayEventMessageCallback { get; set; }
```

**VB**<br />
``` VB
Property SendOneWayEventMessageCallback As SendOneWayMessageCallback
	Get
	Set
```

**C++**<br />
``` C++
property SendOneWayMessageCallback^ SendOneWayEventMessageCallback {
	SendOneWayMessageCallback^ get ();
	void set (SendOneWayMessageCallback^ value);
}
```

**F#**<br />
``` F#
abstract SendOneWayEventMessageCallback : SendOneWayMessageCallback with get, set

```


#### Property Value
Type: <a href="T_SecretNest_RemoteAgency_SendOneWayMessageCallback">SendOneWayMessageCallback</a>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_IServiceWrapperCommunicate">IServiceWrapperCommunicate Interface</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />