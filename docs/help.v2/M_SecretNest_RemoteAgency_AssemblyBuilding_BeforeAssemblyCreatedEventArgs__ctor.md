# BeforeAssemblyCreatedEventArgs Constructor 
 

Initialize an instance of BeforeAssemblyCreatedEventArgs.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_AssemblyBuilding">SecretNest.RemoteAgency.AssemblyBuilding</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public BeforeAssemblyCreatedEventArgs(
	AssemblyBuilder assemblyBuilder,
	ModuleBuilder moduleBuilder,
	Type sourceInterface,
	Type builtProxy,
	Type builtServiceWrapper,
	IReadOnlyList<Type> builtEntities
)
```

**VB**<br />
``` VB
Public Sub New ( 
	assemblyBuilder As AssemblyBuilder,
	moduleBuilder As ModuleBuilder,
	sourceInterface As Type,
	builtProxy As Type,
	builtServiceWrapper As Type,
	builtEntities As IReadOnlyList(Of Type)
)
```

**C++**<br />
``` C++
public:
BeforeAssemblyCreatedEventArgs(
	AssemblyBuilder^ assemblyBuilder, 
	ModuleBuilder^ moduleBuilder, 
	Type^ sourceInterface, 
	Type^ builtProxy, 
	Type^ builtServiceWrapper, 
	IReadOnlyList<Type^>^ builtEntities
)
```

**F#**<br />
``` F#
new : 
        assemblyBuilder : AssemblyBuilder * 
        moduleBuilder : ModuleBuilder * 
        sourceInterface : Type * 
        builtProxy : Type * 
        builtServiceWrapper : Type * 
        builtEntities : IReadOnlyList<Type> -> BeforeAssemblyCreatedEventArgs
```


#### Parameters
&nbsp;<dl><dt>assemblyBuilder</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.reflection.emit.assemblybuilder" target="_blank">System.Reflection.Emit.AssemblyBuilder</a><br />Builder for building assembly.</dd><dt>moduleBuilder</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.reflection.emit.modulebuilder" target="_blank">System.Reflection.Emit.ModuleBuilder</a><br />Builder for building module.</dd><dt>sourceInterface</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.type" target="_blank">System.Type</a><br />Type of source interface.</dd><dt>builtProxy</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.type" target="_blank">System.Type</a><br />Type of built proxy</dd><dt>builtServiceWrapper</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.type" target="_blank">System.Type</a><br />Type of built service wrapper.</dd><dt>builtEntities</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.collections.generic.ireadonlylist-1" target="_blank">System.Collections.Generic.IReadOnlyList</a>(<a href="https://docs.microsoft.com/dotnet/api/system.type" target="_blank">Type</a>)<br />Types of built entities.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_AssemblyBuilding_BeforeAssemblyCreatedEventArgs">BeforeAssemblyCreatedEventArgs Class</a><br /><a href="N_SecretNest_RemoteAgency_AssemblyBuilding">SecretNest.RemoteAgency.AssemblyBuilding Namespace</a><br />