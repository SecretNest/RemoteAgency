# IServiceWrapperCommunicate.ProcessPropertySettingMessage Method 
 

Processes a property setting message and returns response.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
IRemoteAgencyMessage ProcessPropertySettingMessage(
	IRemoteAgencyMessage message,
	out Exception exception,
	out LocalExceptionHandlingMode localExceptionHandlingMode
)
```

**VB**<br />
``` VB
Function ProcessPropertySettingMessage ( 
	message As IRemoteAgencyMessage,
	<OutAttribute> ByRef exception As Exception,
	<OutAttribute> ByRef localExceptionHandlingMode As LocalExceptionHandlingMode
) As IRemoteAgencyMessage
```

**C++**<br />
``` C++
IRemoteAgencyMessage^ ProcessPropertySettingMessage(
	IRemoteAgencyMessage^ message, 
	[OutAttribute] Exception^% exception, 
	[OutAttribute] LocalExceptionHandlingMode% localExceptionHandlingMode
)
```

**F#**<br />
``` F#
abstract ProcessPropertySettingMessage : 
        message : IRemoteAgencyMessage * 
        exception : Exception byref * 
        localExceptionHandlingMode : LocalExceptionHandlingMode byref -> IRemoteAgencyMessage 

```


#### Parameters
&nbsp;<dl><dt>message</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_IRemoteAgencyMessage">SecretNest.RemoteAgency.IRemoteAgencyMessage</a><br />Message to be processed.</dd><dt>exception</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">System.Exception</a><br />Exception thrown while running user code.</dd><dt>localExceptionHandlingMode</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_Attributes_LocalExceptionHandlingMode">SecretNest.RemoteAgency.Attributes.LocalExceptionHandlingMode</a><br />Local exception handling mode.</dd></dl>

#### Return Value
Type: <a href="T_SecretNest_RemoteAgency_IRemoteAgencyMessage">IRemoteAgencyMessage</a><br />Message contains the data to be returned.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_IServiceWrapperCommunicate">IServiceWrapperCommunicate Interface</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />