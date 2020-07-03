# SerializingHelperBase(*TSerialized*, *TEntityBase*) Class
 

Provides serializing and deserializing methods for entities. This is an abstract class.


## Inheritance Hierarchy
<a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">System.Object</a><br />&nbsp;&nbsp;SecretNest.RemoteAgency.SerializingHelperBase(TSerialized, TEntityBase)<br />&nbsp;&nbsp;&nbsp;&nbsp;<a href="T_SecretNest_RemoteAgency_BinarySerializer_RemoteAgencyBinarySerializer">SecretNest.RemoteAgency.BinarySerializer.RemoteAgencyBinarySerializer</a><br />&nbsp;&nbsp;&nbsp;&nbsp;<a href="T_SecretNest_RemoteAgency_JsonSerializer_RemoteAgencyJsonSerializer">SecretNest.RemoteAgency.JsonSerializer.RemoteAgencyJsonSerializer</a><br />
**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public abstract class SerializingHelperBase<TSerialized, TEntityBase>

```

**VB**<br />
``` VB
Public MustInherit Class SerializingHelperBase(Of TSerialized, TEntityBase)
```

**C++**<br />
``` C++
generic<typename TSerialized, typename TEntityBase>
public ref class SerializingHelperBase abstract
```

**F#**<br />
``` F#
[<AbstractClassAttribute>]
type SerializingHelperBase<'TSerialized, 'TEntityBase> =  class end
```


#### Type Parameters
&nbsp;<dl><dt>TSerialized</dt><dd>Type of the serialized data.</dd><dt>TEntityBase</dt><dd>Type of the parent class of all entities.</dd></dl>&nbsp;
The SerializingHelperBase(TSerialized, TEntityBase) type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_SecretNest_RemoteAgency_SerializingHelperBase_2__ctor">SerializingHelperBase(TSerialized, TEntityBase)</a></td><td>
Initializes a new instance of the SerializingHelperBase(TSerialized, TEntityBase) class</td></tr></table>&nbsp;
<a href="#serializinghelperbase(*tserialized*,-*tentitybase*)-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_SerializingHelperBase_2_Deserialize">Deserialize</a></td><td>
Deserializes the data to the original format.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_SerializingHelperBase_2_DeserializeWithExceptionTolerance">DeserializeWithExceptionTolerance</a></td><td>
Deserializes the data to the original format with exception redirection.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.equals#System_Object_Equals_System_Object_" target="_blank">Equals</a></td><td>
Determines whether the specified object is equal to the current object.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.finalize#System_Object_Finalize" target="_blank">Finalize</a></td><td>
Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.gethashcode#System_Object_GetHashCode" target="_blank">GetHashCode</a></td><td>
Serves as the default hash function.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.gettype#System_Object_GetType" target="_blank">GetType</a></td><td>
Gets the <a href="https://docs.microsoft.com/dotnet/api/system.type" target="_blank">Type</a> of the current instance.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.memberwiseclone#System_Object_MemberwiseClone" target="_blank">MemberwiseClone</a></td><td>
Creates a shallow copy of the current <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_SerializingHelperBase_2_Serialize">Serialize</a></td><td>
Serializes the entity object.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_SerializingHelperBase_2_SerializeWithExceptionRedirection">SerializeWithExceptionRedirection</a></td><td>
Serializes the entity object with exception redirection.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.tostring#System_Object_ToString" target="_blank">ToString</a></td><td>
Returns a string that represents the current object.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr></table>&nbsp;
<a href="#serializinghelperbase(*tserialized*,-*tentitybase*)-class">Back to Top</a>

## See Also


#### Reference
<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />