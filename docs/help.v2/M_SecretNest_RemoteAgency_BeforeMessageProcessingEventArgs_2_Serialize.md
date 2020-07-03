# BeforeMessageProcessingEventArgs(*TSerialized*, *TEntityBase*).Serialize Method 
 

Serializes this message using the serializer from the Remote Agency instance.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public TSerialized Serialize()
```

**VB**<br />
``` VB
Public Function Serialize As TSerialized
```

**C++**<br />
``` C++
public:
virtual TSerialized Serialize() sealed
```

**F#**<br />
``` F#
abstract Serialize : unit -> 'TSerialized 
override Serialize : unit -> 'TSerialized 
```


#### Return Value
Type: <a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgs_2">*TSerialized*</a><br />Serialized data.

#### Implements
<a href="M_SecretNest_RemoteAgency_IMessageBodyGenericEventArgs_2_Serialize">IMessageBodyGenericEventArgs(TSerialized, TEntityBase).Serialize()</a><br />

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgs_2">BeforeMessageProcessingEventArgs(TSerialized, TEntityBase) Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />