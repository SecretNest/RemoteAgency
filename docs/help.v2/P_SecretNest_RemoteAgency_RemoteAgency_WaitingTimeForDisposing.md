# RemoteAgency.WaitingTimeForDisposing Property 
 

Gets or sets the waiting time in milliseconds for waiting a managing object to complete all communication operations before being disposed.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public int WaitingTimeForDisposing { get; set; }
```

**VB**<br />
``` VB
Public Property WaitingTimeForDisposing As Integer
	Get
	Set
```

**C++**<br />
``` C++
public:
property int WaitingTimeForDisposing {
	int get ();
	void set (int value);
}
```

**F#**<br />
``` F#
member WaitingTimeForDisposing : int with get, set

```


#### Property Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.int32" target="_blank">Int32</a>

## Remarks
The code waiting for response will throw a <a href="https://docs.microsoft.com/dotnet/api/system.objectdisposedexception" target="_blank">ObjectDisposedException</a> when the communication operation is halt due to disposing.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />