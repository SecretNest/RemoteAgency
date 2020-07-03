# CustomizedMethodEntityNameAttribute Constructor 
 

Initializes an instance of the CustomizedMethodEntityNameAttribute.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public CustomizedMethodEntityNameAttribute(
	string parameterEntityName,
	string returnValueEntityName
)
```

**VB**<br />
``` VB
Public Sub New ( 
	parameterEntityName As String,
	returnValueEntityName As String
)
```

**C++**<br />
``` C++
public:
CustomizedMethodEntityNameAttribute(
	String^ parameterEntityName, 
	String^ returnValueEntityName
)
```

**F#**<br />
``` F#
new : 
        parameterEntityName : string * 
        returnValueEntityName : string -> CustomizedMethodEntityNameAttribute
```


#### Parameters
&nbsp;<dl><dt>parameterEntityName</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Name of the entity class generated for holding parameters of this asset. When the value is a null reference (`Nothing` in Visual Basic) or empty string, name is chosen automatically.</dd><dt>returnValueEntityName</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Name of the entity class generated for holding two way parameters, output parameters and return value of this asset. When the value is a null reference (`Nothing` in Visual Basic) or empty string, name is chosen automatically.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_CustomizedMethodEntityNameAttribute">CustomizedMethodEntityNameAttribute Class</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />