# ThreadLockAttribute.TaskSchedulerName Property 
 

Gets the name of the task scheduler to be used while accessing assets within. Only valid when <a href="P_SecretNest_RemoteAgency_Attributes_ThreadLockAttribute_ThreadLockMode">ThreadLockMode</a> is set to <a href="T_SecretNest_RemoteAgency_Attributes_ThreadLockMode">TaskSchedulerSpecified</a>.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public string TaskSchedulerName { get; }
```

**VB**<br />
``` VB
Public ReadOnly Property TaskSchedulerName As String
	Get
```

**C++**<br />
``` C++
public:
property String^ TaskSchedulerName {
	String^ get ();
}
```

**F#**<br />
``` F#
member TaskSchedulerName : string with get

```


#### Property Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">String</a>

## Remarks
One task scheduler with the same name should be added to the instance of Remote Agency before processing the interface related.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_ThreadLockAttribute">ThreadLockAttribute Class</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />