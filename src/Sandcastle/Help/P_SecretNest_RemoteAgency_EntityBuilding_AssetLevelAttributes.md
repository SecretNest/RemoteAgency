# EntityBuilding.AssetLevelAttributes Property 
 

Gets the metadata objects marked with derived class specified by <a href="P_SecretNest_RemoteAgency_EntityCodeBuilderBase_AssetLevelAttributeBaseType">AssetLevelAttributeBaseType</a> in asset level.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public IReadOnlyList<Attribute> AssetLevelAttributes { get; }
```

**VB**<br />
``` VB
Public ReadOnly Property AssetLevelAttributes As IReadOnlyList(Of Attribute)
	Get
```

**C++**<br />
``` C++
public:
property IReadOnlyList<Attribute^>^ AssetLevelAttributes {
	IReadOnlyList<Attribute^>^ get ();
}
```

**F#**<br />
``` F#
member AssetLevelAttributes : IReadOnlyList<Attribute> with get

```


#### Property Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.collections.generic.ireadonlylist-1" target="_blank">IReadOnlyList</a>(<a href="https://docs.microsoft.com/dotnet/api/system.attribute" target="_blank">Attribute</a>)

## Remarks
This will be set to a null reference (`Nothing` in Visual Basic) when <a href="P_SecretNest_RemoteAgency_EntityCodeBuilderBase_AssetLevelAttributeBaseType">AssetLevelAttributeBaseType</a> is set to a null reference (`Nothing` in Visual Basic).

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_EntityBuilding">EntityBuilding Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />