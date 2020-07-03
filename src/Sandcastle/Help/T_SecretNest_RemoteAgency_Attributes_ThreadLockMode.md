# ThreadLockMode Enumeration
 

Thread choosing for accessing assets.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public enum ThreadLockMode
```

**VB**<br />
``` VB
Public Enumeration ThreadLockMode
```

**C++**<br />
``` C++
public enum class ThreadLockMode
```

**F#**<br />
``` F#
type ThreadLockMode
```


## Members
&nbsp;<table><tr><th></th><th>Member name</th><th>Value</th><th>Description</th></tr><tr><td /><td target="F:SecretNest.RemoteAgency.Attributes.ThreadLockMode.None">**None**</td><td>0</td><td>Not specified. The same thread of sending message to Remote Agency will be used to access asset specified.</td></tr><tr><td /><td target="F:SecretNest.RemoteAgency.Attributes.ThreadLockMode.SynchronizationContext">**SynchronizationContext**</td><td>1</td><td>Always use SynchronizationContext to access assets within this object.</td></tr><tr><td /><td target="F:SecretNest.RemoteAgency.Attributes.ThreadLockMode.AnyButSameThread">**AnyButSameThread**</td><td>2</td><td>Always use one thread to access assets within this object.</td></tr><tr><td /><td target="F:SecretNest.RemoteAgency.Attributes.ThreadLockMode.TaskSchedulerSpecified">**TaskSchedulerSpecified**</td><td>3</td><td>Always use the TaskScheduler specified by name to access assets within this object.</td></tr></table>

## See Also


#### Reference
<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />