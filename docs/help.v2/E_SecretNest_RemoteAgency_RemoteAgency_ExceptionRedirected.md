# RemoteAgency.ExceptionRedirected Event
 

Occurs when an exception thrown from user code.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public event EventHandler<ExceptionRedirectedEventArgs> ExceptionRedirected
```

**VB**<br />
``` VB
Public Event ExceptionRedirected As EventHandler(Of ExceptionRedirectedEventArgs)
```

**C++**<br />
``` C++
public:
 event EventHandler<ExceptionRedirectedEventArgs^>^ ExceptionRedirected {
	void add (EventHandler<ExceptionRedirectedEventArgs^>^ value);
	void remove (EventHandler<ExceptionRedirectedEventArgs^>^ value);
}
```

**F#**<br />
``` F#
member ExceptionRedirected : IEvent<EventHandler<ExceptionRedirectedEventArgs>,
    ExceptionRedirectedEventArgs>

```


#### Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.eventhandler-1" target="_blank">System.EventHandler</a>(<a href="T_SecretNest_RemoteAgency_ExceptionRedirectedEventArgs">ExceptionRedirectedEventArgs</a>)

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br /><a href="T_SecretNest_RemoteAgency_Attributes_LocalExceptionHandlingAttribute">SecretNest.RemoteAgency.Attributes.LocalExceptionHandlingAttribute</a><br />