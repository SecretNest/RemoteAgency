# MessageType Enumeration
 

Contains a list of message type

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency.Base (in SecretNest.RemoteAgency.Base.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
[SerializableAttribute]
public enum MessageType
```

**VB**<br />
``` VB
<SerializableAttribute>
Public Enumeration MessageType
```

**C++**<br />
``` C++
[SerializableAttribute]
public enum class MessageType
```

**F#**<br />
``` F#
[<SerializableAttribute>]
type MessageType
```


## Members
&nbsp;<table><tr><th></th><th>Member name</th><th>Value</th><th>Description</th></tr><tr><td /><td target="F:SecretNest.RemoteAgency.MessageType.Method">**Method**</td><td>0</td><td>Declares this message is related to a method calling or the returning of it.</td></tr><tr><td /><td target="F:SecretNest.RemoteAgency.MessageType.EventAdd">**EventAdd**</td><td>1</td><td>Declares this message is related to adding event handler or the result of it.</td></tr><tr><td /><td target="F:SecretNest.RemoteAgency.MessageType.EventRemove">**EventRemove**</td><td>2</td><td>Declares this message is related to removing event handler or the result of it.</td></tr><tr><td /><td target="F:SecretNest.RemoteAgency.MessageType.Event">**Event**</td><td>3</td><td>Declares this message is related to an event raised or the returning of it.</td></tr><tr><td /><td target="F:SecretNest.RemoteAgency.MessageType.PropertyGet">**PropertyGet**</td><td>4</td><td>Declares this message is related to getting value of a property or the returning of it.</td></tr><tr><td /><td target="F:SecretNest.RemoteAgency.MessageType.PropertySet">**PropertySet**</td><td>5</td><td>Declares this message is related to setting value of a property or the result of it.</td></tr><tr><td /><td target="F:SecretNest.RemoteAgency.MessageType.SpecialCommand">**SpecialCommand**</td><td>6</td><td>Declares this message is a system reserved message.</td></tr></table>

## See Also


#### Reference
<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />