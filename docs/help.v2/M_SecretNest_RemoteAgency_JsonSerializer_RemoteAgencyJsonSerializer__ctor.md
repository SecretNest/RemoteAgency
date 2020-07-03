# RemoteAgencyJsonSerializer Constructor 
 

Initializes an instance of RemoteAgencyJsonSerializer.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_JsonSerializer">SecretNest.RemoteAgency.JsonSerializer</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public RemoteAgencyJsonSerializer(
	bool intented = false,
	bool includingFullAssemblyName = true
)
```

**VB**<br />
``` VB
Public Sub New ( 
	Optional intented As Boolean = false,
	Optional includingFullAssemblyName As Boolean = true
)
```

**C++**<br />
``` C++
public:
RemoteAgencyJsonSerializer(
	bool intented = false, 
	bool includingFullAssemblyName = true
)
```

**F#**<br />
``` F#
new : 
        ?intented : bool * 
        ?includingFullAssemblyName : bool 
(* Defaults:
        let _intented = defaultArg intented false
        let _includingFullAssemblyName = defaultArg includingFullAssemblyName true
*)
-> RemoteAgencyJsonSerializer
```


#### Parameters
&nbsp;<dl><dt>intented (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">System.Boolean</a><br />Whether should cause child objects to be indented according to the Indentation and IndentChar settings. Default value is false.</dd><dt>includingFullAssemblyName (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">System.Boolean</a><br />Whether should include full assembly name in serialized data and use 
```
Load
```
 method of the <a href="https://docs.microsoft.com/dotnet/api/system.reflection.assembly" target="_blank">Assembly</a> class is used to load the assembly instead of using 
```
Load
```
 method. Default value is true</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_JsonSerializer_RemoteAgencyJsonSerializer">RemoteAgencyJsonSerializer Class</a><br /><a href="N_SecretNest_RemoteAgency_JsonSerializer">SecretNest.RemoteAgency.JsonSerializer Namespace</a><br />