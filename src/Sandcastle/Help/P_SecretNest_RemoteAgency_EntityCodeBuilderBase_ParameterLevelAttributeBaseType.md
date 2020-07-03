# EntityCodeBuilderBase.ParameterLevelAttributeBaseType Property 
 

Gets the type of the base class of attributes which are used to mark metadata on parameter level.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public abstract Type ParameterLevelAttributeBaseType { get; }
```

**VB**<br />
``` VB
Public MustOverride ReadOnly Property ParameterLevelAttributeBaseType As Type
	Get
```

**C++**<br />
``` C++
public:
virtual property Type^ ParameterLevelAttributeBaseType {
	Type^ get () abstract;
}
```

**F#**<br />
``` F#
abstract ParameterLevelAttributeBaseType : Type with get

```


#### Property Value
Type: <a href="https://docs.microsoft.com/dotnet/api/system.type" target="_blank">Type</a>

## Remarks
The parameter level attributes will be searched from parameter of method, parameter of delegate and property itself.

## See Also


#### Reference
<a href="T_SecretNest_RemoteAgency_EntityCodeBuilderBase">EntityCodeBuilderBase Class</a><br /><a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />