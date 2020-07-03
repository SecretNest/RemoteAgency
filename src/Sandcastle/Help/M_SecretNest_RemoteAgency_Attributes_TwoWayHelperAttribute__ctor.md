# TwoWayHelperAttribute Constructor 
 

Initializes an instance of TwoWayHelperAttribute.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public TwoWayHelperAttribute(
	bool isTwoWay = true,
	string responseEntityPropertyName = null,
	bool isIncludedWhenExceptionThrown = false
)
```

**VB**<br />
``` VB
Public Sub New ( 
	Optional isTwoWay As Boolean = true,
	Optional responseEntityPropertyName As String = Nothing,
	Optional isIncludedWhenExceptionThrown As Boolean = false
)
```

**C++**<br />
``` C++
public:
TwoWayHelperAttribute(
	bool isTwoWay = true, 
	String^ responseEntityPropertyName = nullptr, 
	bool isIncludedWhenExceptionThrown = false
)
```

**F#**<br />
``` F#
new : 
        ?isTwoWay : bool * 
        ?responseEntityPropertyName : string * 
        ?isIncludedWhenExceptionThrown : bool 
(* Defaults:
        let _isTwoWay = defaultArg isTwoWay true
        let _responseEntityPropertyName = defaultArg responseEntityPropertyName null
        let _isIncludedWhenExceptionThrown = defaultArg isIncludedWhenExceptionThrown false
*)
-> TwoWayHelperAttribute
```


#### Parameters
&nbsp;<dl><dt>isTwoWay (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">System.Boolean</a><br />Whether this parameter should be included in return entity. Default value is `true` (`True` in Visual Basic).</dd><dt>responseEntityPropertyName (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Preferred property name in response entity. When the value is a null reference (`Nothing` in Visual Basic) or empty string, name is chosen automatically.</dd><dt>isIncludedWhenExceptionThrown (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">System.Boolean</a><br />Whether this property should be included in return entity when exception thrown by the user code on the remote site. Default value is `false` (`False` in Visual Basic).</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_TwoWayHelperAttribute">TwoWayHelperAttribute Class</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />