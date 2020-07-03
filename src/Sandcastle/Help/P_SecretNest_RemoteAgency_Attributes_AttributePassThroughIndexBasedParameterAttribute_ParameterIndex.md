# AttributePassThroughIndexBasedParameterAttribute.ParameterIndex Property 
 

Gets the index of the parameter.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public int ParameterIndex { get; }
```

**VB**<br />
``` VB
Public ReadOnly Property ParameterIndex As Integer
	Get
```

**C++**<br />
``` C++
public:
property int ParameterIndex {
	int get ();
}
```

**F#**<br />
``` F#
member ParameterIndex : int with get

```


#### Return Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.int32" target="_blank">Int32</a><br />
The value cannot be equal or larger than the length of <a href="P_SecretNest_RemoteAgency_Attributes_AttributePassThroughAttribute_AttributeConstructorParameterTypes">AttributeConstructorParameterTypes</a> marked at the same place with the value <a href="P_SecretNest_RemoteAgency_Attributes_AttributePassThroughAttribute_AttributeId">AttributeId</a> is the same as <a href="P_SecretNest_RemoteAgency_Attributes_AttributePassThroughIndexBasedParameterAttribute_AttributeId">AttributeId</a>.

When the index is smaller than the length of <a href="P_SecretNest_RemoteAgency_Attributes_AttributePassThroughAttribute_AttributeConstructorParameters">AttributeConstructorParameters</a> marked at the same place with the value <a href="P_SecretNest_RemoteAgency_Attributes_AttributePassThroughAttribute_AttributeId">AttributeId</a> is the same as <a href="P_SecretNest_RemoteAgency_Attributes_AttributePassThroughIndexBasedParameterAttribute_AttributeId">AttributeId</a>, the parameter in <a href="P_SecretNest_RemoteAgency_Attributes_AttributePassThroughAttribute_AttributeConstructorParameters">AttributeConstructorParameters</a> with the index specified by ParameterIndex is replaced with <a href="P_SecretNest_RemoteAgency_Attributes_AttributePassThroughIndexBasedParameterAttribute_Value">Value</a>.


## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_AttributePassThroughIndexBasedParameterAttribute">AttributePassThroughIndexBasedParameterAttribute Class</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />