# CustomizedMethodEntityNameAttribute.ParameterEntityName Property 
 

Gets the name of the entity class generated for holding parameters of this asset.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public string ParameterEntityName { get; }
```

**VB**<br />
``` VB
Public ReadOnly Property ParameterEntityName As String
	Get
```

**C++**<br />
``` C++
public:
property String^ ParameterEntityName {
	String^ get ();
}
```

**F#**<br />
``` F#
member ParameterEntityName : string with get

```


#### Property Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">String</a>

## Remarks
When the value is a null reference (`Nothing` in Visual Basic) or empty string, name is chosen automatically.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_CustomizedMethodEntityNameAttribute">CustomizedMethodEntityNameAttribute Class</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />