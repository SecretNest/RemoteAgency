# InvalidAttributeDataException Constructor (String, Attribute, Stack(MemberInfo))
 

Initializes an instance of InvalidAttributeDataException.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Inspecting">SecretNest.RemoteAgency.Inspecting</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public InvalidAttributeDataException(
	string message,
	Attribute attribute,
	Stack<MemberInfo> memberPath
)
```

**VB**<br />
``` VB
Public Sub New ( 
	message As String,
	attribute As Attribute,
	memberPath As Stack(Of MemberInfo)
)
```

**C++**<br />
``` C++
public:
InvalidAttributeDataException(
	String^ message, 
	Attribute^ attribute, 
	Stack<MemberInfo^>^ memberPath
)
```

**F#**<br />
``` F#
new : 
        message : string * 
        attribute : Attribute * 
        memberPath : Stack<MemberInfo> -> InvalidAttributeDataException
```


#### Parameters
&nbsp;<dl><dt>message</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Exception message.</dd><dt>attribute</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.attribute" target="_blank">System.Attribute</a><br />Attribute that cause this exception.</dd><dt>memberPath</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.collections.generic.stack-1" target="_blank">System.Collections.Generic.Stack</a>(<a href="https://docs.microsoft.com/dotnet/api/system.reflection.memberinfo" target="_blank">MemberInfo</a>)<br />Member path.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Inspecting_InvalidAttributeDataException">InvalidAttributeDataException Class</a><br /><a href="Overload_SecretNest_RemoteAgency_Inspecting_InvalidAttributeDataException__ctor">InvalidAttributeDataException Overload</a><br /><a href="N_SecretNest_RemoteAgency_Inspecting">SecretNest.RemoteAgency.Inspecting Namespace</a><br />