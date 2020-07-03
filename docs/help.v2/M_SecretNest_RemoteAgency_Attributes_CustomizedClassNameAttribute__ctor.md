# CustomizedClassNameAttribute Constructor 
 

Initializes an instance of the CustomizedAssemblyNameAttribute.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Abstraction (in SecretNest.RemoteAgency.Abstraction.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public CustomizedClassNameAttribute(
	string proxyClassName = null,
	string serviceWrapperClassName = null,
	string assemblyName = null
)
```

**VB**<br />
``` VB
Public Sub New ( 
	Optional proxyClassName As String = Nothing,
	Optional serviceWrapperClassName As String = Nothing,
	Optional assemblyName As String = Nothing
)
```

**C++**<br />
``` C++
public:
CustomizedClassNameAttribute(
	String^ proxyClassName = nullptr, 
	String^ serviceWrapperClassName = nullptr, 
	String^ assemblyName = nullptr
)
```

**F#**<br />
``` F#
new : 
        ?proxyClassName : string * 
        ?serviceWrapperClassName : string * 
        ?assemblyName : string 
(* Defaults:
        let _proxyClassName = defaultArg proxyClassName null
        let _serviceWrapperClassName = defaultArg serviceWrapperClassName null
        let _assemblyName = defaultArg assemblyName null
*)
-> CustomizedClassNameAttribute
```


#### Parameters
&nbsp;<dl><dt>proxyClassName (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Name of proxy class. When the value is a null reference (`Nothing` in Visual Basic) or empty string, name is chosen automatically. Default value is a null reference (`Nothing` in Visual Basic).</dd><dt>serviceWrapperClassName (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Name of service wrapper class. When the value is a null reference (`Nothing` in Visual Basic) or empty string,name is chosen automatically. Default value is a null reference (`Nothing` in Visual Basic).</dd><dt>assemblyName (Optional)</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />Name of assembly. When the value is a null reference (`Nothing` in Visual Basic) or empty string, name is chosen automatically. Default value is a null reference (`Nothing` in Visual Basic).</dd></dl>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_Attributes_CustomizedClassNameAttribute">CustomizedClassNameAttribute Class</a><br /><a href="N_SecretNest_RemoteAgency_Attributes">SecretNest.RemoteAgency.Attributes Namespace</a><br />