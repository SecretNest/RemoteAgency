using System;
using System.Collections.Generic;
using System.Text;

namespace SecretNest.RemoteAgency
{
    /// <summary>
    /// Represents an redirected exception thrown from user code.
    /// </summary>
    public class ExceptionRedirectedEventArgs
    {
        /// <summary>
        /// Gets the type of the service contract interface.
        /// </summary>
        public Type ServiceContractInterface { get; }

        /// <summary>
        /// Gets the instance id of the instance which throws exception.
        /// </summary>
        public Guid InstanceId { get; }

        /// <summary>
        /// Gets the name of the asset which throws exception.
        /// </summary>
        public string AssetName { get; }

        /// <summary>
        /// Gets the exception thrown.
        /// </summary>
        public Exception RedirectedException { get; }

        /// <summary>
        /// Initializes an instance of ExceptionRedirectedEventArgs.
        /// </summary>
        /// <param name="serviceContractInterface">The type of the service contract interface.</param>
        /// <param name="instanceId">Instance id of the instance which throws exception.</param>
        /// <param name="assetName">The name of the asset which throws exception.</param>
        /// <param name="exception">Exception thrown.</param>
        public ExceptionRedirectedEventArgs(Type serviceContractInterface, Guid instanceId, string assetName, Exception exception)
        {
            ServiceContractInterface = serviceContractInterface;
            InstanceId = instanceId;
            AssetName = assetName;
            RedirectedException = exception;
        }
    }
}
