# EventParameterIgnoredAttribute Constructor 
 

Initializes an instance of the EventParameterIgnoredAttribute.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public EventParameterIgnoredAttribute(
	string parameterName,
	bool isIgnored = true
)
```

**VB**<br />
``` VB
Public Sub New ( 
	parameterName As String,
	Optional isIgnored As Boolean = true
)
```

**C++**<br />
``` C++
public:
EventParameterIgnoredAttribute(
	String^ parameterName, 
	bool isIgnored = true
)
```

**F#**<br />
``` F#
new : 
        parameterName : string * 
        ?isIgnored : bool 
(* Defaults:
        let _isIgnored = defaultArg isIgnored true
*)
-> EventParameterIgnoredAttribute
```


#### Parameters
&nbsp;<dl><dt>parameterName</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Parameter name of the event.</dd><dt>isIgnored (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">System.Boolean</a><br />Ignored from parameter. If set to true, this parameter should not be transferred to remote site.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_EventParameterIgnoredAttribute">EventParameterIgnoredAttribute Class</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />