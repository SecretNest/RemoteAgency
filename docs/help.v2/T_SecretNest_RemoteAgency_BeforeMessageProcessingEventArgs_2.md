# BeforeMessageProcessingEventArgs(*TSerialized*, *TEntityBase*) Class
 

Represents a message to be checked for sending or processing after received.


## Inheritance Hierarchy
<a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">System.Object</a><br />&nbsp;&nbsp;<a href="https://docs.microsoft.com/dotnet/api/system.eventargs" target="_blank">System.EventArgs</a><br />&nbsp;&nbsp;&nbsp;&nbsp;<a href="T_SecretNest_RemoteAgency_MessageBodyEventArgsBase">SecretNest.RemoteAgency.MessageBodyEventArgsBase</a><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgsBase">SecretNest.RemoteAgency.BeforeMessageProcessingEventArgsBase</a><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;SecretNest.RemoteAgency.BeforeMessageProcessingEventArgs(TSerialized, TEntityBase)<br />
**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public class BeforeMessageProcessingEventArgs<TSerialized, TEntityBase> : BeforeMessageProcessingEventArgsBase, 
	IMessageBodyGenericEventArgs<TSerialized, TEntityBase>

```

**VB**<br />
``` VB
Public Class BeforeMessageProcessingEventArgs(Of TSerialized, TEntityBase)
	Inherits BeforeMessageProcessingEventArgsBase
	Implements IMessageBodyGenericEventArgs(Of TSerialized, TEntityBase)
```

**C++**<br />
``` C++
generic<typename TSerialized, typename TEntityBase>
public ref class BeforeMessageProcessingEventArgs : public BeforeMessageProcessingEventArgsBase, 
	IMessageBodyGenericEventArgs<TSerialized, TEntityBase>
```

**F#**<br />
``` F#
type BeforeMessageProcessingEventArgs<'TSerialized, 'TEntityBase> =  
    class
        inherit BeforeMessageProcessingEventArgsBase
        interface IMessageBodyGenericEventArgs<'TSerialized, 'TEntityBase>
    end
```


#### Type Parameters
&nbsp;<dl><dt>TSerialized</dt><dd>Type of the serialized data.</dd><dt>TEntityBase</dt><dd>Type of the parent class of all entities.</dd></dl>&nbsp;
The BeforeMessageProcessingEventArgs(TSerialized, TEntityBase) type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgs_2__ctor">BeforeMessageProcessingEventArgs(TSerialized, TEntityBase)</a></td><td>
Initializes an instance of BeforeMessageProcessingEventArgs.</td></tr></table>&nbsp;
<a href="#beforemessageprocessingeventargs(*tserialized*,-*tentitybase*)-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_AssetName">AssetName</a></td><td>
Gets the asset name.
 (Inherited from <a href="T_SecretNest_RemoteAgency_MessageBodyEventArgsBase">MessageBodyEventArgsBase</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_Exception">Exception</a></td><td>
Gets the exception object.
 (Inherited from <a href="T_SecretNest_RemoteAgency_MessageBodyEventArgsBase">MessageBodyEventArgsBase</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgsBase_FurtherProcessing">FurtherProcessing</a></td><td>
Defines the further processing of this message.
 (Inherited from <a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgsBase">BeforeMessageProcessingEventArgsBase</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgs_2_IncludeTerminatedMessageInException">IncludeTerminatedMessageInException</a></td><td>
Gets or sets whether the terminated message should be included in exception. Default value is `true` (`True` in Visual Basic).</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_IsEmptyMessage">IsEmptyMessage</a></td><td>
Gets whether this message is empty, not containing parameters required by asset.
 (Inherited from <a href="T_SecretNest_RemoteAgency_MessageBodyEventArgsBase">MessageBodyEventArgsBase</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_IsOneWay">IsOneWay</a></td><td>
Gets whether this message is one way (do not need any response).
 (Inherited from <a href="T_SecretNest_RemoteAgency_MessageBodyEventArgsBase">MessageBodyEventArgsBase</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgs_2_MessageBody">MessageBody</a></td><td>
Gets the message body.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgs_2_MessageBodyGeneric">MessageBodyGeneric</a></td><td>
Gets the message body.
 (Overrides <a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_MessageBodyGeneric">MessageBodyEventArgsBase.MessageBodyGeneric</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgsBase_MessageDirection">MessageDirection</a></td><td>
Gets the direction of the message.
 (Inherited from <a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgsBase">BeforeMessageProcessingEventArgsBase</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_MessageId">MessageId</a></td><td>
Gets the message id.
 (Inherited from <a href="T_SecretNest_RemoteAgency_MessageBodyEventArgsBase">MessageBodyEventArgsBase</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgsBase_MessageOfMessageProcessTerminatedException">MessageOfMessageProcessTerminatedException</a></td><td>
Gets text will be used as the message of <a href="T_SecretNest_RemoteAgency_MessageProcessTerminatedException">MessageProcessTerminatedException</a>.
 (Inherited from <a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgsBase">BeforeMessageProcessingEventArgsBase</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_MessageType">MessageType</a></td><td>
Gets the message type.
 (Inherited from <a href="T_SecretNest_RemoteAgency_MessageBodyEventArgsBase">MessageBodyEventArgsBase</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_SenderInstanceId">SenderInstanceId</a></td><td>
Gets the instance id of the source proxy or service wrapper.
 (Inherited from <a href="T_SecretNest_RemoteAgency_MessageBodyEventArgsBase">MessageBodyEventArgsBase</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_SenderSiteId">SenderSiteId</a></td><td>
Gets the site id of the source Remote Agency instance.
 (Inherited from <a href="T_SecretNest_RemoteAgency_MessageBodyEventArgsBase">MessageBodyEventArgsBase</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_TargetInstanceId">TargetInstanceId</a></td><td>
Gets the instance id of the target proxy or service wrapper.
 (Inherited from <a href="T_SecretNest_RemoteAgency_MessageBodyEventArgsBase">MessageBodyEventArgsBase</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_TargetSiteId">TargetSiteId</a></td><td>
Gets the site id of the target Remote Agency instance.
 (Inherited from <a href="T_SecretNest_RemoteAgency_MessageBodyEventArgsBase">MessageBodyEventArgsBase</a>.)</td></tr></table>&nbsp;
<a href="#beforemessageprocessingeventargs(*tserialized*,-*tentitybase*)-class">Back to Top</a>

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
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgs_2_Serialize">Serialize</a></td><td>
Serializes this message using the serializer from the Remote Agency instance.</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgsBase_SetToContinue">SetToContinue</a></td><td>
Lets Remote Agency continue processing.
 (Inherited from <a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgsBase">BeforeMessageProcessingEventArgsBase</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgsBase_SetToReplaceWithException">SetToReplaceWithException</a></td><td>
Replaces this message by an instance of <a href="T_SecretNest_RemoteAgency_MessageProcessTerminatedException">MessageProcessTerminatedException</a> then sends it to the receiver. Cannot be used when <a href="P_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgsBase_MessageDirection">MessageDirection</a> is <a href="T_SecretNest_RemoteAgency_MessageDirection">Sending</a>.
 (Inherited from <a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgsBase">BeforeMessageProcessingEventArgsBase</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgsBase_SetToReplaceWithExceptionAndReturn">SetToReplaceWithExceptionAndReturn</a></td><td>
Replaces this message by an instance of <a href="T_SecretNest_RemoteAgency_MessageProcessTerminatedException">MessageProcessTerminatedException</a> then sends it to the receiver and the sender.
 (Inherited from <a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgsBase">BeforeMessageProcessingEventArgsBase</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgsBase_SetToTerminateAndReturnException">SetToTerminateAndReturnException</a></td><td>
Terminates this process and send an instance of <a href="T_SecretNest_RemoteAgency_MessageProcessTerminatedException">MessageProcessTerminatedException</a> back to the sender. Cannot be used when <a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_IsOneWay">IsOneWay</a> is `true` (`True` in Visual Basic).
 (Inherited from <a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgsBase">BeforeMessageProcessingEventArgsBase</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgsBase_SetToTerminateSilently">SetToTerminateSilently</a></td><td>
Terminates this process silently.
 (Inherited from <a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgsBase">BeforeMessageProcessingEventArgsBase</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.tostring#System_Object_ToString" target="_blank">ToString</a></td><td>
Returns a string that represents the current object.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr></table>&nbsp;
<a href="#beforemessageprocessingeventargs(*tserialized*,-*tentitybase*)-class">Back to Top</a>

## See Also


#### Reference
<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br /><a href="T_SecretNest_RemoteAgency_MessageProcessTerminatedException">SecretNest.RemoteAgency.MessageProcessTerminatedException</a><br />