# EventRouterBase Class
 

Defines a helper class to be implanted into built assembly for handling an event handler in service wrapper. This is an abstract class.


## Inheritance Hierarchy
<a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">System.Object</a><br />&nbsp;&nbsp;SecretNest.RemoteAgency.Helper.EventRouterBase<br />
**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Helper">SecretNest.RemoteAgency.Helper</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public abstract class EventRouterBase
```

**VB**<br />
``` VB
Public MustInherit Class EventRouterBase
```

**C++**<br />
``` C++
public ref class EventRouterBase abstract
```

**F#**<br />
``` F#
[<AbstractClassAttribute>]
type EventRouterBase =  class end
```

The EventRouterBase type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_SecretNest_RemoteAgency_Helper_EventRouterBase__ctor">EventRouterBase</a></td><td>
Initializes a new instance of the EventRouterBase class</td></tr></table>&nbsp;
<a href="#eventrouterbase-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_Helper_EventRouterBase_AssetName">AssetName</a></td><td>
Gets or sets the asset name.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_Helper_EventRouterBase_RemoteInstanceId">RemoteInstanceId</a></td><td>
Gets or sets the remote instance id. Remote instance is the instance contains event handler.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_Helper_EventRouterBase_RemoteSiteId">RemoteSiteId</a></td><td>
Gets or sets the remote site id. Remote site is the site contains event handler.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_Helper_EventRouterBase_SendEventMessageCallback">SendEventMessageCallback</a></td><td>
Will be called while an event raising message need to be sent to a remote site and get response of it.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_Helper_EventRouterBase_SendOneWayEventMessageCallback">SendOneWayEventMessageCallback</a></td><td>
Will be called while an event raising message need to be sent to a remote site without getting response.</td></tr></table>&nbsp;
<a href="#eventrouterbase-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_Helper_EventRouterBase_AddHandler">AddHandler</a></td><td>
Adds the handler.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_Helper_EventRouterBase_CloseRequestedByManagingObject">CloseRequestedByManagingObject</a></td><td>
Sends message to relevant object and closes the functions of this object.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.equals#System_Object_Equals_System_Object_" target="_blank">Equals</a></td><td>
Determines whether the specified object is equal to the current object.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.finalize#System_Object_Finalize" target="_blank">Finalize</a></td><td>
Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.gethashcode#System_Object_GetHashCode" target="_blank">GetHashCode</a></td><td>
Serves as the default hash function.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.gettype#System_Object_GetType" target="_blank">GetType</a></td><td>
Gets the <a href="https://docs.microsoft.com/dotnet/api/system.type" target="_blank">Type</a> of the current instance.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.memberwiseclone#System_Object_MemberwiseClone" target="_blank">MemberwiseClone</a></td><td>
Creates a shallow copy of the current <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_Helper_EventRouterBase_RemoveHandler">RemoveHandler</a></td><td>
Removes the handler.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.tostring#System_Object_ToString" target="_blank">ToString</a></td><td>
Returns a string that represents the current object.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr></table>&nbsp;
<a href="#eventrouterbase-class">Back to Top</a>

## See Also


#### Reference
<a href="N_SecretNest_RemoteAgency_Helper">SecretNest.RemoteAgency.Helper Namespace</a><br />