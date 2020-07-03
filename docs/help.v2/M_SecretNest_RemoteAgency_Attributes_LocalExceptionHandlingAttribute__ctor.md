# LocalExceptionHandlingAttribute Constructor 
 

Initializes an instance of the LocalExceptionHandlingAttribute.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public LocalExceptionHandlingAttribute(
	LocalExceptionHandlingMode mode = LocalExceptionHandlingMode.Throw
)
```

**VB**<br />
``` VB
Public Sub New ( 
	Optional mode As LocalExceptionHandlingMode = LocalExceptionHandlingMode.Throw
)
```

**C++**<br />
``` C++
public:
LocalExceptionHandlingAttribute(
	LocalExceptionHandlingMode mode = LocalExceptionHandlingMode::Throw
)
```

**F#**<br />
``` F#
new : 
        ?mode : LocalExceptionHandlingMode 
(* Defaults:
        let _mode = defaultArg mode LocalExceptionHandlingMode.Throw
*)
-> LocalExceptionHandlingAttribute
```


#### Parameters
&nbsp;<dl><dt>mode (Optional)</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_Attributes_LocalExceptionHandlingMode">SecretNest.RemoteAgency.Attributes.LocalExceptionHandlingMode</a><br />Local exception handling mode.</dd></dl>

## Remarks
The default setting is <a href="P_SecretNest_RemoteAgency_Attributes_LocalExceptionHandlingAttribute_LocalExceptionHandlingMode">LocalExceptionHandlingMode</a>.Suppress if this attribute absents.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_LocalExceptionHandlingAttribute">LocalExceptionHandlingAttribute Class</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />