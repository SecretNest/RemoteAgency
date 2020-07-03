# RemoteAgency(*TSerialized*, *TEntityBase*).MessageForSendingPrepared Event
 

Occurs when a message is generated and ready to be sent.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public event EventHandler<MessageBodyEventArgs<TSerialized, TEntityBase>> MessageForSendingPrepared
```

**VB**<br />
``` VB
Public Event MessageForSendingPrepared As EventHandler(Of MessageBodyEventArgs(Of TSerialized, TEntityBase))
```

**C++**<br />
``` C++
public:
 event EventHandler<MessageBodyEventArgs<TSerialized, TEntityBase>^>^ MessageForSendingPrepared {
	void add (EventHandler<MessageBodyEventArgs<TSerialized, TEntityBase>^>^ value);
	void remove (EventHandler<MessageBodyEventArgs<TSerialized, TEntityBase>^>^ value);
}
```

**F#**<br />
``` F#
member MessageForSendingPrepared : IEvent<EventHandler<MessageBodyEventArgs<'TSerialized, 'TEntityBase>>,
    MessageBodyEventArgs<'TSerialized, 'TEntityBase>>

```


#### Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.eventhandler-1" target="_blank">System.EventHandler</a>(<a href="T_SecretNest_RemoteAgency_MessageBodyEventArgs_2">MessageBodyEventArgs</a>(<a href="T_SecretNest_RemoteAgency_RemoteAgency_2">*TSerialized*</a>, <a href="T_SecretNest_RemoteAgency_RemoteAgency_2">*TEntityBase*</a>))

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency_2">RemoteAgency(TSerialized, TEntityBase) Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />