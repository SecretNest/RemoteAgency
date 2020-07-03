# RemoteAgencyBinarySerializerEntityCodeBuilder.ParameterLevelAttributeBaseType Property 
 

Gets the type of the base class of attributes which are used to mark metadata on parameter level.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency_BinarySerializer">SecretNest.RemoteAgency.BinarySerializer</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public override Type ParameterLevelAttributeBaseType { get; }
```

**VB**<br />
``` VB
Public Overrides ReadOnly Property ParameterLevelAttributeBaseType As Type
	Get
```

**C++**<br />
``` C++
public:
virtual property Type^ ParameterLevelAttributeBaseType {
	Type^ get () override;
}
```

**F#**<br />
``` F#
abstract ParameterLevelAttributeBaseType : Type with get
override ParameterLevelAttributeBaseType : Type with get
```


#### Property Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.type" target="_blank">Type</a>

## Remarks
The parameter level attributes will be searched from parameter of method, parameter of delegate and property itself.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_BinarySerializer_RemoteAgencyBinarySerializerEntityCodeBuilder">RemoteAgencyBinarySerializerEntityCodeBuilder Class</a><br /><a href="N_SecretNest_RemoteAgency_BinarySerializer">SecretNest.RemoteAgency.BinarySerializer Namespace</a><br />