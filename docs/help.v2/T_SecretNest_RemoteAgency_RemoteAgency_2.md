# RemoteAgency(*TSerialized*, *TEntityBase*) Class
 

Remote Agency from SecretNest.info.


## Inheritance Hierarchy
<a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">System.Object</a><br />&nbsp;&nbsp;<a href="T_SecretNest_RemoteAgency_RemoteAgency">SecretNest.RemoteAgency.RemoteAgency</a><br />&nbsp;&nbsp;&nbsp;&nbsp;SecretNest.RemoteAgency.RemoteAgency(TSerialized, TEntityBase)<br />
**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public sealed class RemoteAgency<TSerialized, TEntityBase> : RemoteAgency

```

**VB**<br />
``` VB
Public NotInheritable Class RemoteAgency(Of TSerialized, TEntityBase)
	Inherits RemoteAgency
```

**C++**<br />
``` C++
generic<typename TSerialized, typename TEntityBase>
public ref class RemoteAgency sealed : public RemoteAgency
```

**F#**<br />
``` F#
[<SealedAttribute>]
type RemoteAgency<'TSerialized, 'TEntityBase> =  
    class
        inherit RemoteAgency
    end
```


#### Type Parameters
&nbsp;<dl><dt>TSerialized</dt><dd>Type of the serialized data.</dd><dt>TEntityBase</dt><dd>Type of the parent class of all entities.</dd></dl>&nbsp;
The RemoteAgency(TSerialized, TEntityBase) type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_2__ctor">RemoteAgency(TSerialized, TEntityBase)</a></td><td>
Initializes an instance of Remote Agency.</td></tr></table>&nbsp;
<a href="#remoteagency(*tserialized*,-*tentitybase*)-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_RemoteAgency_2_BypassSystemMessagesFromFiltering">BypassSystemMessagesFromFiltering</a></td><td>
Gets or sets whether system messages should be bypassed from filtering. Default value is `true` (`True` in Visual Basic).</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_RemoteAgency_DefaultEventAddingTimeoutForBuilding">DefaultEventAddingTimeoutForBuilding</a></td><td>
Gets or sets default setting for event adding timeout in milliseconds. Default value is 0 (use default value while initializing). Only valid when building type (not on building instance).
 (Inherited from <a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_RemoteAgency_DefaultEventRaisingTimeoutForBuilding">DefaultEventRaisingTimeoutForBuilding</a></td><td>
Gets or sets default setting for event raising timeout in milliseconds. Default value is 0 (use default value while initializing). Only valid when building type (not on building instance).
 (Inherited from <a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_RemoteAgency_DefaultEventRemovingTimeoutForBuilding">DefaultEventRemovingTimeoutForBuilding</a></td><td>
Gets or sets default setting for event removing timeout in milliseconds. Default value is 0 (use default value while initializing). Only valid when building type (not on building instance).
 (Inherited from <a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_RemoteAgency_DefaultMethodCallingTimeoutForBuilding">DefaultMethodCallingTimeoutForBuilding</a></td><td>
Gets of sets default setting for method calling timeout in milliseconds. Default value is 0 (use default value while initializing). Only valid when building type (not on building instance).
 (Inherited from <a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_RemoteAgency_DefaultPropertyGettingTimeoutForBuilding">DefaultPropertyGettingTimeoutForBuilding</a></td><td>
Gets or sets default setting for property getting timeout in milliseconds. Default value is 0 (use default value while initializing). Only valid when building type (not on building instance).
 (Inherited from <a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_RemoteAgency_DefaultPropertySettingTimeoutForBuilding">DefaultPropertySettingTimeoutForBuilding</a></td><td>
Gets or sets default setting for property setting timeout in milliseconds. Default value is 0 (use default value while initializing). Only valid when building type (not on building instance).
 (Inherited from <a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_RemoteAgency_SiteId">SiteId</a></td><td>
Gets or sets the site id of this instance.
 (Inherited from <a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_RemoteAgency_WaitingTimeForDisposing">WaitingTimeForDisposing</a></td><td>
Gets or sets the waiting time in milliseconds for waiting a managing object to complete all communication operations before being disposed.
 (Inherited from <a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency</a>.)</td></tr></table>&nbsp;
<a href="#remoteagency(*tserialized*,-*tentitybase*)-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_2_CloseAllInstances">CloseAllInstances</a></td><td>
Closes all proxy and service wrapper objects.
 (Overrides <a href="M_SecretNest_RemoteAgency_RemoteAgency_CloseAllInstances">RemoteAgency.CloseAllInstances()</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_2_CloseInstance">CloseInstance</a></td><td>
Closes the proxy or service wrapper by instance id.
 (Overrides <a href="M_SecretNest_RemoteAgency_RemoteAgency_CloseInstance">RemoteAgency.CloseInstance(Guid)</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_CloseProxy">CloseProxy</a></td><td>
Closes the proxy object.
 (Inherited from <a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_2_CreateProxy">CreateProxy(Type, Guid, Guid, Guid, Int32, Boolean)</a></td><td>
Creates proxy of the interface specified.
 (Overrides <a href="M_SecretNest_RemoteAgency_RemoteAgency_CreateProxy">RemoteAgency.CreateProxy(Type, Guid, Guid, Guid, Int32, Boolean)</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_2_CreateProxy__1">CreateProxy(TInterface)(Guid, Guid, Guid, Int32, Boolean)</a></td><td>
Creates proxy of the interface specified.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_2_CreateServiceWrapper">CreateServiceWrapper(Type, Object, Guid, Int32, Boolean)</a></td><td>
Creates service wrapper of the interface and the service object specified.
 (Overrides <a href="M_SecretNest_RemoteAgency_RemoteAgency_CreateServiceWrapper">RemoteAgency.CreateServiceWrapper(Type, Object, Guid, Int32, Boolean)</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_2_CreateServiceWrapper__1">CreateServiceWrapper(TInterface)(TInterface, Guid, Int32, Boolean)</a></td><td>
Creates service wrapper of the interface and the service object specified.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_2_Deserialize">Deserialize</a></td><td>
Deserializes data using the serializer passed from constructor.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_Dispose">Dispose()</a></td><td>
Releases all resources used by this instance.
 (Inherited from <a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.equals#System_Object_Equals_System_Object_" target="_blank">Equals</a></td><td>
Determines whether the specified object is equal to the current object.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.gethashcode#System_Object_GetHashCode" target="_blank">GetHashCode</a></td><td>
Serves as the default hash function.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.gettype#System_Object_GetType" target="_blank">GetType</a></td><td>
Gets the <a href="https://docs.microsoft.com/dotnet/api/system.type" target="_blank">Type</a> of the current instance.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_2_OnRemoteProxyClosing">OnRemoteProxyClosing</a></td><td>
Unlinks specified remote proxy from the event registered in service wrapper objects.
 (Overrides <a href="M_SecretNest_RemoteAgency_RemoteAgency_OnRemoteProxyClosing">RemoteAgency.OnRemoteProxyClosing(Guid, Nullable(Guid))</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_2_OnRemoteServiceWrapperClosing">OnRemoteServiceWrapperClosing</a></td><td>
Resets sticky target site of all affected proxies when the service wrapper is closing.
 (Overrides <a href="M_SecretNest_RemoteAgency_RemoteAgency_OnRemoteServiceWrapperClosing">RemoteAgency.OnRemoteServiceWrapperClosing(Guid, Nullable(Guid))</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_2_ProcessReceivedMessage_1">ProcessReceivedMessage(TEntityBase)</a></td><td>
Processes a message received.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_2_ProcessReceivedMessage">ProcessReceivedMessage(IRemoteAgencyMessage)</a></td><td>
Processes a message received.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_2_ProcessReceivedSerializedMessage">ProcessReceivedSerializedMessage</a></td><td>
Processes a serialized message received.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_ProxyStickyTargetSiteQuery">ProxyStickyTargetSiteQuery</a></td><td>
Queries the proxy sticky target site setting state.
 (Inherited from <a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_ResetProxyStickyTargetSite">ResetProxyStickyTargetSite</a></td><td>
Resets sticky target site of the proxy specified.
 (Inherited from <a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_2_Serialize">Serialize</a></td><td>
Serializes an entity using the serializer passed from constructor.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_SetExceptionAsResponse">SetExceptionAsResponse</a></td><td>
Sets an exception as the result of a message waiting for response and break the waiting.
 (Inherited from <a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.tostring#System_Object_ToString" target="_blank">ToString</a></td><td>
Returns a string that represents the current object.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_TryAddSequentialScheduler">TryAddSequentialScheduler</a></td><td>
Tries to add a task scheduler, which run tasks on a single thread, for accessing assets to the instance of Remote Agency.
 (Inherited from <a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_TryAddTaskScheduler">TryAddTaskScheduler</a></td><td>
Tries to add a task scheduler for accessing assets to the instance of Remote Agency.
 (Inherited from <a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_TryGetTaskScheduler">TryGetTaskScheduler</a></td><td>
Tries to get a task scheduler from the instance of Remote Agency.
 (Inherited from <a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_RemoteAgency_TryRemoveTaskScheduler">TryRemoveTaskScheduler</a></td><td>
Tries to remove a task scheduler from the instance of Remote Agency.
 (Inherited from <a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency</a>.)</td></tr></table>&nbsp;
<a href="#remoteagency(*tserialized*,-*tentitybase*)-class">Back to Top</a>

## Events
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_SecretNest_RemoteAgency_RemoteAgency_2_AfterMessageReceived">AfterMessageReceived</a></td><td>
Occurs when a message need to be checked for sending.</td></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_SecretNest_RemoteAgency_RemoteAgency_AfterTypeAndAssemblyBuilt">AfterTypeAndAssemblyBuilt</a></td><td>
Occurs when an assembly is built.
 (Inherited from <a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency</a>.)</td></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_SecretNest_RemoteAgency_RemoteAgency_BeforeAssemblyCreated">BeforeAssemblyCreated</a></td><td>
Occurs before module and assembly building finished.
 (Inherited from <a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency</a>.)</td></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_SecretNest_RemoteAgency_RemoteAgency_2_BeforeMessageSending">BeforeMessageSending</a></td><td>
Occurs when a message need to be checked for sending.</td></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_SecretNest_RemoteAgency_RemoteAgency_BeforeTypeCreated">BeforeTypeCreated</a></td><td>
Occurs before type building finished.
 (Inherited from <a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency</a>.)</td></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_SecretNest_RemoteAgency_RemoteAgency_ExceptionRedirected">ExceptionRedirected</a></td><td>
Occurs when an exception thrown from user code.
 (Inherited from <a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency</a>.)</td></tr><tr><td>![Public event](media/pubevent.gif "Public event")</td><td><a href="E_SecretNest_RemoteAgency_RemoteAgency_2_MessageForSendingPrepared">MessageForSendingPrepared</a></td><td>
Occurs when a message is generated and ready to be sent.</td></tr></table>&nbsp;
<a href="#remoteagency(*tserialized*,-*tentitybase*)-class">Back to Top</a>

## See Also


#### Reference
<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />