using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// The exception that is thrown when <see cref="BeforeMessageProcessingEventArgsBase.FurtherProcessing"/> is set as TerminateWithExceptionReturned.
    /// </summary>
    /// <seealso cref="BeforeMessageProcessingEventArgs{TSerialized}"/>
    /// <seealso cref="BeforeExceptionMessageProcessingEventArgs{TSerialized}"/>
    /// <seealso cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}.BeforeMessageSending"/>
    /// <seealso cref="RemoteAgencyManager{TNetworkMessage, TSerialized, TEntityBase}.AfterMessageReceived"/>
    public class MessageProcessTerminatedException : Exception
    {
        internal MessageProcessTerminatedException() { }
    }
}
