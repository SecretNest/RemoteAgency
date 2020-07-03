# RemoteAgency(*TSerialized*, *TEntityBase*).CloseInstance Method 
 

Closes the proxy or service wrapper by instance id.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public override bool CloseInstance(
	Guid instanceId
)
```

**VB**<br />
``` VB
Public Overrides Function CloseInstance ( 
	instanceId As Guid
) As Boolean
```

**C++**<br />
``` C++
public:
virtual bool CloseInstance(
	Guid instanceId
) override
```

**F#**<br />
``` F#
abstract CloseInstance : 
        instanceId : Guid -> bool 
override CloseInstance : 
        instanceId : Guid -> bool 
```


#### Parameters
&nbsp;<dl><dt>instanceId</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.guid" target="_blank">System.Guid</a><br />Instance id of the proxy or service wrapper to be closed.</dd></dl>

#### Return Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.boolean" target="_blank">Boolean</a><br />Result. `true` (`True` in Visual Basic) when instance is located and closed; `false` (`False` in Visual Basic) when instance is not found.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_RemoteAgency_2">RemoteAgency(TSerialized, TEntityBase) Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />