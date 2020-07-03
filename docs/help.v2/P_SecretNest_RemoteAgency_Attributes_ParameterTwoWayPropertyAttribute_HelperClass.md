# ParameterTwoWayPropertyAttribute.HelperClass Property 
 

Gets the type of the helper class.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public Type HelperClass { get; }
```

**VB**<br />
``` VB
Public ReadOnly Property HelperClass As Type
	Get
```

**C++**<br />
``` C++
public:
property Type^ HelperClass {
	Type^ get ();
}
```

**F#**<br />
``` F#
member HelperClass : Type with get

```


#### Property Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.type" target="_blank">Type</a>

## Remarks

Only valid when <a href="P_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute_IsSimpleMode">IsSimpleMode</a> is set to `false` (`False` in Visual Basic).

The helper class should have a public constructor with one parameter in the same type of the parameter marked with this attribute. All properties in the helper class marked with <a href="T_SecretNest_RemoteAgency_Attributes_TwoWayHelperAttribute">TwoWayHelperAttribute</a> and <a href="P_SecretNest_RemoteAgency_Attributes_TwoWayHelperAttribute_IsTwoWay">IsTwoWay</a> set to `true` (`True` in Visual Basic) are used as the helper fow two way property accessing.


## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute">ParameterTwoWayPropertyAttribute Class</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />