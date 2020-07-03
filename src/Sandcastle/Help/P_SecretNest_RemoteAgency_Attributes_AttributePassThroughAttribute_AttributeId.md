# AttributePassThroughAttribute.AttributeId Property 
 

Gets the id of the instance of the attribute.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public string AttributeId { get; }
```

**VB**<br />
``` VB
Public ReadOnly Property AttributeId As String
	Get
```

**C++**<br />
``` C++
public:
property String^ AttributeId {
	String^ get ();
}
```

**F#**<br />
``` F#
member AttributeId : string with get

```


#### Property Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">String</a>

## Remarks

This id is designed for linking <a href="T_SecretNest_RemoteAgency_Attributes_AttributePassThroughAttribute">AttributePassThroughAttribute</a> with <a href="T_SecretNest_RemoteAgency_Attributes_AttributePassThroughIndexBasedParameterAttribute">AttributePassThroughIndexBasedParameterAttribute</a>, <a href="T_SecretNest_RemoteAgency_Attributes_AttributePassThroughPropertyAttribute">AttributePassThroughPropertyAttribute</a> and <a href="T_SecretNest_RemoteAgency_Attributes_AttributePassThroughFieldAttribute">AttributePassThroughFieldAttribute</a> on the same member. When no need to mark with <a href="T_SecretNest_RemoteAgency_Attributes_AttributePassThroughIndexBasedParameterAttribute">AttributePassThroughIndexBasedParameterAttribute</a> or <a href="T_SecretNest_RemoteAgency_Attributes_AttributePassThroughPropertyAttribute">AttributePassThroughPropertyAttribute</a>, this value is optional.


## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_AttributePassThroughAttribute">AttributePassThroughAttribute Class</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />