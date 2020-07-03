# CustomizedEventEntityNameAttribute.RaisingNotificationEntityName Property 
 

Gets the name of the entity class for processing event raising notification.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public string RaisingNotificationEntityName { get; }
```

**VB**<br />
``` VB
Public ReadOnly Property RaisingNotificationEntityName As String
	Get
```

**C++**<br />
``` C++
public:
property String^ RaisingNotificationEntityName {
	String^ get ();
}
```

**F#**<br />
``` F#
member RaisingNotificationEntityName : string with get

```


#### Property Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">String</a>

## Remarks
When the value is a null reference (`Nothing` in Visual Basic) or empty string, name is chosen automatically.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_CustomizedEventEntityNameAttribute">CustomizedEventEntityNameAttribute Class</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />