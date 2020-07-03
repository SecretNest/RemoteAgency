# AssetOneWayOperatingAttribute Class
 

Specifies the operating should be one way. No response is required.


## Inheritance Hierarchy
<a href="https://docs.microsoft.com/dotnet/api/system.object" target="_blank">System.Object</a><br />&nbsp;&nbsp;<a href="https://docs.microsoft.com/dotnet/api/system.attribute" target="_blank">System.Attribute</a><br />&nbsp;&nbsp;&nbsp;&nbsp;SecretNest.RemoteAgency.Attributes.AssetOneWayOperatingAttribute<br />
**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public class AssetOneWayOperatingAttribute : Attribute
```

**VB**<br />
``` VB
Public Class AssetOneWayOperatingAttribute
	Inherits Attribute
```

**C++**<br />
``` C++
public ref class AssetOneWayOperatingAttribute : public Attribute
```

**F#**<br />
``` F#
type AssetOneWayOperatingAttribute =  
    class
        inherit Attribute
    end
```

The AssetOneWayOperatingAttribute type exposes the following members.


## Constructors
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_Attributes_AssetOneWayOperatingAttribute__ctor">AssetOneWayOperatingAttribute</a></td><td>
Initializes an instance of AssetOneWayOperatingAttribute.</td></tr></table>&nbsp;
<a href="#assetonewayoperatingattribute-class">Back to Top</a>

## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_Attributes_AssetOneWayOperatingAttribute_IsOneWay">IsOneWay</a></td><td>
Gets whether the operating is one way.</td></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="https://docs.microsoft.com/dotnet/api/system.attribute.typeid#System_Attribute_TypeId" target="_blank">TypeId</a></td><td>
When implemented in a derived class, gets a unique identifier for this <a href="https://docs.microsoft.com/dotnet/api/system.attribute" target="_blank">Attribute</a>.
 (Inherited from <a href="https://docs.microsoft.com/dotnet/api/system.attribute" target="_blank">Attribute</a>.)</td></tr></table>&nbsp;
<a href="#assetonewayoperatingattribute-class">Back to Top</a>

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
<a href="#assetonewayoperatingattribute-class">Back to Top</a>

## Remarks

When <a href="P_SecretNest_RemoteAgency_Attributes_AssetOneWayOperatingAttribute_IsOneWay">IsOneWay</a> is set to `true` (`True` in Visual Basic), any output parameters and return value will always be set to default value due to lack of response, any exception raised from the user code on target site will not be transferred to the caller site.

When <a href="P_SecretNest_RemoteAgency_Attributes_AssetOneWayOperatingAttribute_IsOneWay">IsOneWay</a> is set to `true` (`True` in Visual Basic), <a href="T_SecretNest_RemoteAgency_Attributes_ParameterTwoWayAttribute">ParameterTwoWayAttribute</a>, <a href="T_SecretNest_RemoteAgency_Attributes_ParameterTwoWayPropertyAttribute">ParameterTwoWayPropertyAttribute</a>, <a href="T_SecretNest_RemoteAgency_Attributes_EventParameterTwoWayAttribute">EventParameterTwoWayAttribute</a>, <a href="T_SecretNest_RemoteAgency_Attributes_EventParameterTwoWayPropertyAttribute">EventParameterTwoWayPropertyAttribute</a>, <a href="T_SecretNest_RemoteAgency_Attributes_ReturnIgnoredAttribute">ReturnIgnoredAttribute</a>, <a href="P_SecretNest_RemoteAgency_Attributes_CustomizedPropertyGetEntityNameAttribute_ResponseEntityName">ResponseEntityName</a>, <a href="T_SecretNest_RemoteAgency_Attributes_CustomizedPropertyGetResponsePropertyNameAttribute">CustomizedPropertyGetResponsePropertyNameAttribute</a>, <a href="P_SecretNest_RemoteAgency_Attributes_CustomizedPropertySetEntityNameAttribute_ResponseEntityName">ResponseEntityName</a>, <a href="P_SecretNest_RemoteAgency_Attributes_CustomizedMethodEntityNameAttribute_ReturnValueEntityName">ReturnValueEntityName</a>, <a href="P_SecretNest_RemoteAgency_Attributes_CustomizedEventEntityNameAttribute_RaisingFeedbackEntityName">RaisingFeedbackEntityName</a>, <a href="T_SecretNest_RemoteAgency_Attributes_CustomizedReturnValueEntityPropertyNameAttribute">CustomizedReturnValueEntityPropertyNameAttribute</a> and <a href="T_SecretNest_RemoteAgency_Attributes_OperatingTimeoutTimeAttribute">OperatingTimeoutTimeAttribute</a> on or in the same asset, and the delegate related to the asset if the asset is an event, will be ignored.

By specifying this on event, only event raising operating is affected. Event adding and removing are always two way.

By specifying this on properties, only setting operating is affected. If one way getting operating, which gets from a property but ignore the result and exception from the caller, is expected, mark the property with <a href="T_SecretNest_RemoteAgency_Attributes_PropertyGetOneWayOperatingAttribute">PropertyGetOneWayOperatingAttribute</a>.

The one marked on the event has higher priority than the one marked on the delegate of the same event.

When this attribute is absent, the one way mode of this asset other than property get operating is disabled.


## See Also


#### Reference
<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />