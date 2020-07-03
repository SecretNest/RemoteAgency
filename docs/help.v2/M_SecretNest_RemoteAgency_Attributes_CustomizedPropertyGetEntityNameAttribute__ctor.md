# CustomizedPropertyGetEntityNameAttribute Constructor 
 

Initializes an instance of the CustomizedPropertyGetEntityNameAttribute.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public CustomizedPropertyGetEntityNameAttribute(
	string requestEntityName,
	string responseEntityName
)
```

**VB**<br />
``` VB
Public Sub New ( 
	requestEntityName As String,
	responseEntityName As String
)
```

**C++**<br />
``` C++
public:
CustomizedPropertyGetEntityNameAttribute(
	String^ requestEntityName, 
	String^ responseEntityName
)
```

**F#**<br />
``` F#
new : 
        requestEntityName : string * 
        responseEntityName : string -> CustomizedPropertyGetEntityNameAttribute
```


#### Parameters
&nbsp;<dl><dt>requestEntityName</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Name entity class generated for holding the request of getting this property. When the value is a null reference (`Nothing` in Visual Basic) or empty string, name is chosen automatically.</dd><dt>responseEntityName</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Name entity class generated for holding the response of getting this property. When the value is a null reference (`Nothing` in Visual Basic) or empty string, name is chosen automatically.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_CustomizedPropertyGetEntityNameAttribute">CustomizedPropertyGetEntityNameAttribute Class</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />