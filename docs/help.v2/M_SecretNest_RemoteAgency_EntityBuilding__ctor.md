# EntityBuilding Constructor 
 

Initializes an instance of EntityBuilding.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public EntityBuilding(
	string entityClassName,
	IReadOnlyList<EntityProperty> properties,
	IReadOnlyList<Attribute> interfaceLevelAttributes,
	IReadOnlyList<Attribute> assetLevelAttributes,
	IReadOnlyList<Attribute> delegateLevelAttributes,
	Type[] genericParameters
)
```

**VB**<br />
``` VB
Public Sub New ( 
	entityClassName As String,
	properties As IReadOnlyList(Of EntityProperty),
	interfaceLevelAttributes As IReadOnlyList(Of Attribute),
	assetLevelAttributes As IReadOnlyList(Of Attribute),
	delegateLevelAttributes As IReadOnlyList(Of Attribute),
	genericParameters As Type()
)
```

**C++**<br />
``` C++
public:
EntityBuilding(
	String^ entityClassName, 
	IReadOnlyList<EntityProperty^>^ properties, 
	IReadOnlyList<Attribute^>^ interfaceLevelAttributes, 
	IReadOnlyList<Attribute^>^ assetLevelAttributes, 
	IReadOnlyList<Attribute^>^ delegateLevelAttributes, 
	array<Type^>^ genericParameters
)
```

**F#**<br />
``` F#
new : 
        entityClassName : string * 
        properties : IReadOnlyList<EntityProperty> * 
        interfaceLevelAttributes : IReadOnlyList<Attribute> * 
        assetLevelAttributes : IReadOnlyList<Attribute> * 
        delegateLevelAttributes : IReadOnlyList<Attribute> * 
        genericParameters : Type[] -> EntityBuilding
```


#### Parameters
&nbsp;<dl><dt>entityClassName</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Name of the entity class.</dd><dt>properties</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.collections.generic.ireadonlylist-1" target="_blank">System.Collections.Generic.IReadOnlyList</a>(<a href="T_SecretNest_RemoteAgency_EntityProperty">EntityProperty</a>)<br />Properties other than in interface.</dd><dt>interfaceLevelAttributes</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.collections.generic.ireadonlylist-1" target="_blank">System.Collections.Generic.IReadOnlyList</a>(<a href="https://docs.microsoft.com/dotnet/api/system.attribute" target="_blank">Attribute</a>)<br />Metadata objects marked with derived class specified by <a href="P_SecretNest_RemoteAgency_EntityCodeBuilderBase_InterfaceLevelAttributeBaseType">InterfaceLevelAttributeBaseType</a> in interface level. This will be set to a null reference (`Nothing` in Visual Basic) when <a href="P_SecretNest_RemoteAgency_EntityCodeBuilderBase_InterfaceLevelAttributeBaseType">InterfaceLevelAttributeBaseType</a> is set to a null reference (`Nothing` in Visual Basic).</dd><dt>assetLevelAttributes</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.collections.generic.ireadonlylist-1" target="_blank">System.Collections.Generic.IReadOnlyList</a>(<a href="https://docs.microsoft.com/dotnet/api/system.attribute" target="_blank">Attribute</a>)<br />Metadata objects marked with derived class specified by <a href="P_SecretNest_RemoteAgency_EntityCodeBuilderBase_AssetLevelAttributeBaseType">AssetLevelAttributeBaseType</a> in asset level. This will be set to a null reference (`Nothing` in Visual Basic) when <a href="P_SecretNest_RemoteAgency_EntityCodeBuilderBase_AssetLevelAttributeBaseType">AssetLevelAttributeBaseType</a> is set to a null reference (`Nothing` in Visual Basic).</dd><dt>delegateLevelAttributes</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.collections.generic.ireadonlylist-1" target="_blank">System.Collections.Generic.IReadOnlyList</a>(<a href="https://docs.microsoft.com/dotnet/api/system.attribute" target="_blank">Attribute</a>)<br />Metadata objects marked with derived class specified by <a href="P_SecretNest_RemoteAgency_EntityCodeBuilderBase_DelegateLevelAttributeBaseType">DelegateLevelAttributeBaseType</a> for the delegate of event. Only available when processing events. This will be set to a null reference (`Nothing` in Visual Basic) when <a href="P_SecretNest_RemoteAgency_EntityCodeBuilderBase_DelegateLevelAttributeBaseType">DelegateLevelAttributeBaseType</a> is set to a null reference (`Nothing` in Visual Basic).</dd><dt>genericParameters</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.type" target="_blank">System.Type</a>[]<br />Generic parameters of this entity class.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_EntityBuilding">EntityBuilding Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />