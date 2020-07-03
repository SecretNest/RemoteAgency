# EntityCodeBuilderBase.CreateEmptyMessage Method 
 

Creates an empty message which is allowed to be serialized.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public abstract IRemoteAgencyMessage CreateEmptyMessage()
```

**VB**<br />
``` VB
Public MustOverride Function CreateEmptyMessage As IRemoteAgencyMessage
```

**C++**<br />
``` C++
public:
virtual IRemoteAgencyMessage^ CreateEmptyMessage() abstract
```

**F#**<br />
``` F#
abstract CreateEmptyMessage : unit -> IRemoteAgencyMessage 

```


#### Return Value
Type: <a href="T_SecretNest_RemoteAgency_IRemoteAgencyMessage">IRemoteAgencyMessage</a><br />Empty message.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_EntityCodeBuilderBase">EntityCodeBuilderBase Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />