# ParameterTwoWayPropertyAttribute.IsIncludedWhenExceptionThrown Property 
 

Gets whether this property should be included in return entity when exception thrown by the user code on the remote site.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public bool IsIncludedWhenExceptionThrown { get; }
```

**VB**<br />
``` VB
Public ReadOnly Property IsIncludedWhenExceptionThrown As Boolean
	Get
```

**C++**<br />
``` C++
public:
property bool IsIncludedWhenExceptionThrown {
	bool get ();
}
```

**F#**<br />
``` F#
member IsIncludedWhenExceptionThrown : bool with get

```


#### Property Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">Boolean</a>

## Remarks
Only valid when <a href="P_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute_IsSimpleMode">IsSimpleMode</a> is set to `false` (`False` in Visual Basic).

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute">ParameterTwoWayPropertyAttribute Class</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />