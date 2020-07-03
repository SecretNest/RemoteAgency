# RemoteAgency.TryRemoveTaskScheduler Method 
 

Tries to remove a task scheduler from the instance of Remote Agency.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public bool TryRemoveTaskScheduler(
	string name,
	out TaskScheduler removed
)
```

**VB**<br />
``` VB
Public Function TryRemoveTaskScheduler ( 
	name As String,
	<OutAttribute> ByRef removed As TaskScheduler
) As Boolean
```

**C++**<br />
``` C++
public:
bool TryRemoveTaskScheduler(
	String^ name, 
	[OutAttribute] TaskScheduler^% removed
)
```

**F#**<br />
``` F#
member TryRemoveTaskScheduler : 
        name : string * 
        removed : TaskScheduler byref -> bool 

```


#### Parameters
&nbsp;<dl><dt>name</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Name of the task scheduler.</dd><dt>removed</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.threading.tasks.taskscheduler" target="_blank">System.Threading.Tasks.TaskScheduler</a><br />Removed task scheduler.</dd></dl>

#### Return Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">Boolean</a><br />Result

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br /><a href="T_SecretNest_RemoteAgency_Attributes_ThreadLockAttribute">SecretNest.RemoteAgency.Attributes.ThreadLockAttribute</a><br />