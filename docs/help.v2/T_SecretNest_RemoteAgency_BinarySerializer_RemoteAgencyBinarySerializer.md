# RemoteAgencyBinarySerializer Class
 

Provides binary based serializing and deserializing methods for entities.


## Inheritance Hierarchy
<a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">System.Object</a><br />&nbsp;&nbsp;<a href="T_SecretNest_RemoteAgency_SerializingHelperBase_2">SecretNest.RemoteAgency.SerializingHelperBase</a>(<a href="https://docs.microsoft.com/dotnet/api/system.byte" target="_blank">Byte</a>[], <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>)<br />&nbsp;&nbsp;&nbsp;&nbsp;SecretNest.RemoteAgency.BinarySerializer.RemoteAgencyBinarySerializer<br />
**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_BinarySerializer">SecretNest.RemoteAgency.BinarySerializer</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public class RemoteAgencyBinarySerializer : SerializingHelperBase<byte[], Object>
```

**VB**<br />
``` VB
Public Class RemoteAgencyBinarySerializer
	Inherits SerializingHelperBase(Of Byte(), Object)
```

**C++**<br />
``` C++
public ref class RemoteAgencyBinarySerializer : public SerializingHelperBase<array<unsigned char>^, Object^>
```

**F#**<br />
``` F#
type RemoteAgencyBinarySerializer =  
    class
        inherit SerializingHelperBase<byte[], Object>
    end
```

The RemoteAgencyBinarySerializer type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_BinarySerializer_RemoteAgencyBinarySerializer__ctor">RemoteAgencyBinarySerializer</a></td><td>
Initializes an instance of RemoteAgencyBinarySerializer.</td></tr></table>&nbsp;
<a href="#remoteagencybinaryserializer-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_BinarySerializer_RemoteAgencyBinarySerializer_Deserialize">Deserialize</a></td><td>
Deserializes the data to the original format.
 (Overrides <a href="M_SecretNest_RemoteAgency_SerializingHelperBase_2_Deserialize">SerializingHelperBase(TSerialized, TEntityBase).Deserialize(TSerialized)</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_SerializingHelperBase_2_DeserializeWithExceptionTolerance">DeserializeWithExceptionTolerance</a></td><td>
Deserializes the data to the original format with exception redirection.
 (Inherited from <a href="T_SecretNest_RemoteAgency_SerializingHelperBase_2">SerializingHelperBase(TSerialized, TEntityBase)</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.equals#System_Object_Equals_System_Object_" target="_blank">Equals</a></td><td>
Determines whether the specified object is equal to the current object.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.finalize#System_Object_Finalize" target="_blank">Finalize</a></td><td>
Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.gethashcode#System_Object_GetHashCode" target="_blank">GetHashCode</a></td><td>
Serves as the default hash function.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.gettype#System_Object_GetType" target="_blank">GetType</a></td><td>
Gets the <a href="https://docs.microsoft.com/dotnet/api/system.type" target="_blank">Type</a> of the current instance.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.memberwiseclone#System_Object_MemberwiseClone" target="_blank">MemberwiseClone</a></td><td>
Creates a shallow copy of the current <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_BinarySerializer_RemoteAgencyBinarySerializer_Serialize">Serialize</a></td><td>
Serializes the entity object.
 (Overrides <a href="M_SecretNest_RemoteAgency_SerializingHelperBase_2_Serialize">SerializingHelperBase(TSerialized, TEntityBase).Serialize(TEntityBase)</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_SerializingHelperBase_2_SerializeWithExceptionRedirection">SerializeWithExceptionRedirection</a></td><td>
Serializes the entity object with exception redirection.
 (Inherited from <a href="T_SecretNest_RemoteAgency_SerializingHelperBase_2">SerializingHelperBase(TSerialized, TEntityBase)</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.tostring#System_Object_ToString" target="_blank">ToString</a></td><td>
Returns a string that represents the current object.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr></table>&nbsp;
<a href="#remoteagencybinaryserializer-class">Back to Top</a>

## See Also


#### Reference
<a href="N_SecretNest_RemoteAgency_BinarySerializer">SecretNest.RemoteAgency.BinarySerializer Namespace</a><br />