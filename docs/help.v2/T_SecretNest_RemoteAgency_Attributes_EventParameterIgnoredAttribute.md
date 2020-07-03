# EventParameterIgnoredAttribute Class
 

Specifies a parameter of the event should or should not be transferred to remote site.


## Inheritance Hierarchy
<a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">System.Object</a><br />&nbsp;&nbsp;<a href="https://docs.microsoft.com/dotnet/api/system.attribute" target="_blank">System.Attribute</a><br />&nbsp;&nbsp;&nbsp;&nbsp;<a href="T_SecretNest_RemoteAgency_Attributes_ParameterIgnoredAttribute">SecretNest.RemoteAgency.Attributes.ParameterIgnoredAttribute</a><br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;SecretNest.RemoteAgency.Attributes.EventParameterIgnoredAttribute<br />
**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public class EventParameterIgnoredAttribute : ParameterIgnoredAttribute
```

**VB**<br />
``` VB
Public Class EventParameterIgnoredAttribute
	Inherits ParameterIgnoredAttribute
```

**C++**<br />
``` C++
public ref class EventParameterIgnoredAttribute : public ParameterIgnoredAttribute
```

**F#**<br />
``` F#
type EventParameterIgnoredAttribute =  
    class
        inherit ParameterIgnoredAttribute
    end
```

The EventParameterIgnoredAttribute type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_Attributes_EventParameterIgnoredAttribute__ctor">EventParameterIgnoredAttribute</a></td><td>
Initializes an instance of the EventParameterIgnoredAttribute.</td></tr></table>&nbsp;
<a href="#eventparameterignoredattribute-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_Attributes_ParameterIgnoredAttribute_IsIgnored">IsIgnored</a></td><td>
Gets whether this parameter is excluded from parameter entity. If set to true, this parameter should not be transferred to remote site.
 (Inherited from <a href="T_SecretNest_RemoteAgency_Attributes_ParameterIgnoredAttribute">ParameterIgnoredAttribute</a>.)</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_Attributes_EventParameterIgnoredAttribute_ParameterName">ParameterName</a></td><td>
Gets the parameter name of the event.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.attribute.typeid#System_Attribute_TypeId" target="_blank">TypeId</a></td><td>
When implemented in a derived class, gets a unique identifier for this <a href="https://docs.microsoft.com/dotnet/api/system.attribute" target="_blank">Attribute</a>.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.attribute" target="_blank">Attribute</a>.)</td></tr></table>&nbsp;
<a href="#eventparameterignoredattribute-class">Back to Top</a>

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
<a href="#eventparameterignoredattribute-class">Back to Top</a>

## Remarks

<a href="T_SecretNest_RemoteAgency_Attributes_ParameterIgnoredAttribute">ParameterIgnoredAttribute</a> can be marked on parameters of the delegate related to this event, with lower priority than EventParameterIgnoredAttribute.

When <a href="P_SecretNest_RemoteAgency_Attributes_ParameterIgnoredAttribute_IsIgnored">IsIgnored</a> is set to `true` (`True` in Visual Basic), <a href="T_SecretNest_RemoteAgency_Attributes_ParameterTwoWayAttribute">ParameterTwoWayAttribute</a>, <a href="T_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute">ParameterTwoWayPropertyAttribute</a>, <a href="T_SecretNest_RemoteAgency_Attributes_EventParameterTwoWayAttribute">EventParameterTwoWayAttribute</a>, <a href="T_SecretNest_RemoteAgency_Attributes_EventParameterTwoWayPropertyAttribute">EventParameterTwoWayPropertyAttribute</a>, <a href="T_SecretNest_RemoteAgency_Attributes_CustomizedEventParameterEntityPropertyNameAttribute">CustomizedEventParameterEntityPropertyNameAttribute</a> and <a href="T_SecretNest_RemoteAgency_Attributes_CustomizedParameterEntityPropertyNameAttribute">CustomizedParameterEntityPropertyNameAttribute</a> on or in the same parameter of the asset and the delegate related to the asset if the asset is an event, will be ignored.

Without EventParameterIgnoredAttribute or <a href="T_SecretNest_RemoteAgency_Attributes_ParameterIgnoredAttribute">ParameterIgnoredAttribute</a> specified, no parameter is ignored.


## See Also


#### Reference
<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />