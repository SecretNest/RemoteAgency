# ParameterTwoWayPropertyAttribute.IsSimpleMode Property 
 

Gets whether this is in simple mode.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public bool IsSimpleMode { get; }
```

**VB**<br />
``` VB
Public ReadOnly Property IsSimpleMode As Boolean
	Get
```

**C++**<br />
``` C++
public:
property bool IsSimpleMode {
	bool get ();
}
```

**F#**<br />
``` F#
member IsSimpleMode : bool with get

```


#### Property Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">Boolean</a>

## Remarks
When the value is `true` (`True` in Visual Basic), property specified by <a href="P_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute_PropertyNameInParameter">PropertyNameInParameter</a> need to be sent back to the caller; when the value is `false` (`False` in Visual Basic), properties marked with <a href="T_SecretNest_RemoteAgency_Attributes_TwoWayHelperAttribute">TwoWayHelperAttribute</a> and <a href="P_SecretNest_RemoteAgency_Attributes_TwoWayHelperAttribute_IsTwoWay">IsTwoWay</a> set to `true` (`True` in Visual Basic) are used as the helper fow two way property accessing.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute">ParameterTwoWayPropertyAttribute Class</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />