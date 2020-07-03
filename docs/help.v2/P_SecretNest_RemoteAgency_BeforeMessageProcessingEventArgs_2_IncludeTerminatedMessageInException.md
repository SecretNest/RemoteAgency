# BeforeMessageProcessingEventArgs(*TSerialized*, *TEntityBase*).IncludeTerminatedMessageInException Property 
 

Gets or sets whether the terminated message should be included in exception. Default value is `true` (`True` in Visual Basic).

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public bool IncludeTerminatedMessageInException { get; set; }
```

**VB**<br />
``` VB
Public Property IncludeTerminatedMessageInException As Boolean
	Get
	Set
```

**C++**<br />
``` C++
public:
property bool IncludeTerminatedMessageInException {
	bool get ();
	void set (bool value);
}
```

**F#**<br />
``` F#
member IncludeTerminatedMessageInException : bool with get, set

```


#### Property Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">Boolean</a>

## Remarks
If enabled, the terminated message will be saved in <a href="P_SecretNest_RemoteAgency_MessageProcessTerminatedException_TerminatedMessage">TerminatedMessage</a>.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgs_2">BeforeMessageProcessingEventArgs(TSerialized, TEntityBase) Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />