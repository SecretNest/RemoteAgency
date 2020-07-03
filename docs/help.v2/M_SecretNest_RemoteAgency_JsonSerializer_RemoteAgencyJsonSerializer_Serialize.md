# RemoteAgencyJsonSerializer.Serialize Method 
 

Serializes the entity object.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_JsonSerializer">SecretNest.RemoteAgency.JsonSerializer</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public override string Serialize(
	Object original
)
```

**VB**<br />
``` VB
Public Overrides Function Serialize ( 
	original As Object
) As String
```

**C++**<br />
``` C++
public:
virtual String^ Serialize(
	Object^ original
) override
```

**F#**<br />
``` F#
abstract Serialize : 
        original : Object -> string 
override Serialize : 
        original : Object -> string 
```


#### Parameters
&nbsp;<dl><dt>original</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">System.Object</a><br />The entity object to be serialized.</dd></dl>

#### Return Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">String</a><br />Serialized data.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_JsonSerializer_RemoteAgencyJsonSerializer">RemoteAgencyJsonSerializer Class</a><br /><a href="N_SecretNest_RemoteAgency_JsonSerializer">SecretNest.RemoteAgency.JsonSerializer Namespace</a><br />