# RemoteAgencyJsonSerializer.Deserialize Method 
 

Deserializes the data to the original format.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_JsonSerializer">SecretNest.RemoteAgency.JsonSerializer</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public override Object Deserialize(
	string serialized
)
```

**VB**<br />
``` VB
Public Overrides Function Deserialize ( 
	serialized As String
) As Object
```

**C++**<br />
``` C++
public:
virtual Object^ Deserialize(
	String^ serialized
) override
```

**F#**<br />
``` F#
abstract Deserialize : 
        serialized : string -> Object 
override Deserialize : 
        serialized : string -> Object 
```


#### Parameters
&nbsp;<dl><dt>serialized</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />The serialized data to be deserialized.</dd></dl>

#### Return Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a><br />Entity object.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_JsonSerializer_RemoteAgencyJsonSerializer">RemoteAgencyJsonSerializer Class</a><br /><a href="N_SecretNest_RemoteAgency_JsonSerializer">SecretNest.RemoteAgency.JsonSerializer Namespace</a><br />