# SerializingHelperBase(*TSerialized*, *TEntityBase*).DeserializeWithExceptionTolerance Method 
 

Deserializes the data to the original format with exception redirection.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public virtual TEntityBase DeserializeWithExceptionTolerance(
	TSerialized serialized,
	out Exception deserializingException
)
```

**VB**<br />
``` VB
Public Overridable Function DeserializeWithExceptionTolerance ( 
	serialized As TSerialized,
	<OutAttribute> ByRef deserializingException As Exception
) As TEntityBase
```

**C++**<br />
``` C++
public:
virtual TEntityBase DeserializeWithExceptionTolerance(
	TSerialized serialized, 
	[OutAttribute] Exception^% deserializingException
)
```

**F#**<br />
``` F#
abstract DeserializeWithExceptionTolerance : 
        serialized : 'TSerialized * 
        deserializingException : Exception byref -> 'TEntityBase 
override DeserializeWithExceptionTolerance : 
        serialized : 'TSerialized * 
        deserializingException : Exception byref -> 'TEntityBase 
```


#### Parameters
&nbsp;<dl><dt>serialized</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_SerializingHelperBase_2">*TSerialized*</a><br />The serialized data to be deserialized.</dd><dt>deserializingException</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">System.Exception</a><br />The exception occurred in deserializing process.</dd></dl>

#### Return Value
Type: <a href="T_SecretNest_RemoteAgency_SerializingHelperBase_2">*TEntityBase*</a><br />Entity object.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_SerializingHelperBase_2">SerializingHelperBase(TSerialized, TEntityBase) Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />