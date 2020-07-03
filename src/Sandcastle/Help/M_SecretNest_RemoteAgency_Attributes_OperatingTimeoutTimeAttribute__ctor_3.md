# OperatingTimeoutTimeAttribute Constructor (Int32, Int32, Int32)
 

Initializes an instance of the OperatingTimeoutTimeAttribute with event mode.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public OperatingTimeoutTimeAttribute(
	int eventAddingTimeout,
	int eventRemovingTimeout,
	int eventRaisingTimeout
)
```

**VB**<br />
``` VB
Public Sub New ( 
	eventAddingTimeout As Integer,
	eventRemovingTimeout As Integer,
	eventRaisingTimeout As Integer
)
```

**C++**<br />
``` C++
public:
OperatingTimeoutTimeAttribute(
	int eventAddingTimeout, 
	int eventRemovingTimeout, 
	int eventRaisingTimeout
)
```

**F#**<br />
``` F#
new : 
        eventAddingTimeout : int * 
        eventRemovingTimeout : int * 
        eventRaisingTimeout : int -> OperatingTimeoutTimeAttribute
```


#### Parameters
&nbsp;<dl><dt>eventAddingTimeout</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.int32" target="_blank">System.Int32</a><br />The length of time for waiting response for event adding, in milliseconds; or -1 to indicate that the waiting does not time out.</dd><dt>eventRemovingTimeout</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.int32" target="_blank">System.Int32</a><br />The length of time for waiting response for event removing, in milliseconds; or -1 to indicate that the waiting does not time out.</dd><dt>eventRaisingTimeout</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.int32" target="_blank">System.Int32</a><br />The length of time for waiting response for event raising, in milliseconds; or -1 to indicate that the waiting does not time out.</dd></dl>

## Remarks

This constructor is for setting the timeout for event adding, removing and raising separately only. To set to the same value, or set for asset other than event, uses <a href="M_SecretNest_RemoteAgency_Attributes_OperatingTimeoutTimeAttribute__ctor_1">OperatingTimeoutTimeAttribute(Int32)</a> instead.

To set the timeout for property getting and setting separately, uses <a href="M_SecretNest_RemoteAgency_Attributes_OperatingTimeoutTimeAttribute__ctor_2">OperatingTimeoutTimeAttribute(Int32, Int32)</a> instead.


## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_OperatingTimeoutTimeAttribute">OperatingTimeoutTimeAttribute Class</a><br /><a href="Overload_SecretNest_RemoteAgency_Attributes_OperatingTimeoutTimeAttribute__ctor">OperatingTimeoutTimeAttribute Overload</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />