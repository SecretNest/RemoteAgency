# EntityProperty Constructor 
 

Initializes an instance of EntityProperty.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public EntityProperty(
	Type type,
	string name,
	IReadOnlyList<EntityPropertyAttribute> attributes
)
```

**VB**<br />
``` VB
Public Sub New ( 
	type As Type,
	name As String,
	attributes As IReadOnlyList(Of EntityPropertyAttribute)
)
```

**C++**<br />
``` C++
public:
EntityProperty(
	Type^ type, 
	String^ name, 
	IReadOnlyList<EntityPropertyAttribute^>^ attributes
)
```

**F#**<br />
``` F#
new : 
        type : Type * 
        name : string * 
        attributes : IReadOnlyList<EntityPropertyAttribute> -> EntityProperty
```


#### Parameters
&nbsp;<dl><dt>type</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.type" target="_blank">System.Type</a><br />Type of the property.</dd><dt>name</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Name of the property.</dd><dt>attributes</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.collections.generic.ireadonlylist-1" target="_blank">System.Collections.Generic.IReadOnlyList</a>(<a href="T_SecretNest_RemoteAgency_EntityPropertyAttribute">EntityPropertyAttribute</a>)<br />Metadata objects marked with derived class specified by <a href="P_SecretNest_RemoteAgency_EntityCodeBuilderBase_ParameterLevelAttributeBaseType">ParameterLevelAttributeBaseType</a> in parameter level.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_EntityProperty">EntityProperty Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />