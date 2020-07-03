# BeforeMessageProcessingEventArgs(*TSerialized*, *TEntityBase*) Constructor 
 

Initializes an instance of BeforeMessageProcessingEventArgs.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public BeforeMessageProcessingEventArgs(
	MessageDirection messageDirection,
	TEntityBase messageBody,
	Func<TEntityBase, TSerialized> serializerCallback
)
```

**VB**<br />
``` VB
Public Sub New ( 
	messageDirection As MessageDirection,
	messageBody As TEntityBase,
	serializerCallback As Func(Of TEntityBase, TSerialized)
)
```

**C++**<br />
``` C++
public:
BeforeMessageProcessingEventArgs(
	MessageDirection messageDirection, 
	TEntityBase messageBody, 
	Func<TEntityBase, TSerialized>^ serializerCallback
)
```

**F#**<br />
``` F#
new : 
        messageDirection : MessageDirection * 
        messageBody : 'TEntityBase * 
        serializerCallback : Func<'TEntityBase, 'TSerialized> -> BeforeMessageProcessingEventArgs
```


#### Parameters
&nbsp;<dl><dt>messageDirection</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_MessageDirection">SecretNest.RemoteAgency.MessageDirection</a><br />Direction of the message.</dd><dt>messageBody</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgs_2">*TEntityBase*</a><br />Message.</dd><dt>serializerCallback</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.func-2" target="_blank">System.Func</a>(<a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgs_2">*TEntityBase*</a>, <a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgs_2">*TSerialized*</a>)<br />Callback for serializing message body.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgs_2">BeforeMessageProcessingEventArgs(TSerialized, TEntityBase) Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />