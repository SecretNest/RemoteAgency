# RemoteAgencyBinarySerializer.Deserialize Method 
 

Deserializes the data to the original format.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_BinarySerializer">SecretNest.RemoteAgency.BinarySerializer</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public override Object Deserialize(
	byte[] serialized
)
```

**VB**<br />
``` VB
Public Overrides Function Deserialize ( 
	serialized As Byte()
) As Object
```

**C++**<br />
``` C++
public:
virtual Object^ Deserialize(
	array<unsigned char>^ serialized
) override
```

**F#**<br />
``` F#
abstract Deserialize : 
        serialized : byte[] -> Object 
override Deserialize : 
        serialized : byte[] -> Object 
```


#### Parameters
&nbsp;<dl><dt>serialized</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.byte" target="_blank">System.Byte</a>[]<br />The serialized data to be deserialized.</dd></dl>

#### Return Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a><br />Entity object.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_BinarySerializer_RemoteAgencyBinarySerializer">RemoteAgencyBinarySerializer Class</a><br /><a href="N_SecretNest_RemoteAgency_BinarySerializer">SecretNest.RemoteAgency.BinarySerializer Namespace</a><br />