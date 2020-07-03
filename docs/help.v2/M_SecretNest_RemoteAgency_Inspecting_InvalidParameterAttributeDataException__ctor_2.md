# InvalidParameterAttributeDataException Constructor (String, Attribute, ParameterInfo, MemberInfo, Stack(MemberInfo))
 

Initializes an instance of InvalidParameterAttributeDataException.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Inspecting">SecretNest.RemoteAgency.Inspecting</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public InvalidParameterAttributeDataException(
	string message,
	Attribute attribute,
	ParameterInfo parameter,
	MemberInfo memberInfo,
	Stack<MemberInfo> memberParentPath
)
```

**VB**<br />
``` VB
Public Sub New ( 
	message As String,
	attribute As Attribute,
	parameter As ParameterInfo,
	memberInfo As MemberInfo,
	memberParentPath As Stack(Of MemberInfo)
)
```

**C++**<br />
``` C++
public:
InvalidParameterAttributeDataException(
	String^ message, 
	Attribute^ attribute, 
	ParameterInfo^ parameter, 
	MemberInfo^ memberInfo, 
	Stack<MemberInfo^>^ memberParentPath
)
```

**F#**<br />
``` F#
new : 
        message : string * 
        attribute : Attribute * 
        parameter : ParameterInfo * 
        memberInfo : MemberInfo * 
        memberParentPath : Stack<MemberInfo> -> InvalidParameterAttributeDataException
```


#### Parameters
&nbsp;<dl><dt>message</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Exception message.</dd><dt>attribute</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.attribute" target="_blank">System.Attribute</a><br />Attribute that cause this exception.</dd><dt>parameter</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.reflection.parameterinfo" target="_blank">System.Reflection.ParameterInfo</a><br />Parameter which the attribute is on.</dd><dt>memberInfo</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.reflection.memberinfo" target="_blank">System.Reflection.MemberInfo</a><br />Member where this attribute is marked on.</dd><dt>memberParentPath</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.collections.generic.stack-1" target="_blank">System.Collections.Generic.Stack</a>(<a href="https://docs.microsoft.com/dotnet/api/system.reflection.memberinfo" target="_blank">MemberInfo</a>)<br />Parent path.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Inspecting_InvalidParameterAttributeDataException">InvalidParameterAttributeDataException Class</a><br /><a href="Overload_SecretNest_RemoteAgency_Inspecting_InvalidParameterAttributeDataException__ctor">InvalidParameterAttributeDataException Overload</a><br /><a href="N_SecretNest_RemoteAgency_Inspecting">SecretNest.RemoteAgency.Inspecting Namespace</a><br />