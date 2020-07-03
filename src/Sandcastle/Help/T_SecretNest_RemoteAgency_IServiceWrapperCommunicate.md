# IServiceWrapperCommunicate Interface
 

Represents a created service wrapper object that can communicate with Remote Agency.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public interface IServiceWrapperCommunicate : IManagedObjectCommunicate
```

**VB**<br />
``` VB
Public Interface IServiceWrapperCommunicate
	Inherits IManagedObjectCommunicate
```

**C++**<br />
``` C++
public interface class IServiceWrapperCommunicate : IManagedObjectCommunicate
```

**F#**<br />
``` F#
type IServiceWrapperCommunicate =  
    interface
        interface IManagedObjectCommunicate
    end
```

The IServiceWrapperCommunicate type exposes the following members.


## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_IManagedObjectCommunicate_GetSiteIdCallback">GetSiteIdCallback</a></td><td>
Will be called when site id is required.
 (Inherited from <a href="T_SecretNest_RemoteAgency_IManagedObjectCommunicate">IManagedObjectCommunicate</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_IServiceWrapperCommunicate_SendEventMessageCallback">SendEventMessageCallback</a></td><td>
Will be called while an event raising message need to be sent to a remote site and get response of it.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_IServiceWrapperCommunicate_SendOneWayEventMessageCallback">SendOneWayEventMessageCallback</a></td><td>
Will be called while an event raising message need to be sent to a remote site without getting response.</td></tr></table>&nbsp;
<a href="#iservicewrappercommunicate-interface">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_IManagedObjectCommunicate_CloseRequestedByManagingObject">CloseRequestedByManagingObject</a></td><td>
Sends messages to all relevant objects and closes the functions of this object.
 (Inherited from <a href="T_SecretNest_RemoteAgency_IManagedObjectCommunicate">IManagedObjectCommunicate</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_IServiceWrapperCommunicate_OnRemoteProxyClosing">OnRemoteProxyClosing</a></td><td>
Unlinks specified remote proxy from the event registered in service wrapper objects.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_IServiceWrapperCommunicate_ProcessEventAddingMessage">ProcessEventAddingMessage</a></td><td>
Processes an event adding.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_IServiceWrapperCommunicate_ProcessEventRemovingMessage">ProcessEventRemovingMessage</a></td><td>
Processes an event removing.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_IServiceWrapperCommunicate_ProcessMethodMessage">ProcessMethodMessage</a></td><td>
Processes a method calling message and returns response.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_IServiceWrapperCommunicate_ProcessOneWayMethodMessage">ProcessOneWayMethodMessage</a></td><td>
Processes a one way method calling message.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_IServiceWrapperCommunicate_ProcessOneWayPropertyGettingMessage">ProcessOneWayPropertyGettingMessage</a></td><td>
Processes a property getting message.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_IServiceWrapperCommunicate_ProcessOneWayPropertySettingMessage">ProcessOneWayPropertySettingMessage</a></td><td>
Processes a property setting message.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_IServiceWrapperCommunicate_ProcessPropertyGettingMessage">ProcessPropertyGettingMessage</a></td><td>
Processes a property getting message and returns response.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_IServiceWrapperCommunicate_ProcessPropertySettingMessage">ProcessPropertySettingMessage</a></td><td>
Processes a property setting message and returns response.</td></tr></table>&nbsp;
<a href="#iservicewrappercommunicate-interface">Back to Top</a>

## See Also


#### Reference
<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />