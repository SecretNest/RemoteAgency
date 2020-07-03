# AccessingTimeOutException Class
 

The exception that is thrown when the accessing is timed out.


## Inheritance Hierarchy
<a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">System.Object</a><br />&nbsp;&nbsp;<a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">System.Exception</a><br />&nbsp;&nbsp;&nbsp;&nbsp;<a href="https://docs.microsoft.com/dotnet/api/system.systemexception" target="_blank">System.SystemException</a><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="https://docs.microsoft.com/dotnet/api/system.timeoutexception" target="_blank">System.TimeoutException</a><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;SecretNest.RemoteAgency.AccessingTimeOutException<br />
**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
[SerializableAttribute]
public sealed class AccessingTimeOutException : TimeoutException
```

**VB**<br />
``` VB
<SerializableAttribute>
Public NotInheritable Class AccessingTimeOutException
	Inherits TimeoutException
```

**C++**<br />
``` C++
[SerializableAttribute]
public ref class AccessingTimeOutException sealed : public TimeoutException
```

**F#**<br />
``` F#
[<SealedAttribute>]
[<SerializableAttribute>]
type AccessingTimeOutException =  
    class
        inherit TimeoutException
    end
```

The AccessingTimeOutException type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_AccessingTimeOutException__ctor">AccessingTimeOutException</a></td><td>
Initializes an instance of AccessingTimeOutException.</td></tr></table>&nbsp;
<a href="#accessingtimeoutexception-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.exception.data#System_Exception_Data" target="_blank">Data</a></td><td>
Gets a collection of key/value pairs that provide additional user-defined information about the exception.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.exception.helplink#System_Exception_HelpLink" target="_blank">HelpLink</a></td><td>
Gets or sets a link to the help file associated with this exception.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.exception.hresult#System_Exception_HResult" target="_blank">HResult</a></td><td>
Gets or sets HRESULT, a coded numerical value that is assigned to a specific exception.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.exception.innerexception#System_Exception_InnerException" target="_blank">InnerException</a></td><td>
Gets the <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a> instance that caused the current exception.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.exception.message#System_Exception_Message" target="_blank">Message</a></td><td>
Gets a message that describes the current exception.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_AccessingTimeOutException_OriginalMessage">OriginalMessage</a></td><td>
Gets the message which causes this exception thrown.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.exception.source#System_Exception_Source" target="_blank">Source</a></td><td>
Gets or sets the name of the application or the object that causes the error.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.exception.stacktrace#System_Exception_StackTrace" target="_blank">StackTrace</a></td><td>
Gets a string representation of the immediate frames on the call stack.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.exception.targetsite#System_Exception_TargetSite" target="_blank">TargetSite</a></td><td>
Gets the method that throws the current exception.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a>.)</td></tr></table>&nbsp;
<a href="#accessingtimeoutexception-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.equals#System_Object_Equals_System_Object_" target="_blank">Equals</a></td><td>
Determines whether the specified object is equal to the current object.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.exception.getbaseexception#System_Exception_GetBaseException" target="_blank">GetBaseException</a></td><td>
When overridden in a derived class, returns the <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a> that is the root cause of one or more subsequent exceptions.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.gethashcode#System_Object_GetHashCode" target="_blank">GetHashCode</a></td><td>
Serves as the default hash function.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_AccessingTimeOutException_GetObjectData">GetObjectData</a></td><td>
When overridden in a derived class, sets the <a href="https://docs.microsoft.com/dotnet/api/system.runtime.serialization.serializationinfo" target="_blank">SerializationInfo</a> with information about the exception.
 (Overrides <a href="https://docs.microsoft.com/dotnet/api/system.exception.getobjectdata#System_Exception_GetObjectData_System_Runtime_Serialization_SerializationInfo_System_Runtime_Serialization_StreamingContext_" target="_blank">Exception.GetObjectData(SerializationInfo, StreamingContext)</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.exception.gettype#System_Exception_GetType" target="_blank">GetType</a></td><td>
Gets the runtime type of the current instance.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.exception.tostring#System_Exception_ToString" target="_blank">ToString</a></td><td>
Creates and returns a string representation of the current exception.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.exception" target="_blank">Exception</a>.)</td></tr></table>&nbsp;
<a href="#accessingtimeoutexception-class">Back to Top</a>

## See Also


#### Reference
<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />