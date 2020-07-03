# IManagedObjectCommunicate.GetSiteIdCallback Property 
 

Will be called when site id is required.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
Func<Guid> GetSiteIdCallback { get; set; }
```

**VB**<br />
``` VB
Property GetSiteIdCallback As Func(Of Guid)
	Get
	Set
```

**C++**<br />
``` C++
property Func<Guid>^ GetSiteIdCallback {
	Func<Guid>^ get ();
	void set (Func<Guid>^ value);
}
```

**F#**<br />
``` F#
abstract GetSiteIdCallback : Func<Guid> with get, set

```


#### Property Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.func-1" target="_blank">Func</a>(<a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">Guid</a>)

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_IManagedObjectCommunicate">IManagedObjectCommunicate Interface</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />