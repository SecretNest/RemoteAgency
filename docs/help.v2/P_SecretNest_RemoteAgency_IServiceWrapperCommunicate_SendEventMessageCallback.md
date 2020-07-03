# IServiceWrapperCommunicate.SendEventMessageCallback Property 
 

Will be called while an event raising message need to be sent to a remote site and get response of it.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
SendTwoWayMessageCallback SendEventMessageCallback { get; set; }
```

**VB**<br />
``` VB
Property SendEventMessageCallback As SendTwoWayMessageCallback
	Get
	Set
```

**C++**<br />
``` C++
property SendTwoWayMessageCallback^ SendEventMessageCallback {
	SendTwoWayMessageCallback^ get ();
	void set (SendTwoWayMessageCallback^ value);
}
```

**F#**<br />
``` F#
abstract SendEventMessageCallback : SendTwoWayMessageCallback with get, set

```


#### Property Value
Type: <a href="T_SecretNest_RemoteAgency_SendTwoWayMessageCallback">SendTwoWayMessageCallback</a>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_IServiceWrapperCommunicate">IServiceWrapperCommunicate Interface</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />