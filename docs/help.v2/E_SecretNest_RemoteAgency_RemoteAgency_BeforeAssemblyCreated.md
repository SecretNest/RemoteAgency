# RemoteAgency.BeforeAssemblyCreated Event
 

Occurs before module and assembly building finished.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public event EventHandler<BeforeAssemblyCreatedEventArgs> BeforeAssemblyCreated
```

**VB**<br />
``` VB
Public Event BeforeAssemblyCreated As EventHandler(Of BeforeAssemblyCreatedEventArgs)
```

**C++**<br />
``` C++
public:
 event EventHandler<BeforeAssemblyCreatedEventArgs^>^ BeforeAssemblyCreated {
	void add (EventHandler<BeforeAssemblyCreatedEventArgs^>^ value);
	void remove (EventHandler<BeforeAssemblyCreatedEventArgs^>^ value);
}
```

**F#**<br />
``` F#
member BeforeAssemblyCreated : IEvent<EventHandler<BeforeAssemblyCreatedEventArgs>,
    BeforeAssemblyCreatedEventArgs>

```


#### Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.eventhandler-1" target="_blank">System.EventHandler</a>(<a href="T_SecretNest_RemoteAgency_AssemblyBuilding_BeforeAssemblyCreatedEventArgs">BeforeAssemblyCreatedEventArgs</a>)

## Remarks
Additional code can be added to the type through <a href="P_SecretNest_RemoteAgency_AssemblyBuilding_BeforeAssemblyCreatedEventArgs_ModuleBuilder">ModuleBuilder</a> and <a href="P_SecretNest_RemoteAgency_AssemblyBuilding_BeforeAssemblyCreatedEventArgs_AssemblyBuilder">AssemblyBuilder</a>.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />