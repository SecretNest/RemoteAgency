# RemoteAgency.CreateWithJsonSerializer Method 
 

Creates an instance of Remote Agency using Json serializer.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public static RemoteAgency<string, Object> CreateWithJsonSerializer(
	Nullable<Guid> siteId = null
)
```

**VB**<br />
``` VB
Public Shared Function CreateWithJsonSerializer ( 
	Optional siteId As Nullable(Of Guid) = Nothing
) As RemoteAgency(Of String, Object)
```

**C++**<br />
``` C++
public:
static RemoteAgency<String^, Object^>^ CreateWithJsonSerializer(
	Nullable<Guid> siteId = nullptr
)
```

**F#**<br />
``` F#
static member CreateWithJsonSerializer : 
        ?siteId : Nullable<Guid> 
(* Defaults:
        let _siteId = defaultArg siteId null
*)
-> RemoteAgency<string, Object> 

```


#### Parameters
&nbsp;<dl><dt>siteId (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.nullable-1" target="_blank">System.Nullable</a>(<a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">Guid</a>)<br />Site id. A randomized value is used when it is set to <a href="https://docs.microsoft.com/dotnet/api/system.guid.empty" target="_blank">Empty</a> or absent.</dd></dl>

#### Return Value
Type: <a href="T_SecretNest_RemoteAgency_RemoteAgency_2">RemoteAgency</a>(<a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">String</a>, <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>)<br />Created Remote Agency instance.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />