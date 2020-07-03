# CustomizedEventParameterEntityPropertyNameAttribute Constructor 
 

Initializes an instance of the CustomizedEventParameterEntityPropertyNameAttribute.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public CustomizedEventParameterEntityPropertyNameAttribute(
	string parameterName,
	string entityPropertyName
)
```

**VB**<br />
``` VB
Public Sub New ( 
	parameterName As String,
	entityPropertyName As String
)
```

**C++**<br />
``` C++
public:
CustomizedEventParameterEntityPropertyNameAttribute(
	String^ parameterName, 
	String^ entityPropertyName
)
```

**F#**<br />
``` F#
new : 
        parameterName : string * 
        entityPropertyName : string -> CustomizedEventParameterEntityPropertyNameAttribute
```


#### Parameters
&nbsp;<dl><dt>parameterName</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Parameter name of the event.</dd><dt>entityPropertyName</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Property name in entity class. When the value is a null reference (`Nothing` in Visual Basic) or empty string, name is chosen automatically.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_CustomizedEventParameterEntityPropertyNameAttribute">CustomizedEventParameterEntityPropertyNameAttribute Class</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />