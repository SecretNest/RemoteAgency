# EventParameterTwoWayPropertyAttribute Constructor (String, String, String, Boolean, Boolean)
 

Initializes an instance of the EventParameterTwoWayPropertyAttribute. <a href="P_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute_IsSimpleMode">IsSimpleMode</a> will be set to `true` (`True` in Visual Basic).

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public EventParameterTwoWayPropertyAttribute(
	string parameterName,
	string propertyNameInParameter,
	string responseEntityPropertyName = null,
	bool isIncludedWhenExceptionThrown = false,
	bool disable = false
)
```

**VB**<br />
``` VB
Public Sub New ( 
	parameterName As String,
	propertyNameInParameter As String,
	Optional responseEntityPropertyName As String = Nothing,
	Optional isIncludedWhenExceptionThrown As Boolean = false,
	Optional disable As Boolean = false
)
```

**C++**<br />
``` C++
public:
EventParameterTwoWayPropertyAttribute(
	String^ parameterName, 
	String^ propertyNameInParameter, 
	String^ responseEntityPropertyName = nullptr, 
	bool isIncludedWhenExceptionThrown = false, 
	bool disable = false
)
```

**F#**<br />
``` F#
new : 
        parameterName : string * 
        propertyNameInParameter : string * 
        ?responseEntityPropertyName : string * 
        ?isIncludedWhenExceptionThrown : bool * 
        ?disable : bool 
(* Defaults:
        let _responseEntityPropertyName = defaultArg responseEntityPropertyName null
        let _isIncludedWhenExceptionThrown = defaultArg isIncludedWhenExceptionThrown false
        let _disable = defaultArg disable false
*)
-> EventParameterTwoWayPropertyAttribute
```


#### Parameters
&nbsp;<dl><dt>parameterName</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Parameter name of the event.</dd><dt>propertyNameInParameter</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />The name of property or field of the parameter entity.</dd><dt>responseEntityPropertyName (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Preferred property name in response entity. When the value is a null reference (`Nothing` in Visual Basic) or empty string, name is chosen automatically. Default value is a null reference (`Nothing` in Visual Basic).</dd><dt>isIncludedWhenExceptionThrown (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">System.Boolean</a><br />Whether this property should be included in return entity when exception thrown by the user code on the remote site. Default value is `false` (`False` in Visual Basic).</dd><dt>disable (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">System.Boolean</a><br />Disable the function specified with this property or field.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_EventParameterTwoWayPropertyAttribute">EventParameterTwoWayPropertyAttribute Class</a><br /><a href="Overload_SecretNest_RemoteAgency_Attributes_EventParameterTwoWayPropertyAttribute__ctor">EventParameterTwoWayPropertyAttribute Overload</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />