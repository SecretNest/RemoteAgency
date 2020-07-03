# RemoteAgencyBinarySerializerEntityCodeBuilder Class
 

Provides code generating for entity classes working with <a href="T_SecretNest_RemoteAgency_BinarySerializer_RemoteAgencyBinarySerializer">RemoteAgencyBinarySerializer</a>


## Inheritance Hierarchy
<a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">System.Object</a><br />&nbsp;&nbsp;<a href="T_SecretNest_RemoteAgency_EntityCodeBuilderBase">SecretNest.RemoteAgency.EntityCodeBuilderBase</a><br />&nbsp;&nbsp;&nbsp;&nbsp;SecretNest.RemoteAgency.BinarySerializer.RemoteAgencyBinarySerializerEntityCodeBuilder<br />
**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_BinarySerializer">SecretNest.RemoteAgency.BinarySerializer</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public class RemoteAgencyBinarySerializerEntityCodeBuilder : EntityCodeBuilderBase
```

**VB**<br />
``` VB
Public Class RemoteAgencyBinarySerializerEntityCodeBuilder
	Inherits EntityCodeBuilderBase
```

**C++**<br />
``` C++
public ref class RemoteAgencyBinarySerializerEntityCodeBuilder : public EntityCodeBuilderBase
```

**F#**<br />
``` F#
type RemoteAgencyBinarySerializerEntityCodeBuilder =  
    class
        inherit EntityCodeBuilderBase
    end
```

The RemoteAgencyBinarySerializerEntityCodeBuilder type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_BinarySerializer_RemoteAgencyBinarySerializerEntityCodeBuilder__ctor">RemoteAgencyBinarySerializerEntityCodeBuilder</a></td><td>
Initializes a new instance of the RemoteAgencyBinarySerializerEntityCodeBuilder class</td></tr></table>&nbsp;
<a href="#remoteagencybinaryserializerentitycodebuilder-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_BinarySerializer_RemoteAgencyBinarySerializerEntityCodeBuilder_AssetLevelAttributeBaseType">AssetLevelAttributeBaseType</a></td><td>
Gets the type of the base class of attributes which are used to mark metadata on asset level.
 (Overrides <a href="P_SecretNest_RemoteAgency_EntityCodeBuilderBase_AssetLevelAttributeBaseType">EntityCodeBuilderBase.AssetLevelAttributeBaseType</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_BinarySerializer_RemoteAgencyBinarySerializerEntityCodeBuilder_DelegateLevelAttributeBaseType">DelegateLevelAttributeBaseType</a></td><td>
Gets the type of the base class of attributes which are used to mark metadata on delegate level. Only works with processing events.
 (Overrides <a href="P_SecretNest_RemoteAgency_EntityCodeBuilderBase_DelegateLevelAttributeBaseType">EntityCodeBuilderBase.DelegateLevelAttributeBaseType</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_BinarySerializer_RemoteAgencyBinarySerializerEntityCodeBuilder_InterfaceLevelAttributeBaseType">InterfaceLevelAttributeBaseType</a></td><td>
Gets the type of the base class of attributes which are used to mark metadata on interface level.
 (Overrides <a href="P_SecretNest_RemoteAgency_EntityCodeBuilderBase_InterfaceLevelAttributeBaseType">EntityCodeBuilderBase.InterfaceLevelAttributeBaseType</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_BinarySerializer_RemoteAgencyBinarySerializerEntityCodeBuilder_ParameterLevelAttributeBaseType">ParameterLevelAttributeBaseType</a></td><td>
Gets the type of the base class of attributes which are used to mark metadata on parameter level.
 (Overrides <a href="P_SecretNest_RemoteAgency_EntityCodeBuilderBase_ParameterLevelAttributeBaseType">EntityCodeBuilderBase.ParameterLevelAttributeBaseType</a>.)</td></tr></table>&nbsp;
<a href="#remoteagencybinaryserializerentitycodebuilder-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_EntityCodeBuilderBase_BuildEntity">BuildEntity(TypeBuilder, EntityBuilding)</a></td><td>
Builds an entity class type.
 (Inherited from <a href="T_SecretNest_RemoteAgency_EntityCodeBuilderBase">EntityCodeBuilderBase</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_BinarySerializer_RemoteAgencyBinarySerializerEntityCodeBuilder_BuildEntity">BuildEntity(TypeBuilder, EntityBuilding)</a></td><td>
Builds an entity class type.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_BinarySerializer_RemoteAgencyBinarySerializerEntityCodeBuilder_CreateEmptyMessage">CreateEmptyMessage</a></td><td>
Creates an empty message which is allowed to be serialized.
 (Overrides <a href="M_SecretNest_RemoteAgency_EntityCodeBuilderBase_CreateEmptyMessage">EntityCodeBuilderBase.CreateEmptyMessage()</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.equals#System_Object_Equals_System_Object_" target="_blank">Equals</a></td><td>
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
<a href="#remoteagencybinaryserializerentitycodebuilder-class">Back to Top</a>

## See Also


#### Reference
<a href="N_SecretNest_RemoteAgency_BinarySerializer">SecretNest.RemoteAgency.BinarySerializer Namespace</a><br />