# BeforeAssemblyCreatedEventArgs Class
 

Represents an argument of the <a href="E_SecretNest_RemoteAgency_RemoteAgency_BeforeAssemblyCreated">BeforeAssemblyCreated</a>.


## Inheritance Hierarchy
<a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">System.Object</a><br />&nbsp;&nbsp;<a href="https://docs.microsoft.com/dotnet/api/system.eventargs" target="_blank">System.EventArgs</a><br />&nbsp;&nbsp;&nbsp;&nbsp;SecretNest.RemoteAgency.AssemblyBuilding.BeforeAssemblyCreatedEventArgs<br />
**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_AssemblyBuilding">SecretNest.RemoteAgency.AssemblyBuilding</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public class BeforeAssemblyCreatedEventArgs : EventArgs
```

**VB**<br />
``` VB
Public Class BeforeAssemblyCreatedEventArgs
	Inherits EventArgs
```

**C++**<br />
``` C++
public ref class BeforeAssemblyCreatedEventArgs : public EventArgs
```

**F#**<br />
``` F#
type BeforeAssemblyCreatedEventArgs =  
    class
        inherit EventArgs
    end
```

The BeforeAssemblyCreatedEventArgs type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_AssemblyBuilding_BeforeAssemblyCreatedEventArgs__ctor">BeforeAssemblyCreatedEventArgs</a></td><td>
Initialize an instance of BeforeAssemblyCreatedEventArgs.</td></tr></table>&nbsp;
<a href="#beforeassemblycreatedeventargs-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_AssemblyBuilding_BeforeAssemblyCreatedEventArgs_AssemblyBuilder">AssemblyBuilder</a></td><td>
Gets the builder for building assembly.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_AssemblyBuilding_BeforeAssemblyCreatedEventArgs_BuiltEntities">BuiltEntities</a></td><td>
Gets the types of built entities.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_AssemblyBuilding_BeforeAssemblyCreatedEventArgs_BuiltProxy">BuiltProxy</a></td><td>
Gets the type of built proxy. When proxy is not built, the value is a null reference (`Nothing` in Visual Basic).</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_AssemblyBuilding_BeforeAssemblyCreatedEventArgs_BuiltServiceWrapper">BuiltServiceWrapper</a></td><td>
Gets the type of built service wrapper. When service wrapper is not built, the value is a null reference (`Nothing` in Visual Basic).</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_AssemblyBuilding_BeforeAssemblyCreatedEventArgs_ModuleBuilder">ModuleBuilder</a></td><td>
Gets the builder for building module.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_AssemblyBuilding_BeforeAssemblyCreatedEventArgs_SourceInterface">SourceInterface</a></td><td>
Gets the type of source interface.</td></tr></table>&nbsp;
<a href="#beforeassemblycreatedeventargs-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.equals#System_Object_Equals_System_Object_" target="_blank">Equals</a></td><td>
Determines whether the specified object is equal to the current object.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.finalize#System_Object_Finalize" target="_blank">Finalize</a></td><td>
Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.gethashcode#System_Object_GetHashCode" target="_blank">GetHashCode</a></td><td>
Serves as the default hash function.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.gettype#System_Object_GetType" target="_blank">GetType</a></td><td>
Gets the <a href="https://docs.microsoft.com/dotnet/api/system.type" target="_blank">Type</a> of the current instance.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.memberwiseclone#System_Object_MemberwiseClone" target="_blank">MemberwiseClone</a></td><td>
Creates a shallow copy of the current <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.tostring#System_Object_ToString" target="_blank">ToString</a></td><td>
Returns a string that represents the current object.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr></table>&nbsp;
<a href="#beforeassemblycreatedeventargs-class">Back to Top</a>

## See Also


#### Reference
<a href="N_SecretNest_RemoteAgency_AssemblyBuilding">SecretNest.RemoteAgency.AssemblyBuilding Namespace</a><br />