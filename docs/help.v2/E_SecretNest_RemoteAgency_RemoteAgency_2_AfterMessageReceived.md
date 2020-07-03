# RemoteAgency(*TSerialized*, *TEntityBase*).AfterMessageReceived Event
 

Occurs when a message need to be checked for sending.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public event EventHandler<BeforeMessageProcessingEventArgs<TSerialized, TEntityBase>> AfterMessageReceived
```

**VB**<br />
``` VB
Public Event AfterMessageReceived As EventHandler(Of BeforeMessageProcessingEventArgs(Of TSerialized, TEntityBase))
```

**C++**<br />
``` C++
public:
 event EventHandler<BeforeMessageProcessingEventArgs<TSerialized, TEntityBase>^>^ AfterMessageReceived {
	void add (EventHandler<BeforeMessageProcessingEventArgs<TSerialized, TEntityBase>^>^ value);
	void remove (EventHandler<BeforeMessageProcessingEventArgs<TSerialized, TEntityBase>^>^ value);
}
```

**F#**<br />
``` F#
member AfterMessageReceived : IEvent<EventHandler<BeforeMessageProcessingEventArgs<'TSerialized, 'TEntityBase>>,
    BeforeMessageProcessingEventArgs<'TSerialized, 'TEntityBase>>

```


#### Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.eventhandler-1" target="_blank">System.EventHandler</a>(<a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgs_2">BeforeMessageProcessingEventArgs</a>(<a href="T_SecretNest_RemoteAgency_RemoteAgency_2">*TSerialized*</a>, <a href="T_SecretNest_RemoteAgency_RemoteAgency_2">*TEntityBase*</a>))

## Remarks
Internal routing message, which is sent from object managed by the same instance of Remote Agency, never raise this event.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency_2">RemoteAgency(TSerialized, TEntityBase) Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br /><a href="T_SecretNest_RemoteAgency_MessageProcessTerminatedException">SecretNest.RemoteAgency.MessageProcessTerminatedException</a><br />