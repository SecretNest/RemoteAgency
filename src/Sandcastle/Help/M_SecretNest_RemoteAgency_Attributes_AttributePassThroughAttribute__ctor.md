# AttributePassThroughAttribute Constructor 
 

Initializes an instance of AttributePassThroughAttribute.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public AttributePassThroughAttribute(
	Type attribute,
	Type[] attributeConstructorParameterTypes = null,
	Object[] attributeConstructorParameters = null,
	string attributeId = null
)
```

**VB**<br />
``` VB
Public Sub New ( 
	attribute As Type,
	Optional attributeConstructorParameterTypes As Type() = Nothing,
	Optional attributeConstructorParameters As Object() = Nothing,
	Optional attributeId As String = Nothing
)
```

**C++**<br />
``` C++
public:
AttributePassThroughAttribute(
	Type^ attribute, 
	array<Type^>^ attributeConstructorParameterTypes = nullptr, 
	array<Object^>^ attributeConstructorParameters = nullptr, 
	String^ attributeId = nullptr
)
```

**F#**<br />
``` F#
new : 
        attribute : Type * 
        ?attributeConstructorParameterTypes : Type[] * 
        ?attributeConstructorParameters : Object[] * 
        ?attributeId : string 
(* Defaults:
        let _attributeConstructorParameterTypes = defaultArg attributeConstructorParameterTypes null
        let _attributeConstructorParameters = defaultArg attributeConstructorParameters null
        let _attributeId = defaultArg attributeId null
*)
-> AttributePassThroughAttribute
```


#### Parameters
&nbsp;<dl><dt>attribute</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.type" target="_blank">System.Type</a><br />Type of the attribute.</dd><dt>attributeConstructorParameterTypes (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.type" target="_blank">System.Type</a>[]<br />Types to identify the constructor of attribute. Default value is a null reference (`Nothing` in Visual Basic). Set the value to a null reference (`Nothing` in Visual Basic) or empty array to use parameterless constructor.</dd><dt>attributeConstructorParameters (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">System.Object</a>[]<br />Parameters used in constructor. The length can not exceed the length of *attributeConstructorParameterTypes*.</dd><dt>attributeId (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br /></dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_AttributePassThroughAttribute">AttributePassThroughAttribute Class</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />