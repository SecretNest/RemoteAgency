# RemoteAgency.CreateWithBinarySerializer Method 
 

Creates an instance of Remote Agency using binary serializer.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public static RemoteAgency<byte[], Object> CreateWithBinarySerializer(
	Nullable<Guid> siteId = null
)
```

**VB**<br />
``` VB
Public Shared Function CreateWithBinarySerializer ( 
	Optional siteId As Nullable(Of Guid) = Nothing
) As RemoteAgency(Of Byte(), Object)
```

**C++**<br />
``` C++
public:
static RemoteAgency<array<unsigned char>^, Object^>^ CreateWithBinarySerializer(
	Nullable<Guid> siteId = nullptr
)
```

**F#**<br />
``` F#
static member CreateWithBinarySerializer : 
        ?siteId : Nullable<Guid> 
(* Defaults:
        let _siteId = defaultArg siteId null
*)
-> RemoteAgency<byte[], Object> 

```


#### Parameters
&nbsp;<dl><dt>siteId (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.nullable-1" target="_blank">System.Nullable</a>(<a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">Guid</a>)<br />Site id. A randomized value is used when it is set to <a href="https://docs.microsoft.com/dotnet/api/system.guid.empty" target="_blank">Empty</a> or absent.</dd></dl>

#### Return Value
Type: <a href="T_SecretNest_RemoteAgency_RemoteAgency_2">RemoteAgency</a>(<a href="https://docs.microsoft.com/dotnet/api/system.byte" target="_blank">Byte</a>[], <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>)<br />Created Remote Agency instance.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />