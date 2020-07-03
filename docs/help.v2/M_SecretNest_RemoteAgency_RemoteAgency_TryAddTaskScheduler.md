# RemoteAgency.TryAddTaskScheduler Method 
 

Tries to add a task scheduler for accessing assets to the instance of Remote Agency.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public bool TryAddTaskScheduler(
	string name,
	TaskScheduler taskScheduler
)
```

**VB**<br />
``` VB
Public Function TryAddTaskScheduler ( 
	name As String,
	taskScheduler As TaskScheduler
) As Boolean
```

**C++**<br />
``` C++
public:
bool TryAddTaskScheduler(
	String^ name, 
	TaskScheduler^ taskScheduler
)
```

**F#**<br />
``` F#
member TryAddTaskScheduler : 
        name : string * 
        taskScheduler : TaskScheduler -> bool 

```


#### Parameters
&nbsp;<dl><dt>name</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Name of the task scheduler.</dd><dt>taskScheduler</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.threading.tasks.taskscheduler" target="_blank">System.Threading.Tasks.TaskScheduler</a><br />Task scheduler.</dd></dl>

#### Return Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">Boolean</a><br />Result

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br /><a href="T_SecretNest_RemoteAgency_Attributes_ThreadLockAttribute">SecretNest.RemoteAgency.Attributes.ThreadLockAttribute</a><br />