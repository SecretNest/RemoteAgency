# RemoteAgency.TryGetTaskScheduler Method 
 

Tries to get a task scheduler from the instance of Remote Agency.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public bool TryGetTaskScheduler(
	string name,
	out TaskScheduler taskScheduler
)
```

**VB**<br />
``` VB
Public Function TryGetTaskScheduler ( 
	name As String,
	<OutAttribute> ByRef taskScheduler As TaskScheduler
) As Boolean
```

**C++**<br />
``` C++
public:
bool TryGetTaskScheduler(
	String^ name, 
	[OutAttribute] TaskScheduler^% taskScheduler
)
```

**F#**<br />
``` F#
member TryGetTaskScheduler : 
        name : string * 
        taskScheduler : TaskScheduler byref -> bool 

```


#### Parameters
&nbsp;<dl><dt>name</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Name of the task scheduler.</dd><dt>taskScheduler</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.threading.tasks.taskscheduler" target="_blank">System.Threading.Tasks.TaskScheduler</a><br />Task scheduler.</dd></dl>

#### Return Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">Boolean</a><br />Result

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br /><a href="T_SecretNest_RemoteAgency_Attributes_ThreadLockAttribute">SecretNest.RemoteAgency.Attributes.ThreadLockAttribute</a><br />