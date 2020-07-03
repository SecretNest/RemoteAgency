# RemoteAgency Class
 

Remote Agency from SecretNest.info. This is an abstract class.


## Inheritance Hierarchy
<a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">System.Object</a><br />&nbsp;&nbsp;SecretNest.RemoteAgency.RemoteAgency<br />&nbsp;&nbsp;&nbsp;&nbsp;<a href="T_SecretNest_RemoteAgency_RemoteAgency_2">SecretNest.RemoteAgency.RemoteAgency(TSerialized, TEntityBase)</a><br />
**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public abstract class RemoteAgency : IDisposable
```

**VB**<br />
``` VB
Public MustInherit Class RemoteAgency
	Implements IDisposable
```

**C++**<br />
``` C++
public ref class RemoteAgency abstract : IDisposable
```

**F#**<br />
``` F#
[<AbstractClassAttribute>]
type RemoteAgency =  
    class
        interface IDisposable
    end
```

The RemoteAgency type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency__ctor">RemoteAgency</a></td><td>
Initializes the instance of Remote Agency.</td></tr></table>&nbsp;
<a href="#remoteagency-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_RemoteAgency_DefaultEventAddingTimeoutForBuilding">DefaultEventAddingTimeoutForBuilding</a></td><td>
Gets or sets default setting for event adding timeout in milliseconds. Default value is 0 (use default value while initializing). Only valid when building type (not on building instance).</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_RemoteAgency_DefaultEventRaisingTimeoutForBuilding">DefaultEventRaisingTimeoutForBuilding</a></td><td>
Gets or sets default setting for event raising timeout in milliseconds. Default value is 0 (use default value while initializing). Only valid when building type (not on building instance).</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_RemoteAgency_DefaultEventRemovingTimeoutForBuilding">DefaultEventRemovingTimeoutForBuilding</a></td><td>
Gets or sets default setting for event removing timeout in milliseconds. Default value is 0 (use default value while initializing). Only valid when building type (not on building instance).</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_RemoteAgency_DefaultMethodCallingTimeoutForBuilding">DefaultMethodCallingTimeoutForBuilding</a></td><td>
Gets of sets default setting for method calling timeout in milliseconds. Default value is 0 (use default value while initializing). Only valid when building type (not on building instance).</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_RemoteAgency_DefaultPropertyGettingTimeoutForBuilding">DefaultPropertyGettingTimeoutForBuilding</a></td><td>
Gets or sets default setting for property getting timeout in milliseconds. Default value is 0 (use default value while initializing). Only valid when building type (not on building instance).</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_RemoteAgency_DefaultPropertySettingTimeoutForBuilding">DefaultPropertySettingTimeoutForBuilding</a></td><td>
Gets or sets default setting for property setting timeout in milliseconds. Default value is 0 (use default value while initializing). Only valid when building type (not on building instance).</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_RemoteAgency_SiteId">SiteId</a></td><td>
Gets or sets the site id of this instance.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_RemoteAgency_WaitingTimeForDisposing">WaitingTimeForDisposing</a></td><td>
Gets or sets the waiting time in milliseconds for waiting a managing object to complete all communication operations before being disposed.</td></tr></table>&nbsp;
<a href="#remoteagency-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_CloseAllInstances">CloseAllInstances</a></td><td>
Closes all proxy and service wrapper objects.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_CloseInstance">CloseInstance</a></td><td>
Closes the proxy or service wrapper by instance id.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_CloseProxy">CloseProxy</a></td><td>
Closes the proxy object.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")![Static member](media/static.gif "Static member")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_Create__2">Create(TSerialized, TEntityBase)</a></td><td>
Creates an instance of Remote Agency.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_CreateProxy">CreateProxy</a></td><td>
Creates proxy of the interface specified.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_CreateServiceWrapper">CreateServiceWrapper</a></td><td>
Creates service wrapper of the interface and the service object specified.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")![Static member](media/static.gif "Static member")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_CreateWithBinarySerializer">CreateWithBinarySerializer</a></td><td>
Creates an instance of Remote Agency using binary serializer.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")![Static member](media/static.gif "Static member")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_CreateWithJsonSerializer">CreateWithJsonSerializer</a></td><td>
Creates an instance of Remote Agency using Json serializer.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_Dispose">Dispose()</a></td><td>
Releases all resources used by this instance.</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_Dispose_1">Dispose(Boolean)</a></td><td>
Disposes of the resources (other than memory) used by this instance.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.equals#System_Object_Equals_System_Object_" target="_blank">Equals</a></td><td>
Determines whether the specified object is equal to the current object.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.finalize#System_Object_Finalize" target="_blank">Finalize</a></td><td>
Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_GenerateEmptyMessage">GenerateEmptyMessage(Guid, Guid, MessageType, String, Guid, Exception)</a></td><td>
Creates an empty message with sender instance id set to <a href="https://docs.microsoft.com/dotnet/api/system.guid.empty" target="_blank">Empty</a> and one way is `true` (`True` in Visual Basic).</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_GenerateEmptyMessage_1">GenerateEmptyMessage(Guid, Guid, Guid, MessageType, String, Guid, Exception, Boolean)</a></td><td>
Creates an empty message.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.gethashcode#System_Object_GetHashCode" target="_blank">GetHashCode</a></td><td>
Serves as the default hash function.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.gettype#System_Object_GetType" target="_blank">GetType</a></td><td>
Gets the <a href="https://docs.microsoft.com/dotnet/api/system.type" target="_blank">Type</a> of the current instance.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.memberwiseclone#System_Object_MemberwiseClone" target="_blank">MemberwiseClone</a></td><td>
Creates a shallow copy of the current <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_OnRemoteProxyClosing">OnRemoteProxyClosing</a></td><td>
Unlinks specified remote proxy from the event registered in service wrapper objects.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_OnRemoteServiceWrapperClosing">OnRemoteServiceWrapperClosing</a></td><td>
Resets sticky target site of all affected proxies when the service wrapper is closing.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_ProxyStickyTargetSiteQuery">ProxyStickyTargetSiteQuery</a></td><td>
Queries the proxy sticky target site setting state.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_ResetProxyStickyTargetSite">ResetProxyStickyTargetSite</a></td><td>
Resets sticky target site of the proxy specified.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_SetExceptionAsResponse">SetExceptionAsResponse</a></td><td>
Sets an exception as the result of a message waiting for response and break the waiting.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.tostring#System_Object_ToString" target="_blank">ToString</a></td><td>
Returns a string that represents the current object.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_TryAddSequentialScheduler">TryAddSequentialScheduler</a></td><td>
Tries to add a task scheduler, which run tasks on a single thread, for accessing assets to the instance of Remote Agency.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_TryAddTaskScheduler">TryAddTaskScheduler</a></td><td>
Tries to add a task scheduler for accessing assets to the instance of Remote Agency.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_TryGetTaskScheduler">TryGetTaskScheduler</a></td><td>
Tries to get a task scheduler from the instance of Remote Agency.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_TryRemoveTaskScheduler">TryRemoveTaskScheduler</a></td><td>
Tries to remove a task scheduler from the instance of Remote Agency.</td></tr></table>&nbsp;
<a href="#remoteagency-class">Back to Top</a>

## Events
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_SecretNest_RemoteAgency_RemoteAgency_AfterTypeAndAssemblyBuilt">AfterTypeAndAssemblyBuilt</a></td><td>
Occurs when an assembly is built.</td></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_SecretNest_RemoteAgency_RemoteAgency_BeforeAssemblyCreated">BeforeAssemblyCreated</a></td><td>
Occurs before module and assembly building finished.</td></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_SecretNest_RemoteAgency_RemoteAgency_BeforeTypeCreated">BeforeTypeCreated</a></td><td>
Occurs before type building finished.</td></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_SecretNest_RemoteAgency_RemoteAgency_ExceptionRedirected">ExceptionRedirected</a></td><td>
Occurs when an exception thrown from user code.</td></tr></table>&nbsp;
<a href="#remoteagency-class">Back to Top</a>

## Fields
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Protected field](media/protfield.gif "Protected field")</td><td><a href="F_SecretNest_RemoteAgency_RemoteAgency_EntityCodeBuilder">EntityCodeBuilder</a></td><td>
Instance of entity code builder.</td></tr></table>&nbsp;
<a href="#remoteagency-class">Back to Top</a>

## See Also


#### Reference
<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br /><a href="T_SecretNest_RemoteAgency_RemoteAgency_2">SecretNest.RemoteAgency.RemoteAgency(TSerialized, TEntityBase)</a><br />