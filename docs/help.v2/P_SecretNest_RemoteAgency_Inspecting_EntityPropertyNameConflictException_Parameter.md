# EntityPropertyNameConflictException.Parameter Property 
 

Gets the parameter which the attribute is on.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Inspecting">SecretNest.RemoteAgency.Inspecting</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public ParameterInfo Parameter { get; }
```

**VB**<br />
``` VB
Public ReadOnly Property Parameter As ParameterInfo
	Get
```

**C++**<br />
``` C++
public:
property ParameterInfo^ Parameter {
	ParameterInfo^ get ();
}
```

**F#**<br />
``` F#
member Parameter : ParameterInfo with get

```


#### Property Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.reflection.parameterinfo" target="_blank">ParameterInfo</a>

## Remarks
Only valid when <a href="P_SecretNest_RemoteAgency_Inspecting_EntityPropertyNameConflictException_CausedMemberType">CausedMemberType</a> is set to <a href="T_SecretNest_RemoteAgency_Inspecting_EntityPropertyNameConflictExceptionCausedMemberType">EntityPropertyNameConflictExceptionCausedMemberType</a>.Parameter.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Inspecting_EntityPropertyNameConflictException">EntityPropertyNameConflictException Class</a><br /><a href="N_SecretNest_RemoteAgency_Inspecting">SecretNest.RemoteAgency.Inspecting Namespace</a><br />