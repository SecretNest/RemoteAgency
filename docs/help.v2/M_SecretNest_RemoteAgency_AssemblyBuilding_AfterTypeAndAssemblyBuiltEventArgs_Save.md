# AfterTypeAndAssemblyBuiltEventArgs.Save Method 
 

Saves the built assembly to the file specified.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_AssemblyBuilding">SecretNest.RemoteAgency.AssemblyBuilding</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public void Save(
	string assemblyFileName
)
```

**VB**<br />
``` VB
Public Sub Save ( 
	assemblyFileName As String
)
```

**C++**<br />
``` C++
public:
void Save(
	String^ assemblyFileName
)
```

**F#**<br />
``` F#
member Save : 
        assemblyFileName : string -> unit 

```


#### Parameters
&nbsp;<dl><dt>assemblyFileName</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.string" target="_blank">System.String</a><br />File name to be written to.</dd></dl>

## Exceptions
&nbsp;<table><tr><th>Exception</th><th>Condition</th></tr><tr><td><a href="https://docs.microsoft.com/dotnet/api/system.notsupportedexception" target="_blank">NotSupportedException</a></td><td>Thrown when called in .net core app.</td></tr></table>

## Remarks
Assembly saving is not supported by .net core. This method is only for .net framework.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_AssemblyBuilding_AfterTypeAndAssemblyBuiltEventArgs">AfterTypeAndAssemblyBuiltEventArgs Class</a><br /><a href="N_SecretNest_RemoteAgency_AssemblyBuilding">SecretNest.RemoteAgency.AssemblyBuilding Namespace</a><br />