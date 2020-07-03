# FastActivator(*TArg*).CreateInstance(*T*) Method (Type, *TArg*)
 

Creates an instance of the type specified and cast it to the type specified.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_AssemblyBuilding">SecretNest.RemoteAgency.AssemblyBuilding</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public static T CreateInstance<T>(
	Type type,
	TArg arg1
)
where T : class

```

**VB**<br />
``` VB
Public Shared Function CreateInstance(Of T As Class) ( 
	type As Type,
	arg1 As TArg
) As T
```

**C++**<br />
``` C++
public:
generic<typename T>
where T : ref class
static T CreateInstance(
	Type^ type, 
	TArg arg1
)
```

**F#**<br />
``` F#
static member CreateInstance : 
        type : Type * 
        arg1 : 'TArg -> 'T  when 'T : not struct

```


#### Parameters
&nbsp;<dl><dt>type</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.type" target="_blank">System.Type</a><br />The type of the instance to be created.</dd><dt>arg1</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_AssemblyBuilding_FastActivator_1">*TArg*</a><br />The argument which will be passed to the constructor.</dd></dl>

#### Type Parameters
&nbsp;<dl><dt>T</dt><dd>Target type to be cast to.</dd></dl>

#### Return Value
Type: *T*<br />The created instance.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_AssemblyBuilding_FastActivator_1">FastActivator(TArg) Class</a><br /><a href="Overload_SecretNest_RemoteAgency_AssemblyBuilding_FastActivator_1_CreateInstance">CreateInstance Overload</a><br /><a href="N_SecretNest_RemoteAgency_AssemblyBuilding">SecretNest.RemoteAgency.AssemblyBuilding Namespace</a><br />