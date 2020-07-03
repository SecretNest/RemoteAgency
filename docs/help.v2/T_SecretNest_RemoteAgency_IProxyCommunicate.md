# IProxyCommunicate Interface
 

Represents a created proxy object that can communicate with Remote Agency.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public interface IProxyCommunicate : IManagedObjectCommunicate
```

**VB**<br />
``` VB
Public Interface IProxyCommunicate
	Inherits IManagedObjectCommunicate
```

**C++**<br />
``` C++
public interface class IProxyCommunicate : IManagedObjectCommunicate
```

**F#**<br />
``` F#
type IProxyCommunicate =  
    interface
        interface IManagedObjectCommunicate
    end
```

The IProxyCommunicate type exposes the following members.


## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_IManagedObjectCommunicate_GetSiteIdCallback">GetSiteIdCallback</a></td><td>
Will be called when site id is required.
 (Inherited from <a href="T_SecretNest_RemoteAgency_IManagedObjectCommunicate">IManagedObjectCommunicate</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_IProxyCommunicate_InstanceId">InstanceId</a></td><td>
Gets or sets the id of this proxy instance.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_IProxyCommunicate_ProxyStickyTargetSiteQueryCallback">ProxyStickyTargetSiteQueryCallback</a></td><td>
Will be called for querying the proxy sticky target site state.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_IProxyCommunicate_ProxyStickyTargetSiteResetCallback">ProxyStickyTargetSiteResetCallback</a></td><td>
Will be called for resetting the proxy sticky target site to the original state.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_IProxyCommunicate_SendEventAddingMessageCallback">SendEventAddingMessageCallback</a></td><td>
Will be called while an event adding is requested.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_IProxyCommunicate_SendEventRemovingMessageCallback">SendEventRemovingMessageCallback</a></td><td>
Will be called while an event removing is requested.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_IProxyCommunicate_SendMethodMessageCallback">SendMethodMessageCallback</a></td><td>
Will be called while a method calling message need to be sent to a remote site and get response of it.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_IProxyCommunicate_SendOneWayMethodMessageCallback">SendOneWayMethodMessageCallback</a></td><td>
Will be called while a method calling message need to be sent to a remote site without getting response.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_IProxyCommunicate_SendOneWayPropertyGetMessageCallback">SendOneWayPropertyGetMessageCallback</a></td><td>
Will be called while a property getting message need to be sent to a remote site without getting response.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_IProxyCommunicate_SendOneWayPropertySetMessageCallback">SendOneWayPropertySetMessageCallback</a></td><td>
Will be called while a property setting message need to be sent to a remote site without getting response.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_IProxyCommunicate_SendPropertyGetMessageCallback">SendPropertyGetMessageCallback</a></td><td>
Will be called while a property getting message need to be sent to a remote site and get response of it.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_IProxyCommunicate_SendPropertySetMessageCallback">SendPropertySetMessageCallback</a></td><td>
Will be called while a property setting message need to be sent to a remote site and get response of it.</td></tr></table>&nbsp;
<a href="#iproxycommunicate-interface">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_IManagedObjectCommunicate_CloseRequestedByManagingObject">CloseRequestedByManagingObject</a></td><td>
Sends messages to all relevant objects and closes the functions of this object.
 (Inherited from <a href="T_SecretNest_RemoteAgency_IManagedObjectCommunicate">IManagedObjectCommunicate</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_IProxyCommunicate_ProcessEventRaisingMessage">ProcessEventRaisingMessage</a></td><td>
Processes an event raising message and returns response.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_IProxyCommunicate_ProcessOneWayEventRaisingMessage">ProcessOneWayEventRaisingMessage</a></td><td>
Processes an event raising message.</td></tr></table>&nbsp;
<a href="#iproxycommunicate-interface">Back to Top</a>

## See Also


#### Reference
<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />