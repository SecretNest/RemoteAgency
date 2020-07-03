# CustomizedPropertyGetResponsePropertyNameAttribute.EntityPropertyName Property 
 

Gets the property name in entity class.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public string EntityPropertyName { get; }
```

**VB**<br />
``` VB
Public ReadOnly Property EntityPropertyName As String
	Get
```

**C++**<br />
``` C++
public:
property String^ EntityPropertyName {
	String^ get ();
}
```

**F#**<br />
``` F#
member EntityPropertyName : string with get

```


#### Property Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">String</a>

## Remarks
When the value is a null reference (`Nothing` in Visual Basic) or empty string, name is chosen automatically.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_CustomizedPropertyGetResponsePropertyNameAttribute">CustomizedPropertyGetResponsePropertyNameAttribute Class</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />