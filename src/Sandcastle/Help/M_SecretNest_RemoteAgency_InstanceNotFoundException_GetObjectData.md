# InstanceNotFoundException.GetObjectData Method 
 

When overridden in a derived class, sets the <a href="https://docs.microsoft.com/dotnet/api/system.runtime.serialization.serializationinfo" target="_blank">SerializationInfo</a> with information about the exception.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public override void GetObjectData(
	SerializationInfo info,
	StreamingContext context
)
```

**VB**<br />
``` VB
Public Overrides Sub GetObjectData ( 
	info As SerializationInfo,
	context As StreamingContext
)
```

**C++**<br />
``` C++
public:
virtual void GetObjectData(
	SerializationInfo^ info, 
	StreamingContext context
) override
```

**F#**<br />
``` F#
abstract GetObjectData : 
        info : SerializationInfo * 
        context : StreamingContext -> unit 
override GetObjectData : 
        info : SerializationInfo * 
        context : StreamingContext -> unit 
```


#### Parameters
&nbsp;<dl><dt>info</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.runtime.serialization.serializationinfo" target="_blank">System.Runtime.Serialization.SerializationInfo</a><br />The <a href="https://docs.microsoft.com/dotnet/api/system.runtime.serialization.serializationinfo" target="_blank">SerializationInfo</a> that holds the serialized object data about the exception being thrown.</dd><dt>context</dt><dd>Type: <a href="https://docs.microsoft.com/dotnet/api/system.runtime.serialization.streamingcontext" target="_blank">System.Runtime.Serialization.StreamingContext</a><br />The <a href="https://docs.microsoft.com/dotnet/api/system.runtime.serialization.streamingcontext" target="_blank">StreamingContext</a> that contains contextual information about the source or destination.</dd></dl>

#### Implements
<a href="https://docs.microsoft.com/dotnet/api/system.runtime.serialization.iserializable.getobjectdata#System_Runtime_Serialization_ISerializable_GetObjectData_System_Runtime_Serialization_SerializationInfo_System_Runtime_Serialization_StreamingContext_" target="_blank">ISerializable.GetObjectData(SerializationInfo, StreamingContext)</a><br /><a href="https://docs.microsoft.com/dotnet/api/system.runtime.interopservices._exception.getobjectdata#System_Runtime_InteropServices__Exception_GetObjectData_System_Runtime_Serialization_SerializationInfo_System_Runtime_Serialization_StreamingContext_" target="_blank">_Exception.GetObjectData(SerializationInfo, StreamingContext)</a><br />

## Exceptions
&nbsp;<table><tr><th>Exception</th><th>Condition</th></tr><tr><td><a href="https://docs.microsoft.com/dotnet/api/system.argumentnullexception" target="_blank">ArgumentNullException</a></td><td>The *info* parameter is a null reference (a null reference (`Nothing` in Visual Basic) in Visual Basic).</td></tr><tr><td><a href="https://docs.microsoft.com/dotnet/api/system.security.securityexception" target="_blank">SecurityException</a></td><td>The caller does not have the required permission.</td></tr></table>

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_InstanceNotFoundException">InstanceNotFoundException Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />