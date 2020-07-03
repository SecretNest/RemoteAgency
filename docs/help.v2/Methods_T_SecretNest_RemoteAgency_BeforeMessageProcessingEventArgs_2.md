# BeforeMessageProcessingEventArgs(*TSerialized*, *TEntityBase*) Methods
 

The <a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgs_2">BeforeMessageProcessingEventArgs(TSerialized, TEntityBase)</a> generic type exposes the following members.


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
<a href="#beforemessageprocessingeventargs(*tserialized*,-*tentitybase*)-methods">Back to Top</a>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgs_2">BeforeMessageProcessingEventArgs(TSerialized, TEntityBase) Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />