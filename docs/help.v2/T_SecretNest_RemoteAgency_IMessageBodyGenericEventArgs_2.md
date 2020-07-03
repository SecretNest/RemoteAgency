# IMessageBodyGenericEventArgs(*TSerialized*, *TEntityBase*) Interface
 

Defines property and method to hold the message body and provide an access point for serialization.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public interface IMessageBodyGenericEventArgs<out TSerialized, out TEntityBase>

```

**VB**<br />
``` VB
Public Interface IMessageBodyGenericEventArgs(Of Out TSerialized, Out TEntityBase)
```

**C++**<br />
``` C++
generic<typename TSerialized, typename TEntityBase>
public interface class IMessageBodyGenericEventArgs
```

**F#**<br />
``` F#
type IMessageBodyGenericEventArgs<'TSerialized, 'TEntityBase> =  interface end
```


#### Type Parameters
&nbsp;<dl><dt>TSerialized</dt><dd>Type of the serialized data.</dd><dt>TEntityBase</dt><dd>Type of the parent class of all entities.</dd></dl>&nbsp;
The IMessageBodyGenericEventArgs(TSerialized, TEntityBase) type exposes the following members.


## Properties
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public property](media/pubproperty.gif "Public property")</td><td><a href="P_SecretNest_RemoteAgency_IMessageBodyGenericEventArgs_2_MessageBody">MessageBody</a></td><td>
Gets the message body.</td></tr></table>&nbsp;
<a href="#imessagebodygenericeventargs(*tserialized*,-*tentitybase*)-interface">Back to Top</a>

## Methods
&nbsp;<table><tr><th></th><th>Name</th><th>Description</th></tr><tr><td>![Public method](media/pubmethod.gif "Public method")</td><td><a href="M_SecretNest_RemoteAgency_IMessageBodyGenericEventArgs_2_Serialize">Serialize</a></td><td>
Serializes this message using the serializer from the Remote Agency instance.</td></tr></table>&nbsp;
<a href="#imessagebodygenericeventargs(*tserialized*,-*tentitybase*)-interface">Back to Top</a>

## See Also


#### Reference
<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />