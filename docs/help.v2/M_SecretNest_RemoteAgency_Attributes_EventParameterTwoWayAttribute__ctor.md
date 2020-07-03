# EventParameterTwoWayAttribute Constructor 
 

Initializes an instance of EventParameterTwoWayAttribute.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public EventParameterTwoWayAttribute(
	string parameterName,
	bool isTwoWay = true,
	bool isIncludedWhenExceptionThrown = true,
	string responseEntityPropertyName = null
)
```

**VB**<br />
``` VB
Public Sub New ( 
	parameterName As String,
	Optional isTwoWay As Boolean = true,
	Optional isIncludedWhenExceptionThrown As Boolean = true,
	Optional responseEntityPropertyName As String = Nothing
)
```

**C++**<br />
``` C++
public:
EventParameterTwoWayAttribute(
	String^ parameterName, 
	bool isTwoWay = true, 
	bool isIncludedWhenExceptionThrown = true, 
	String^ responseEntityPropertyName = nullptr
)
```

**F#**<br />
``` F#
new : 
        parameterName : string * 
        ?isTwoWay : bool * 
        ?isIncludedWhenExceptionThrown : bool * 
        ?responseEntityPropertyName : string 
(* Defaults:
        let _isTwoWay = defaultArg isTwoWay true
        let _isIncludedWhenExceptionThrown = defaultArg isIncludedWhenExceptionThrown true
        let _responseEntityPropertyName = defaultArg responseEntityPropertyName null
*)
-> EventParameterTwoWayAttribute
```


#### Parameters
&nbsp;<dl><dt>parameterName</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Parameter name of the event.</dd><dt>isTwoWay (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">System.Boolean</a><br />Whether this parameter should be included in return entity. Default value is `true` (`True` in Visual Basic).</dd><dt>isIncludedWhenExceptionThrown (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">System.Boolean</a><br />Whether this parameter should be included in return entity when exception thrown by the user code on the remote site. Default value is `true` (`True` in Visual Basic).</dd><dt>responseEntityPropertyName (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Preferred property name in response entity. When the value is a null reference (`Nothing` in Visual Basic) or empty string, name is chosen automatically. Default value is a null reference (`Nothing` in Visual Basic).</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_EventParameterTwoWayAttribute">EventParameterTwoWayAttribute Class</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />