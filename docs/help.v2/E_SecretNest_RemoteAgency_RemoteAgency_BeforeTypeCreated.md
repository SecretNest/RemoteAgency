# RemoteAgency.BeforeTypeCreated Event
 

Occurs before type building finished.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public event EventHandler<BeforeTypeCreatedEventArgs> BeforeTypeCreated
```

**VB**<br />
``` VB
Public Event BeforeTypeCreated As EventHandler(Of BeforeTypeCreatedEventArgs)
```

**C++**<br />
``` C++
public:
 event EventHandler<BeforeTypeCreatedEventArgs^>^ BeforeTypeCreated {
	void add (EventHandler<BeforeTypeCreatedEventArgs^>^ value);
	void remove (EventHandler<BeforeTypeCreatedEventArgs^>^ value);
}
```

**F#**<br />
``` F#
member BeforeTypeCreated : IEvent<EventHandler<BeforeTypeCreatedEventArgs>,
    BeforeTypeCreatedEventArgs>

```


#### Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.eventhandler-1" target="_blank">System.EventHandler</a>(<a href="T_SecretNest_RemoteAgency_AssemblyBuilding_BeforeTypeCreatedEventArgs">BeforeTypeCreatedEventArgs</a>)

## Remarks
Additional code can be added to the type through <a href="P_SecretNest_RemoteAgency_AssemblyBuilding_BeforeTypeCreatedEventArgs_TypeBuilder">TypeBuilder</a>.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />