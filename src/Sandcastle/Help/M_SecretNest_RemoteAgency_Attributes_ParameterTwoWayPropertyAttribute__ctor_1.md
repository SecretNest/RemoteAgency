# ParameterTwoWayPropertyAttribute Constructor (Type, Boolean)
 

Initializes an instance of the ParameterTwoWayPropertyAttribute. <a href="P_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute_IsSimpleMode">IsSimpleMode</a> will be set to `false` (`False` in Visual Basic).

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public ParameterTwoWayPropertyAttribute(
	Type helperClass,
	bool disable = false
)
```

**VB**<br />
``` VB
Public Sub New ( 
	helperClass As Type,
	Optional disable As Boolean = false
)
```

**C++**<br />
``` C++
public:
ParameterTwoWayPropertyAttribute(
	Type^ helperClass, 
	bool disable = false
)
```

**F#**<br />
``` F#
new : 
        helperClass : Type * 
        ?disable : bool 
(* Defaults:
        let _disable = defaultArg disable false
*)
-> ParameterTwoWayPropertyAttribute
```


#### Parameters
&nbsp;<dl><dt>helperClass</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.type" target="_blank">System.Type</a><br />Type of the helper class.</dd><dt>disable (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">System.Boolean</a><br />Disable the function specified with this helper class.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute">ParameterTwoWayPropertyAttribute Class</a><br /><a href="Overload_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute__ctor">ParameterTwoWayPropertyAttribute Overload</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br /><a href="P_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute_HelperClass">ParameterTwoWayPropertyAttribute.HelperClass</a><br />