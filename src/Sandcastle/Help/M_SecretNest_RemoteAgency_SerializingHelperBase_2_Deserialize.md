# SerializingHelperBase(*TSerialized*, *TEntityBase*).Deserialize Method 
 

Deserializes the data to the original format.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public abstract TEntityBase Deserialize(
	TSerialized serialized
)
```

**VB**<br />
``` VB
Public MustOverride Function Deserialize ( 
	serialized As TSerialized
) As TEntityBase
```

**C++**<br />
``` C++
public:
virtual TEntityBase Deserialize(
	TSerialized serialized
) abstract
```

**F#**<br />
``` F#
abstract Deserialize : 
        serialized : 'TSerialized -> 'TEntityBase 

```


#### Parameters
&nbsp;<dl><dt>serialized</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_SerializingHelperBase_2">*TSerialized*</a><br />The serialized data to be deserialized.</dd></dl>

#### Return Value
Type: <a href="T_SecretNest_RemoteAgency_SerializingHelperBase_2">*TEntityBase*</a><br />Entity object.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_SerializingHelperBase_2">SerializingHelperBase(TSerialized, TEntityBase) Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />