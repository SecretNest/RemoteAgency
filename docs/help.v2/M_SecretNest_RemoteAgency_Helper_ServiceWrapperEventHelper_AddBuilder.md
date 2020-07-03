# ServiceWrapperEventHelper.AddBuilder Method 
 

Adds a builder callback.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Helper">SecretNest.RemoteAgency.Helper</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public void AddBuilder(
	string assetName,
	Func<Object, EventRouterBase> callback
)
```

**VB**<br />
``` VB
Public Sub AddBuilder ( 
	assetName As String,
	callback As Func(Of Object, EventRouterBase)
)
```

**C++**<br />
``` C++
public:
void AddBuilder(
	String^ assetName, 
	Func<Object^, EventRouterBase^>^ callback
)
```

**F#**<br />
``` F#
member AddBuilder : 
        assetName : string * 
        callback : Func<Object, EventRouterBase> -> unit 

```


#### Parameters
&nbsp;<dl><dt>assetName</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Name of the event.</dd><dt>callback</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.func-2" target="_blank">System.Func</a>(<a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>, <a href="T_SecretNest_RemoteAgency_Helper_EventRouterBase">EventRouterBase</a>)<br />Callback for creating an instance of a derived class of EventRouterBase.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Helper_ServiceWrapperEventHelper">ServiceWrapperEventHelper Class</a><br /><a href="N_SecretNest_RemoteAgency_Helper">SecretNest.RemoteAgency.Helper Namespace</a><br />