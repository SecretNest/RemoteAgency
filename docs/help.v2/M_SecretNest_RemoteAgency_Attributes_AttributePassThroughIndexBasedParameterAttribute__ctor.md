# AttributePassThroughIndexBasedParameterAttribute Constructor 
 

Initializes an instance of AttributePassThroughIndexBasedParameterAttribute.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public AttributePassThroughIndexBasedParameterAttribute(
	string attributeId,
	int parameterIndex,
	Object value
)
```

**VB**<br />
``` VB
Public Sub New ( 
	attributeId As String,
	parameterIndex As Integer,
	value As Object
)
```

**C++**<br />
``` C++
public:
AttributePassThroughIndexBasedParameterAttribute(
	String^ attributeId, 
	int parameterIndex, 
	Object^ value
)
```

**F#**<br />
``` F#
new : 
        attributeId : string * 
        parameterIndex : int * 
        value : Object -> AttributePassThroughIndexBasedParameterAttribute
```


#### Parameters
&nbsp;<dl><dt>attributeId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Id of the instance of the attribute. This value should be same as the <a href="P_SecretNest_RemoteAgency_Attributes_AttributePassThroughAttribute_AttributeId">AttributeId</a> marked at the same place for the same instance of attribute.</dd><dt>parameterIndex</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.int32" target="_blank">System.Int32</a><br />Index of the parameter.</dd><dt>value</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">System.Object</a><br />Value of the parameter.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_AttributePassThroughIndexBasedParameterAttribute">AttributePassThroughIndexBasedParameterAttribute Class</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />