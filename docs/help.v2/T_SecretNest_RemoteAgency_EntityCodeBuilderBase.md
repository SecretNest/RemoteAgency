# EntityCodeBuilderBase Class
 

Provides code generation for entity classes. This is an abstract class.


## Inheritance Hierarchy
<a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">System.Object</a><br />&nbsp;&nbsp;SecretNest.RemoteAgency.EntityCodeBuilderBase<br />&nbsp;&nbsp;&nbsp;&nbsp;<a href="T_SecretNest_RemoteAgency_BinarySerializer_RemoteAgencyBinarySerializerEntityCodeBuilder">SecretNest.RemoteAgency.BinarySerializer.RemoteAgencyBinarySerializerEntityCodeBuilder</a><br />&nbsp;&nbsp;&nbsp;&nbsp;<a href="T_SecretNest_RemoteAgency_JsonSerializer_RemoteAgencyJsonSerializerEntityCodeBuilder">SecretNest.RemoteAgency.JsonSerializer.RemoteAgencyJsonSerializerEntityCodeBuilder</a><br />
**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public abstract class EntityCodeBuilderBase
```

**VB**<br />
``` VB
Public MustInherit Class EntityCodeBuilderBase
```

**C++**<br />
``` C++
public ref class EntityCodeBuilderBase abstract
```

**F#**<br />
``` F#
[<AbstractClassAttribute>]
type EntityCodeBuilderBase =  class end
```

The EntityCodeBuilderBase type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_SecretNest_RemoteAgency_EntityCodeBuilderBase__ctor">EntityCodeBuilderBase</a></td><td>
Initializes a new instance of the EntityCodeBuilderBase class</td></tr></table>&nbsp;
<a href="#entitycodebuilderbase-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_EntityCodeBuilderBase_AssetLevelAttributeBaseType">AssetLevelAttributeBaseType</a></td><td>
Gets the type of the base class of attributes which are used to mark metadata on asset level.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_EntityCodeBuilderBase_DelegateLevelAttributeBaseType">DelegateLevelAttributeBaseType</a></td><td>
Gets the type of the base class of attributes which are used to mark metadata on delegate level. Only works with processing events.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_EntityCodeBuilderBase_InterfaceLevelAttributeBaseType">InterfaceLevelAttributeBaseType</a></td><td>
Gets the type of the base class of attributes which are used to mark metadata on interface level.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_EntityCodeBuilderBase_ParameterLevelAttributeBaseType">ParameterLevelAttributeBaseType</a></td><td>
Gets the type of the base class of attributes which are used to mark metadata on parameter level.</td></tr></table>&nbsp;
<a href="#entitycodebuilderbase-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_EntityCodeBuilderBase_BuildEntity">BuildEntity</a></td><td>
Builds an entity class type.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_EntityCodeBuilderBase_CreateEmptyMessage">CreateEmptyMessage</a></td><td>
Creates an empty message which is allowed to be serialized.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.equals#System_Object_Equals_System_Object_" target="_blank">Equals</a></td><td>
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
<a href="#entitycodebuilderbase-class">Back to Top</a>

## See Also


#### Reference
<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />