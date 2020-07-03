# AttributePassThroughFieldAttribute Constructor 
 

Initializes an instance of AttributePassThroughFieldAttribute.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public AttributePassThroughFieldAttribute(
	string attributeId,
	string fieldName,
	Object value,
	int order = 0
)
```

**VB**<br />
``` VB
Public Sub New ( 
	attributeId As String,
	fieldName As String,
	value As Object,
	Optional order As Integer = 0
)
```

**C++**<br />
``` C++
public:
AttributePassThroughFieldAttribute(
	String^ attributeId, 
	String^ fieldName, 
	Object^ value, 
	int order = 0
)
```

**F#**<br />
``` F#
new : 
        attributeId : string * 
        fieldName : string * 
        value : Object * 
        ?order : int 
(* Defaults:
        let _order = defaultArg order 0
*)
-> AttributePassThroughFieldAttribute
```


#### Parameters
&nbsp;<dl><dt>attributeId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Id of the instance of the attribute. This value should be same as the <a href="P_SecretNest_RemoteAgency_Attributes_AttributePassThroughAttribute_AttributeId">AttributeId</a> marked at the same place for the same instance of attribute.</dd><dt>fieldName</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Name of the field.</dd><dt>value</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">System.Object</a><br />Value of the field.</dd><dt>order (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.int32" target="_blank">System.Int32</a><br />Order for setting the field. All setting operations are performed sequentially. Default value is 0.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_AttributePassThroughFieldAttribute">AttributePassThroughFieldAttribute Class</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />