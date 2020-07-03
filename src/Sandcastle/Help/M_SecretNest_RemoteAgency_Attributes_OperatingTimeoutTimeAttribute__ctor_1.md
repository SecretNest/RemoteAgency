# OperatingTimeoutTimeAttribute Constructor (Int32)
 

Initializes an instance of the OperatingTimeoutTimeAttribute with simple mode.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public OperatingTimeoutTimeAttribute(
	int timeout
)
```

**VB**<br />
``` VB
Public Sub New ( 
	timeout As Integer
)
```

**C++**<br />
``` C++
public:
OperatingTimeoutTimeAttribute(
	int timeout
)
```

**F#**<br />
``` F#
new : 
        timeout : int -> OperatingTimeoutTimeAttribute
```


#### Parameters
&nbsp;<dl><dt>timeout</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.int32" target="_blank">System.Int32</a><br />The length of time for waiting response, in milliseconds; or -1 to indicate that the waiting does not time out.</dd></dl>

## Remarks

All timeout settings will be set as the value specified by *timeout*.

To set the timeout for event adding, removing and raising separately, uses <a href="M_SecretNest_RemoteAgency_Attributes_OperatingTimeoutTimeAttribute__ctor_3">OperatingTimeoutTimeAttribute(Int32, Int32, Int32)</a> instead.

To set the timeout for property getting and setting separately, uses <a href="M_SecretNest_RemoteAgency_Attributes_OperatingTimeoutTimeAttribute__ctor_2">OperatingTimeoutTimeAttribute(Int32, Int32)</a> instead.


## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_OperatingTimeoutTimeAttribute">OperatingTimeoutTimeAttribute Class</a><br /><a href="Overload_SecretNest_RemoteAgency_Attributes_OperatingTimeoutTimeAttribute__ctor">OperatingTimeoutTimeAttribute Overload</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />