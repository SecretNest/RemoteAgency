# FastActivator(*TArg*).CreateInstance Method (Type, *TArg*)
 

Creates an instance of the type specified.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_AssemblyBuilding">SecretNest.RemoteAgency.AssemblyBuilding</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public static Object CreateInstance(
	Type type,
	TArg arg1
)
```

**VB**<br />
``` VB
Public Shared Function CreateInstance ( 
	type As Type,
	arg1 As TArg
) As Object
```

**C++**<br />
``` C++
public:
static Object^ CreateInstance(
	Type^ type, 
	TArg arg1
)
```

**F#**<br />
``` F#
static member CreateInstance : 
        type : Type * 
        arg1 : 'TArg -> Object 

```


#### Parameters
&nbsp;<dl><dt>type</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.type" target="_blank">System.Type</a><br />The type of the instance to be created.</dd><dt>arg1</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_AssemblyBuilding_FastActivator_1">*TArg*</a><br />The argument which will be passed to the constructor.</dd></dl>

#### Return Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a><br />The created instance.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_AssemblyBuilding_FastActivator_1">FastActivator(TArg) Class</a><br /><a href="Overload_SecretNest_RemoteAgency_AssemblyBuilding_FastActivator_1_CreateInstance">CreateInstance Overload</a><br /><a href="N_SecretNest_RemoteAgency_AssemblyBuilding">SecretNest.RemoteAgency.AssemblyBuilding Namespace</a><br />