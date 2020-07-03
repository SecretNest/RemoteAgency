# EventParameterTwoWayPropertyAttribute Class
 

Specifies a parameter of the event contains a property or field which value should be send back to the caller.


## Inheritance Hierarchy
<a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">System.Object</a><br />&nbsp;&nbsp;<a href="https://docs.microsoft.com/dotnet/api/system.attribute" target="_blank">System.Attribute</a><br />&nbsp;&nbsp;&nbsp;&nbsp;<a href="T_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute">SecretNest.RemoteAgency.Attributes.ParameterTwoWayPropertyAttribute</a><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;SecretNest.RemoteAgency.Attributes.EventParameterTwoWayPropertyAttribute<br />
**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public class EventParameterTwoWayPropertyAttribute : ParameterTwoWayPropertyAttribute
```

**VB**<br />
``` VB
Public Class EventParameterTwoWayPropertyAttribute
	Inherits ParameterTwoWayPropertyAttribute
```

**C++**<br />
``` C++
public ref class EventParameterTwoWayPropertyAttribute : public ParameterTwoWayPropertyAttribute
```

**F#**<br />
``` F#
type EventParameterTwoWayPropertyAttribute =  
    class
        inherit ParameterTwoWayPropertyAttribute
    end
```

The EventParameterTwoWayPropertyAttribute type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_Attributes_EventParameterTwoWayPropertyAttribute__ctor_1">EventParameterTwoWayPropertyAttribute(String, Type, Boolean)</a></td><td>
Initializes an instance of the EventParameterTwoWayPropertyAttribute. <a href="P_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute_IsSimpleMode">IsSimpleMode</a> will be set to `false` (`False` in Visual Basic).</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_Attributes_EventParameterTwoWayPropertyAttribute__ctor">EventParameterTwoWayPropertyAttribute(String, String, String, Boolean, Boolean)</a></td><td>
Initializes an instance of the EventParameterTwoWayPropertyAttribute. <a href="P_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute_IsSimpleMode">IsSimpleMode</a> will be set to `true` (`True` in Visual Basic).</td></tr></table>&nbsp;
<a href="#eventparametertwowaypropertyattribute-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute_HelperClass">HelperClass</a></td><td>
Gets the type of the helper class.
 (Inherited from <a href="T_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute">ParameterTwoWayPropertyAttribute</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute_IsIncludedWhenExceptionThrown">IsIncludedWhenExceptionThrown</a></td><td>
Gets whether this property should be included in return entity when exception thrown by the user code on the remote site.
 (Inherited from <a href="T_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute">ParameterTwoWayPropertyAttribute</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute_IsSimpleMode">IsSimpleMode</a></td><td>
Gets whether this is in simple mode.
 (Inherited from <a href="T_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute">ParameterTwoWayPropertyAttribute</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute_IsTwoWay">IsTwoWay</a></td><td>
Gets whether properties or fields specified should be included in return entity.
 (Inherited from <a href="T_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute">ParameterTwoWayPropertyAttribute</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_Attributes_EventParameterTwoWayPropertyAttribute_ParameterName">ParameterName</a></td><td>
Gets the parameter name of the event.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute_PropertyNameInParameter">PropertyNameInParameter</a></td><td>
Gets the property name in the parameter which need to be sent back to the caller.
 (Inherited from <a href="T_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute">ParameterTwoWayPropertyAttribute</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute_ResponseEntityPropertyName">ResponseEntityPropertyName</a></td><td>
Gets the preferred property name in response entity.
 (Inherited from <a href="T_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute">ParameterTwoWayPropertyAttribute</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.attribute.typeid#System_Attribute_TypeId" target="_blank">TypeId</a></td><td>
When implemented in a derived class, gets a unique identifier for this <a href="https://docs.microsoft.com/dotnet/api/system.attribute" target="_blank">Attribute</a>.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.attribute" target="_blank">Attribute</a>.)</td></tr></table>&nbsp;
<a href="#eventparametertwowaypropertyattribute-class">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.attribute.equals#System_Attribute_Equals_System_Object_" target="_blank">Equals</a></td><td>
Returns a value that indicates whether this instance is equal to a specified object.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.attribute" target="_blank">Attribute</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.finalize#System_Object_Finalize" target="_blank">Finalize</a></td><td>
Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.attribute.gethashcode#System_Attribute_GetHashCode" target="_blank">GetHashCode</a></td><td>
Returns the hash code for this instance.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.attribute" target="_blank">Attribute</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.gettype#System_Object_GetType" target="_blank">GetType</a></td><td>
Gets the <a href="https://docs.microsoft.com/dotnet/api/system.type" target="_blank">Type</a> of the current instance.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.attribute.isdefaultattribute#System_Attribute_IsDefaultAttribute" target="_blank">IsDefaultAttribute</a></td><td>
When overridden in a derived class, indicates whether the value of this instance is the default value for the derived class.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.attribute" target="_blank">Attribute</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.attribute.match#System_Attribute_Match_System_Object_" target="_blank">Match</a></td><td>
When overridden in a derived class, returns a value that indicates whether this instance equals a specified object.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.attribute" target="_blank">Attribute</a>.)</td></tr><tr><td>![Protected method](media/protmethod.gif "Protected method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.memberwiseclone#System_Object_MemberwiseClone" target="_blank">MemberwiseClone</a></td><td>
Creates a shallow copy of the current <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.object.tostring#System_Object_ToString" target="_blank">ToString</a></td><td>
Returns a string that represents the current object.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">Object</a>.)</td></tr></table>&nbsp;
<a href="#eventparametertwowaypropertyattribute-class">Back to Top</a>

## Remarks

When a parameter contains properties or fields which may be changed on the target site and need to be sent back to the caller, use EventParameterTwoWayPropertyAttribute or <a href="T_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute">ParameterTwoWayPropertyAttribute</a> on related properties.

When a parameter marked with "ref / ByRef", the value of the parameter will be passed back to the caller. Due to lack of tracking information, regardless of whether this parameter contains changed properties or fields, the whole object will be transferred and replaced. If this is not the expected operation, use EventParameterTwoWayPropertyAttribute or <a href="T_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute">ParameterTwoWayPropertyAttribute</a> on related properties and fields instead of marking "ref / ByRef".

<a href="T_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute">ParameterTwoWayPropertyAttribute</a> can be marked on parameters of the delegate related to this event, with lower priority than EventParameterTwoWayPropertyAttribute.

Without EventParameterTwoWayPropertyAttribute or <a href="T_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute">ParameterTwoWayPropertyAttribute</a> specified, properties will not be send back to the caller unless the parameter is marked with "ref / ByRef".


## See Also


#### Reference
<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />