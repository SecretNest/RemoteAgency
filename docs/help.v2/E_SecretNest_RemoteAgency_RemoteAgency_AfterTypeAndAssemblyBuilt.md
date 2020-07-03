# RemoteAgency.AfterTypeAndAssemblyBuilt Event
 

Occurs when an assembly is built.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public event EventHandler<AfterTypeAndAssemblyBuiltEventArgs> AfterTypeAndAssemblyBuilt
```

**VB**<br />
``` VB
Public Event AfterTypeAndAssemblyBuilt As EventHandler(Of AfterTypeAndAssemblyBuiltEventArgs)
```

**C++**<br />
``` C++
public:
 event EventHandler<AfterTypeAndAssemblyBuiltEventArgs^>^ AfterTypeAndAssemblyBuilt {
	void add (EventHandler<AfterTypeAndAssemblyBuiltEventArgs^>^ value);
	void remove (EventHandler<AfterTypeAndAssemblyBuiltEventArgs^>^ value);
}
```

**F#**<br />
``` F#
member AfterTypeAndAssemblyBuilt : IEvent<EventHandler<AfterTypeAndAssemblyBuiltEventArgs>,
    AfterTypeAndAssemblyBuiltEventArgs>

```


#### Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.eventhandler-1" target="_blank">System.EventHandler</a>(<a href="T_SecretNest_RemoteAgency_AssemblyBuilding_AfterTypeAndAssemblyBuiltEventArgs">AfterTypeAndAssemblyBuiltEventArgs</a>)

## Remarks
The handler of this event can contains the code for saving assembly for further use, aka caching.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />