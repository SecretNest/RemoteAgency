# OperatingTimeoutTimeAttribute Constructor (Int32, Int32)
 

Initializes an instance of the OperatingTimeoutTimeAttribute with property mode.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public OperatingTimeoutTimeAttribute(
	int propertyGettingTimeout,
	int propertySettingTimeout
)
```

**VB**<br />
``` VB
Public Sub New ( 
	propertyGettingTimeout As Integer,
	propertySettingTimeout As Integer
)
```

**C++**<br />
``` C++
public:
OperatingTimeoutTimeAttribute(
	int propertyGettingTimeout, 
	int propertySettingTimeout
)
```

**F#**<br />
``` F#
new : 
        propertyGettingTimeout : int * 
        propertySettingTimeout : int -> OperatingTimeoutTimeAttribute
```


#### Parameters
&nbsp;<dl><dt>propertyGettingTimeout</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.int32" target="_blank">System.Int32</a><br />The length of time for waiting response for property getting, in milliseconds; or -1 to indicate that the waiting does not time out.</dd><dt>propertySettingTimeout</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.int32" target="_blank">System.Int32</a><br />The length of time for waiting response for property setting, in milliseconds; or -1 to indicate that the waiting does not time out.</dd></dl>

## Remarks

This constructor is for setting the timeout for property getting and setting separately only. To set to the same value, or set for asset other than property, uses <a href="M_SecretNest_RemoteAgency_Attributes_OperatingTimeoutTimeAttribute__ctor_1">OperatingTimeoutTimeAttribute(Int32)</a> instead.

To set the timeout for event adding, removing and raising separately, uses <a href="M_SecretNest_RemoteAgency_Attributes_OperatingTimeoutTimeAttribute__ctor_3">OperatingTimeoutTimeAttribute(Int32, Int32, Int32)</a> instead.


## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_OperatingTimeoutTimeAttribute">OperatingTimeoutTimeAttribute Class</a><br /><a href="Overload_SecretNest_RemoteAgency_Attributes_OperatingTimeoutTimeAttribute__ctor">OperatingTimeoutTimeAttribute Overload</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />