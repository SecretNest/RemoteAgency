# AssetIgnoredAttribute Constructor 
 

Initializes an instance of AssetIgnoredAttribute.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public AssetIgnoredAttribute(
	bool isIgnored = true,
	bool willThrownException = true
)
```

**VB**<br />
``` VB
Public Sub New ( 
	Optional isIgnored As Boolean = true,
	Optional willThrownException As Boolean = true
)
```

**C++**<br />
``` C++
public:
AssetIgnoredAttribute(
	bool isIgnored = true, 
	bool willThrownException = true
)
```

**F#**<br />
``` F#
new : 
        ?isIgnored : bool * 
        ?willThrownException : bool 
(* Defaults:
        let _isIgnored = defaultArg isIgnored true
        let _willThrownException = defaultArg willThrownException true
*)
-> AssetIgnoredAttribute
```


#### Parameters
&nbsp;<dl><dt>isIgnored (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">System.Boolean</a><br />Whether the asset is ignored. Default value is true.</dd><dt>willThrownException (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">System.Boolean</a><br />Whether an exception should be thrown while accessing this asset. Default value is true.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_AssetIgnoredAttribute">AssetIgnoredAttribute Class</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />