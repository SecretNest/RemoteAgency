# BeforeMessageProcessingEventArgsBase.SetToTerminateAndReturnException Method 
 

Terminates this process and send an instance of <a href="T_SecretNest_RemoteAgency_MessageProcessTerminatedException">MessageProcessTerminatedException</a> back to the sender. Cannot be used when <a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_IsOneWay">IsOneWay</a> is `true` (`True` in Visual Basic).

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public void SetToTerminateAndReturnException(
	string message = "Remote Agency Manager terminated this message processing due to user request."
)
```

**VB**<br />
``` VB
Public Sub SetToTerminateAndReturnException ( 
	Optional message As String = "Remote Agency Manager terminated this message processing due to user request."
)
```

**C++**<br />
``` C++
public:
void SetToTerminateAndReturnException(
	String^ message = L"Remote Agency Manager terminated this message processing due to user request."
)
```

**F#**<br />
``` F#
member SetToTerminateAndReturnException : 
        ?message : string 
(* Defaults:
        let _message = defaultArg message "Remote Agency Manager terminated this message processing due to user request."
*)
-> unit 

```


#### Parameters
&nbsp;<dl><dt>message (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Message of the exception.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgsBase">BeforeMessageProcessingEventArgsBase Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />