# MessageFurtherProcessing Enumeration
 

Defines the further processing of this message.

**Namespace:**&nbsp;<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency</a><br />**Assembly:**&nbsp;SecretNest.RemoteAgency (in SecretNest.RemoteAgency.dll) Version: 2.0.0

## Syntax

**C#**<br />
``` C#
public enum MessageFurtherProcessing
```

**VB**<br />
``` VB
Public Enumeration MessageFurtherProcessing
```

**C++**<br />
``` C++
public enum class MessageFurtherProcessing
```

**F#**<br />
``` F#
type MessageFurtherProcessing
```


## Members
&nbsp;<table><tr><th></th><th>Member name</th><th>Value</th><th>Description</th></tr><tr><td /><td target="F:SecretNest.RemoteAgency.MessageFurtherProcessing.Continue">**Continue**</td><td>0</td><td>Continues.</td></tr><tr><td /><td target="F:SecretNest.RemoteAgency.MessageFurtherProcessing.TerminateAndReturnException">**TerminateAndReturnException**</td><td>1</td><td>Terminates this process and send an instance of <a href="T_SecretNest_RemoteAgency_MessageProcessTerminatedException">MessageProcessTerminatedException</a> back to the sender. Cannot be used when <a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_IsOneWay">IsOneWay</a> is `true` (`True` in Visual Basic).</td></tr><tr><td /><td target="F:SecretNest.RemoteAgency.MessageFurtherProcessing.ReplaceWithException">**ReplaceWithException**</td><td>2</td><td>Replaces this message by an instance of <a href="T_SecretNest_RemoteAgency_MessageProcessTerminatedException">MessageProcessTerminatedException</a> then sends it to the receiver. Cannot be used when <a href="P_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgsBase_MessageDirection">MessageDirection</a> is <a href="T_SecretNest_RemoteAgency_MessageDirection">Sending</a>.</td></tr><tr><td /><td target="F:SecretNest.RemoteAgency.MessageFurtherProcessing.ReplaceWithExceptionAndReturn">**ReplaceWithExceptionAndReturn**</td><td>3</td><td>Replaces this message by an instance of <a href="T_SecretNest_RemoteAgency_MessageProcessTerminatedException">MessageProcessTerminatedException</a> then sends it to the receiver and the sender. Cannot be used when <a href="P_SecretNest_RemoteAgency_BeforeMessageProcessingEventArgsBase_MessageDirection">MessageDirection</a> is <a href="T_SecretNest_RemoteAgency_MessageDirection">Sending</a> or <a href="P_SecretNest_RemoteAgency_MessageBodyEventArgsBase_IsOneWay">IsOneWay</a> is `true` (`True` in Visual Basic).</td></tr><tr><td /><td target="F:SecretNest.RemoteAgency.MessageFurtherProcessing.TerminateSilently">**TerminateSilently**</td><td>4</td><td>Terminates this process silently.</td></tr></table>

## See Also


#### Reference
<a href="N_SecretNest_RemoteAgency">SecretNest.RemoteAgency Namespace</a><br />