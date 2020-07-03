# RemoteAgencyBinarySerializer.Serialize Method 
 

Serializes the entity object.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_BinarySerializer">SecretNest.RemoteAgency.BinarySerializer</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public override byte[] Serialize(
	Object original
)
```

**VB**<br />
``` VB
Public Overrides Function Serialize ( 
	original As Object
) As Byte()
```

**C++**<br />
``` C++
public:
virtual array<unsigned char>^ Serialize(
	Object^ original
) override
```

**F#**<br />
``` F#
abstract Serialize : 
        original : Object -> byte[] 
override Serialize : 
        original : Object -> byte[] 
```


#### Parameters
&nbsp;<dl><dt>original</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">System.Object</a><br />The entity object to be serialized.</dd></dl>

#### Return Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.byte" target="_blank">Byte</a>[]<br />Serialized data.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_BinarySerializer_RemoteAgencyBinarySerializer">RemoteAgencyBinarySerializer Class</a><br /><a href="N_SecretNest_RemoteAgency_BinarySerializer">SecretNest.RemoteAgency.BinarySerializer Namespace</a><br />