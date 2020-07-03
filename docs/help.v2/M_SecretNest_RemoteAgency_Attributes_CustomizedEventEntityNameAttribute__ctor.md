# CustomizedEventEntityNameAttribute Constructor 
 

Initializes an instance of CustomizedEventEntityNameAttribute.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public CustomizedEventEntityNameAttribute(
	string addingRequestEntityName = null,
	string addingResponseEntityName = null,
	string removingRequestEntityName = null,
	string removingResponseEntityName = null,
	string raisingNotificationEntityName = null,
	string raisingFeedbackEntityName = null
)
```

**VB**<br />
``` VB
Public Sub New ( 
	Optional addingRequestEntityName As String = Nothing,
	Optional addingResponseEntityName As String = Nothing,
	Optional removingRequestEntityName As String = Nothing,
	Optional removingResponseEntityName As String = Nothing,
	Optional raisingNotificationEntityName As String = Nothing,
	Optional raisingFeedbackEntityName As String = Nothing
)
```

**C++**<br />
``` C++
public:
CustomizedEventEntityNameAttribute(
	String^ addingRequestEntityName = nullptr, 
	String^ addingResponseEntityName = nullptr, 
	String^ removingRequestEntityName = nullptr, 
	String^ removingResponseEntityName = nullptr, 
	String^ raisingNotificationEntityName = nullptr, 
	String^ raisingFeedbackEntityName = nullptr
)
```

**F#**<br />
``` F#
new : 
        ?addingRequestEntityName : string * 
        ?addingResponseEntityName : string * 
        ?removingRequestEntityName : string * 
        ?removingResponseEntityName : string * 
        ?raisingNotificationEntityName : string * 
        ?raisingFeedbackEntityName : string 
(* Defaults:
        let _addingRequestEntityName = defaultArg addingRequestEntityName null
        let _addingResponseEntityName = defaultArg addingResponseEntityName null
        let _removingRequestEntityName = defaultArg removingRequestEntityName null
        let _removingResponseEntityName = defaultArg removingResponseEntityName null
        let _raisingNotificationEntityName = defaultArg raisingNotificationEntityName null
        let _raisingFeedbackEntityName = defaultArg raisingFeedbackEntityName null
*)
-> CustomizedEventEntityNameAttribute
```


#### Parameters
&nbsp;<dl><dt>addingRequestEntityName (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Name of the entity class for processing event adding request. When the value is a null reference (`Nothing` in Visual Basic) or empty string, name is chosen automatically. Default value is a null reference (`Nothing` in Visual Basic).</dd><dt>addingResponseEntityName (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Name of the entity class for processing event adding response. When the value is a null reference (`Nothing` in Visual Basic) or empty string, name is chosen automatically. Default value is a null reference (`Nothing` in Visual Basic).</dd><dt>removingRequestEntityName (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Name of the entity class for processing event removing response. When the value is a null reference (`Nothing` in Visual Basic) or empty string, name is chosen automatically. Default value is a null reference (`Nothing` in Visual Basic).</dd><dt>removingResponseEntityName (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Name of the entity class for processing event removing response. When the value is a null reference (`Nothing` in Visual Basic) or empty string, name is chosen automatically. Default value is a null reference (`Nothing` in Visual Basic).</dd><dt>raisingNotificationEntityName (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Name of the entity class for processing event raising notification. When the value is a null reference (`Nothing` in Visual Basic) or empty string, name is chosen automatically. Default value is a null reference (`Nothing` in Visual Basic).</dd><dt>raisingFeedbackEntityName (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Name of the entity class for processing event raising feedback. When the value is a null reference (`Nothing` in Visual Basic) or empty string, name is chosen automatically. Default value is a null reference (`Nothing` in Visual Basic).</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_CustomizedEventEntityNameAttribute">CustomizedEventEntityNameAttribute Class</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />