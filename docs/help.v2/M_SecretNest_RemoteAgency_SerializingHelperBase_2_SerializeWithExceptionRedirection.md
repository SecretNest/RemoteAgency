# SerializingHelperBase(*TSerialized*, *TEntityBase*).SerializeWithExceptionRedirection Method 
 

Serializes the entity object with exception redirection.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public virtual TSerialized SerializeWithExceptionRedirection(
	TEntityBase original,
	out Exception serializingException
)
```

**VB**<br />
``` VB
Public Overridable Function SerializeWithExceptionRedirection ( 
	original As TEntityBase,
	<OutAttribute> ByRef serializingException As Exception
) As TSerialized
```

**C++**<br />
``` C++
public:
virtual TSerialized SerializeWithExceptionRedirection(
	TEntityBase original, 
	[OutAttribute] Exception^% serializingException
)
```

**F#**<br />
``` F#
abstract SerializeWithExceptionRedirection : 
        original : 'TEntityBase * 
        serializingException : Exception byref -> 'TSerialized 
override SerializeWithExceptionRedirection : 
        original : 'TEntityBase * 
        serializingException : Exception byref -> 'TSerialized 
```


#### Parameters
&nbsp;<dl><dt>original</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_SerializingHelperBase_2">*TEntityBase*</a><br />The entity object to be serialized.</dd><dt>serializingException</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">System.Exception</a><br />The exception occurred in serializing process.</dd></dl>

#### Return Value
Type: <a href="T_SecretNest_RemoteAgency_SerializingHelperBase_2">*TSerialized*</a><br />Serialized data.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_SerializingHelperBase_2">SerializingHelperBase(TSerialized, TEntityBase) Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />