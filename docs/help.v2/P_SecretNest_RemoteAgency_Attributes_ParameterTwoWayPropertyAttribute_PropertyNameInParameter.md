# ParameterTwoWayPropertyAttribute.PropertyNameInParameter Property 
 

Gets the property name in the parameter which need to be sent back to the caller.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public string PropertyNameInParameter { get; }
```

**VB**<br />
``` VB
Public ReadOnly Property PropertyNameInParameter As String
	Get
```

**C++**<br />
``` C++
public:
property String^ PropertyNameInParameter {
	String^ get ();
}
```

**F#**<br />
``` F#
member PropertyNameInParameter : string with get

```


#### Property Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">String</a>

## Remarks
Only valid when <a href="P_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute_IsSimpleMode">IsSimpleMode</a> is set to `true` (`True` in Visual Basic).

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute">ParameterTwoWayPropertyAttribute Class</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />