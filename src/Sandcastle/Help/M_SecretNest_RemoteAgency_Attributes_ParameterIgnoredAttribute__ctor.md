# ParameterIgnoredAttribute Constructor 
 

Initializes an instance of the ParameterIgnoredAttribute.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public ParameterIgnoredAttribute(
	bool isIgnored = true
)
```

**VB**<br />
``` VB
Public Sub New ( 
	Optional isIgnored As Boolean = true
)
```

**C++**<br />
``` C++
public:
ParameterIgnoredAttribute(
	bool isIgnored = true
)
```

**F#**<br />
``` F#
new : 
        ?isIgnored : bool 
(* Defaults:
        let _isIgnored = defaultArg isIgnored true
*)
-> ParameterIgnoredAttribute
```


#### Parameters
&nbsp;<dl><dt>isIgnored (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">System.Boolean</a><br />Ignored from parameter. If set to true, this parameter should not be transferred to remote site.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_ParameterIgnoredAttribute">ParameterIgnoredAttribute Class</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />