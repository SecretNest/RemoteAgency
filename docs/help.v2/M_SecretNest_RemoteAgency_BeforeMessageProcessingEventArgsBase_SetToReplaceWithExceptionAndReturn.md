# BeforeMessageProcessingEventArgsBase.SetToReplaceWithExceptionAndReturn Method 
 

Replaces this message by an instance of <a href="T_SecretNest_RemoteAgency_MessageProcessTerminatedException">MessageProcessTerminatedException</a> then sends it to the receiver and the sender.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public void SetToReplaceWithExceptionAndReturn(
	string message = "Remote Agency Manager terminated this message processing due to user request."
)
```

**VB**<br />
``` VB
Public Sub SetToReplaceWithExceptionAndReturn ( 
	Optional message As String = "Remote Agency Manager terminated this message processing due to user request."
)
```

**C++**<br />
``` C++
public:
void SetToReplaceWithExceptionAndReturn(
	String^ message = L"Remote Agency Manager terminated this message processing due to user request."
)
```

**F#**<br />
``` F#
member SetToReplaceWithExceptionAndReturn : 
        ?message : string 
(* Defaults:
        let _message = defaultArg message "Remote Agency Manager terminated this message processing due to user request."
*)
-> unit 

```


#### Parameters
&nbsp;<dl><dt>message (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br /></dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgsBase">BeforeMessageProcessingEventArgsBase Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />