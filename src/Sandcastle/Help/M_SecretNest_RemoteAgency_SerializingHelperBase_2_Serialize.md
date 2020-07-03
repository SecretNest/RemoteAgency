# SerializingHelperBase(*TSerialized*, *TEntityBase*).Serialize Method 
 

Serializes the entity object.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public abstract TSerialized Serialize(
	TEntityBase original
)
```

**VB**<br />
``` VB
Public MustOverride Function Serialize ( 
	original As TEntityBase
) As TSerialized
```

**C++**<br />
``` C++
public:
virtual TSerialized Serialize(
	TEntityBase original
) abstract
```

**F#**<br />
``` F#
abstract Serialize : 
        original : 'TEntityBase -> 'TSerialized 

```


#### Parameters
&nbsp;<dl><dt>original</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_SerializingHelperBase_2">*TEntityBase*</a><br />The entity object to be serialized.</dd></dl>

#### Return Value
Type: <a href="T_SecretNest_RemoteAgency_SerializingHelperBase_2">*TSerialized*</a><br />Serialized data.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_SerializingHelperBase_2">SerializingHelperBase(TSerialized, TEntityBase) Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />