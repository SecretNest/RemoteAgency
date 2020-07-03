# RemoteAgency.TryAddSequentialScheduler Method 
 

Tries to add a task scheduler, which run tasks on a single thread, for accessing assets to the instance of Remote Agency.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public bool TryAddSequentialScheduler(
	string name,
	out SequentialScheduler taskScheduler,
	bool waitForThread = false
)
```

**VB**<br />
``` VB
Public Function TryAddSequentialScheduler ( 
	name As String,
	<OutAttribute> ByRef taskScheduler As SequentialScheduler,
	Optional waitForThread As Boolean = false
) As Boolean
```

**C++**<br />
``` C++
public:
bool TryAddSequentialScheduler(
	String^ name, 
	[OutAttribute] SequentialScheduler^% taskScheduler, 
	bool waitForThread = false
)
```

**F#**<br />
``` F#
member TryAddSequentialScheduler : 
        name : string * 
        taskScheduler : SequentialScheduler byref * 
        ?waitForThread : bool 
(* Defaults:
        let _waitForThread = defaultArg waitForThread false
*)
-> bool 

```


#### Parameters
&nbsp;<dl><dt>name</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Name of the task scheduler.</dd><dt>taskScheduler</dt><dd>Type: SequentialScheduler<br />Created task scheduler.</dd><dt>waitForThread (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">System.Boolean</a><br />Waiting for SequentialScheduler.Run() to provide thread. Default is `false` (`False` in Visual Basic).</dd></dl>

#### Return Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">Boolean</a><br />Result

## Remarks

When initializing with *waitForThread* set to `false` (`False` in Visual Basic), a free thread is created for this scheduler.

When initializing with *waitForThread* set to `true` (`True` in Visual Basic), SequentialScheduler.Run() should be called from the thread which intends to be used for this scheduler before processing required by any interface.

For details please refer to <a href="https://github.com/SecretNest/SequentialScheduler/" target="_blank">https://github.com/SecretNest/SequentialScheduler/</a>.

This event is not present in Neat release.


## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br /><a href="T_SecretNest_RemoteAgency_Attributes_ThreadLockAttribute">SecretNest.RemoteAgency.Attributes.ThreadLockAttribute</a><br />