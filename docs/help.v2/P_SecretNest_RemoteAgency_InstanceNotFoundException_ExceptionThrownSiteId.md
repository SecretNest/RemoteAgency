# InstanceNotFoundException.ExceptionThrownSiteId Property 
 

Gets the site id of the Remote Agency instance which throws the exception.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public Guid ExceptionThrownSiteId { get; }
```

**VB**<br />
``` VB
Public ReadOnly Property ExceptionThrownSiteId As Guid
	Get
```

**C++**<br />
``` C++
public:
property Guid ExceptionThrownSiteId {
	Guid get ();
}
```

**F#**<br />
``` F#
member ExceptionThrownSiteId : Guid with get

```


#### Return Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">Guid</a><br />Due to routing mechanism, like load-balancing, the actual target site may not the same as requested in the message. This property contains the site id of the site id of the actual Remote Agency site.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_InstanceNotFoundException">InstanceNotFoundException Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />