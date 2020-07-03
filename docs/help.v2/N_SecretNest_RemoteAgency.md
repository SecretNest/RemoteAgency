# SecretNest.RemoteAgency Namespace
 

Remote Agency from SecretNest.info.


## Classes
&nbsp;<table><tr><th></th><th>Class</th><th>Description</th></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_SecretNest_RemoteAgency_AccessingTimeOutException">AccessingTimeOutException</a></td><td>
The exception that is thrown when the accessing is timed out.</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_SecretNest_RemoteAgency_AssetNotFoundException">AssetNotFoundException</a></td><td>
The exception that is thrown when the asset specified cannot be found.</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgs_2">BeforeMessageProcessingEventArgs(TSerialized, TEntityBase)</a></td><td>
Represents a message to be checked for sending or processing after received.</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgsBase">BeforeMessageProcessingEventArgsBase</a></td><td>
Represents a message to be checked for sending or processing after received. This is an abstract class.</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_SecretNest_RemoteAgency_EntityBuilding">EntityBuilding</a></td><td>
Represents an entity class to be created.</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_SecretNest_RemoteAgency_EntityCodeBuilderBase">EntityCodeBuilderBase</a></td><td>
Provides code generation for entity classes. This is an abstract class.</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_SecretNest_RemoteAgency_EntityProperty">EntityProperty</a></td><td>
Represents a property to be created in entity class.</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_SecretNest_RemoteAgency_EntityPropertyAttribute">EntityPropertyAttribute</a></td><td>
Represents an attribute that marked for this property.</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_SecretNest_RemoteAgency_ExceptionRedirectedEventArgs">ExceptionRedirectedEventArgs</a></td><td>
Represents an redirected exception thrown from user code.</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_SecretNest_RemoteAgency_InstanceNotFoundException">InstanceNotFoundException</a></td><td>
The exception that is thrown when the object specified by instance id cannot be found in target instance of Remote Agency.</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_SecretNest_RemoteAgency_MessageBodyEventArgs_2">MessageBodyEventArgs(TSerialized, TEntityBase)</a></td><td>
Defines a class contains message body derived from <a href="https://docs.microsoft.com/dotnet/api/system.eventargs" target="_blank">EventArgs</a> and implemented from <a href="T_SecretNest_RemoteAgency_IMessageBodyGenericEventArgs_2">IMessageBodyGenericEventArgs(TSerialized, TEntityBase)</a>.</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_SecretNest_RemoteAgency_MessageBodyEventArgsBase">MessageBodyEventArgsBase</a></td><td>
Defines a class contains message body derived from <a href="https://docs.microsoft.com/dotnet/api/system.eventargs" target="_blank">EventArgs</a>. This is an abstract class.</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_SecretNest_RemoteAgency_MessageProcessTerminatedException">MessageProcessTerminatedException</a></td><td>
The exception to indicate a message processing is terminated.</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_SecretNest_RemoteAgency_RemoteAgency">RemoteAgency</a></td><td>
Remote Agency from SecretNest.info. This is an abstract class.</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_SecretNest_RemoteAgency_RemoteAgency_2">RemoteAgency(TSerialized, TEntityBase)</a></td><td>
Remote Agency from SecretNest.info.</td></tr><tr><td>![Public class](media/pubclass.gif "Public class")</td><td><a href="T_SecretNest_RemoteAgency_SerializingHelperBase_2">SerializingHelperBase(TSerialized, TEntityBase)</a></td><td>
Provides serializing and deserializing methods for entities. This is an abstract class.</td></tr></table>

## Interfaces
&nbsp;<table><tr><th></th><th>Interface</th><th>Description</th></tr><tr><td>![Public interface](media/pubinterface.gif "Public interface")</td><td><a href="T_SecretNest_RemoteAgency_IManagedObjectCommunicate">IManagedObjectCommunicate</a></td><td>
Represents a created managed object that can communicate with Remote Agency.</td></tr><tr><td>![Public interface](media/pubinterface.gif "Public interface")</td><td><a href="T_SecretNest_RemoteAgency_IMessageBodyGenericEventArgs_2">IMessageBodyGenericEventArgs(TSerialized, TEntityBase)</a></td><td>
Defines property and method to hold the message body and provide an access point for serialization.</td></tr><tr><td>![Public interface](media/pubinterface.gif "Public interface")</td><td><a href="T_SecretNest_RemoteAgency_IProxyCommunicate">IProxyCommunicate</a></td><td>
Represents a created proxy object that can communicate with Remote Agency.</td></tr><tr><td>![Public interface](media/pubinterface.gif "Public interface")</td><td><a href="T_SecretNest_RemoteAgency_IRemoteAgencyMessage">IRemoteAgencyMessage</a></td><td>
Defines the common properties contained in messages.</td></tr><tr><td>![Public interface](media/pubinterface.gif "Public interface")</td><td><a href="T_SecretNest_RemoteAgency_IServiceWrapperCommunicate">IServiceWrapperCommunicate</a></td><td>
Represents a created service wrapper object that can communicate with Remote Agency.</td></tr></table>

## Delegates
&nbsp;<table><tr><th></th><th>Delegate</th><th>Description</th></tr><tr><td>![Public delegate](media/pubdelegate.gif "Public delegate")</td><td><a href="T_SecretNest_RemoteAgency_ProxyStickyTargetSiteQueryCallback">ProxyStickyTargetSiteQueryCallback</a></td><td>
Queries the proxy sticky target site setting state.</td></tr><tr><td>![Public delegate](media/pubdelegate.gif "Public delegate")</td><td><a href="T_SecretNest_RemoteAgency_SendEmptyMessageCallback">SendEmptyMessageCallback</a></td><td>
Sends an empty message out.</td></tr><tr><td>![Public delegate](media/pubdelegate.gif "Public delegate")</td><td><a href="T_SecretNest_RemoteAgency_SendOneWayMessageCallback">SendOneWayMessageCallback</a></td><td>
Sends a message out.</td></tr><tr><td>![Public delegate](media/pubdelegate.gif "Public delegate")</td><td><a href="T_SecretNest_RemoteAgency_SendTwoWayMessageCallback">SendTwoWayMessageCallback</a></td><td>
Sends a message out and gets a response message.</td></tr></table>

## Enumerations
&nbsp;<table><tr><th></th><th>Enumeration</th><th>Description</th></tr><tr><td>![Public enumeration](media/pubenumeration.gif "Public enumeration")</td><td><a href="T_SecretNest_RemoteAgency_AttributePosition">AttributePosition</a></td><td>
Contains a list of position where the attribute can be found.</td></tr><tr><td>![Public enumeration](media/pubenumeration.gif "Public enumeration")</td><td><a href="T_SecretNest_RemoteAgency_MessageDirection">MessageDirection</a></td><td>
Indicates the direction of the message.</td></tr><tr><td>![Public enumeration](media/pubenumeration.gif "Public enumeration")</td><td><a href="T_SecretNest_RemoteAgency_MessageFurtherProcessing">MessageFurtherProcessing</a></td><td>
Defines the further processing of this message.</td></tr><tr><td>![Public enumeration](media/pubenumeration.gif "Public enumeration")</td><td><a href="T_SecretNest_RemoteAgency_MessageProcessTerminatedPosition">MessageProcessTerminatedPosition</a></td><td>
Defines the position where the message can be terminated.</td></tr><tr><td>![Public enumeration](media/pubenumeration.gif "Public enumeration")</td><td><a href="T_SecretNest_RemoteAgency_MessageType">MessageType</a></td><td>
Contains a list of message type</td></tr></table>&nbsp;
