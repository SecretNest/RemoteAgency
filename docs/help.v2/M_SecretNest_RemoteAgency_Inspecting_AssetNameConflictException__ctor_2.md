# AssetNameConflictException Constructor (String, Attribute, MemberInfo, Stack(MemberInfo))
 

Initializes an instance of AssetNameConflictException.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Inspecting">SecretNest.RemoteAgency.Inspecting</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public AssetNameConflictException(
	string message,
	Attribute attribute,
	MemberInfo memberInfo,
	Stack<MemberInfo> memberParentPath
)
```

**VB**<br />
``` VB
Public Sub New ( 
	message As String,
	attribute As Attribute,
	memberInfo As MemberInfo,
	memberParentPath As Stack(Of MemberInfo)
)
```

**C++**<br />
``` C++
public:
AssetNameConflictException(
	String^ message, 
	Attribute^ attribute, 
	MemberInfo^ memberInfo, 
	Stack<MemberInfo^>^ memberParentPath
)
```

**F#**<br />
``` F#
new : 
        message : string * 
        attribute : Attribute * 
        memberInfo : MemberInfo * 
        memberParentPath : Stack<MemberInfo> -> AssetNameConflictException
```


#### Parameters
&nbsp;<dl><dt>message</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Exception message.</dd><dt>attribute</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.attribute" target="_blank">System.Attribute</a><br />Attribute that cause this exception.</dd><dt>memberInfo</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.reflection.memberinfo" target="_blank">System.Reflection.MemberInfo</a><br />Member where this attribute is marked on.</dd><dt>memberParentPath</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.collections.generic.stack-1" target="_blank">System.Collections.Generic.Stack</a>(<a href="https://docs.microsoft.com/dotnet/api/system.reflection.memberinfo" target="_blank">MemberInfo</a>)<br />Parent path.</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Inspecting_AssetNameConflictException">AssetNameConflictException Class</a><br /><a href="Overload_SecretNest_RemoteAgency_Inspecting_AssetNameConflictException__ctor">AssetNameConflictException Overload</a><br /><a href="N_SecretNest_RemoteAgency_Inspecting">SecretNest.RemoteAgency.Inspecting Namespace</a><br />