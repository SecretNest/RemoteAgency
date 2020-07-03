# IServiceWrapperCommunicate.ProcessOneWayPropertySettingMessage Method 
 

Processes a property setting message.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
void ProcessOneWayPropertySettingMessage(
	IRemoteAgencyMessage message,
	out LocalExceptionHandlingMode localExceptionHandlingMode
)
```

**VB**<br />
``` VB
Sub ProcessOneWayPropertySettingMessage ( 
	message As IRemoteAgencyMessage,
	<OutAttribute> ByRef localExceptionHandlingMode As LocalExceptionHandlingMode
)
```

**C++**<br />
``` C++
void ProcessOneWayPropertySettingMessage(
	IRemoteAgencyMessage^ message, 
	[OutAttribute] LocalExceptionHandlingMode% localExceptionHandlingMode
)
```

**F#**<br />
``` F#
abstract ProcessOneWayPropertySettingMessage : 
        message : IRemoteAgencyMessage * 
        localExceptionHandlingMode : LocalExceptionHandlingMode byref -> unit 

```


#### Parameters
&nbsp;<dl><dt>message</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_IRemoteAgencyMessage">SecretNest.RemoteAgency.IRemoteAgencyMessage</a><br />Message to be processed.</dd><dt>localExceptionHandlingMode</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_Attributes_LocalExceptionHandlingMode">SecretNest.RemoteAgency.Attributes.LocalExceptionHandlingMode</a><br />Local exception handling mode.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_IServiceWrapperCommunicate">IServiceWrapperCommunicate Interface</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />