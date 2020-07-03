# BeforeTypeCreatedEventArgs Constructor 
 

Initialize an instance of BeforeTypeCreatedEventArgs.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_AssemblyBuilding">SecretNest.RemoteAgency.AssemblyBuilding</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public BeforeTypeCreatedEventArgs(
	TypeBuilder typeBuilder,
	Type sourceInterface,
	BuiltClassType builtClassType
)
```

**VB**<br />
``` VB
Public Sub New ( 
	typeBuilder As TypeBuilder,
	sourceInterface As Type,
	builtClassType As BuiltClassType
)
```

**C++**<br />
``` C++
public:
BeforeTypeCreatedEventArgs(
	TypeBuilder^ typeBuilder, 
	Type^ sourceInterface, 
	BuiltClassType builtClassType
)
```

**F#**<br />
``` F#
new : 
        typeBuilder : TypeBuilder * 
        sourceInterface : Type * 
        builtClassType : BuiltClassType -> BeforeTypeCreatedEventArgs
```


#### Parameters
&nbsp;<dl><dt>typeBuilder</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.reflection.emit.typebuilder" target="_blank">System.Reflection.Emit.TypeBuilder</a><br />Builder for building type.</dd><dt>sourceInterface</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.type" target="_blank">System.Type</a><br />Type of source interface.</dd><dt>builtClassType</dt><dd>Type: <a href="T_SecretNest_RemoteAgency_AssemblyBuilding_BuiltClassType">SecretNest.RemoteAgency.AssemblyBuilding.BuiltClassType</a><br />Type of the class to be built, proxy or service wrapper.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_AssemblyBuilding_BeforeTypeCreatedEventArgs">BeforeTypeCreatedEventArgs Class</a><br /><a href="N_SecretNest_RemoteAgency_AssemblyBuilding">SecretNest.RemoteAgency.AssemblyBuilding Namespace</a><br />