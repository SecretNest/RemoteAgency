# InvalidParameterAttributeDataException Class
 

The exception that is thrown when the invalid attribute or data within attribute is found on a parameter.


## Inheritance Hierarchy
<a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">System.Object</a><br />&nbsp;&nbsp;<a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">System.Exception</a><br />&nbsp;&nbsp;&nbsp;&nbsp;<a href="T_SecretNest_RemoteAgency_Inspecting_InvalidAttributeDataException">SecretNest.RemoteAgency.Inspecting.InvalidAttributeDataException</a><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;SecretNest.RemoteAgency.Inspecting.InvalidParameterAttributeDataException<br />
**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Inspecting">SecretNest.RemoteAgency.Inspecting</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
[SerializableAttribute]
public class InvalidParameterAttributeDataException : InvalidAttributeDataException
```

**VB**<br />
``` VB
<SerializableAttribute>
Public Class InvalidParameterAttributeDataException
	Inherits InvalidAttributeDataException
```

**C++**<br />
``` C++
[SerializableAttribute]
public ref class InvalidParameterAttributeDataException : public InvalidAttributeDataException
```

**F#**<br />
``` F#
[<SerializableAttribute>]
type InvalidParameterAttributeDataException =  
    class
        inherit InvalidAttributeDataException
    end
```

The InvalidParameterAttributeDataException type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_SecretNest_RemoteAgency_Inspecting_InvalidParameterAttributeDataException__ctor">InvalidParameterAttributeDataException(SerializationInfo, StreamingContext)</a></td><td>
Initializes a new instance of the InvalidParameterAttributeDataException class with serialized data.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_Inspecting_InvalidParameterAttributeDataException__ctor_1">InvalidParameterAttributeDataException(String, Attribute, ParameterInfo, Stack(MemberInfo))</a></td><td>
Initializes an instance of InvalidParameterAttributeDataException.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_Inspecting_InvalidParameterAttributeDataException__ctor_2">InvalidParameterAttributeDataException(String, Attribute, ParameterInfo, MemberInfo, Stack(MemberInfo))</a></td><td>
Initializes an instance of InvalidParameterAttributeDataException.</td></tr></table>&nbsp;
<a href="#invalidparameterattributedataexception-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_Inspecting_InvalidAttributeDataException_Attribute">Attribute</a></td><td>
Gets the attribute that cause this exception.
 (Inherited from <a href="T_SecretNest_RemoteAgency_Inspecting_InvalidAttributeDataException">InvalidAttributeDataException</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.exception.data#System_Exception_Data" target="_blank">Data</a></td><td>
Gets a collection of key/value pairs that provide additional user-defined information about the exception.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.exception.helplink#System_Exception_HelpLink" target="_blank">HelpLink</a></td><td>
Gets or sets a link to the help file associated with this exception.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.exception.hresult#System_Exception_HResult" target="_blank">HResult</a></td><td>
Gets or sets HRESULT, a coded numerical value that is assigned to a specific exception.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.exception.innerexception#System_Exception_InnerException" target="_blank">InnerException</a></td><td>
Gets the <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a> instance that caused the current exception.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_Inspecting_InvalidAttributeDataException_MemberPath">MemberPath</a></td><td>
Gets the member path.
 (Inherited from <a href="T_SecretNest_RemoteAgency_Inspecting_InvalidAttributeDataException">InvalidAttributeDataException</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.exception.message#System_Exception_Message" target="_blank">Message</a></td><td>
Gets a message that describes the current exception.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_Inspecting_InvalidParameterAttributeDataException_Parameter">Parameter</a></td><td>
Gets the parameter which the attribute is on.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.exception.source#System_Exception_Source" target="_blank">Source</a></td><td>
Gets or sets the name of the application or the object that causes the error.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.exception.stacktrace#System_Exception_StackTrace" target="_blank">StackTrace</a></td><td>
Gets a string representation of the immediate frames on the call stack.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.exception.targetsite#System_Exception_TargetSite" target="_blank">TargetSite</a></td><td>
Gets the method that throws the current exception.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a>.)</td></tr></table>&nbsp;
<a href="#invalidparameterattributedataexception-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.equals#System_Object_Equals_System_Object_" target="_blank">Equals</a></td><td>
Determines whether the specified object is equal to the current object.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.finalize#System_Object_Finalize" target="_blank">Finalize</a></td><td>
Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.exception.getbaseexception#System_Exception_GetBaseException" target="_blank">GetBaseException</a></td><td>
When overridden in a derived class, returns the <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a> that is the root cause of one or more subsequent exceptions.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.gethashcode#System_Object_GetHashCode" target="_blank">GetHashCode</a></td><td>
Serves as the default hash function.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_Inspecting_InvalidParameterAttributeDataException_GetObjectData">GetObjectData</a></td><td>
When overridden in a derived class, sets the <a href="https://docs.microsoft.com/dotnet/api/system.runtime.serialization.serializationinfo" target="_blank">SerializationInfo</a> with information about the exception.
 (Overrides <a href="M_SecretNest_RemoteAgency_Inspecting_InvalidAttributeDataException_GetObjectData">InvalidAttributeDataException.GetObjectData(SerializationInfo, StreamingContext)</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.exception.gettype#System_Exception_GetType" target="_blank">GetType</a></td><td>
Gets the runtime type of the current instance.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.memberwiseclone#System_Object_MemberwiseClone" target="_blank">MemberwiseClone</a></td><td>
Creates a shallow copy of the current <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.exception.tostring#System_Exception_ToString" target="_blank">ToString</a></td><td>
Creates and returns a string representation of the current exception.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a>.)</td></tr></table>&nbsp;
<a href="#invalidparameterattributedataexception-class">Back to Top</a>

## Events
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Protected event](media/protevent.gif "Protected event")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.exception.serializeobjectstate" target="_blank">SerializeObjectState</a></td><td>
Occurs when an exception is serialized to create an exception state object that contains serialized data about the exception.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a>.)</td></tr></table>&nbsp;
<a href="#invalidparameterattributedataexception-class">Back to Top</a>

## See Also


#### Reference
<a href="N_SecretNest_RemoteAgency_Inspecting">SecretNest.RemoteAgency.Inspecting Namespace</a><br />