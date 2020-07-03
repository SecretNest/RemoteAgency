# MessageBodyEventArgsBase Class
 

Defines a class contains message body derived from <a href="https://docs.microsoft.com/dotnet/api/system.eventargs" target="_blank">EventArgs</a>. This is an abstract class.


## Inheritance Hierarchy
<a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">System.Object</a><br />&nbsp;&nbsp;<a href="https://docs.microsoft.com/dotnet/api/system.eventargs" target="_blank">System.EventArgs</a><br />&nbsp;&nbsp;&nbsp;&nbsp;SecretNest.RemoteAgency.MessageBodyEventArgsBase<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgsBase">SecretNest.RemoteAgency.BeforeMessageProcessingEventArgsBase</a><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="T_SecretNest_RemoteAgency_MessageBodyEventArgs_2">SecretNest.RemoteAgency.MessageBodyEventArgs(TSerialized, TEntityBase)</a><br />
**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public abstract class MessageBodyEventArgsBase : EventArgs
```

**VB**<br />
``` VB
Public MustInherit Class MessageBodyEventArgsBase
	Inherits EventArgs
```

**C++**<br />
``` C++
public ref class MessageBodyEventArgsBase abstract : public EventArgs
```

**F#**<br />
``` F#
[<AbstractClassAttribute>]
type MessageBodyEventArgsBase =  
    class
        inherit EventArgs
    end
```

The MessageBodyEventArgsBase type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_SecretNest_RemoteAgency_MessageBodyEventArgsBase__ctor">MessageBodyEventArgsBase</a></td><td>
Initializes a new instance of the MessageBodyEventArgsBase class</td></tr></table>&nbsp;
<a href="#messagebodyeventargsbase-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_AssetName">AssetName</a></td><td>
Gets the asset name.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_Exception">Exception</a></td><td>
Gets the exception object.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_IsEmptyMessage">IsEmptyMessage</a></td><td>
Gets whether this message is empty, not containing parameters required by asset.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_IsOneWay">IsOneWay</a></td><td>
Gets whether this message is one way (do not need any response).</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_MessageBodyGeneric">MessageBodyGeneric</a></td><td>
Gets the message body.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_MessageId">MessageId</a></td><td>
Gets the message id.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_MessageType">MessageType</a></td><td>
Gets the message type.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_SenderInstanceId">SenderInstanceId</a></td><td>
Gets the instance id of the source proxy or service wrapper.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_SenderSiteId">SenderSiteId</a></td><td>
Gets the site id of the source Remote Agency instance.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_TargetInstanceId">TargetInstanceId</a></td><td>
Gets the instance id of the target proxy or service wrapper.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_TargetSiteId">TargetSiteId</a></td><td>
Gets the site id of the target Remote Agency instance.</td></tr></table>&nbsp;
<a href="#messagebodyeventargsbase-class">Back to Top</a>

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
<a href="#messagebodyeventargsbase-class">Back to Top</a>

## See Also


#### Reference
<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br /><a href="T_SecretNest_RemoteAgency_MessageBodyEventArgs_2">SecretNest.RemoteAgency.MessageBodyEventArgs(TSerialized, TEntityBase)</a><br />