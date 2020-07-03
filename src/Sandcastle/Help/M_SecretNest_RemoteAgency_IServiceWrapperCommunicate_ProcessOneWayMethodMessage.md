# IServiceWrapperCommunicate.ProcessOneWayMethodMessage Method 
 

Processes a one way method calling message.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
void ProcessOneWayMethodMessage(
	IRemoteAgencyMessage message,
	out LocalExceptionHandlingMode localExceptionHandlingMode
)
```

**VB**<br />
``` VB
Sub ProcessOneWayMethodMessage ( 
	message As IRemoteAgencyMessage,
	<OutAttribute> ByRef localExceptionHandlingMode As LocalExceptionHandlingMode
)
```

**C++**<br />
``` C++
void ProcessOneWayMethodMessage(
	IRemoteAgencyMessage^ message, 
	[OutAttribute] LocalExceptionHandlingMode% localExceptionHandlingMode
)
```

**F#**<br />
``` F#
abstract ProcessOneWayMethodMessage : 
        message : IRemoteAgencyMessage * 
        localExceptionHandlingMode : LocalExceptionHandlingMode byref -> unit 

```


#### Parameters
&nbsp;<dl><dt>message</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_IRemoteAgencyMessage">SecretNest.RemoteAgency.IRemoteAgencyMessage</a><br />Message to be processed.</dd><dt>localExceptionHandlingMode</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_Attributes_LocalExceptionHandlingMode">SecretNest.RemoteAgency.Attributes.LocalExceptionHandlingMode</a><br />Local exception handling mode.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_IServiceWrapperCommunicate">IServiceWrapperCommunicate Interface</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />