# EntityProperty.Attributes Property 
 

Gets the metadata objects marked with derived class specified by <a href="P_SecretNest_RemoteAgency_EntityCodeBuilderBase_ParameterLevelAttributeBaseType">ParameterLevelAttributeBaseType</a> in parameter level.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public IReadOnlyList<EntityPropertyAttribute> Attributes { get; }
```

**VB**<br />
``` VB
Public ReadOnly Property Attributes As IReadOnlyList(Of EntityPropertyAttribute)
	Get
```

**C++**<br />
``` C++
public:
property IReadOnlyList<EntityPropertyAttribute^>^ Attributes {
	IReadOnlyList<EntityPropertyAttribute^>^ get ();
}
```

**F#**<br />
``` F#
member Attributes : IReadOnlyList<EntityPropertyAttribute> with get

```


#### Property Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.collections.generic.ireadonlylist-1" target="_blank">IReadOnlyList</a>(<a href="T_SecretNest_RemoteAgency_EntityPropertyAttribute">EntityPropertyAttribute</a>)

## Remarks
This will be set to a null reference (`Nothing` in Visual Basic) when <a href="P_SecretNest_RemoteAgency_EntityCodeBuilderBase_ParameterLevelAttributeBaseType">ParameterLevelAttributeBaseType</a> is set to a null reference (`Nothing` in Visual Basic).

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_EntityProperty">EntityProperty Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />