# BeforeMessageProcessingEventArgs(*TSerialized*, *TEntityBase*).MessageBody Property 
 

Gets the message body.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public TEntityBase MessageBody { get; }
```

**VB**<br />
``` VB
Public ReadOnly Property MessageBody As TEntityBase
	Get
```

**C++**<br />
``` C++
public:
virtual property TEntityBase MessageBody {
	TEntityBase get () sealed;
}
```

**F#**<br />
``` F#
abstract MessageBody : 'TEntityBase with get
override MessageBody : 'TEntityBase with get
```


#### Property Value
Type: <a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgs_2">*TEntityBase*</a>

#### Implements
<a href="P_SecretNest_RemoteAgency_IMessageBodyGenericEventArgs_2_MessageBody">IMessageBodyGenericEventArgs(TSerialized, TEntityBase).MessageBody</a><br />

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgs_2">BeforeMessageProcessingEventArgs(TSerialized, TEntityBase) Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />