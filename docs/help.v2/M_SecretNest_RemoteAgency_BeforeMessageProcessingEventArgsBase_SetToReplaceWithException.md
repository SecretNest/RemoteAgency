# BeforeMessageProcessingEventArgsBase.SetToReplaceWithException Method 
 

Replaces this message by an instance of <a href="T_SecretNest_RemoteAgency_MessageProcessTerminatedException">MessageProcessTerminatedException</a> then sends it to the receiver. Cannot be used when <a href="P_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgsBase_MessageDirection">MessageDirection</a> is <a href="T_SecretNest_RemoteAgency_MessageDirection">Sending</a>.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public void SetToReplaceWithException(
	string message = "Remote Agency Manager terminated this message processing due to user request."
)
```

**VB**<br />
``` VB
Public Sub SetToReplaceWithException ( 
	Optional message As String = "Remote Agency Manager terminated this message processing due to user request."
)
```

**C++**<br />
``` C++
public:
void SetToReplaceWithException(
	String^ message = L"Remote Agency Manager terminated this message processing due to user request."
)
```

**F#**<br />
``` F#
member SetToReplaceWithException : 
        ?message : string 
(* Defaults:
        let _message = defaultArg message "Remote Agency Manager terminated this message processing due to user request."
*)
-> unit 

```


#### Parameters
&nbsp;<dl><dt>message (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Message of the exception.</dd></dl>

## Remarks
Caution: This may cause the sender throw <a href="T_SecretNest_RemoteAgency_AccessingTimeOutException">AccessingTimeOutException</a> if <a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_IsOneWay">IsOneWay</a> is set to `false` (`False` in Visual Basic).

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgsBase">BeforeMessageProcessingEventArgsBase Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />