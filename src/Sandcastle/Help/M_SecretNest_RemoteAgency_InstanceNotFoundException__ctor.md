# InstanceNotFoundException Constructor 
 

Initializes an instance of InstanceNotFoundException.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public InstanceNotFoundException(
	IRemoteAgencyMessage originalMessage,
	Guid exceptionThrownSiteId
)
```

**VB**<br />
``` VB
Public Sub New ( 
	originalMessage As IRemoteAgencyMessage,
	exceptionThrownSiteId As Guid
)
```

**C++**<br />
``` C++
public:
InstanceNotFoundException(
	IRemoteAgencyMessage^ originalMessage, 
	Guid exceptionThrownSiteId
)
```

**F#**<br />
``` F#
new : 
        originalMessage : IRemoteAgencyMessage * 
        exceptionThrownSiteId : Guid -> InstanceNotFoundException
```


#### Parameters
&nbsp;<dl><dt>originalMessage</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_IRemoteAgencyMessage">SecretNest.RemoteAgency.IRemoteAgencyMessage</a><br />Message which causes this exception thrown.</dd><dt>exceptionThrownSiteId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">System.Guid</a><br />Site id of the Remote Agency instance which throws the exception.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_InstanceNotFoundException">InstanceNotFoundException Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />