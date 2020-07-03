# CustomizedPropertyGetEntityNameAttribute.ResponseEntityName Property 
 

Gets the name entity class generated for holding the response of getting this property.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public string ResponseEntityName { get; }
```

**VB**<br />
``` VB
Public ReadOnly Property ResponseEntityName As String
	Get
```

**C++**<br />
``` C++
public:
property String^ ResponseEntityName {
	String^ get ();
}
```

**F#**<br />
``` F#
member ResponseEntityName : string with get

```


#### Property Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">String</a>

## Remarks
When the value is a null reference (`Nothing` in Visual Basic) or empty string, name is chosen automatically.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_CustomizedPropertyGetEntityNameAttribute">CustomizedPropertyGetEntityNameAttribute Class</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />